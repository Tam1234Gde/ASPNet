using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HIDE.RestAndRant.Database.Definitions_ForMenu
{
    /// <summary>
    /// Ингридиенты блюд меню
    /// </summary>
    [Table("Menu_Dish_Ingridients")]
    [Comment("Ингридиенты блюд меню")]
    public class Menu_Dish_Ingridient_Def
    {

        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Код Привязан к меню
        /// </summary>
        [Comment("Код Привязан к меню")]
        public int Menu_DishID { get; set; }


        /// <summary>
        /// Привязан к меню
        /// </summary>
        [Comment("Привязан к меню")]
        public Menu_Dish_Def Menu_Dish { get; set; } = null!;


        /// <summary>
        /// Код Привязан к Ингридиенту
        /// </summary>
        [Comment("Код Привязан к Ингридиенту")]
        public int Menu_IngridientID { get; set; }


        /// <summary>
        /// Привязан к Ингридиенту
        /// </summary>
        [Comment("Привязан к Ингридиенту")]
        public Menu_Ingridient_Def Menu_Ingridient { get; set; } = null!;
    }
}
