using FoodStoreApp.Data;
using FoodStoreApp.Models;
using FoodStoreApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodStoreApp.Controllers
{

    public class ProductController : Controller
    {
        // Переменная контекста
        private readonly ApplicationDbContext _db;
        // Внедрение зависимости создания контекста через конструктор
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            IEnumerable<Product> productList = _db.Product;

            foreach (var product in productList)
            {
                product.Category = _db.Category.FirstOrDefault(u => u.Id == product.CategoryId);
            };

            return View(productList);
        }
        // GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            // Список для выбора категории
            ProductViewModel viewModel = new ProductViewModel()
            {
                Product = new(),
                CategorySelectList = _db.Category.Select(item => new SelectListItem
                {
                    Text = item.Title,
                    Value = item.Id.ToString()
                })
            };


            if (id == null)
            {
                return View(viewModel);
            }
            else
            {
                // Обновляем выбранную позицию товара
                viewModel.Product = _db.Product.Find(id);
                if (viewModel.Product == null)
                    return NotFound();

                return View(viewModel);
            }



        }
    }
}
