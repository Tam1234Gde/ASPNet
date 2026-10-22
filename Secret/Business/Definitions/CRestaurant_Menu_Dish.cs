using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Блюда меню ресторана
    /// </summary>
    public class CRestaurant_Menu_Dish
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Код меню
        /// </summary>
        [DisplayName("Код меню")]
        public int Restaurant_MenuID { get; set; }


        /// <summary>
        /// Меню
        /// </summary>
        [DisplayName("Меню")]
        public string? Restaurant_Menu { get; set; }


        /// <summary>
        /// Код блюда
        /// </summary>
        [DisplayName("Код блюда")]
        public int DishID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        [DisplayName("Блюдо")]
        public string? Dish { get; set; }


        /// <summary>
        /// Стоимость блюда
        /// </summary>
        [DisplayName("Стоимость блюда")]
        public decimal? Price { get; set; }


        /// <summary>
        /// Порядковый номер
        /// </summary>
        [Comment("Порядковый номер")]
        public int OrderNum { get; set; }


        /// <summary>
        /// Новинка
        /// </summary>
        [DisplayName("Новинка")]
        public bool FlagNew { get; set; }


        /// <summary>
        /// Флаг доступности
        /// </summary>
        [DisplayName("Флаг доступности")]
        public bool Flag_Available { get; set; }


        /// <summary>
        /// Объемы блюда с ценой
        /// </summary>
        [DisplayName("Объемы блюда с ценой")]
        public List<CRestaurant_Menu_Dish_Volume>? Restaurant_Menu_Dish_Volumes { get; set; }


        /// <summary>
        /// Ингридиенты блюда
        /// </summary>
        [DisplayName("Ингридиенты блюда")]
        public List<CRestaurant_Menu_Dish_Ingredient>? Restaurant_Menu_Dish_Ingredients { get; set; }

    }
}