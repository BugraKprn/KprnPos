using Business.Layer.Abstract;
using Business.Layer.Concrete;
using DataAccess.Layer.Concrete;
using DataAccess.Layer.EntityFramework;
using Dto.Layer.Dtos.OrderHistory;
using Entity.Layer.Concrete;
using KprnRestaurantPos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;


namespace KprnRestaurantPos.Controllers
{
    [Authorize]
    public class PosController : Controller
    {
        Context context = new Context();
        ProductManager productManager = new ProductManager(new EfProductDal());
        CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());

        [Route("/Pos/Order")]
        public async Task<IActionResult> Index(int tableId)
        {
            string? tableName = context.Tables.FirstOrDefault(t => t.TableId == tableId)?.TableName;
            var siparis = await context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.TableId == tableId && o.OrderStatus == true)
                .ToListAsync();

            // Viewbag Kısımları

            ViewBag.SipCount = siparis.Count;
            ViewBag.TableId = tableId;
            ViewBag.TableName = tableName;

            MenuDto model = new MenuDto()
            {
                ProductModel = productManager.TGetList(),
                CategoryModel = categoryManager.TGetList()
            };

            List<Order> orders = HttpContext.Session.GetObject<List<Order>>("Orders") ?? new List<Order>();

            model.OrderModel = orders;

