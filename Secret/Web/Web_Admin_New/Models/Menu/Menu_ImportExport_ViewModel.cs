using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель для импорта и экспорта данных меню.
    /// </summary>
    public class Menu_ImportExport_ViewModel
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт признак, нужно ли удалять записи, отсутствующие в импортируемом файле.
        /// </summary>
        [Display(Name = "Удалять записи, которых нет в файле")]
        public bool DeleteMissing { get; set; }


        /// <summary>
        /// Получает или задаёт файл Excel для импорта.
        /// </summary>
        [Display(Name = "Выберите файл Excel")]
        public IFormFile? ExcelFile { get; set; }


        /// <summary>
        /// Получает или задаёт тип данных для экспорта.
        /// </summary>
        [Display(Name = "Тип данных")]
        public ExportDataType ExportType { get; set; } = ExportDataType.All;


        /// <summary>
        /// Получает или задаёт результат выполненного импорта.
        /// </summary>
        public ImportResult? ImportResult { get; set; }

        #endregion
    }


    /// <summary>
    /// Перечисление типов данных, доступных для экспорта.
    /// </summary>
    public enum ExportDataType
    {
        /// <summary>Все данные.</summary>
        [Display(Name = "Все данные")]
        All = 0,

        /// <summary>Только меню.</summary>
        [Display(Name = "Меню")]
        Menu = 1,

        /// <summary>Только категории.</summary>
        [Display(Name = "Категории")]
        Categories = 2,

        /// <summary>Только подкатегории.</summary>
        [Display(Name = "Подкатегории")]
        Subcategories = 3,

        /// <summary>Только блюда.</summary>
        [Display(Name = "Блюда")]
        Dishes = 4,

        /// <summary>Только ингредиенты.</summary>
        [Display(Name = "Ингредиенты")]
        Ingredients = 5,

        /// <summary>Только группы ингредиентов.</summary>
        [Display(Name = "Группы ингредиентов")]
        IngredientGroups = 6
    }


    /// <summary>
    /// Результат операции импорта.
    /// </summary>
    public class ImportResult
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт общее количество ошибок.
        /// </summary>
        public int Errors { get; set; }


        /// <summary>
        /// Получает или задаёт список деталей ошибок (построчный разбор).
        /// </summary>
        public List<ImportError>? ErrorDetails { get; set; }


        /// <summary>
        /// Получает или задаёт количество вставленных записей.
        /// </summary>
        public int Inserted { get; set; }


        /// <summary>
        /// Получает или задаёт сообщение, описывающее результат.
        /// </summary>
        public string? Message { get; set; }


        /// <summary>
        /// Получает или задаёт признак успешного выполнения импорта.
        /// </summary>
        public bool Success { get; set; }


        /// <summary>
        /// Получает или задаёт количество обновлённых записей.
        /// </summary>
        public int Updated { get; set; }

        #endregion
    }


    /// <summary>
    /// Детали ошибки, возникшей при импорте конкретной строки.
    /// </summary>
    public class ImportError
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт название колонки, в которой произошла ошибка.
        /// </summary>
        public string? Column { get; set; }


        /// <summary>
        /// Получает или задаёт текст ошибки.
        /// </summary>
        public string? Error { get; set; }


        /// <summary>
        /// Получает или задаёт номер строки в файле (начиная с 1).
        /// </summary>
        public int Row { get; set; }


        /// <summary>
        /// Получает или задаёт значение ячейки, вызвавшее ошибку.
        /// </summary>
        public string? Value { get; set; }

        #endregion
    }
}