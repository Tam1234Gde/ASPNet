using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Товар, блюдо
    /// </summary>
    public class CDish
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Категория блюда - код
        /// </summary>
        public int Dish_CategoryID { get; set; }


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
        [DisplayName("Краткое описание блюда")]
        public string? Teaser { get; set; }


        /// <summary>
        /// Полное описание блюда
        /// </summary>
        [DisplayName("Полное описание блюда")]
        public string? Text { get; set; }


        /// <summary>
        /// Количество минут для приготовления блюда
        /// </summary>
        [DisplayName("Количество минут для приготовления блюда")]
        public int? Cooking_Minutes { get; set; }


        /// <summary>
        /// Изображение блюда
        /// </summary>
        [Comment("Изображение блюда")]
        [JsonIgnore]
        public byte[]? Image { get; set; }


        /// <summary>
        /// Стоимость
        /// </summary>
        [DisplayName("Стоимость")]
        public decimal Price { get; set; }


        /// <summary>
        /// Флаг того, что этот товар кофе и для него нужно подгрузить шапочку напитка и менять цвет стакана
        /// </summary>
        public bool Flag_AllowColors { get; set; }


        /// <summary>
        /// Флаг того, что блюдо можно купить за бонусы
        /// </summary>
        [DisplayName("Блюдо можно купить за бонусы")]
        public bool Flag_Bonus { get; set; }


        /// <summary>
        /// Количество бонусов, начисляемых за покупку
        /// </summary>
        [DisplayName("Количество бонусов, начисляемых за покупку")]
        public decimal? Bonuses { get; set; }


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
        [DisplayName("Скрыть")]
        public bool FlagNoActive { get; set; }


        /// <summary>
        /// Дата изменения изображения
        /// </summary>
        public DateTime? Image_ChangeDate { get; set; }


        /// <summary>
        /// Коды рекомендованных блюд
        /// </summary>
        [Comment("Коды рекомендованных блюд")]
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
        public List<CDish_Ingridient>? List_Dish_Ingridients { get; set; }


        /// <summary>
        /// Дата до которой отображается новинка
        /// </summary>
        [DisplayName("Дата до которой отображается новинка")]
        public DateTime? New_DateFrom { get; set; }
    }
}