            return View(model);
        }

        [HttpGet]
        public IActionResult GetProductById(int productid)
        {
            var findProduct = context.Products.FirstOrDefault(x => x.ProductId == productid);
            var jsonProducts = JsonConvert.SerializeObject(findProduct);
            return Json(jsonProducts);
        }

        [HttpGet]
        public IActionResult GetProductDetails(int productId)
        {
            var product = context.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound();
            }

            return Json(new
            {
                productNo = productId,
                productName = product.ProductName,
                productDescription = product.ProductDescription,
                productPrice = product.ProductPrice,
                productStatus = product.ProductStatus,
                productImage = product.ProductImage

            });
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            List<SiparisViewModel> orders = HttpContext.Session.GetObject<List<SiparisViewModel>>("Orders") ?? new List<SiparisViewModel>();

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int orderId, int productId, int change)
        {
            // Session'dan mevcut sipariş listesini alın
            var sessionOrders = HttpContext.Session.GetObjectFromJson<List<Order>>("Orders");

            // Eğer session'da siparişler varsa
            if (sessionOrders != null)
            {
                // İlgili siparişi bulun
                var order = sessionOrders.FirstOrDefault(o => o.OrderId == orderId);

                if (order != null)
                {
                    // İlgili ürünü bulun
                    var orderItem = order.OrderItems.FirstOrDefault(oi => oi.ProductId == productId);

                    if (orderItem != null)
                    {
                        // Miktarı güncelle
                        orderItem.Quantity += change;

                        // Eğer miktar 0 veya altına düşerse, ürünü siparişten kaldır
                        if (orderItem.Quantity <= 0)
                        {
                            order.OrderItems.Remove(orderItem);
                        }

                        // Eğer siparişin içindeki ürünlerin sayısı 0 ise, siparişi tamamen kaldır
                        if (order.OrderItems.Count == 0)
                        {
                            sessionOrders.Remove(order);
                        }

                        // Güncellenmiş sipariş listesini session'a geri kaydedin
                        HttpContext.Session.SetObjectAsJson("Orders", sessionOrders);

                        // Başarılı yanıt dön
                        return Json(new { success = true });
                    }
                }
            }

            // Başarısız yanıt dön
            return Json(new { success = false });
        }



        [HttpPost]
        [Route("/Pos/AddToSession")]
        public IActionResult AddToSession(int tableId, int productId, int quantity)
        {
            List<SiparisViewModel> orders = HttpContext.Session.GetObject<List<SiparisViewModel>>("Orders") ?? new List<SiparisViewModel>();

            Random random = new Random();
            int sipNo = random.Next(1000, 10000);
            string? tableName = context.Tables.FirstOrDefault(t => t.TableId == tableId)?.TableName;
            ViewBag.TableName = tableName;
            var product = context.Products.Find(productId);

            if (product != null && product.ProductPrice != null)
            {
                var order = orders.FirstOrDefault(o => o.TableId == tableId);
                if (order == null)
                {
                    order = new SiparisViewModel
                    {
                        TableId = tableId,
                        OrderNumber = sipNo,
                        OrderDate = DateTime.Now,
                        OrderStatus = true,
                        OrderItems = new List<SiparisItemViewModel>(),
                        TotalPrice = 0 // Initializing the TotalPrice
                    };
                    orders.Add(order);
                }

                var unitPrice = product.ProductPrice.GetValueOrDefault(); // Dönüşüm burada yapılıyor
                var orderItem = new SiparisItemViewModel
                {
                    ProductId = productId,
                    ProductName = product.ProductName,
                    ProductImage = product.ProductImage,
                    Quantity = quantity,
                    UnitPrice = unitPrice
                };

                order.OrderItems.Add(orderItem);

                // Update the TotalPrice
                order.TotalPrice += unitPrice * quantity;

                HttpContext.Session.SetObject("Orders", orders);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("/Pos/AddToOrder")]
        public IActionResult AddToOrder()
        {
            try
            {
                // Session'dan sipariş listesini al
                List<SiparisViewModel> orders = HttpContext.Session.GetObject<List<SiparisViewModel>>("Orders") ?? new List<SiparisViewModel>();

                // Veritabanına kaydetmek için siparişleri döngüye al
                foreach (var siparisViewModel in orders)
                {
                    // Siparişi ve sipariş kalemlerini veritabanına kaydet
                    var siparis = new Order
                    {
                        OrderNumber = siparisViewModel.OrderNumber,
                        OrderStatus = true,
                        TableId = siparisViewModel.TableId,
                        OrderDate = siparisViewModel.OrderDate,
                        OrderItems = new List<OrderItem>(),
                        TotalPrice = 0 // Initializing the TotalPrice
                    };

                    foreach (var siparisItemViewModel in siparisViewModel.OrderItems)
                    {
                        var unitPrice = siparisItemViewModel.UnitPrice.GetValueOrDefault(); // Dönüşüm burada yapılıyor
                        var siparisItem = new OrderItem
                        {
                            ProductId = siparisItemViewModel.ProductId,
                            Quantity = siparisItemViewModel.Quantity,
                            UnitPrice = unitPrice
                        };

                        siparis.OrderItems.Add(siparisItem);

                        // Update the TotalPrice
                        siparis.TotalPrice += unitPrice * siparisItem.Quantity;
                    }

                    // Siparişi veritabanına ekle
                    context.Orders.Add(siparis);
                }

                // Değişiklikleri kaydet
                context.SaveChanges();

                // Siparişleri Session'dan temizle
                HttpContext.Session.Remove("Orders");

                return Ok(); // Başarılı olduğunu bildirir
            }
            catch (Exception ex)
            {
                // Hata oluştuğunda loglama
                Console.WriteLine($"Siparişleri kaydetmede bir hata oluştu: {ex.Message}");
                return StatusCode(500, "Siparişleri kaydetmede bir hata oluştu: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("/Pos/GetOrderHistoryByTableId/{tableId}")]
        public async Task<IActionResult> GetOrderHistoryByTableId(int tableId)
        {
            var table = await context.Tables
                .Where(t => t.TableId == tableId)
                .FirstOrDefaultAsync();

            if (table == null)
            {
                return NotFound();
            }

            // Masaya ait siparişleri çekiyoruz
            var orders = await context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Category)
                .Where(o => o.TableId == tableId && o.OrderStatus == true)
                .ToListAsync();

            // Eğer sipariş varsa DTO'ya ekliyoruz
            var orderDTOs = orders.Select(order => new OrderDTO
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                OrderStatus = true,
                TableName = table.TableName,
                OrderItems = order.OrderItems.Select(item => new OrderItemDTO
                {
                    OrderItemId = item.OrderItemId,
                    ProductId = item.Product.ProductId,
                    ProductImage = item.Product.ProductImage,
                    CategoryName = item.Product.Category?.CategoryName,
                    ProductName = item.Product.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            }).ToList();

            // Eğer sipariş yoksa bile masa adını göstermek için boş bir DTO ekliyoruz
            if (!orderDTOs.Any())
            {
                orderDTOs.Add(new OrderDTO
                {
                    TableName = table.TableName, // Masa adı burada
                    OrderItems = new List<OrderItemDTO>() // Boş sipariş listesi
                });
            }

            return Json(orderDTOs);
        }



        public class MenuDto
        {
            public List<Product> ProductModel { get; set; } = new List<Product>() { };
            public List<Category> CategoryModel { get; set; } = new List<Category>() { };
            public List<Order>? OrderModel { get; set; }
        }
    }

    public static class SessionExtensions2
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
