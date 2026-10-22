using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HIDE.RestAndRant.Database.Definitions_ForMenu
{
    /// <summary>
    /// Подкатегории меню
    /// </summary>
    [Table("Menu_Subcategories")]
    [Comment("Подкатегории меню")]
    public class Menu_Subcategory_Def
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
        /// Код Привязан к категории
        /// </summary>
        [Comment("Код Привязан к категории")]
        public int Menu_CategoryID { get; set; }

        /// <summary>
        /// Привязан к категории
        /// </summary>
        [Comment("Привязан к категории")]
        public Menu_Category_Def Menu_Category { get; set; } = null!;

        /// <summary>
        /// Код Родитель
        /// </summary>
        [Comment("Код Родитель")]
        public int? ParentID { get; set; }

        /// <summary>
        /// Родитель
        /// </summary>
        [Comment("Родитель")]
        public Menu_Subcategory_Def? Parent { get; set; }

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