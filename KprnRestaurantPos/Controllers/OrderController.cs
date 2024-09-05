using Business.Layer.Abstract;
using Business.Layer.Concrete;
using DataAccess.Layer.Concrete;
using Dto.Layer.Dtos.Payment;
using Entity.Layer.Concrete;
using KprnRestaurantPos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;

namespace KprnRestaurantPos.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        Context context = new Context();

        [Route("/Pos")]
        public IActionResult Index()
        {
            var tables = context.Tables.ToList();

            var tableViewModels = tables.Select(table => new TableViewModel
            {
                TableId = table.TableId,
                TableName = table.TableName,
                OrderDate = context.Orders
                        .Where(o => o.TableId == table.TableId && o.OrderStatus == true)
                        .OrderByDescending(o => o.OrderDate)
                        .Select(o => o.OrderDate)
                        .FirstOrDefault(),
                // Sipariş sayısı
                OrderCount = context.Orders.Count(o => o.TableId == table.TableId && o.OrderStatus == true),
                //Siparişlerin Toplam Fiyatı
                OrderTotalPrice = context.Orders.Where(o => o.TableId == table.TableId && o.OrderStatus == true).Sum(o => o.TotalPrice),
                // Masanın dolu olup olmadığı
                IsOccupied = context.Orders.Any(o => o.TableId == table.TableId && o.OrderStatus == false)
            }).ToList();


            return View(tableViewModels);
        }

        [HttpPost]
        [Route("/Payment/ProcessPayment")]
        public IActionResult ProcessPayment([FromBody] PaymentRequestDto paymentRequestDto)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                foreach (var payment in paymentRequestDto.Payments)
                {
                    var paymentEntity = new Payment
                    {
                        TableId = paymentRequestDto.TableId,
                        PaymentType = payment.PaymentType,
                        Amount = payment.Amount,
                        PaymentDate = DateTime.Now
                    };
                    context.Payments.Add(paymentEntity);
                }
                context.SaveChanges();

                var orders = context.Orders.Where(o => o.TableId == paymentRequestDto.TableId && o.OrderStatus == true).ToList();
                foreach (var order in orders)
                {
                    order.OrderStatus = false;
                }
                context.SaveChanges();

                var table = context.Tables.FirstOrDefault(t => t.TableId == paymentRequestDto.TableId);
                if (table != null)
                {
                    table.IsOccupied = "Boş";
                    context.SaveChanges();
                }

                transaction.Commit();

                var response = new PaymentResponseDto
                {
                    Success = true,
                    Message = "Ödeme işlemi başarıyla tamamlandı."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                var response = new PaymentResponseDto
                {
                    Success = false,
                    Message = $"Ödeme işlemi başarısız: {ex.Message}"
                };

                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("/Pos/MoveOrderToTable")]
        public IActionResult MoveOrderToTable(int currentTableId, int newTableId)
        {
            // İlgili siparişi mevcut masadan alın
            var order = context.Orders.Include(o => o.OrderItems)
                                      .FirstOrDefault(o => o.TableId == currentTableId && o.OrderStatus == true);

            if (order != null)
            {
                // Siparişin masasını güncelle
                order.TableId = newTableId;

                // Veritabanına güncellemeyi kaydedin
                context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Taşınacak sipariş bulunamadı." });
        }


    }

    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
