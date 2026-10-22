using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Endpoints_ForMenu;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    public class MenuImportExportController : Controller
    {
        private readonly ILogger _log;
        private readonly Menu _bl_Menu;
        private readonly Menu_Category _bl_Menu_Category;
        private readonly Menu_Subcategories _bl_Menu_Subcategory;
        private readonly Menu_Dish _bl_Menu_Dish;
        private readonly Menu_Ingridient _bl_Menu_Ingredient;
        private readonly Menu_Ingridient_Group _bl_Menu_Ingredient_Group;
        private readonly Menu_OptionState _bl_Menu_OptionState;
        private readonly Menu_Dish_Ingridient _bl_Menu_Dish_Ingredient;

        public MenuImportExportController()
        {
            _log = AppLog.CreateLogger(GetType());
            _bl_Menu = new Menu();
            _bl_Menu_Category = new Menu_Category();
            _bl_Menu_Subcategory = new Menu_Subcategories();
            _bl_Menu_Dish = new Menu_Dish();
            _bl_Menu_Ingredient = new Menu_Ingridient();
            _bl_Menu_Ingredient_Group = new Menu_Ingridient_Group();
            _bl_Menu_OptionState = new Menu_OptionState();
            _bl_Menu_Dish_Ingredient = new Menu_Dish_Ingridient();
        }

        public IActionResult Index()
        {
            var model = new Menu_ImportExport_ViewModel();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Export(ExportDataType exportType)
        {
            try
            {
                ExcelPackage.License.SetNonCommercialPersonal("YourLicenseKey");

                using (var package = new ExcelPackage())
                {
                    switch (exportType)
                    {
                        case ExportDataType.Menu:
                            MenuExportMenu(package);
                            break;
                        case ExportDataType.Categories:
                            MenuExportCategories(package);
                            break;
                        case ExportDataType.Subcategories:
                            MenuExportSubcategories(package);
                            break;
                        case ExportDataType.Dishes:
                            MenuExportDishes(package);
                            break;
                        case ExportDataType.Ingredients:
                            MenuExportIngredients(package);
                            break;
                        case ExportDataType.IngredientGroups:
                            MenuExportIngredientGroups(package);
                            break;
                        default:
                            MenuExportAll(package);
                            break;
                    }

                    var fileName = $"Menu_Export_{exportType}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    var fileBytes = await package.GetAsByteArrayAsync();

                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Ошибка при экспорте данных");
                TempData["Error"] = "Ошибка при экспорте данных: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(Menu_ImportExport_ViewModel model)
        {
            if (model.ExcelFile == null && Request.Form.Files.Count > 0)
                model.ExcelFile = Request.Form.Files[0];

            if (model.ExcelFile == null || model.ExcelFile.Length == 0)
            {
                ModelState.AddModelError("", "Пожалуйста, выберите файл для импорта");
                return View(nameof(Index), model);
            }

            try
            {
                ExcelPackage.License.SetNonCommercialPersonal("YourLicenseKey");

                var result = new ImportResult
                {
                    Success = true,
                    Inserted = 0,
                    Updated = 0,
                    Errors = 0,
                    ErrorDetails = new List<ImportError>()
                };

                using (var stream = new MemoryStream())
                {
                    await model.ExcelFile.CopyToAsync(stream);
                    stream.Position = 0;

                    using (var package = new ExcelPackage(stream))
                    {
                        if (package.Workbook.Worksheets.Count == 0)
                        {
                            result.Success = false;
                            result.Message = "Файл не содержит листов с данными";
                            model.ImportResult = result;
                            return View(nameof(Index), model);
                        }

                        // 1) Справочники
                        foreach (var ws in package.Workbook.Worksheets)
                        {
                            if (ws.Dimension == null) continue;
                            string name = ws.Name?.ToLower()?.Trim() ?? "";
                            if (name == "меню") MenuImportMenu(ws, result);
                            else if (name == "категории") MenuImportCategories(ws, result);
                            else if (name == "подкатегории") MenuImportSubcategories(ws, result);
                            else if (name == "группы ингредиентов") MenuImportIngredientGroups(ws, result);
                        }

                        // 2) Основные сущности
                        foreach (var ws in package.Workbook.Worksheets)
                        {
                            if (ws.Dimension == null) continue;
                            string name = ws.Name?.ToLower()?.Trim() ?? "";
                            if (name == "ингредиенты") MenuImportIngredients(ws, result);
                            else if (name == "блюда") MenuImportDishes(ws, result);
                        }

                        // 3) Связи
                        foreach (var ws in package.Workbook.Worksheets)
                        {
                            if (ws.Dimension == null) continue;
                            string name = ws.Name?.ToLower()?.Trim() ?? "";
                            if (name == "ингредиенты блюд") MenuImportDishIngredients(ws, result);
                        }
                    }
                }

                if (result.Errors > 0)
                {
                    result.Success = false;
                    result.Message = $"Импорт завершен с ошибками. Добавлено: {result.Inserted}, Обновлено: {result.Updated}, Ошибок: {result.Errors}";
                }
                else if (result.Inserted == 0 && result.Updated == 0)
                {
                    result.Success = false;
                    result.Message = "Не найдено данных для импорта. Проверьте структуру файла.";
                }
                else
                {
                    result.Message = $"Импорт успешно завершен. Добавлено: {result.Inserted}, Обновлено: {result.Updated}";
                }

                model.ImportResult = result;
                TempData["Success"] = result.Message;
                return View(nameof(Index), model);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Ошибка при импорте данных");
                var result = new ImportResult
                {
                    Success = false,
                    Message = "Ошибка при импорте данных: " + ex.Message,
                    ErrorDetails = new List<ImportError>()
                };
                model.ImportResult = result;
                TempData["Error"] = "Ошибка при импорте данных: " + ex.Message;
                return View(nameof(Index), model);
            }
        }

        #region Экспорт

        private void MenuExportAll(ExcelPackage package)
        {
            MenuExportMenu(package);
            MenuExportCategories(package);
            MenuExportSubcategories(package);
            MenuExportDishes(package);
            MenuExportIngredientGroups(package);
            MenuExportIngredients(package);
            MenuExportDishIngredients(package);
        }

        private void MenuExportMenu(ExcelPackage package)
        {
            var ws = package.Workbook.Worksheets.Add("Меню");
            ws.Cells[1, 1].Value = "ID";
            ws.Cells[1, 2].Value = "Наименование";
            ws.Cells[1, 3].Value = "Состояние";

            var items = _bl_Menu.GetList();
            int row = 2;
            foreach (var item in items)
            {
                ws.Cells[row, 1].Value = item.ID;
                ws.Cells[row, 2].Value = item.Name;
                ws.Cells[row, 3].Value = item.Menu_OptionStateID;
                row++;
            }
            if (items.Any()) ws.Cells[ws.Dimension.Address].AutoFitColumns();
        }

        private void MenuExportCategories(ExcelPackage package)
        {
            var ws = package.Workbook.Worksheets.Add("Категории");
            ws.Cells[1, 1].Value = "ID";
            ws.Cells[1, 2].Value = "Наименование";
            ws.Cells[1, 3].Value = "ID Меню";

            var items = _bl_Menu_Category.GetList();
            int row = 2;
            foreach (var item in items)
            {
                ws.Cells[row, 1].Value = item.ID;
                ws.Cells[row, 2].Value = item.Name;
                ws.Cells[row, 3].Value = item.MenuID;
                row++;
            }
            if (items.Any()) ws.Cells[ws.Dimension.Address].AutoFitColumns();
        }

        private void MenuExportSubcategories(ExcelPackage package)
        {
            var ws = package.Workbook.Worksheets.Add("Подкатегории");
            ws.Cells[1, 1].Value = "ID";
            ws.Cells[1, 2].Value = "Наименование";
            ws.Cells[1, 3].Value = "ID Категории";
            ws.Cells[1, 4].Value = "ID Родителя";

            var items = _bl_Menu_Subcategory.GetList();
            int row = 2;
            foreach (var item in items)
            {
                ws.Cells[row, 1].Value = item.ID;
                ws.Cells[row, 2].Value = item.Name;
                ws.Cells[row, 3].Value = item.Menu_CategoryID;
                ws.Cells[row, 4].Value = item.ParentID;
                row++;
            }
            if (items.Any()) ws.Cells[ws.Dimension.Address].AutoFitColumns();
        }

        private void MenuExportDishes(ExcelPackage package)
        {
            var ws = package.Workbook.Worksheets.Add("Блюда");
            string[] headers = {
                "ID", "ID Подкатегории", "Наименование", "Краткое описание", "Полное описание",
                "Время приготовления (мин)", "Цена", "Разрешить цвета", "За бонусы", "Бонусы",
                "Порядок", "Новинка", "Рекомендуемые ID", "Отключено", "Дата окончания новинки",
                "Объем 1", "Цена объема 1", "Объем 2", "Цена объема 2", "Объем 3", "Цена объема 3",
                "Объем 4", "Цена объема 4", "Объем 5", "Цена объема 5", "Состояние"
            };
            for (int i = 0; i < headers.Length; i++)
                ws.Cells[1, i + 1].Value = headers[i];

            var items = _bl_Menu_Dish.GetList();
            int row = 2;
            foreach (var d in items)
            {
                ws.Cells[row, 1].Value = d.ID;
                ws.Cells[row, 2].Value = d.Menu_SubcategoryID;
                ws.Cells[row, 3].Value = d.Name;
                ws.Cells[row, 4].Value = d.Teaser;
                ws.Cells[row, 5].Value = d.Text;
                ws.Cells[row, 6].Value = d.Cooking_Minutes;
                ws.Cells[row, 7].Value = d.Price;
                ws.Cells[row, 8].Value = d.Flag_AllowColors;
                ws.Cells[row, 9].Value = d.Flag_Bonus;
                ws.Cells[row, 10].Value = d.Bonuses;
                ws.Cells[row, 11].Value = d.OrderNum;
                ws.Cells[row, 12].Value = d.FlagNew;
                ws.Cells[row, 13].Value = d.RecommendDishIDs;
                ws.Cells[row, 14].Value = d.FlagNoActive;
                ws.Cells[row, 15].Value = d.New_DateFrom?.ToString("yyyy-MM-dd HH:mm:ss");
                ws.Cells[row, 16].Value = d.Vol1;
                ws.Cells[row, 17].Value = d.Vol1_Price;
                ws.Cells[row, 18].Value = d.Vol2;
                ws.Cells[row, 19].Value = d.Vol2_Price;
                ws.Cells[row, 20].Value = d.Vol3;
                ws.Cells[row, 21].Value = d.Vol3_Price;
                ws.Cells[row, 22].Value = d.Vol4;
                ws.Cells[row, 23].Value = d.Vol4_Price;
                ws.Cells[row, 24].Value = d.Vol5;
                ws.Cells[row, 25].Value = d.Vol5_Price;
                ws.Cells[row, 26].Value = d.Menu_OptionStateID;
                row++;
            }
            if (items.Any()) ws.Cells[ws.Dimension.Address].AutoFitColumns();
        }

        private void MenuExportIngredients(ExcelPackage package)
        {
            var ws = package.Workbook.Worksheets.Add("Ингредиенты");
            ws.Cells[1, 1].Value = "ID";
            ws.Cells[1, 2].Value = "Наименование";
            ws.Cells[1, 3].Value = "Цена";
            ws.Cells[1, 4].Value = "ID Группы";
            ws.Cells[1, 5].Value = "Состояние";

            var items = _bl_Menu_Ingredient.GetList();
            int row = 2;
            foreach (var i in items)
            {
                ws.Cells[row, 1].Value = i.ID;
                ws.Cells[row, 2].Value = i.Name;
                ws.Cells[row, 3].Value = i.Price;
                ws.Cells[row, 4].Value = i.Menu_Ingridient_GroupID;
                ws.Cells[row, 5].Value = i.Menu_OptionStateID;
                row++;
            }
            if (items.Any()) ws.Cells[ws.Dimension.Address].AutoFitColumns();
        }

        private void MenuExportIngredientGroups(ExcelPackage package)
        {
            var ws = package.Workbook.Worksheets.Add("Группы ингредиентов");
            ws.Cells[1, 1].Value = "ID";
            ws.Cells[1, 2].Value = "Наименование";
            ws.Cells[1, 3].Value = "На выбор одна";
            ws.Cells[1, 4].Value = "Цена по умолчанию";
            ws.Cells[1, 5].Value = "Состояние";

            var items = _bl_Menu_Ingredient_Group.GetList();
            int row = 2;
            foreach (var g in items)
            {
                ws.Cells[row, 1].Value = g.ID;
                ws.Cells[row, 2].Value = g.Name;
                ws.Cells[row, 3].Value = g.IsSelectable ? "Да" : "Нет";
                ws.Cells[row, 4].Value = g.Default_Price;
                ws.Cells[row, 5].Value = g.Menu_OptionStateID;
                row++;
            }
            if (items.Any()) ws.Cells[ws.Dimension.Address].AutoFitColumns();
        }

        private void MenuExportDishIngredients(ExcelPackage package)
        {
            var ws = package.Workbook.Worksheets.Add("Ингредиенты блюд");
            ws.Cells[1, 1].Value = "ID Блюда";
            ws.Cells[1, 2].Value = "ID Ингредиента";

            var allDishes = _bl_Menu_Dish.GetList();
            int row = 2;
            foreach (var dish in allDishes)
            {
                var list = _bl_Menu_Dish_Ingredient.GetList_ByMenu_DishID(dish.ID);
                foreach (var di in list)
                {
                    ws.Cells[row, 1].Value = dish.ID;
                    ws.Cells[row, 2].Value = di.Menu_IngridientID;
                    row++;
                }
            }
            if (row > 2) ws.Cells[ws.Dimension.Address].AutoFitColumns();
        }

        #endregion

        #region Импорт с поиском по ID

        private void MenuImportMenu(ExcelWorksheet ws, ImportResult result)
        {
            int row = 2;
            while (ws.Cells[row, 2].Value != null)  // проверка наличия имени
            {
                try
                {
                    var id = ws.Cells[row, 1].Value != null ? Convert.ToInt32(ws.Cells[row, 1].Value) : 0;
                    var name = ws.Cells[row, 2].Value?.ToString()?.Trim();
                    var stateId = Convert.ToInt32(ws.Cells[row, 3].Value ?? 1);

                    if (string.IsNullOrEmpty(name))
                    {
                        AddError(result, row, "Наименование", "Пустое значение");
                        row++;
                        continue;
                    }

                    // Ищем по ID (если указан)
                    CMenu existing = null;
                    if (id > 0)
                        existing = _bl_Menu.Get(id);

                    // Если не найден по ID, ищем по имени (для обновления существующих без ID)
                    if (existing == null && !string.IsNullOrEmpty(name))
                        existing = _bl_Menu.GetList().FirstOrDefault(x => x.Name == name);

                    if (existing != null)
                    {
                        _bl_Menu.Edit(existing.ID, name, stateId);
                        result.Updated++;
                    }
                    else
                    {
                        _bl_Menu.Add(name, stateId);
                        result.Inserted++;
                    }
                }
                catch (Exception ex)
                {
                    AddError(result, row, "", ex.Message);
                }
                row++;
            }
        }

        private void MenuImportCategories(ExcelWorksheet ws, ImportResult result)
        {
            int row = 2;
            while (ws.Cells[row, 2].Value != null)
            {
                try
                {
                    var id = ws.Cells[row, 1].Value != null ? Convert.ToInt32(ws.Cells[row, 1].Value) : 0;
                    var name = ws.Cells[row, 2].Value?.ToString()?.Trim();
                    var menuId = Convert.ToInt32(ws.Cells[row, 3].Value ?? 0);

                    if (string.IsNullOrEmpty(name))
                    {
                        AddError(result, row, "Наименование", "Пустое значение");
                        row++;
                        continue;
                    }
                    if (menuId == 0)
                    {
                        AddError(result, row, "ID Меню", "Не указан ID меню");
                        row++;
                        continue;
                    }

                    // Ищем по ID
                    CMenu_Category existing = null;
                    if (id > 0)
                        existing = _bl_Menu_Category.Get(id);

                    // Если не найден, ищем по имени + MenuID
                    if (existing == null && !string.IsNullOrEmpty(name))
                        existing = _bl_Menu_Category.GetList().FirstOrDefault(x => x.Name == name && x.MenuID == menuId);

                    if (existing != null)
                    {
                        _bl_Menu_Category.Edit(existing.ID, name, menuId);
                        result.Updated++;
                    }
                    else
                    {
                        _bl_Menu_Category.Add(name, menuId);
                        result.Inserted++;
                    }
                }
                catch (Exception ex)
                {
                    AddError(result, row, "", ex.Message);
                }
                row++;
            }
        }

        private void MenuImportSubcategories(ExcelWorksheet ws, ImportResult result)
        {
            int row = 2;
            while (ws.Cells[row, 2].Value != null)
            {
                try
                {
                    var id = ws.Cells[row, 1].Value != null ? Convert.ToInt32(ws.Cells[row, 1].Value) : 0;
                    var name = ws.Cells[row, 2].Value?.ToString()?.Trim();
                    var catId = Convert.ToInt32(ws.Cells[row, 3].Value ?? 0);
                    var parentId = ws.Cells[row, 4].Value != null ? Convert.ToInt32(ws.Cells[row, 4].Value) : (int?)null;

                    if (string.IsNullOrEmpty(name))
                    {
                        AddError(result, row, "Наименование", "Пустое значение");
                        row++;
                        continue;
                    }
                    if (catId == 0)
                    {
                        AddError(result, row, "ID Категории", "Не указан ID категории");
                        row++;
                        continue;
                    }

                    CMenu_Subcategory existing = null;
                    if (id > 0)
                        existing = _bl_Menu_Subcategory.Get(id);

                    if (existing == null && !string.IsNullOrEmpty(name))
                        existing = _bl_Menu_Subcategory.GetList().FirstOrDefault(x => x.Name == name && x.Menu_CategoryID == catId);

                    if (existing != null)
                    {
                        _bl_Menu_Subcategory.Edit(existing.ID, name, catId, parentId);
                        result.Updated++;
                    }
                    else
                    {
                        _bl_Menu_Subcategory.Add(name, catId, parentId);
                        result.Inserted++;
                    }
                }
                catch (Exception ex)
                {
                    AddError(result, row, "", ex.Message);
                }
                row++;
            }
        }

        private void MenuImportIngredientGroups(ExcelWorksheet ws, ImportResult result)
        {
            int row = 2;
            while (ws.Cells[row, 2].Value != null)
            {
                try
                {
                    var id = ws.Cells[row, 1].Value != null ? Convert.ToInt32(ws.Cells[row, 1].Value) : 0;
                    var name = ws.Cells[row, 2].Value?.ToString()?.Trim();
                    var isSelectable = ws.Cells[row, 3].Value?.ToString()?.ToLower() == "да";
                    // Колонка 4 — Цена по умолчанию (может отсутствовать в старых файлах)
                    var defaultPrice = ws.Cells[row, 4].Value != null ? Convert.ToInt32(ws.Cells[row, 4].Value) : (int?)null;
                    // Колонка 5 — Состояние. Если файл старый (без колонки 4), состояние было в колонке 4.
                    var stateId = ws.Cells[row, 5].Value != null
                        ? Convert.ToInt32(ws.Cells[row, 5].Value)
                        : Convert.ToInt32(ws.Cells[row, 4].Value ?? 1);

                    if (string.IsNullOrEmpty(name))
                    {
                        AddError(result, row, "Наименование", "Пустое значение");
                        row++;
                        continue;
                    }

                    CMenu_Ingridient_Group existing = null;
                    if (id > 0)
                        existing = _bl_Menu_Ingredient_Group.Get(id);

                    if (existing == null && !string.IsNullOrEmpty(name))
                        existing = _bl_Menu_Ingredient_Group.GetList().FirstOrDefault(x => x.Name == name);

                    if (existing != null)
                    {
                        _bl_Menu_Ingredient_Group.Edit(existing.ID, name, isSelectable, stateId, defaultPrice);
                        result.Updated++;
                    }
                    else
                    {
                        _bl_Menu_Ingredient_Group.Add(name, isSelectable, stateId, defaultPrice);
                        result.Inserted++;
                    }
                }
                catch (Exception ex)
                {
                    AddError(result, row, "", ex.Message);
                }
                row++;
            }
        }

        private void MenuImportIngredients(ExcelWorksheet ws, ImportResult result)
        {
            int row = 2;
            while (ws.Cells[row, 2].Value != null)
            {
                try
                {
                    var id = ws.Cells[row, 1].Value != null ? Convert.ToInt32(ws.Cells[row, 1].Value) : 0;
                    var name = ws.Cells[row, 2].Value?.ToString()?.Trim();
                    var price = ws.Cells[row, 3].Value != null ? Convert.ToInt32(ws.Cells[row, 3].Value) : (int?)null;
                    var groupId = Convert.ToInt32(ws.Cells[row, 4].Value ?? 0);
                    var stateId = Convert.ToInt32(ws.Cells[row, 5].Value ?? 1);

                    if (string.IsNullOrEmpty(name))
                    {
                        AddError(result, row, "Наименование", "Пустое значение");
                        row++;
                        continue;
                    }
                    if (groupId == 0)
                    {
                        AddError(result, row, "ID Группы", "Не указан ID группы");
                        row++;
                        continue;
                    }

                    CMenu_Ingridient existing = null;
                    if (id > 0)
                        existing = _bl_Menu_Ingredient.Get(id);

                    if (existing == null && !string.IsNullOrEmpty(name))
                        existing = _bl_Menu_Ingredient.GetList().FirstOrDefault(x => x.Name == name && x.Menu_Ingridient_GroupID == groupId);

                    if (existing != null)
                    {
                        _bl_Menu_Ingredient.Edit(existing.ID, name, existing.Image, price, groupId, stateId);
                        result.Updated++;
                    }
                    else
                    {
                        _bl_Menu_Ingredient.Add(name, null, price, groupId, stateId);
                        result.Inserted++;
                    }
                }
                catch (Exception ex)
                {
                    AddError(result, row, "", ex.Message);
                }
                row++;
            }
        }

        private void MenuImportDishes(ExcelWorksheet ws, ImportResult result)
        {
            int row = 2;
            while (ws.Cells[row, 3].Value != null) // колонка 3 – наименование
            {
                try
                {
                    var id = ws.Cells[row, 1].Value != null ? Convert.ToInt32(ws.Cells[row, 1].Value) : 0;
                    var subcatId = Convert.ToInt32(ws.Cells[row, 2].Value ?? 0);
                    var name = ws.Cells[row, 3].Value?.ToString()?.Trim();
                    var teaser = ws.Cells[row, 4].Value?.ToString()?.Trim();
                    var text = ws.Cells[row, 5].Value?.ToString()?.Trim();
                    var cooking = ws.Cells[row, 6].Value != null ? Convert.ToInt32(ws.Cells[row, 6].Value) : (int?)null;
                    var price = Convert.ToDecimal(ws.Cells[row, 7].Value ?? 0);
                    var allowColors = Convert.ToBoolean(ws.Cells[row, 8].Value ?? false);
                    var flagBonus = Convert.ToBoolean(ws.Cells[row, 9].Value ?? false);
                    var bonuses = ws.Cells[row, 10].Value != null ? Convert.ToDecimal(ws.Cells[row, 10].Value) : (decimal?)null;
                    var order = Convert.ToInt32(ws.Cells[row, 11].Value ?? 0);
                    var flagNew = Convert.ToBoolean(ws.Cells[row, 12].Value ?? false);
                    var recommend = ws.Cells[row, 13].Value?.ToString()?.Trim();
                    var noActive = Convert.ToBoolean(ws.Cells[row, 14].Value ?? false);
                    var newDate = ws.Cells[row, 15].Value != null ? Convert.ToDateTime(ws.Cells[row, 15].Value) : (DateTime?)null;
                    var v1 = ws.Cells[row, 16].Value != null ? Convert.ToInt32(ws.Cells[row, 16].Value) : (int?)null;
                    var v1p = ws.Cells[row, 17].Value != null ? Convert.ToInt32(ws.Cells[row, 17].Value) : (int?)null;
                    var v2 = ws.Cells[row, 18].Value != null ? Convert.ToInt32(ws.Cells[row, 18].Value) : (int?)null;
                    var v2p = ws.Cells[row, 19].Value != null ? Convert.ToInt32(ws.Cells[row, 19].Value) : (int?)null;
                    var v3 = ws.Cells[row, 20].Value != null ? Convert.ToInt32(ws.Cells[row, 20].Value) : (int?)null;
                    var v3p = ws.Cells[row, 21].Value != null ? Convert.ToInt32(ws.Cells[row, 21].Value) : (int?)null;
                    var v4 = ws.Cells[row, 22].Value != null ? Convert.ToInt32(ws.Cells[row, 22].Value) : (int?)null;
                    var v4p = ws.Cells[row, 23].Value != null ? Convert.ToInt32(ws.Cells[row, 23].Value) : (int?)null;
                    var v5 = ws.Cells[row, 24].Value != null ? Convert.ToInt32(ws.Cells[row, 24].Value) : (int?)null;
                    var v5p = ws.Cells[row, 25].Value != null ? Convert.ToInt32(ws.Cells[row, 25].Value) : (int?)null;
                    var stateId = Convert.ToInt32(ws.Cells[row, 26].Value ?? 1);

                    if (string.IsNullOrEmpty(name))
                    {
                        AddError(result, row, "Наименование", "Пустое значение");
                        row++;
                        continue;
                    }
                    if (subcatId == 0)
                    {
                        AddError(result, row, "ID Подкатегории", "Не указан ID подкатегории");
                        row++;
                        continue;
                    }
                    if (_bl_Menu_Subcategory.Get(subcatId) == null)
                    {
                        AddError(result, row, "ID Подкатегории", $"Подкатегория с ID {subcatId} не найдена");
                        row++;
                        continue;
                    }

                    CMenu_Dish existing = null;
                    if (id > 0)
                        existing = _bl_Menu_Dish.Get(id);

                    if (existing == null && !string.IsNullOrEmpty(name))
                        existing = _bl_Menu_Dish.GetList().FirstOrDefault(x => x.Name == name && x.Menu_SubcategoryID == subcatId);

                    if (existing != null)
                    {
                        _bl_Menu_Dish.Edit(
                            existing.ID, subcatId, name, teaser, text, cooking,
                            price, allowColors, flagBonus, bonuses, order, flagNew,
                            existing.Image, existing.Image_ChangeDate, recommend,
                            noActive, newDate,
                            v1, v1p, v2, v2p, v3, v3p, v4, v4p, v5, v5p, stateId
                        );
                        result.Updated++;
                    }
                    else
                    {
                        _bl_Menu_Dish.Add(
                            subcatId, name, teaser, text, cooking, price,
                            allowColors, flagBonus, bonuses, order, flagNew,
                            null, null, recommend, noActive, newDate,
                            v1, v1p, v2, v2p, v3, v3p, v4, v4p, v5, v5p, stateId
                        );
                        result.Inserted++;
                    }
                }
                catch (Exception ex)
                {
                    AddError(result, row, "", ex.Message);
                }
                row++;
            }
        }

        private void MenuImportDishIngredients(ExcelWorksheet ws, ImportResult result)
        {
            int row = 2;
            while (ws.Cells[row, 1].Value != null && ws.Cells[row, 2].Value != null)
            {
                try
                {
                    var dishId = Convert.ToInt32(ws.Cells[row, 1].Value);
                    var ingId = Convert.ToInt32(ws.Cells[row, 2].Value);
                    if (dishId == 0 || ingId == 0) { AddError(result, row, "ID", "Неверный ID"); row++; continue; }

                    var dish = _bl_Menu_Dish.Get(dishId);
                    var ing = _bl_Menu_Ingredient.Get(ingId);
                    if (dish == null || ing == null)
                    { AddError(result, row, "ID", $"Блюдо (ID {dishId}) или ингредиент (ID {ingId}) не найдены"); row++; continue; }

                    var existing = _bl_Menu_Dish_Ingredient.GetList_ByMenu_DishID(dishId).FirstOrDefault(x => x.Menu_IngridientID == ingId);
                    if (existing == null) { _bl_Menu_Dish_Ingredient.Add(dishId, ingId); result.Inserted++; }
                    else { result.Updated++; }
                }
                catch (Exception ex)
                {
                    AddError(result, row, "", ex.Message);
                }
                row++;
            }
        }

        private void AddError(ImportResult result, int row, string column, string error)
        {
            result.Errors++;
            result.ErrorDetails?.Add(new ImportError { Row = row, Column = column, Error = error });
        }

        #endregion
    }
}