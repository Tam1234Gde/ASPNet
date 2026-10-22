using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;

namespace HIDE.RestAndRant.Web_Admin.Models.RestaurantMenu_Dishes
{
    /// <summary>
    /// CRUD - Блюда
    /// </summary>
    public class Model_RestaurantMenu_Dishes_CRUD
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
        /// Код категории
        /// </summary>
        [DisplayName("Код категории")]
        public int Dish_CategoryID { get; set; }


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
        [Comment("Новинка")]
        public bool FlagNew { get; set; }


        /// <summary>
        /// Флаг доступности
        /// </summary>
        [DisplayName("Флаг доступности")]
        public bool Flag_Available { get; set; }


        /// <summary>
        /// Список блюд
        /// </summary>
        [DisplayName("Список блюд")]
        public List<CDish_ForAdmin>? List_Dishes { get; set; }

    }
}