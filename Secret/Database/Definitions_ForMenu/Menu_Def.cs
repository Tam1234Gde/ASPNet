using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions_ForMenu
{
    /// <summary>
    /// Меню
    /// </summary>
    [Table("Menus")]
    [Comment("Меню")]
    public class Menu_Def
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Comment("Наименование")]
        public string Name { get; set; } = null!;

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