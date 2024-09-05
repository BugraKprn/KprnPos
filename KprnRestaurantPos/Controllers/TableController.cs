using Business.Layer.Concrete;
using DataAccess.Layer.Concrete;
using DataAccess.Layer.EntityFramework;
using Dto.Layer.Dtos.TableAndArea;
using Entity.Layer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KprnRestaurantPos.Controllers
{
    [Authorize]
    public class TableController : Controller
    {
        Context context = new Context();
        TableManager tableManager = new TableManager(new EfTableDal());

        [Route("/Pos/Table")]
        public IActionResult Index()
        {
            var values = context.Tables.ToList();
            return View(values);
        }

        [Route("/Pos/TableAndArea")]
        public IActionResult Index1()
        {
            var tableAreas = context.TableAreas.ToList();
            var tables = context.Tables.ToList();

            var model = new TableAndAreaViewModel
            {
                TableAreas = tableAreas,
                Tables = tables
            };

            return View(model);
        }


        [HttpGet]
        public IActionResult List()
        {
            var tables = context.Tables.ToList();
            return Ok(tables);
        }


        [HttpPost]
        public IActionResult UpdateTablePosition([FromBody] TablePositionUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                // Masa kaydını veritabanından bul
                var table = context.Tables.FirstOrDefault(t => t.TableId == model.TableId);
                if (table == null)
                {
                    return NotFound(); // Masa bulunamazsa hata döndür
                }

                // Masa konumunu güncelle
                table.PosX = model.PosX;
                table.PosY = model.PosY;

                // Veritabanını güncelle
                context.Tables.Update(table);
                context.SaveChanges();

                return Ok(); // Güncelleme başarılı
            }

            return BadRequest(); // Geçersiz model durumu
        }




        [HttpGet]
        public IActionResult GetTableById(int tableid)
        {
            var findTable = context.Tables.FirstOrDefault(x => x.TableId == tableid);
            var jsonWriters = JsonConvert.SerializeObject(findTable);
            return Json(jsonWriters);
        }

        [HttpPost]
        public IActionResult Add(Table table)
        {
            context.Tables.Add(table);
            var jsonTable = JsonConvert.SerializeObject(table);
            return Json(new { sucess = true, message = "Başarılı" });
        }

        public IActionResult Update(Table t)
        {
            var table = context.Tables.FirstOrDefault(x => x.TableId == t.TableId);
            table.TableName = t.TableName;
            context.SaveChanges();
            var jsonTable = JsonConvert.SerializeObject(t);
            return Json(new { sucess = true, message = "Başarılı" });
        }

        public IActionResult Delete(int id)
        {
            var table = context.Tables.FirstOrDefault(x => x.TableId == id);
            context.Tables.Remove(table);
            context.SaveChanges();
            return Json(new { sucess = true, message = "Başarılı"});
        }

    }
}
