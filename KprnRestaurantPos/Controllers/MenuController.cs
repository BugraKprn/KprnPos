using Business.Layer.Abstract;
using Business.Layer.Concrete;
using DataAccess.Layer.EntityFramework;
using Entity.Layer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KprnRestaurantPos.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        ProductManager productManager = new ProductManager(new EfProductDal());
        CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
        SettingManager settingManager = new SettingManager(new EfSettingDal());

        [Route("/Menu")]
        public IActionResult Index()
        {
            var setting = settingManager.TGetList();
            return View(setting);
        }

        [Route("/Menu/Categories")]
        public IActionResult Categories()
        {
            var category = categoryManager.TGetList();
            return View(category);
        }

        [Route("/Menu/Categories/{title}-{id}")]
        public IActionResult Products(int id)
        {
            ViewData["Title"] = "Kategori - " ;
            CategoryDto model = new CategoryDto()
            {
                ProductModel = productManager.TGetList(),
                CategoryModel = categoryManager.TGetById(id)
            };
            return View(model);
        }


        public class CategoryDto
        {
            public List<Product> ProductModel { get; set; } = new List<Product>() { };
            public Category CategoryModel { get; set; } = new Category();
        }
    }
}
