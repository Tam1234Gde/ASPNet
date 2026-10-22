using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HIDE.RestAndRant.Database.Definitions_ForMenu
{
    /// <summary>
    /// Опции меню ресторана
    /// </summary>
    [Table("Menu_Restaurant_Options")]
    [Comment("Опции меню ресторана")]
    public class Menu_Restaurant_Option_Def
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Код Соответствие меню ресторану
        /// </summary>
        [Comment("Код Соответствие меню ресторану")]
        public int? Menu_RestaurantID { get; set; }


        /// <summary>
        /// Соответствие меню ресторану
        /// </summary>
        [Comment("Соответствие меню ресторану")]
        public Menu_Restaurant_Def? Menu_Restaurant { get; set; } = null!;


        /// <summary>
        /// Код Блюдо
        /// </summary>
        [Comment("Код Блюдо")]
        public int? Menu_DishID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        [Comment("Блюдо")]
        public Menu_Dish_Def? Menu_Dish { get; set; } = null!;


        /// <summary>
        /// Код Ингридиент
        /// </summary>
        [Comment("Код Ингридиент")]
        public int? Menu_IngridientID { get; set; }


        /// <summary>
        /// Ингридиент
        /// </summary>
        [Comment("Ингридиент")]
        public Menu_Ingridient_Def? Menu_Ingridient { get; set; } = null!;


        /// <summary>
        /// Код Группа ингредиента
        /// </summary>
        [Comment("Код Группа ингредиента")]
        public int? Menu_Ingridient_GroupID { get; set; }


        /// <summary>
        /// Группа ингредиента
        /// </summary>
        [Comment("Группа ингредиента")]
        public Menu_Ingridient_Group_Def? Menu_Ingridient_Group { get; set; } = null!;


        /// <summary>
        /// Код Привязан к меню
        /// </summary>
        [Comment("Код Привязан к меню")]
        public int MenuID { get; set; }


        /// <summary>
        /// Привязан к меню
        /// </summary>
        [Comment("Привязан к меню")]
        public Menu_Def? Menu { get; set; } = null!;
    }
}
