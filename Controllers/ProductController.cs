using FoodStoreApp.Data;
using FoodStoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodStore.Controllers
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
            IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(
                item => new SelectListItem()
                {
                    Text = item.Title,
                    Value = item.Description,
                });

            ViewBag.CategoryDropDown = CategoryDropDown;
            // Создаем новый экземпляр типа Product
            Product product = new();
            // Если идентификатор пуст, то возвращаем представление
            // с товарами
            if (id == null)
            {
                return View(product);
            }
            else
            {
                // Обновляем выбранную позицию товара
                product = _db.Product.Find(id);
                if (product == null)
                    return NotFound();

                return View(product);
            }


        }
    }
}
