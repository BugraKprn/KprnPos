using Business.Layer.Concrete;
using DataAccess.Layer.Concrete;
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
    public class ProductController : Controller
    {
        Context context = new Context();
        ProductManager productManager = new ProductManager(new EfProductDal());
        CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());

        [Route("/Pos/Product")]
        public IActionResult Index()
        {
            var values = productManager.GetProductListWithCategory();
            return View(values);
        }

        [HttpGet]
        [Route("/Pos/Product/Add")]
        public IActionResult Add()
        {
            List<SelectListItem> ctgry = (from x in categoryManager.TGetList()
                                          select new SelectListItem
                                          {
                                              Text = x.CategoryName,
                                              Value = x.CategoryId.ToString()
                                          }).ToList();
            ViewBag.v = ctgry;
            return View();
        }

        [HttpPost]
        [Route("/Pos/Product/Add")]
        public async Task<IActionResult> Add(AddProductImage addProductImage)
        {
            Product product = new Product();
            if (addProductImage.ProductImage != null)
            {
                var resource = Directory.GetCurrentDirectory();
                var extension = Path.GetExtension(addProductImage.ProductImage.FileName);
                var imagename = Guid.NewGuid() + extension;
                var savelocation = resource + "/wwwroot/Pos/assets/img/profiles" + imagename;
                var stream = new FileStream(savelocation, FileMode.Create);
                await addProductImage.ProductImage.CopyToAsync(stream);
                product.ProductImage = imagename;

            }

            product.ProductName = addProductImage.ProductName;
            product.ProductPrice= addProductImage.ProductPrice;
            product.ProductQuantity = 10;
            product.ProductOrder = addProductImage.ProductOrder;
            product.ProductStatus = addProductImage.ProductStatus;
            product.CategoryId = addProductImage.CategoryId;
            product.ProductDescription = addProductImage.ProductDescription;

            List<SelectListItem> ctgry = (from x in categoryManager.TGetList()
                                          select new SelectListItem
                                          {
                                              Text = x.CategoryName,
                                              Value = x.CategoryId.ToString()
                                          }).ToList();
            ViewBag.v = ctgry;
            productManager.TAdd(product);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/Pos/Product/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var values = productManager.TGetById(id);
            List<SelectListItem> ctgry = (from x in categoryManager.TGetList()
                                           select new SelectListItem
                                           {
                                               Text = x.CategoryName,
                                               Value = x.CategoryId.ToString()
                                           }).ToList();
            ViewBag.v = ctgry;
            return View(values);
        }

        [HttpPost]
        [Route("/Pos/Product/Edit/{id}")]
        public IActionResult Edit(Product product)
        {
            productManager.TUpdate(product);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var values = productManager.TGetById(id);
            productManager.TDelete(values);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetProductById(int productid)
        {
            var findProduct = context.Products.FirstOrDefault(x => x.ProductId == productid);
            var jsonProducts = JsonConvert.SerializeObject(findProduct);
            return Json(jsonProducts);
        }
    }
}
