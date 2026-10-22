using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HIDE.RestAndRant.Database.Definitions_ForMenu
{
    /// <summary>
    /// Группы ингредиентов меню
    /// </summary>
    [Table("Menu_Ingridient_Groups")]
    [Comment("Группы ингредиентов меню")]
    public class Menu_Ingridient_Group_Def
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
        /// На выбор одна
        /// </summary>
        [Comment("На выбор одна")]
        public bool IsSelectable { get; set; }


        /// <summary>
        /// Цена по умолчанию для новых ингредиентов группы
        /// </summary>
        /// <remarks>
        /// Используется как значение по умолчанию при создании нового ингредиента в этой группе.
        /// Не влияет на уже существующие ингредиенты — для массового обновления есть отдельное действие.
        /// </remarks>
        [Comment("Цена по умолчанию для новых ингредиентов группы")]
        public int? Default_Price { get; set; }


        /// <summary>
        /// Код Привязан к меню
        /// </summary>
        [Comment("Код Привязан к меню")]
        public int? MenuID { get; set; }


        /// <summary>
        /// Привязан к меню
        /// </summary>
        [Comment("Привязан к меню")]
        [ForeignKey(nameof(MenuID))]
        public Menu_Def? Menu { get; set; } = null!;


        /// <summary>
        /// Код Состоянеие активности
        /// </summary>
        [Comment("Код Состоянеие активности")]
        public int Menu_OptionStateID { get; set; }


        /// <summary>
        /// Состоянеие активности
        /// </summary>
        [Comment("Состоянеие активности")]
        [ForeignKey(nameof(Menu_OptionStateID))]
        public Menu_OptionState_Def Menu_OptionState { get; set; } = null!;


        /// <summary>
        /// Порядок сортировки
        /// </summary>
        [Comment("Порядок сортировки")]
        public int OrderNum { get; set; }
    }
}