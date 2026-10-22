using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HIDE.RestAndRant.Database.Definitions_ForMenu
{
    /// <summary>
    /// Категории меню
    /// </summary>
    [Table("Menu_Categories")]
    [Comment("Категории меню")]
    public class Menu_Category_Def
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
        /// Дата и время удаления записи (NULL – активна, не NULL – удалена)
        /// </summary>
        [Comment("Дата и время удаления записи")]
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        [Comment("Порядок сортировки")]
        public int OrderNum { get; set; }
    }
}