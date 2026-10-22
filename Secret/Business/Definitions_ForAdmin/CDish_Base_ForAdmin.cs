using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Базовый класс для блюда
    /// </summary>
    public abstract class CDish_Base_ForAdmin
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Категория блюда - код
        /// </summary>
        [DisplayName("Код категории")]
        public int Dish_CategoryID { get; set; }


        /// <summary>
        /// Порядок отображения
        /// </summary>
        [DisplayName("Порядок отображения")]
        public int CategoryOrderNum { get; set; }


        /// <summary>
        /// Категория блюда
        /// </summary>
        [DisplayName("Категория блюда")]
        public string? Dish_Category { get; set; }


        /// <summary>
        /// Наименование товара
        /// </summary>
        [DisplayName("Наименование блюда")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Краткое описание блюда
        /// </summary>
        [DisplayName("Краткое описание")]
        public string? Teaser { get; set; }


        /// <summary>
        /// Полное описание блюда
        /// </summary>
        [DisplayName("Полное описание")]
        public string? Text { get; set; }


        /// <summary>
        /// Коды рекомендуемых блюд (через ;)
        /// </summary>
        [Comment("Коды рекомендуемых блюд")]
        [DisplayName("Коды рекомендуемых блюд")]
        public string? RecommendDishIDs { get; set; }


        /// <summary>
        /// Флаг того, что блюдо можно купить за бонусы
        /// </summary>
        [DisplayName("Доступно за бонусы")]
        public bool Flag_Bonus { get; set; }


        /// <summary>
        /// Количество бонусов, начисляемых за покупку
        /// </summary>
        [DisplayName("Бонусы за покупку")]
        public decimal? Bonuses { get; set; }


        /// <summary>
        /// Количество минут для приготовления блюда
        /// </summary>
        [DisplayName("Минут для приготовления")]
        public int? Cooking_Minutes { get; set; }


        /// <summary>
        /// Стоимость
        /// </summary>
        [DisplayName("Стоимость")]
        public decimal Price { get; set; }


        /// <summary>
        /// Флаг того, что этот товар кофе и для него нужно подгрузить шапочку напитка и менять цвет стакана
        /// </summary>
        [DisplayName("Разрешены цвета")]
        public bool Flag_AllowColors { get; set; }


        /// <summary>
        /// Порядковый номер
        /// </summary>
        [DisplayName("Порядковый номер")]
        public int OrderNum { get; set; }


        /// <summary>
        /// Новинка
        /// </summary>
        [DisplayName("Новинка")]
        public bool FlagNew { get; set; }


        /// <summary>
        /// Отключить отображение блюда в основном меню
        /// </summary>
        [DisplayName("Отключить в меню")]
        public bool FlagNoActive { get; set; }


        /// <summary>
        /// Количество
        /// </summary>
        [DisplayName("Количество")]
        public int Count { get; set; }


        /// <summary>
        /// Изображение блюда
        /// </summary>
        [DisplayName("Изображение")]
        public byte[]? Image { get; set; }


        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        [DisplayName("Ссылка на изображение")]
        public string? Image_Url { get; set; }


        /// <summary>
        /// Коды рекомендованных блюд
        /// </summary>
        [Comment("Коды рекомендованных блюд")]
        [DisplayName("Список ID рекомендуемых блюд")]
        public List<int>? List_RecommendDishIDs { get; set; }


        /// <summary>
        /// Список объемов у блюда
        /// </summary>
        [DisplayName("Список объёмов")]
        public List<CDish_Volume>? List_Dish_Volumes { get; set; }


        /// <summary>
        /// Список цветов у блюда
        /// </summary>
        [DisplayName("Список цветов")]
        public List<CDish_Color>? List_Dish_Colors { get; set; }


        /// <summary>
        /// Список ингредиентов у блюда
        /// </summary>
        [DisplayName("Список ингредиентов")]
        public List<CDish_Ingridient_ForAdmin>? List_Dish_Ingridients { get; set; }


        /// <summary>
        /// Удален
        /// </summary>
        [DisplayName("Удален")]
        public bool Flag_Del { get; set; }
    }
}