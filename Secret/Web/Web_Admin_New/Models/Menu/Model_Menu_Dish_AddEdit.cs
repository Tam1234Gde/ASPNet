using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Модель для добавления и редактирования блюда.
    /// </summary>
    public class Model_Menu_Dish_AddEdit
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт количество бонусов за блюдо.
        /// </summary>
        [Display(Name = "Бонусы")]
        public decimal? Bonuses { get; set; }


        /// <summary>
        /// Получает или задаёт время приготовления в минутах.
        /// </summary>
        [Display(Name = "Время приготовления (мин)")]
        public int? Cooking_Minutes { get; set; }


        /// <summary>
        /// Получает или задаёт признак разрешения выбора цвета.
        /// </summary>
        [Display(Name = "Разрешить цвета")]
        public bool Flag_AllowColors { get; set; }


        /// <summary>
        /// Получает или задаёт признак возможности покупки за бонусы.
        /// </summary>
        [Display(Name = "Можно купить за бонусы")]
        public bool Flag_Bonus { get; set; }


        /// <summary>
        /// Получает или задаёт признак "Новинка".
        /// </summary>
        [Display(Name = "Новинка")]
        public bool FlagNew { get; set; }


        /// <summary>
        /// Получает или задаёт признак отключения отображения.
        /// </summary>
        [Display(Name = "Отключить отображение")]
        public bool FlagNoActive { get; set; }


        /// <summary>
        /// Получает или задаёт идентификатор записи.
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Получает или задаёт бинарное изображение.
        /// </summary>
        public byte[]? Image { get; set; }


        /// <summary>
        /// Получает или задаёт файл изображения для загрузки.
        /// </summary>
        [Display(Name = "Изображение")]
        public IFormFile? ImageFile { get; set; }


        /// <summary>
        /// Получает или задаёт список групп ингредиентов с привязками.
        /// </summary>
        public List<IngredientGroupViewModel> IngredientGroups { get; set; } = new();


        /// <summary>
        /// Получает или задаёт список ингредиентов, привязанных к блюду.
        /// </summary>
        public List<CMenu_Dish_Ingridient>? List_Ingredients { get; set; }


        /// <summary>
        /// Получает или задаёт список состояний активности.
        /// </summary>
        public List<CMenu_OptionState>? List_Menu_OptionStates { get; set; }


        /// <summary>
        /// Получает или задаёт идентификатор состояния активности.
        /// </summary>
        [Required(ErrorMessage = "Выберите состояние")]
        [Display(Name = "Состояние")]
        public int Menu_OptionStateID { get; set; }


        /// <summary>
        /// Получает или задаёт идентификатор подкатегории.
        /// </summary>
        public int Menu_SubcategoryID { get; set; }


        /// <summary>
        /// Получает или задаёт наименование блюда.
        /// </summary>
        [Required(ErrorMessage = "Введите наименование блюда")]
        [Display(Name = "Наименование блюда")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Получает или задаёт дату начала действия новинки.
        /// </summary>
        [Display(Name = "Дата окончания новинки")]
        public DateTime? New_DateFrom { get; set; }


        /// <summary>
        /// Получает или задаёт порядковый номер.
        /// </summary>
        [Display(Name = "Порядок")]
        public int OrderNum { get; set; }


        /// <summary>
        /// Получает или задаёт стоимость блюда.
        /// </summary>
        [Required(ErrorMessage = "Введите стоимость")]
        [Display(Name = "Стоимость")]
        [Range(0.01, 999999, ErrorMessage = "Стоимость должна быть больше 0")]
        public decimal Price { get; set; }


        /// <summary>
        /// Получает или задаёт список идентификаторов рекомендуемых блюд (через точку с запятой).
        /// </summary>
        [Display(Name = "Рекомендуемые блюда (ID через ;)")]
        public string? RecommendDishIDs { get; set; }


        /// <summary>
        /// Получает или задаёт список выбранных идентификаторов ингредиентов.
        /// </summary>
        public List<int> SelectedIngredientIDs { get; set; } = new();


        /// <summary>
        /// Получает или задаёт наименование подкатегории.
        /// </summary>
        public string? SubcategoryName { get; set; }


        /// <summary>
        /// Получает или задаёт краткое описание.
        /// </summary>
        [Display(Name = "Краткое описание")]
        public string? Teaser { get; set; }


        /// <summary>
        /// Получает или задаёт полное описание.
        /// </summary>
        [Display(Name = "Полное описание")]
        public string? Text { get; set; }


        /// <summary>
        /// Получает или задаёт объём 1 (мл).
        /// </summary>
        [Display(Name = "Объем 1 (мл)")]
        public int? Vol1 { get; set; }


        /// <summary>
        /// Получает или задаёт цену для объёма 1.
        /// </summary>
        [Display(Name = "Цена объема 1")]
        public int? Vol1_Price { get; set; }


        /// <summary>
        /// Получает или задаёт объём 2 (мл).
        /// </summary>
        [Display(Name = "Объем 2 (мл)")]
        public int? Vol2 { get; set; }


        /// <summary>
        /// Получает или задаёт цену для объёма 2.
        /// </summary>
        [Display(Name = "Цена объема 2")]
        public int? Vol2_Price { get; set; }


        /// <summary>
        /// Получает или задаёт объём 3 (мл).
        /// </summary>
        [Display(Name = "Объем 3 (мл)")]
        public int? Vol3 { get; set; }


        /// <summary>
        /// Получает или задаёт цену для объёма 3.
        /// </summary>
        [Display(Name = "Цена объема 3")]
        public int? Vol3_Price { get; set; }


        /// <summary>
        /// Получает или задаёт объём 4 (мл).
        /// </summary>
        [Display(Name = "Объем 4 (мл)")]
        public int? Vol4 { get; set; }


        /// <summary>
        /// Получает или задаёт цену для объёма 4.
        /// </summary>
        [Display(Name = "Цена объема 4")]
        public int? Vol4_Price { get; set; }


        /// <summary>
        /// Получает или задаёт объём 5 (мл).
        /// </summary>
        [Display(Name = "Объем 5 (мл)")]
        public int? Vol5 { get; set; }


        /// <summary>
        /// Получает или задаёт цену для объёма 5.
        /// </summary>
        [Display(Name = "Цена объема 5")]
        public int? Vol5_Price { get; set; }


        /// <summary>
        /// Получает или задаёт изображение из старой БД в формате Base64.
        /// Используется, если ImageFile не задан.
        /// </summary>
        public string? ImageBase64 { get; set; }

        #endregion
    }


    /// <summary>
    /// Модель группы ингредиентов для отображения в форме блюда.
    /// </summary>
    public class IngredientGroupViewModel
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт идентификатор группы.
        /// </summary>
        public int GroupId { get; set; }


        /// <summary>
        /// Получает или задаёт наименование группы.
        /// </summary>
        public string GroupName { get; set; } = null!;


        /// <summary>
        /// Получает или задаёт список ингредиентов в группе.
        /// </summary>
        public List<IngredientItemViewModel> Ingredients { get; set; } = new();


        /// <summary>
        /// Получает или задаёт признак, что можно выбрать только один ингредиент из группы.
        /// </summary>
        public bool IsSelectable { get; set; }

        #endregion
    }


    /// <summary>
    /// Модель элемента ингредиента для выбора в форме блюда.
    /// </summary>
    public class IngredientItemViewModel
    {
        #region Properties

        /// <summary>
        /// Получает или задаёт идентификатор ингредиента.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Получает или задаёт признак, выбран ли ингредиент.
        /// </summary>
        public bool IsChecked { get; set; }


        /// <summary>
        /// Получает или задаёт наименование ингредиента.
        /// </summary>
        public string Name { get; set; } = null!;


        /// <summary>
        /// Получает или задаёт цену ингредиента.
        /// </summary>
        public int? Price { get; set; }

        #endregion
    }
}