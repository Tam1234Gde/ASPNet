using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Common.ENums;
using HIDE.RestAndRant.Web_Admin_ForMenu.Helpers;
using HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    /// <summary>
    /// Контроллер для управления меню, категориями, подкатегориями, блюдами, ингредиентами и привязками к ресторанам.
    /// </summary>
    public class MenuController : Controller
    {
        #region Поля и конструктор

        private readonly ILogger _log;
        private readonly Business.Endpoints_ForMenu.Menu _bl_Menu;
        private readonly Business.Endpoints_ForMenu.Menu_Category _bl_Menu_Category;
        private readonly Business.Endpoints_ForMenu.Menu_Subcategories _bl_Menu_Subcategory;
        private readonly Business.Endpoints_ForMenu.Menu_Dish _bl_Menu_Dish;
        private readonly Business.Endpoints_ForMenu.Menu_Ingridient _bl_Menu_Ingredient;
        private readonly Business.Endpoints_ForMenu.Menu_Ingridient_Group _bl_Menu_Ingredient_Group;
        private readonly Business.Endpoints_ForMenu.Menu_Restaurant _bl_Menu_Restaurant;
        private readonly Business.Endpoints_ForMenu.Menu_OptionState _bl_Menu_OptionState;
        private readonly Business.Endpoints_ForMenu.Menu_Dish_Ingridient _bl_Menu_Dish_Ingredient;
        private readonly Restaurants _bl_Restaurants;

        public MenuController()
        {
            _log = AppLog.CreateLogger(GetType());
            _bl_Menu = new Business.Endpoints_ForMenu.Menu();
            _bl_Menu_Category = new Business.Endpoints_ForMenu.Menu_Category();
            _bl_Menu_Subcategory = new Business.Endpoints_ForMenu.Menu_Subcategories();
            _bl_Menu_Dish = new Business.Endpoints_ForMenu.Menu_Dish();
            _bl_Menu_Ingredient = new Business.Endpoints_ForMenu.Menu_Ingridient();
            _bl_Menu_Ingredient_Group = new Business.Endpoints_ForMenu.Menu_Ingridient_Group();
            _bl_Menu_Restaurant = new Business.Endpoints_ForMenu.Menu_Restaurant();
            _bl_Menu_OptionState = new Business.Endpoints_ForMenu.Menu_OptionState();
            _bl_Menu_Dish_Ingredient = new Business.Endpoints_ForMenu.Menu_Dish_Ingridient();
            _bl_Restaurants = new Restaurants();
        }

        #endregion

        #region Вспомогательные классы

        public class CategoryCopyItem { public int Id { get; set; } public bool WithContent { get; set; } }
        public class SubcategoryCopyItem { public int Id { get; set; } public bool WithContent { get; set; } }
        public class MenuCopyItem { public int Id { get; set; } public bool WithContent { get; set; } }
        public class GroupCopyItem { public int Id { get; set; } public bool WithContent { get; set; } }
        public class IngredientCopyItem { public int Id { get; set; } public bool WithContent { get; set; } }
        public class RestaurantCopyItem { public int Id { get; set; } public bool WithContent { get; set; } }

        public class UndoInfo
        {
            public string EntityType { get; set; } = "";
            public int EntityId { get; set; }
            public string EntityName { get; set; } = "";
        }

        #endregion

        #region Menu - Основное меню

        public IActionResult Index()
        {
            Model_Menu_List model = new Model_Menu_List { List_Menu = _bl_Menu.GetList() };
            return View(model);
        }

        public IActionResult Add()
        {
            Model_Menu_AddEdit model = new Model_Menu_AddEdit { List_Menu_OptionStates = _bl_Menu_OptionState.GetList() };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Model_Menu_AddEdit model)
        {
            if (ModelState.IsValid)
            {
                int result = _bl_Menu.Add(Name: model.Name, Menu_OptionStateID: model.Menu_OptionStateID);
                if (result > 0) return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Ошибка при добавлении меню");
            }
            model.List_Menu_OptionStates = _bl_Menu_OptionState.GetList();
            return View(model);
        }

        public IActionResult Edit(int ID)
        {
            CMenu menu = _bl_Menu.Get(ID);
            if (menu == null) return NotFound();
            Model_Menu_AddEdit model = new Model_Menu_AddEdit
            {
                ID = menu.ID,
                Name = menu.Name,
                Menu_OptionStateID = menu.Menu_OptionStateID,
                List_Menu_OptionStates = _bl_Menu_OptionState.GetList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Model_Menu_AddEdit model)
        {
            if (ModelState.IsValid)
            {
                int result = _bl_Menu.Edit(ID: model.ID, Name: model.Name, Menu_OptionStateID: model.Menu_OptionStateID);
                if (result > 0) return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Ошибка при редактировании меню");
            }
            model.List_Menu_OptionStates = _bl_Menu_OptionState.GetList();
            return View(model);
        }

        public IActionResult Del(int ID)
        {
            CMenu menu = _bl_Menu.Get(ID);
            if (menu == null) return NotFound();
            Model_Menu_AddEdit model = new Model_Menu_AddEdit { ID = menu.ID, Name = menu.Name };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Del(Model_Menu_AddEdit model)
        {
            int result = _bl_Menu.Delete(model.ID);
            if (result > 0)
            {
                TempData["UndoInfo"] = System.Text.Json.JsonSerializer.Serialize(new UndoInfo { EntityType = "Menu", EntityId = model.ID, EntityName = model.Name });
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Ошибка при удалении меню");
            return View(model);
        }

        [HttpPost]
        public IActionResult RestoreMenu(int ID)
        {
            int result = _bl_Menu.Restore(ID);
            if (result > 0) return Json(new { success = true, message = "Меню восстановлено" });
            return Json(new { success = false, message = "Ошибка восстановления меню" });
        }

        #endregion

        #region Menu_Category

        public IActionResult CategoryList(int MenuID, bool showDeleted = false)
        {
            CMenu menu = _bl_Menu.Get(MenuID);
            if (menu == null) return NotFound();

            Model_Menu_Category_List model = new Model_Menu_Category_List { MenuID = menu.ID, MenuName = menu.Name };
            model.List_Menu_Categories = showDeleted
                ? _bl_Menu_Category.GetList_Deleted_ByMenuID(MenuID)
                : _bl_Menu_Category.GetList_ByMenuID(MenuID);

            ViewBag.ShowDeleted = showDeleted;
            ViewBag.MenuID = MenuID;
            return View(model);
        }

        public IActionResult CategoryAdd(int MenuID)
        {
            CMenu menu = _bl_Menu.Get(MenuID);
            if (menu == null) return NotFound();
            Model_Menu_Category_AddEdit model = new Model_Menu_Category_AddEdit { MenuID = menu.ID, MenuName = menu.Name };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CategoryAdd(Model_Menu_Category_AddEdit model)
        {
            ModelState.Remove("MenuName");
            if (ModelState.IsValid)
            {
                int result = _bl_Menu_Category.Add(Name: model.Name, MenuID: model.MenuID, OrderNum: model.OrderNum);
                if (result > 0) return RedirectToAction(nameof(CategoryList), new { MenuID = model.MenuID });
                ModelState.AddModelError("", "Ошибка при добавлении категории");
            }
            return View(model);
        }

        public IActionResult CategoryEdit(int ID)
        {
            CMenu_Category category = _bl_Menu_Category.Get(ID);
            if (category == null) return NotFound();
            CMenu menu = _bl_Menu.Get(category.MenuID);
            Model_Menu_Category_AddEdit model = new Model_Menu_Category_AddEdit
            {
                ID = category.ID,
                MenuID = category.MenuID,
                MenuName = menu?.Name ?? "",
                Name = category.Name,
                OrderNum = category.OrderNum
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CategoryEdit(Model_Menu_Category_AddEdit model)
        {
            ModelState.Remove("MenuName");
            if (ModelState.IsValid)
            {
                int result = _bl_Menu_Category.Edit(ID: model.ID, Name: model.Name, MenuID: model.MenuID, OrderNum: model.OrderNum);
                if (result > 0) return RedirectToAction(nameof(CategoryList), new { MenuID = model.MenuID });
                ModelState.AddModelError("", "Ошибка при редактировании категории");
            }
            return View(model);
        }

        public IActionResult CategoryDel(int ID)
        {
            CMenu_Category category = _bl_Menu_Category.Get(ID);
            if (category == null) return NotFound();
            Model_Menu_Category_AddEdit model = new Model_Menu_Category_AddEdit
            {
                ID = category.ID,
                Name = category.Name,
                MenuID = category.MenuID
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CategoryDel(Model_Menu_Category_AddEdit model)
        {
            int result = _bl_Menu_Category.Delete(model.ID);
            if (result > 0)
            {
                TempData["UndoInfo"] = System.Text.Json.JsonSerializer.Serialize(new UndoInfo { EntityType = "Category", EntityId = model.ID, EntityName = model.Name });
                return RedirectToAction(nameof(CategoryList), new { MenuID = model.MenuID });
            }
            ModelState.AddModelError("", "Ошибка при удалении категории");
            return View(model);
        }

        [HttpPost]
        public IActionResult RestoreCategory(int ID)
        {
            int result = _bl_Menu_Category.Restore(ID);
            if (result > 0) return Json(new { success = true, message = "Категория восстановлена" });
            return Json(new { success = false, message = "Ошибка восстановления категории" });
        }


        /// <summary>
        /// Массовая перестановка порядка категорий.
        /// Принимает JSON: { ids: [5, 3, 8, 1, ...] }
        /// </summary>
        [HttpPost]
        public IActionResult ReorderCategories([FromBody] ReorderRequest request)
        {
            if (request?.Ids == null || request.Ids.Count == 0)
            {
                return Json(new { success = false, message = "Пустой список ID" });
            }

            int result = _bl_Menu_Category.Reorder(request.Ids);

            if (result >= 0)
            {
                return Json(new { success = true, updated = result });
            }
            return Json(new { success = false, message = "Ошибка сохранения порядка" });
        }

        /// <summary>
        /// Вспомогательный класс для приёма массива ID.
        /// </summary>
        public class ReorderRequest
        {
            public List<int> Ids { get; set; } = new List<int>();
        }

        #endregion

        #region Menu_Subcategory

        public IActionResult SubcategoryList(int Menu_CategoryID, bool showDeleted = false)
        {
            CMenu_Category category = _bl_Menu_Category.Get(Menu_CategoryID);
            if (category == null) return NotFound();

            Model_Menu_Subcategory_List model = new Model_Menu_Subcategory_List
            {
                Menu_CategoryID = category.ID,
                CategoryName = category.Name
            };
            model.List_Menu_Subcategories = showDeleted
                ? _bl_Menu_Subcategory.GetList_Deleted_ByMenu_CategoryID(Menu_CategoryID)
                : _bl_Menu_Subcategory.GetList_ByMenu_CategoryID(Menu_CategoryID);

            ViewBag.ShowDeleted = showDeleted;
            ViewBag.Menu_CategoryID = Menu_CategoryID;
            return View(model);
        }

        public IActionResult SubcategoryAdd(int Menu_CategoryID)
        {
            CMenu_Category category = _bl_Menu_Category.Get(Menu_CategoryID);
            if (category == null) return NotFound();
            Model_Menu_Subcategory_AddEdit model = new Model_Menu_Subcategory_AddEdit
            {
                Menu_CategoryID = category.ID,
                CategoryName = category.Name,
                List_Parents = _bl_Menu_Subcategory.GetList_ByMenu_CategoryID(Menu_CategoryID)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubcategoryAdd(Model_Menu_Subcategory_AddEdit model)
        {
            ModelState.Remove("CategoryName");
            if (ModelState.IsValid)
            {
                int result = _bl_Menu_Subcategory.Add(
                    Name: model.Name,
                    Menu_CategoryID: model.Menu_CategoryID,
                    ParentID: model.ParentID,
                    OrderNum: model.OrderNum);
                if (result > 0) return RedirectToAction(nameof(SubcategoryList), new { Menu_CategoryID = model.Menu_CategoryID });
                ModelState.AddModelError("", "Ошибка при добавлении подкатегории");
            }
            model.List_Parents = _bl_Menu_Subcategory.GetList_ByMenu_CategoryID(model.Menu_CategoryID);
            return View(model);
        }

        public IActionResult SubcategoryEdit(int ID)
        {
            CMenu_Subcategory subcategory = _bl_Menu_Subcategory.Get(ID);
            if (subcategory == null) return NotFound();
            CMenu_Category category = _bl_Menu_Category.Get(subcategory.Menu_CategoryID);
            Model_Menu_Subcategory_AddEdit model = new Model_Menu_Subcategory_AddEdit
            {
                ID = subcategory.ID,
                Menu_CategoryID = subcategory.Menu_CategoryID,
                CategoryName = category?.Name ?? "",
                Name = subcategory.Name,
                ParentID = subcategory.ParentID,
                OrderNum = subcategory.OrderNum,
                List_Parents = _bl_Menu_Subcategory.GetList_ByMenu_CategoryID(subcategory.Menu_CategoryID)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubcategoryEdit(Model_Menu_Subcategory_AddEdit model)
        {
            ModelState.Remove("CategoryName");
            if (ModelState.IsValid)
            {
                int result = _bl_Menu_Subcategory.Edit(
                    ID: model.ID,
                    Name: model.Name,
                    Menu_CategoryID: model.Menu_CategoryID,
                    ParentID: model.ParentID,
                    OrderNum: model.OrderNum);
                if (result > 0) return RedirectToAction(nameof(SubcategoryList), new { Menu_CategoryID = model.Menu_CategoryID });
                ModelState.AddModelError("", "Ошибка при редактировании подкатегории");
            }
            model.List_Parents = _bl_Menu_Subcategory.GetList_ByMenu_CategoryID(model.Menu_CategoryID);
            return View(model);
        }

        public IActionResult SubcategoryDel(int ID)
        {
            CMenu_Subcategory subcategory = _bl_Menu_Subcategory.Get(ID);
            if (subcategory == null) return NotFound();
            Model_Menu_Subcategory_AddEdit model = new Model_Menu_Subcategory_AddEdit
            {
                ID = subcategory.ID,
                Name = subcategory.Name,
                Menu_CategoryID = subcategory.Menu_CategoryID
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubcategoryDel(Model_Menu_Subcategory_AddEdit model)
        {
            int result = _bl_Menu_Subcategory.Delete(model.ID);
            if (result > 0)
            {
                TempData["UndoInfo"] = System.Text.Json.JsonSerializer.Serialize(new UndoInfo { EntityType = "Subcategory", EntityId = model.ID, EntityName = model.Name });
                return RedirectToAction(nameof(SubcategoryList), new { Menu_CategoryID = model.Menu_CategoryID });
            }
            ModelState.AddModelError("", "Ошибка при удалении подкатегории");
            return View(model);
        }

        [HttpPost]
        public IActionResult RestoreSubcategory(int ID)
        {
            int result = _bl_Menu_Subcategory.Restore(ID);
            if (result > 0) return Json(new { success = true, message = "Подкатегория восстановлена" });
            return Json(new { success = false, message = "Ошибка восстановления подкатегории" });
        }


        /// <summary>
        /// Массовая перестановка порядка подкатегорий.
        /// </summary>
        [HttpPost]
        public IActionResult ReorderSubcategories([FromBody] ReorderRequest request)
        {
            if (request?.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "Пустой список ID" });

            int result = _bl_Menu_Subcategory.Reorder(request.Ids);

            if (result >= 0)
                return Json(new { success = true, updated = result });

            return Json(new { success = false, message = "Ошибка сохранения порядка" });
        }

        #endregion

        #region Menu_Dish (без изменений — Dish имеет OrderNum давно)

        public IActionResult DishList(int Menu_SubcategoryID, bool showDeleted = false)
        {
            CMenu_Subcategory subcategory = _bl_Menu_Subcategory.Get(Menu_SubcategoryID);
            if (subcategory == null) return NotFound();

            Model_Menu_Dish_List model = new Model_Menu_Dish_List
            {
                Menu_SubcategoryID = subcategory.ID,
                SubcategoryName = subcategory.Name
            };
            model.List_Menu_Dishes = showDeleted
                ? _bl_Menu_Dish.GetList_Deleted_ByMenu_SubcategoryID(Menu_SubcategoryID)
                : _bl_Menu_Dish.GetList_ByMenu_SubcategoryID(Menu_SubcategoryID);

            ViewBag.ShowDeleted = showDeleted;
            ViewBag.Menu_SubcategoryID = Menu_SubcategoryID;
            ViewBag.Menu_CategoryID = subcategory.Menu_CategoryID;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DishDel(Model_Menu_Dish_AddEdit model)
        {
            int result = _bl_Menu_Dish.Delete(model.ID);
            if (result > 0)
            {
                TempData["UndoInfo"] = System.Text.Json.JsonSerializer.Serialize(new UndoInfo { EntityType = "Dish", EntityId = model.ID, EntityName = model.Name });
                bool showDeleted = ViewBag.ShowDeleted ?? false;
                return RedirectToAction(nameof(DishList), new { Menu_SubcategoryID = model.Menu_SubcategoryID, showDeleted });
            }
            ModelState.AddModelError("", "Ошибка при удалении блюда");
            return View(model);
        }

        [HttpPost]
        public IActionResult RestoreDish(int ID)
        {
            int result = _bl_Menu_Dish.Restore(ID);
            if (result > 0) return Json(new { success = true, message = "Блюдо восстановлено" });
            return Json(new { success = false, message = "Ошибка восстановления блюда" });
        }

        public IActionResult DishAdd(int Menu_SubcategoryID)
        {
            CMenu_Subcategory subcategory = _bl_Menu_Subcategory.Get(Menu_SubcategoryID);
            if (subcategory == null) return NotFound();

            var ingredientGroups = BuildIngredientGroups(null);

            Model_Menu_Dish_AddEdit model = new Model_Menu_Dish_AddEdit
            {
                Menu_SubcategoryID = subcategory.ID,
                SubcategoryName = subcategory.Name,
                OrderNum = 0,
                List_Menu_OptionStates = _bl_Menu_OptionState.GetList(),
                IngredientGroups = ingredientGroups,
                SelectedIngredientIDs = new List<int>()
            };
            ViewBag.MenuID = subcategory.Menu_CategoryID;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DishAdd(Model_Menu_Dish_AddEdit model)
        {
            ModelState.Remove("SubcategoryName");
            ModelState.Remove("List_Menu_OptionStates");
            ModelState.Remove("List_Ingredients");
            ModelState.Remove("IngredientGroups");
            ModelState.Remove("Image");
            ModelState.Remove("ID");
            ModelState.Remove("SelectedIngredientIDs");
            ModelState.Remove("ImageBase64");

            if (ModelState.IsValid)
            {
                byte[] imageBytes = null;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(ms);
                        imageBytes = ms.ToArray();
                    }
                }
                else if (!string.IsNullOrEmpty(model.ImageBase64))
                {
                    try
                    {
                        imageBytes = Convert.FromBase64String(model.ImageBase64);
                    }
                    catch
                    {
                        // невалидная base64 — игнорируем, картинка не будет сохранена
                        imageBytes = null;
                    }
                }

                int result = _bl_Menu_Dish.Add(
                    Menu_SubcategoryID: model.Menu_SubcategoryID,
                    Name: model.Name, Teaser: model.Teaser, Text: model.Text,
                    Cooking_Minutes: model.Cooking_Minutes, Price: model.Price,
                    Flag_AllowColors: model.Flag_AllowColors, Flag_Bonus: model.Flag_Bonus,
                    Bonuses: model.Bonuses, OrderNum: model.OrderNum, FlagNew: model.FlagNew,
                    Image: imageBytes, Image_ChangeDate: DateTime.Now,
                    RecommendDishIDs: model.RecommendDishIDs, FlagNoActive: model.FlagNoActive,
                    New_DateFrom: model.New_DateFrom,
                    Vol1: model.Vol1, Vol1_Price: model.Vol1_Price,
                    Vol2: model.Vol2, Vol2_Price: model.Vol2_Price,
                    Vol3: model.Vol3, Vol3_Price: model.Vol3_Price,
                    Vol4: model.Vol4, Vol4_Price: model.Vol4_Price,
                    Vol5: model.Vol5, Vol5_Price: model.Vol5_Price,
                    Menu_OptionStateID: model.Menu_OptionStateID);

                if (result > 0)
                {
                    string selectedIdsString = Request.Form["SelectedIngredientIDs_Hidden"];
                    if (!string.IsNullOrEmpty(selectedIdsString))
                    {
                        foreach (int ingredientId in selectedIdsString.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse))
                        {
                            if (ingredientId > 0) _bl_Menu_Dish_Ingredient.Add(result, ingredientId);
                        }
                    }
                    return RedirectToAction(nameof(DishList), new { Menu_SubcategoryID = model.Menu_SubcategoryID });
                }
                ModelState.AddModelError("", "Ошибка при добавлении блюда");
            }

            model.List_Menu_OptionStates = _bl_Menu_OptionState.GetList();
            CMenu_Subcategory subcategory = _bl_Menu_Subcategory.Get(model.Menu_SubcategoryID);
            if (subcategory != null)
            {
                model.IngredientGroups = BuildIngredientGroups(null);
                model.SubcategoryName = subcategory.Name;
            }
            return View(model);
        }

        public IActionResult DishEdit(int ID)
        {
            CMenu_Dish dish = _bl_Menu_Dish.Get(ID);
            if (dish == null) return NotFound();

            CMenu_Subcategory subcategory = _bl_Menu_Subcategory.Get(dish.Menu_SubcategoryID);
            List<CMenu_Dish_Ingridient> dishIngredients = _bl_Menu_Dish_Ingredient.GetList_ByMenu_DishID(ID);
            HashSet<int> selectedIds = new HashSet<int>(dishIngredients.Select(x => x.Menu_IngridientID));

            Model_Menu_Dish_AddEdit model = new Model_Menu_Dish_AddEdit
            {
                ID = dish.ID,
                Menu_SubcategoryID = dish.Menu_SubcategoryID,
                SubcategoryName = subcategory?.Name ?? "",
                Name = dish.Name,
                Teaser = dish.Teaser,
                Text = dish.Text,
                Cooking_Minutes = dish.Cooking_Minutes,
                Price = dish.Price,
                Flag_AllowColors = dish.Flag_AllowColors,
                Flag_Bonus = dish.Flag_Bonus,
                Bonuses = dish.Bonuses,
                OrderNum = dish.OrderNum,
                FlagNew = dish.FlagNew,
                Image = dish.Image,
                RecommendDishIDs = dish.RecommendDishIDs,
                FlagNoActive = dish.FlagNoActive,
                New_DateFrom = dish.New_DateFrom,
                Vol1 = dish.Vol1,
                Vol1_Price = dish.Vol1_Price,
                Vol2 = dish.Vol2,
                Vol2_Price = dish.Vol2_Price,
                Vol3 = dish.Vol3,
                Vol3_Price = dish.Vol3_Price,
                Vol4 = dish.Vol4,
                Vol4_Price = dish.Vol4_Price,
                Vol5 = dish.Vol5,
                Vol5_Price = dish.Vol5_Price,
                Menu_OptionStateID = dish.Menu_OptionStateID,
                List_Menu_OptionStates = _bl_Menu_OptionState.GetList(),
                List_Ingredients = dishIngredients,
                IngredientGroups = BuildIngredientGroups(selectedIds),
                SelectedIngredientIDs = selectedIds.ToList()
            };
            ViewBag.MenuID = subcategory?.Menu_CategoryID ?? 0;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DishEdit(Model_Menu_Dish_AddEdit model)
        {
            ModelState.Remove("SubcategoryName");
            ModelState.Remove("List_Menu_OptionStates");
            ModelState.Remove("List_Ingredients");
            ModelState.Remove("IngredientGroups");
            ModelState.Remove("Image");
            ModelState.Remove("SelectedIngredientIDs");

            if (ModelState.IsValid)
            {
                byte[] imageBytes = model.Image;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(ms);
                        imageBytes = ms.ToArray();
                    }
                }

                int result = _bl_Menu_Dish.Edit(
                    ID: model.ID, Menu_SubcategoryID: model.Menu_SubcategoryID,
                    Name: model.Name, Teaser: model.Teaser, Text: model.Text,
                    Cooking_Minutes: model.Cooking_Minutes, Price: model.Price,
                    Flag_AllowColors: model.Flag_AllowColors, Flag_Bonus: model.Flag_Bonus,
                    Bonuses: model.Bonuses, OrderNum: model.OrderNum, FlagNew: model.FlagNew,
                    Image: imageBytes, Image_ChangeDate: DateTime.Now,
                    RecommendDishIDs: model.RecommendDishIDs, FlagNoActive: model.FlagNoActive,
                    New_DateFrom: model.New_DateFrom,
                    Vol1: model.Vol1, Vol1_Price: model.Vol1_Price,
                    Vol2: model.Vol2, Vol2_Price: model.Vol2_Price,
                    Vol3: model.Vol3, Vol3_Price: model.Vol3_Price,
                    Vol4: model.Vol4, Vol4_Price: model.Vol4_Price,
                    Vol5: model.Vol5, Vol5_Price: model.Vol5_Price,
                    Menu_OptionStateID: model.Menu_OptionStateID);

                if (result > 0)
                {
                    _bl_Menu_Dish_Ingredient.Delete_ByMenu_DishID(model.ID);
                    string selectedIdsString = Request.Form["SelectedIngredientIDs_Hidden"];
                    if (!string.IsNullOrEmpty(selectedIdsString))
                    {
                        foreach (int ingredientId in selectedIdsString.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse))
                        {
                            if (ingredientId > 0) _bl_Menu_Dish_Ingredient.Add(model.ID, ingredientId);
                        }
                    }
                    return RedirectToAction(nameof(DishList), new { Menu_SubcategoryID = model.Menu_SubcategoryID });
                }
                ModelState.AddModelError("", "Ошибка при редактировании блюда");
            }

            model.List_Menu_OptionStates = _bl_Menu_OptionState.GetList();
            CMenu_Subcategory subcategory = _bl_Menu_Subcategory.Get(model.Menu_SubcategoryID);
            if (subcategory != null)
            {
                List<CMenu_Dish_Ingridient> dishIngredients = _bl_Menu_Dish_Ingredient.GetList_ByMenu_DishID(model.ID);
                HashSet<int> selectedIds = new HashSet<int>(dishIngredients.Select(x => x.Menu_IngridientID));
                model.IngredientGroups = BuildIngredientGroups(selectedIds);
                model.SubcategoryName = subcategory.Name;
            }
            return View(model);
        }

        public IActionResult DishDel(int ID)
        {
            CMenu_Dish dish = _bl_Menu_Dish.Get(ID);
            if (dish == null) return NotFound();
            Model_Menu_Dish_AddEdit model = new Model_Menu_Dish_AddEdit
            {
                ID = dish.ID,
                Name = dish.Name,
                Menu_SubcategoryID = dish.Menu_SubcategoryID
            };
            return View(model);
        }

        /// <summary>
        /// Построение групп ингредиентов для форм блюд.
        /// </summary>
        private List<IngredientGroupViewModel> BuildIngredientGroups(HashSet<int>? selectedIds)
        {
            List<CMenu_Ingridient> allIngredients = _bl_Menu_Ingredient.GetList();
            List<CMenu_Ingridient_Group> allGroups = _bl_Menu_Ingredient_Group.GetList();
            List<IngredientGroupViewModel> result = new List<IngredientGroupViewModel>();

            foreach (CMenu_Ingridient_Group group in allGroups)
            {
                IngredientGroupViewModel groupModel = new IngredientGroupViewModel
                {
                    GroupId = group.ID,
                    GroupName = group.Name,
                    IsSelectable = group.IsSelectable,
                    Ingredients = new List<IngredientItemViewModel>()
                };

                foreach (CMenu_Ingridient ingredient in allIngredients.Where(x => x.Menu_Ingridient_GroupID == group.ID))
                {
                    groupModel.Ingredients.Add(new IngredientItemViewModel
                    {
                        Id = ingredient.ID,
                        Name = ingredient.Name,
                        Price = ingredient.Price,
                        IsChecked = selectedIds != null && selectedIds.Contains(ingredient.ID)
                    });
                }

                if (groupModel.Ingredients.Count > 0) result.Add(groupModel);
            }
            return result;
        }

        #endregion

        #region Menu_Ingredient_Group

        public IActionResult AllIngredientGroups()
        {
            Model_AllIngredientGroups model = new Model_AllIngredientGroups { List_Groups = _bl_Menu_Ingredient_Group.GetList() };
            return View(model);
        }

        public IActionResult IngredientGroupAdd()
        {
            Model_Menu_Ingredient_Group_AddEdit model = new Model_Menu_Ingredient_Group_AddEdit
            {
                IsSelectable = false,
                List_Menu_OptionStates = _bl_Menu_OptionState.GetList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IngredientGroupAdd(Model_Menu_Ingredient_Group_AddEdit model)
        {
            if (ModelState.IsValid)
            {
                int result = _bl_Menu_Ingredient_Group.Add(
                    Name: model.Name,
                    IsSelectable: model.IsSelectable,
                    Menu_OptionStateID: model.Menu_OptionStateID,
                    Default_Price: model.Default_Price,
                    OrderNum: model.OrderNum);
                if (result > 0) return RedirectToAction(nameof(AllIngredientGroups));
                ModelState.AddModelError("", "Ошибка при добавлении группы");
            }
            model.List_Menu_OptionStates = _bl_Menu_OptionState.GetList();
            return View(model);
        }

        public IActionResult IngredientGroupEdit(int ID)
        {
            CMenu_Ingridient_Group group = _bl_Menu_Ingredient_Group.Get(ID);
            if (group == null) return NotFound();
            Model_Menu_Ingredient_Group_AddEdit model = new Model_Menu_Ingredient_Group_AddEdit
            {
                ID = group.ID,
                Name = group.Name,
                IsSelectable = group.IsSelectable,
                Default_Price = group.Default_Price,
                OrderNum = group.OrderNum,
                Menu_OptionStateID = group.Menu_OptionStateID,
                List_Menu_OptionStates = _bl_Menu_OptionState.GetList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IngredientGroupEdit(Model_Menu_Ingredient_Group_AddEdit model)
        {
            if (ModelState.IsValid)
            {
                int result = _bl_Menu_Ingredient_Group.Edit(
                    ID: model.ID,
                    Name: model.Name,
                    IsSelectable: model.IsSelectable,
                    Menu_OptionStateID: model.Menu_OptionStateID,
                    Default_Price: model.Default_Price,
                    OrderNum: model.OrderNum);
                if (result > 0) return RedirectToAction(nameof(AllIngredientGroups));
                ModelState.AddModelError("", "Ошибка при редактировании группы");
            }
            model.List_Menu_OptionStates = _bl_Menu_OptionState.GetList();
            return View(model);
        }

        public IActionResult IngredientGroupDel(int ID)
        {
            CMenu_Ingridient_Group group = _bl_Menu_Ingredient_Group.Get(ID);
            if (group == null) return NotFound();

            // ингредиенты текущей группы
            List<CMenu_Ingridient> ingredients = _bl_Menu_Ingredient
                .GetList_ByMenu_Ingridient_GroupID(ID);

            // все остальные группы — как цели миграции
            List<CMenu_Ingridient_Group> targetGroups = _bl_Menu_Ingredient_Group
                .GetList()
                .Where(g => g.ID != ID)
                .ToList();

            Model_Menu_Ingredient_Group_Del model = new Model_Menu_Ingredient_Group_Del
            {
                ID = group.ID,
                Name = group.Name,
                Ingredients = ingredients,
                Target_Groups = targetGroups,
                Decisions = ingredients
                    .Select(i => new IngredientDeleteDecision
                    {
                        IngredientID = i.ID,
                        Action = "delete",
                        TargetGroupID = null
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IngredientGroupDel(Model_Menu_Ingredient_Group_Del model)
        {
            if (model == null || model.ID <= 0)
                return NotFound();

            // Шаг 1. Мигрируем выбранные ингредиенты
            if (model.Decisions != null)
            {
                foreach (IngredientDeleteDecision decision in model.Decisions)
                {
                    if (decision.Action == "migrate"
                        && decision.TargetGroupID.HasValue
                        && decision.TargetGroupID.Value > 0
                        && decision.TargetGroupID.Value != model.ID)
                    {
                        CMenu_Ingridient ing = _bl_Menu_Ingredient.Get(decision.IngredientID);
                        if (ing != null)
                        {
                            _bl_Menu_Ingredient.Edit(
                                ID: ing.ID,
                                Name: ing.Name,
                                Image: ing.Image,
                                Price: ing.Price,
                                Menu_Ingridient_GroupID: decision.TargetGroupID.Value,
                                Menu_OptionStateID: ing.Menu_OptionStateID,
                                OrderNum: ing.OrderNum);
                        }
                    }
                    // Action == "delete" — оставляем, ингредиент удалится каскадом вместе с группой
                }
            }

            // Шаг 2. Удаляем саму группу. Оставшиеся ингредиенты уходят каскадом.
            int result = _bl_Menu_Ingredient_Group.Delete(model.ID, MigrateToGroupID: null);

            if (result > 0)
            {
                TempData["Success"] = $"Группа «{model.Name}» удалена";
                return RedirectToAction(nameof(AllIngredientGroups));
            }

            ModelState.AddModelError("", "Ошибка при удалении группы");

            // При ошибке — перезаполняем модель
            model.Ingredients = _bl_Menu_Ingredient.GetList_ByMenu_Ingridient_GroupID(model.ID);
            model.Target_Groups = _bl_Menu_Ingredient_Group
                .GetList()
                .Where(g => g.ID != model.ID)
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IngredientGroupApplyPrice(int GroupID, int? Price, bool SkipOverridden, string? ReturnUrl)
        {
            if (GroupID <= 0)
                return RedirectToError(Errors.Err200, HttpContext.Request.Path.ToString());

            int result = _bl_Menu_Ingredient.ApplyPriceToGroup(GroupID: GroupID, Price: Price, SkipOverridden: SkipOverridden);

            if (result >= 0)
            {
                TempData["Success"] = $"Обновлено ингредиентов: {result}";
                if (!string.IsNullOrWhiteSpace(ReturnUrl)) return Redirect(ReturnUrl);
                return RedirectToAction(nameof(AllIngredients), new { Menu_Ingridient_GroupID = GroupID });
            }
            return RedirectToError(Errors.Err203, HttpContext.Request.Path.ToString());
        }


        /// <summary>
        /// Массовая перестановка порядка групп ингредиентов.
        /// </summary>
        [HttpPost]
        public IActionResult ReorderIngredientGroups([FromBody] ReorderRequest request)
        {
            if (request?.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "Пустой список ID" });

            int result = _bl_Menu_Ingredient_Group.Reorder(request.Ids);

            if (result >= 0)
                return Json(new { success = true, updated = result });

            return Json(new { success = false, message = "Ошибка сохранения порядка" });
        }

        #endregion

        #region Menu_Ingredient

        public IActionResult AllIngredients(int? Menu_Ingridient_GroupID)
        {
            List<CMenu_Ingridient_Group> allGroups = _bl_Menu_Ingredient_Group.GetList();

            Model_AllIngredients model = new Model_AllIngredients
            {
                Menu_Ingridient_GroupID = Menu_Ingridient_GroupID,
                List_Groups = allGroups
            };

            List<CMenu_Ingridient> allIngredients = new List<CMenu_Ingridient>();

            if (Menu_Ingridient_GroupID.HasValue && Menu_Ingridient_GroupID.Value > 0)
            {
                CMenu_Ingridient_Group group = _bl_Menu_Ingredient_Group.Get(Menu_Ingridient_GroupID.Value);
                model.GroupName = group?.Name;
                allIngredients = _bl_Menu_Ingredient.GetList_ByMenu_Ingridient_GroupID(Menu_Ingridient_GroupID.Value);
            }
            else
            {
                foreach (CMenu_Ingridient_Group group in allGroups)
                    allIngredients.AddRange(_bl_Menu_Ingredient.GetList_ByMenu_Ingridient_GroupID(group.ID));
            }

            model.List_Ingredients = allIngredients;
            return View(model);
        }

        public IActionResult IngredientAdd(int? Menu_Ingridient_GroupID)
        {
            Model_Menu_Ingredient_AddEdit model = new Model_Menu_Ingredient_AddEdit
            {
                List_Groups = _bl_Menu_Ingredient_Group.GetList(),
                List_Menu_OptionStates = _bl_Menu_OptionState.GetList()
            };

            if (Menu_Ingridient_GroupID.HasValue && Menu_Ingridient_GroupID.Value > 0)
            {
                model.Menu_Ingridient_GroupID = Menu_Ingridient_GroupID.Value;
                CMenu_Ingridient_Group? group = _bl_Menu_Ingredient_Group.Get(Menu_Ingridient_GroupID.Value);
                if (group != null && group.Default_Price.HasValue)
                    model.Price = group.Default_Price;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IngredientAdd(Model_Menu_Ingredient_AddEdit model)
        {
            if (ModelState.IsValid)
            {
                byte[] imageBytes = null;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(ms);
                        imageBytes = ms.ToArray();
                    }
                }

                int result = _bl_Menu_Ingredient.Add(
                    Name: model.Name,
                    Image: imageBytes,
                    Price: model.Price,
                    Menu_Ingridient_GroupID: model.Menu_Ingridient_GroupID,
                    Menu_OptionStateID: model.Menu_OptionStateID,
                    OrderNum: model.OrderNum);

                if (result > 0)
                    return RedirectToAction(nameof(AllIngredients), new { Menu_Ingridient_GroupID = model.Menu_Ingridient_GroupID });
                ModelState.AddModelError("", "Ошибка при добавлении ингредиента");
            }

            model.List_Groups = _bl_Menu_Ingredient_Group.GetList();
            model.List_Menu_OptionStates = _bl_Menu_OptionState.GetList();
            return View(model);
        }

        public IActionResult IngredientEdit(int ID)
        {
            CMenu_Ingridient ingredient = _bl_Menu_Ingredient.Get(ID);
            if (ingredient == null) return NotFound();
            Model_Menu_Ingredient_AddEdit model = new Model_Menu_Ingredient_AddEdit
            {
                ID = ingredient.ID,
                Name = ingredient.Name,
                Image = ingredient.Image,
                Price = ingredient.Price,
                Menu_Ingridient_GroupID = ingredient.Menu_Ingridient_GroupID,
                OrderNum = ingredient.OrderNum,
                Menu_OptionStateID = ingredient.Menu_OptionStateID,
                List_Groups = _bl_Menu_Ingredient_Group.GetList(),
                List_Menu_OptionStates = _bl_Menu_OptionState.GetList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IngredientEdit(Model_Menu_Ingredient_AddEdit model)
        {
            if (ModelState.IsValid)
            {
                byte[] imageBytes = model.Image;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(ms);
                        imageBytes = ms.ToArray();
                    }
                }

                int result = _bl_Menu_Ingredient.Edit(
                    ID: model.ID,
                    Name: model.Name,
                    Image: imageBytes,
                    Price: model.Price,
                    Menu_Ingridient_GroupID: model.Menu_Ingridient_GroupID,
                    Menu_OptionStateID: model.Menu_OptionStateID,
                    OrderNum: model.OrderNum);

                if (result > 0)
                    return RedirectToAction(nameof(AllIngredients), new { Menu_Ingridient_GroupID = model.Menu_Ingridient_GroupID });
                ModelState.AddModelError("", "Ошибка при редактировании ингредиента");
            }

            model.List_Groups = _bl_Menu_Ingredient_Group.GetList();
            model.List_Menu_OptionStates = _bl_Menu_OptionState.GetList();
            return View(model);
        }

        public IActionResult IngredientDel(int ID)
        {
            CMenu_Ingridient ingredient = _bl_Menu_Ingredient.Get(ID);
            if (ingredient == null) return NotFound();
            Model_Menu_Ingredient_AddEdit model = new Model_Menu_Ingredient_AddEdit
            {
                ID = ingredient.ID,
                Name = ingredient.Name
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IngredientDel(Model_Menu_Ingredient_AddEdit model)
        {
            int result = _bl_Menu_Ingredient.Delete(model.ID);
            if (result > 0)
            {
                TempData["UndoInfo"] = System.Text.Json.JsonSerializer.Serialize(new UndoInfo { EntityType = "Ingredient", EntityId = model.ID, EntityName = model.Name });
                return RedirectToAction(nameof(AllIngredients));
            }
            ModelState.AddModelError("", "Ошибка при удалении ингредиента");
            return View(model);
        }

        [HttpPost]
        public IActionResult RestoreIngredient(int ID)
        {
            int result = _bl_Menu_Ingredient.Restore(ID);
            if (result > 0) return Json(new { success = true, message = "Ингредиент восстановлен" });
            return Json(new { success = false, message = "Ошибка восстановления ингредиента" });
        }

        /// <summary>
        /// Массовая перестановка порядка ингредиентов.
        /// </summary>
        [HttpPost]
        public IActionResult ReorderIngredients([FromBody] ReorderRequest request)
        {
            if (request?.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "Пустой список ID" });

            int result = _bl_Menu_Ingredient.Reorder(request.Ids);

            if (result >= 0)
                return Json(new { success = true, updated = result });

            return Json(new { success = false, message = "Ошибка сохранения порядка" });
        }

        #endregion

        #region Menu_Restaurant

        public IActionResult RestaurantList(int MenuID, bool showDeleted = false)
        {
            CMenu menu = _bl_Menu.Get(MenuID);
            if (menu == null) return NotFound();
            Model_Menu_Restaurant_List model = new Model_Menu_Restaurant_List
            {
                MenuID = menu.ID,
                MenuName = menu.Name
            };
            model.List_Menu_Restaurants = showDeleted
                ? _bl_Menu_Restaurant.GetList_Deleted_ByMenuID(MenuID)
                : _bl_Menu_Restaurant.GetList_ByMenuID(MenuID);
            ViewBag.ShowDeleted = showDeleted;
            return View(model);
        }

        public IActionResult RestaurantAdd(int MenuID)
        {
            CMenu menu = _bl_Menu.Get(MenuID);
            if (menu == null) return NotFound();
            Model_Menu_Restaurant_AddEdit model = new Model_Menu_Restaurant_AddEdit
            {
                MenuID = menu.ID,
                MenuName = menu.Name,
                List_Restaurants = _bl_Restaurants.GetList(),
                List_Menu_OptionStates = _bl_Menu_OptionState.GetList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RestaurantAdd(Model_Menu_Restaurant_AddEdit model)
        {
            ModelState.Remove("MenuName");
            if (ModelState.IsValid)
            {
                int result = _bl_Menu_Restaurant.Add(
                    MenuID: model.MenuID, RestaurantID: model.RestaurantID,
                    DateFrom: model.DateFrom, DateTo: model.DateTo,
                    Menu_OptionStateID: model.Menu_OptionStateID);
                if (result > 0) return RedirectToAction(nameof(RestaurantList), new { MenuID = model.MenuID });
                ModelState.AddModelError("", "Ошибка при добавлении привязки");
            }
            model.List_Restaurants = _bl_Restaurants.GetList();
            model.List_Menu_OptionStates = _bl_Menu_OptionState.GetList();
            return View(model);
        }

        public IActionResult RestaurantEdit(int ID)
        {
            CMenu_Restaurant menuRestaurant = _bl_Menu_Restaurant.Get(ID);
            if (menuRestaurant == null) return NotFound();
            CMenu menu = _bl_Menu.Get(menuRestaurant.MenuID);
            Model_Menu_Restaurant_AddEdit model = new Model_Menu_Restaurant_AddEdit
            {
                ID = menuRestaurant.ID,
                MenuID = menuRestaurant.MenuID,
                MenuName = menu?.Name ?? "",
                RestaurantID = menuRestaurant.RestaurantID,
                DateFrom = menuRestaurant.DateFrom,
                DateTo = menuRestaurant.DateTo,
                Menu_OptionStateID = menuRestaurant.Menu_OptionStateID,
                List_Restaurants = _bl_Restaurants.GetList(),
                List_Menu_OptionStates = _bl_Menu_OptionState.GetList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RestaurantEdit(Model_Menu_Restaurant_AddEdit model)
        {
            ModelState.Remove("MenuName");
            if (ModelState.IsValid)
            {
                int result = _bl_Menu_Restaurant.Edit(
                    ID: model.ID, MenuID: model.MenuID, RestaurantID: model.RestaurantID,
                    DateFrom: model.DateFrom, DateTo: model.DateTo,
                    Menu_OptionStateID: model.Menu_OptionStateID);
                if (result > 0) return RedirectToAction(nameof(RestaurantList), new { MenuID = model.MenuID });
                ModelState.AddModelError("", "Ошибка при редактировании привязки");
            }
            model.List_Restaurants = _bl_Restaurants.GetList();
            model.List_Menu_OptionStates = _bl_Menu_OptionState.GetList();
            return View(model);
        }

        public IActionResult RestaurantDel(int ID)
        {
            CMenu_Restaurant menuRestaurant = _bl_Menu_Restaurant.Get(ID);
            if (menuRestaurant == null) return NotFound();
            Model_Menu_Restaurant_AddEdit model = new Model_Menu_Restaurant_AddEdit
            {
                ID = menuRestaurant.ID,
                MenuID = menuRestaurant.MenuID
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RestaurantDel(Model_Menu_Restaurant_AddEdit model)
        {
            int result = _bl_Menu_Restaurant.Delete(model.ID);
            if (result > 0)
            {
                TempData["UndoInfo"] = System.Text.Json.JsonSerializer.Serialize(new UndoInfo { EntityType = "Restaurant", EntityId = model.ID, EntityName = model.MenuName ?? "" });
                return RedirectToAction(nameof(RestaurantList), new { MenuID = model.MenuID });
            }
            ModelState.AddModelError("", "Ошибка при удалении привязки");
            return View(model);
        }

        [HttpPost]
        public IActionResult RestoreRestaurant(int ID)
        {
            int result = _bl_Menu_Restaurant.Restore(ID);
            if (result > 0) return Json(new { success = true, message = "Привязка восстановлена" });
            return Json(new { success = false, message = "Ошибка восстановления привязки" });
        }

        #endregion

        #region Массовые операции

        [HttpPost]
        public IActionResult BulkPasteCategories(string categoryIds, int targetMenuId)
        {
            try
            {
                if (string.IsNullOrEmpty(categoryIds) || targetMenuId <= 0)
                    return Json(new { success = false, message = "Неверные параметры" });

                List<int> ids = categoryIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s.Split(':')[0]))
                    .ToList();

                int copied = 0;
                List<string> errors = new List<string>();

                foreach (int sourceId in ids)
                {
                    try
                    {
                        CMenu_Category src = _bl_Menu_Category.Get(sourceId);
                        if (src == null) { errors.Add($"Категория {sourceId} не найдена"); continue; }

                        int newId = _bl_Menu_Category.Add(Name: src.Name, MenuID: targetMenuId, OrderNum: 0);
                        if (newId > 0) copied++;
                        else errors.Add($"Ошибка при копировании '{src.Name}'");
                    }
                    catch (Exception ex) { errors.Add(ex.Message); }
                }

                string msg = $"Скопировано {copied} из {ids.Count}";
                if (errors.Count > 0) msg += $". Ошибки: {string.Join("; ", errors.Take(3))}";
                return Json(new { success = true, message = msg, copiedCount = copied });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Внутренняя ошибка: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult BulkPasteSubcategories(string subcategoryIds, int targetCategoryId)
        {
            try
            {
                if (string.IsNullOrEmpty(subcategoryIds) || targetCategoryId <= 0)
                    return Json(new { success = false, message = "Неверные параметры" });

                List<int> ids = subcategoryIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s.Split(':')[0]))
                    .ToList();

                int copied = 0;
                List<string> errors = new List<string>();

                foreach (int sourceId in ids)
                {
                    try
                    {
                        CMenu_Subcategory src = _bl_Menu_Subcategory.Get(sourceId);
                        if (src == null) { errors.Add($"Подкатегория {sourceId} не найдена"); continue; }

                        int newId = _bl_Menu_Subcategory.Add(Name: src.Name, Menu_CategoryID: targetCategoryId, ParentID: null, OrderNum: 0);
                        if (newId > 0) copied++;
                        else errors.Add($"Ошибка при копировании '{src.Name}'");
                    }
                    catch (Exception ex) { errors.Add(ex.Message); }
                }

                string msg = $"Скопировано {copied} из {ids.Count}";
                if (errors.Count > 0) msg += $". Ошибки: {string.Join("; ", errors.Take(3))}";
                return Json(new { success = true, message = msg, copiedCount = copied });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Внутренняя ошибка: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult BulkPasteDishes(string dishIds, int targetSubcategoryId)
        {
            try
            {
                if (string.IsNullOrEmpty(dishIds) || targetSubcategoryId <= 0)
                    return Json(new { success = false, message = "Неверные параметры" });

                List<int> ids = dishIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s.Split(':')[0]))
                    .ToList();

                int copied = 0;
                List<string> errors = new List<string>();

                foreach (int sourceId in ids)
                {
                    try
                    {
                        CMenu_Dish src = _bl_Menu_Dish.Get(sourceId);
                        if (src == null) { errors.Add($"Блюдо {sourceId} не найдено"); continue; }

                        int newId = _bl_Menu_Dish.Add(
                            Menu_SubcategoryID: targetSubcategoryId,
                            Name: src.Name,
                            Teaser: src.Teaser,
                            Text: src.Text,
                            Cooking_Minutes: src.Cooking_Minutes,
                            Price: src.Price,
                            Flag_AllowColors: src.Flag_AllowColors,
                            Flag_Bonus: src.Flag_Bonus,
                            Bonuses: src.Bonuses,
                            OrderNum: 0,
                            FlagNew: src.FlagNew,
                            Image: src.Image,
                            Image_ChangeDate: DateTime.Now,
                            RecommendDishIDs: src.RecommendDishIDs,
                            FlagNoActive: src.FlagNoActive,
                            New_DateFrom: src.New_DateFrom,
                            Vol1: src.Vol1, Vol1_Price: src.Vol1_Price,
                            Vol2: src.Vol2, Vol2_Price: src.Vol2_Price,
                            Vol3: src.Vol3, Vol3_Price: src.Vol3_Price,
                            Vol4: src.Vol4, Vol4_Price: src.Vol4_Price,
                            Vol5: src.Vol5, Vol5_Price: src.Vol5_Price,
                            Menu_OptionStateID: src.Menu_OptionStateID);
                        if (newId > 0) copied++;
                        else errors.Add($"Ошибка при копировании '{src.Name}'");
                    }
                    catch (Exception ex) { errors.Add(ex.Message); }
                }

                string msg = $"Скопировано {copied} из {ids.Count}";
                if (errors.Count > 0) msg += $". Ошибки: {string.Join("; ", errors.Take(3))}";
                return Json(new { success = true, message = msg, copiedCount = copied });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Внутренняя ошибка: " + ex.Message });
            }
        }

        /// <summary>
        /// Массовое удаление категорий (soft-delete).
        /// </summary>
        [HttpPost]
        public IActionResult BulkDeleteCategories([FromBody] ReorderRequest request)
        {
            if (request?.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "Пустой список ID" });

            int deleted = 0;
            List<int> deletedIds = new List<int>();

            foreach (int id in request.Ids)
            {
                int result = _bl_Menu_Category.Delete(id);
                if (result > 0) { deleted++; deletedIds.Add(id); }
            }

            return Json(new { success = true, deleted = deleted, ids = deletedIds });
        }

        [HttpPost]
        public IActionResult BulkDeleteSubcategories([FromBody] ReorderRequest request)
        {
            if (request?.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "Пустой список ID" });

            int deleted = 0;
            List<int> deletedIds = new List<int>();

            foreach (int id in request.Ids)
            {
                int result = _bl_Menu_Subcategory.Delete(id);
                if (result > 0) { deleted++; deletedIds.Add(id); }
            }

            return Json(new { success = true, deleted = deleted, ids = deletedIds });
        }

        [HttpPost]
        public IActionResult BulkDeleteDishes([FromBody] ReorderRequest request)
        {
            if (request?.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "Пустой список ID" });

            int deleted = 0;
            List<int> deletedIds = new List<int>();

            foreach (int id in request.Ids)
            {
                int result = _bl_Menu_Dish.Delete(id);
                if (result > 0) { deleted++; deletedIds.Add(id); }
            }

            return Json(new { success = true, deleted = deleted, ids = deletedIds });
        }

        /// <summary>
        /// Массовое восстановление.
        /// </summary>
        [HttpPost]
        public IActionResult BulkRestoreCategories([FromBody] ReorderRequest request)
        {
            if (request?.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "Пустой список ID" });

            int restored = 0;
            foreach (int id in request.Ids)
            {
                int result = _bl_Menu_Category.Restore(id);
                if (result > 0) restored++;
            }
            return Json(new { success = true, restored = restored });
        }

        [HttpPost]
        public IActionResult BulkRestoreSubcategories([FromBody] ReorderRequest request)
        {
            if (request?.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "Пустой список ID" });

            int restored = 0;
            foreach (int id in request.Ids)
            {
                int result = _bl_Menu_Subcategory.Restore(id);
                if (result > 0) restored++;
            }
            return Json(new { success = true, restored = restored });
        }

        [HttpPost]
        public IActionResult BulkRestoreDishes([FromBody] ReorderRequest request)
        {
            if (request?.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "Пустой список ID" });

            int restored = 0;
            foreach (int id in request.Ids)
            {
                int result = _bl_Menu_Dish.Restore(id);
                if (result > 0) restored++;
            }
            return Json(new { success = true, restored = restored });
        }

        #endregion

        #region Все списки

        public IActionResult AllCategories(int? MenuID, bool showDeleted = false)
        {
            List<CMenu> allMenus = _bl_Menu.GetList();
            Model_AllCategories model = new Model_AllCategories
            {
                MenuID = MenuID,
                List_Menus = allMenus,
                ShowDeleted = showDeleted
            };

            if (MenuID.HasValue && MenuID.Value > 0)
            {
                CMenu menu = _bl_Menu.Get(MenuID.Value);
                model.MenuName = menu?.Name;

                model.List_Categories = showDeleted
                    ? _bl_Menu_Category.GetList_Deleted_ByMenuID(MenuID.Value)
                    : _bl_Menu_Category.GetList_ByMenuID(MenuID.Value);
            }
            else
            {
                List<CMenu_Category> allCategories = new List<CMenu_Category>();
                foreach (CMenu menu in allMenus)
                {
                    allCategories.AddRange(showDeleted
                        ? _bl_Menu_Category.GetList_Deleted_ByMenuID(menu.ID)
                        : _bl_Menu_Category.GetList_ByMenuID(menu.ID));
                }
                model.List_Categories = allCategories;
            }
            return View(model);
        }

        public IActionResult AllSubcategories(int? Menu_CategoryID, bool showDeleted = false)
        {
            List<CMenu_Category> allCategories = _bl_Menu_Category.GetList();
            Model_AllSubcategories model = new Model_AllSubcategories
            {
                Menu_CategoryID = Menu_CategoryID,
                List_Categories = allCategories,
                ShowDeleted = showDeleted
            };

            if (Menu_CategoryID.HasValue && Menu_CategoryID.Value > 0)
            {
                CMenu_Category category = _bl_Menu_Category.Get(Menu_CategoryID.Value);
                model.CategoryName = category?.Name;

                model.List_Subcategories = showDeleted
                    ? _bl_Menu_Subcategory.GetList_Deleted_ByMenu_CategoryID(Menu_CategoryID.Value)
                    : _bl_Menu_Subcategory.GetList_ByMenu_CategoryID(Menu_CategoryID.Value);
            }
            else
            {
                List<CMenu_Subcategory> allSubcategories = new List<CMenu_Subcategory>();
                foreach (CMenu_Category category in allCategories)
                {
                    allSubcategories.AddRange(showDeleted
                        ? _bl_Menu_Subcategory.GetList_Deleted_ByMenu_CategoryID(category.ID)
                        : _bl_Menu_Subcategory.GetList_ByMenu_CategoryID(category.ID));
                }
                model.List_Subcategories = allSubcategories;
            }
            return View(model);
        }

        public IActionResult AllDishes(int? Menu_SubcategoryID, bool showDeleted = false)
        {
            List<CMenu_Subcategory> allSubcategories = _bl_Menu_Subcategory.GetList();
            Model_AllDishes model = new Model_AllDishes
            {
                Menu_SubcategoryID = Menu_SubcategoryID,
                List_Subcategories = allSubcategories,
                ShowDeleted = showDeleted
            };

            if (Menu_SubcategoryID.HasValue && Menu_SubcategoryID.Value > 0)
            {
                CMenu_Subcategory subcategory = _bl_Menu_Subcategory.Get(Menu_SubcategoryID.Value);
                model.SubcategoryName = subcategory?.Name;

                model.List_Dishes = showDeleted
                    ? _bl_Menu_Dish.GetList_Deleted_ByMenu_SubcategoryID(Menu_SubcategoryID.Value)
                    : _bl_Menu_Dish.GetList_ByMenu_SubcategoryID(Menu_SubcategoryID.Value);
            }
            else
            {
                List<CMenu_Dish> allDishes = new List<CMenu_Dish>();
                foreach (CMenu_Subcategory subcategory in allSubcategories)
                {
                    allDishes.AddRange(showDeleted
                        ? _bl_Menu_Dish.GetList_Deleted_ByMenu_SubcategoryID(subcategory.ID)
                        : _bl_Menu_Dish.GetList_ByMenu_SubcategoryID(subcategory.ID));
                }
                model.List_Dishes = allDishes;
            }
            return View(model);
        }

        #endregion

        #region Вспомогательные методы

        private RedirectToActionResult RedirectToError(int ErrCode, string Url)
        {
            return RedirectToAction(
                actionName: Contst_Admin.ErrorActionName,
                controllerName: Contst_Admin.ErrorControllerName,
                routeValues: new { ErrCode, Url });
        }

        #endregion

        /// <summary>
        /// Массовая перестановка порядка блюд.
        /// </summary>
        [HttpPost]
        public IActionResult ReorderDishes([FromBody] ReorderRequest request)
        {
            if (request?.Ids == null || request.Ids.Count == 0)
                return Json(new { success = false, message = "Пустой список ID" });

            int result = _bl_Menu_Dish.Reorder(request.Ids);

            if (result >= 0)
                return Json(new { success = true, updated = result });

            return Json(new { success = false, message = "Ошибка сохранения порядка" });
        }
    }
}