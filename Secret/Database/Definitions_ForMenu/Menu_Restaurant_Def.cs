using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Database.Definitions_ForMenu
{
    /// <summary>
    /// Меню ресторана
    /// </summary>
    [Table("Menu_Restaurants")]
    [Comment("Меню ресторана")]
    public class Menu_Restaurant_Def
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
        public int MenuID { get; set; }

        /// <summary>
        /// Привязан к меню
        /// </summary>
        [Comment("Привязан к меню")]
        public Menu_Def Menu { get; set; } = null!;

        /// <summary>
        /// Код ресторана
        /// </summary>
        [Comment("Код ресторана")]
        public int RestaurantID { get; set; }

        /// <summary>
        /// Ресторан
        /// </summary>
        [Comment("Ресторан")]
        public Restaurant Restaurant { get; set; } = null!;

        /// <summary>
        /// Дата начала показа
        /// </summary>
        [Comment("Дата начала показа")]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Дата окончания показа
        /// </summary>
        [Comment("Дата окончания показа")]
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Код Состоянеие активности
        /// </summary>
        [Comment("Код Состоянеие активности")]
        public int Menu_OptionStateID { get; set; }

        /// <summary>
        /// Состоянеие активности
        /// </summary>
        [Comment("Состоянеие активности")]
        public Menu_OptionState_Def Menu_OptionState { get; set; } = null!;

        /// <summary>
        /// Дата и время удаления записи (NULL – активна, не NULL – удалена)
        /// </summary>
        [Comment("Дата и время удаления записи")]
        public DateTime? DeletedAt { get; set; }
    }
}