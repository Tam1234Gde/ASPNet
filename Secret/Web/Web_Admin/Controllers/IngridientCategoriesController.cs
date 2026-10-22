using Microsoft.AspNetCore.Mvc;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Web_Admin.Models.Ingridients;

namespace HIDE.RestAndRant.Web_Admin.Controllers
{
    public class IngridientCategoriesController : Controller
    {
        private readonly ILogger<IngridientCategoriesController> _logger;
        private readonly Dish_IngridientCategory _bl_IngridientCategory;


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logger"></param>
        public IngridientCategoriesController(ILogger<IngridientCategoriesController> logger)
        {
            _logger = logger;
            _bl_IngridientCategory = new Dish_IngridientCategory();
        }

        /// <summary>
        /// Вывод
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var categories = _bl_IngridientCategory.GetAllIngridients();
            return View(categories);
        }


        /// <summary>
        /// Добавление
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            return View();
        }

        
        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("Name", "Название категории обязательно.");
                return View();
            }

            int id = _bl_IngridientCategory.Add(name);
            if (id > 0)
            {
                TempData["Success"] = "Категория успешно добавлена.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Ошибка при добавлении категории.");
                return View();
            }
        }


        /// <summary>
        /// Редактирование
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id)
        {
            var category = _bl_IngridientCategory.GetIngridient(id);
            if (category == null)
                return NotFound();

            return View(category);
        }


        /// <summary>
        /// Редактирование
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(int ID, string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ModelState.AddModelError("Name", "Название категории обязательно.");
                return View(_bl_IngridientCategory.GetIngridient(ID));
            }

            int result = _bl_IngridientCategory.Edit(ID, Name);
            if (result > 0)
            {
                TempData["Success"] = "Категория обновлена.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Ошибка при обновлении.");
                return View(_bl_IngridientCategory.GetIngridient(ID));
            }
        }



        /// <summary>
        /// Удаление - GET (страница подтверждения)
        /// </summary>
        [HttpGet]
        public IActionResult Del(int ID)
        {
            if (ID <= 0)
                return RedirectToAction("Index", "Error");

            CDish_Ingridient_Category? category = _bl_IngridientCategory.GetIngridient(ID);

            if (category == null)
                return RedirectToAction("Index", "Error");

            // Используем ту же модель, что и для ингредиентов
            Model_Ingridients_AddEdit data = new()
            {
                ID = category.ID,
                Name = category.Name
            };

            return View(data);
        }

        /// <summary>
        /// Удаление - POST (фактическое удаление)
        /// </summary>
        [HttpPost]
        public IActionResult Del(Model_Ingridients_AddEdit Data)
        {
            if (Data == null || Data.ID <= 0)
                return RedirectToAction("Index", "Error");

            int result = _bl_IngridientCategory.Delete(Data.ID);

            if (result > 0)
            {
                TempData["Success"] = "Категория успешно удалена.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Ошибка при удалении категории.");
                return View(Data);
            }
        }


        /// <summary>
        /// Восстановление категории
        /// </summary>
        /// <param name="ID">ID категории</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Restore(int ID)
        {
            if (ID <= 0)
                return RedirectToAction("Index", "Error");

            int result = _bl_IngridientCategory.Restore(ID);
            if (result > 0)
            {
                TempData["Success"] = "Категория восстановлена.";
            }
            else
            {
                TempData["Error"] = "Ошибка при восстановлении.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
