using Business.Layer.Concrete;
using DataAccess.Layer.EntityFramework;
using Entity.Layer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace KprnRestaurantPos.ViewComponents.ProductPage
{
    public class _Product : ViewComponent
    {
        ProductManager productManager = new ProductManager(new EfProductDal());
        CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());
        public IViewComponentResult Invoke()
        {
            MenuDto model = new MenuDto()
            {
                ProductModel = productManager.TGetList(),
                CategoryModel = categoryManager.TGetList()
            };
            return View(model);
        }

        public class MenuDto
        {
            public List<Product> ProductModel { get; set; } = new List<Product>() { };
            public List<Category> CategoryModel { get; set; } = new List<Category>() { };
        }
    }
}
