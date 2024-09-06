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

        [Route("/Pos/Table-Area")]
        public IActionResult Index()
        {
            var tableAreas = context.TableAreas.OrderBy(ta => ta.AreaOrder).ToList();
            var tables = context.Tables.ToList();

            var model = new TableAndAreaViewModel
            {
                TableAreas = tableAreas,
                Tables = tables
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult GetTablesByArea(int areaId)
        {
            var tables = context.Tables
                                 .Where(t => t.TableAreaId == areaId)
                                 .ToList();

            return Json(tables);
        }


        #region Area

        [HttpGet]
        public IActionResult GetRegions()
        {
            var regions = context.TableAreas.Select(r => new
            {
                r.TableAreaId,
                r.AreaName,
                r.AreaOrder
            }).OrderBy(ta => ta.AreaOrder).ToList();

            return Json(regions);
        }

        [HttpPost]
        public IActionResult AddRegion(string regionName)
        {
            if (!string.IsNullOrEmpty(regionName))
            {
                // Yeni bir bölge oluşturun ve veritabanına kaydedin
                var newArea = new TableArea
                {
                    AreaName = regionName
                };

                context.TableAreas.Add(newArea);
                context.SaveChanges(); // Değişiklikleri kaydedin

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Bölge adı boş olamaz." });
        }

        [HttpGet]
        public IActionResult GetRegionById(int id)
        {
            // Veritabanından bölgeyi bul
            var area = context.TableAreas.FirstOrDefault(r => r.TableAreaId == id);

            if (area == null)
            {
                return NotFound(); // Bölge bulunamazsa 404 döner
            }

            // JSON formatında bölge bilgisini döner
            return Json(new
            {
                id = area.TableAreaId,
                areaName = area.AreaName
            });
        }

        [HttpPost]
        public IActionResult UpdateRegion([FromBody] EditAreaDto model)
        {
            if (ModelState.IsValid)
            {
                // Veritabanından güncellenmesi gereken bölgeyi bul
                var area = context.TableAreas.FirstOrDefault(r => r.TableAreaId == model.AreaId);

                if (area == null)
                {
                    return NotFound(); // Bölge bulunamazsa 404 döner
                }

                // Bölge adını güncelle
                area.AreaName = model.AreaName;

                // Değişiklikleri veritabanına kaydet
                context.SaveChanges();

                return Ok(); // Başarılı işlem sonucu 200 döner
            }

            return BadRequest(); // Model geçerli değilse 400 döner
        }

        [HttpPost]
        public IActionResult UpdateRegionOrder([FromBody] List<int> orderedIds)
        {
            if (orderedIds == null || !orderedIds.Any())
            {
                return BadRequest("Geçersiz veri.");
            }

            try
            {
                UpdateRegionsOrder(orderedIds);
                return Ok("Sıralama başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }


        private void UpdateRegionsOrder(List<int> orderedIds)
        {
            using (var context = new Context())
            {
                for (int i = 0; i < orderedIds.Count; i++)
                {
                    var regionId = orderedIds[i];
                    var region = context.TableAreas.Find(regionId);
                    if (region != null)
                    {
                        region.AreaOrder = i + 1; // Sıfırdan başlaması yerine 1'den başlasın
                    }
                }
                context.SaveChanges();
            }
        }




        #endregion


        #region Table

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
            return Json(new { sucess = true, message = "Başarılı" });
        }

        #endregion


    }
}
