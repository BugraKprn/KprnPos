using Business.Layer.Concrete;
using DataAccess.Layer.EntityFramework;
using Entity.Layer.Concrete;
using KprnRestaurantPos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace KprnRestaurantPos.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());

        [Route("/Pos/Category")]
        public IActionResult Index()
        {
            var values = categoryManager.TGetList();
            return View(values);
        }

        [HttpGet]
        [Route("/Pos/Category/Add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Route("/Pos/Category/Add")]
        public IActionResult Add(Category category)
        {
            categoryManager.TAdd(category);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/Pos/Category/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var values = categoryManager.TGetById(id);
            return View(values);
        }

        [HttpPost]
        [Route("/Pos/Category/Edit/{id}")]
        public IActionResult Edit(Category category)
        {
            categoryManager.TUpdate(category);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var values = categoryManager.TGetById(id);
            categoryManager.TDelete(values);
            return RedirectToAction("Index");
        }

    }
}
