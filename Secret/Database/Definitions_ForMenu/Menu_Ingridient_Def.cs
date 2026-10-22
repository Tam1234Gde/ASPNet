using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HIDE.RestAndRant.Database.Definitions_ForMenu
{
    /// <summary>
    /// Ингредиенты меню
    /// </summary>
    [Table("Menu_Ingridients")]
    [Comment("Ингредиенты меню")]
    public class Menu_Ingridient_Def
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }

        /// <summary>
        /// Наименование ингредиента
        /// </summary>
        [Comment("Наименование ингредиента")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Изображение ингредиента
        /// </summary>
        [Comment("Изображение ингредиента")]
        public byte[]? Image { get; set; }

        /// <summary>
        /// Дата изменения изображения
        /// </summary>
        [Comment("Дата изменения изображения")]
        public DateTime? Image_ChangeDate { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        [Comment("Цена")]
        public int? Price { get; set; }

        /// <summary>
        /// Флаг: цена была переопределена вручную.
        /// Если TRUE — при массовом применении цены группы ингредиент пропускается
        /// (если пользователь не снял соответствующий чекбокс).
        /// </summary>
        [Comment("Цена была переопределена вручную")]
        public bool Flag_Price_Overridden { get; set; }

        /// <summary>
        /// Код Привязан к меню
        /// </summary>
        [Comment("Код Привязан к меню")]
        public int? MenuID { get; set; }

        /// <summary>
        /// Привязан к меню
        /// </summary>
        [Comment("Привязан к меню")]
        public Menu_Def? Menu { get; set; } = null!;

        /// <summary>
        /// Код Привязан к группе
        /// </summary>
        [Comment("Код Привязан к группе")]
        public int Menu_Ingridient_GroupID { get; set; }

        /// <summary>
        /// Привязан к группе
        /// </summary>
        [Comment("Привязан к группе")]
        public Menu_Ingridient_Group_Def Menu_Ingridient_Group { get; set; } = null!;

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


        /// <summary>
        /// Порядок сортировки
        /// </summary>
        [Comment("Порядок сортировки")]
        public int OrderNum { get; set; }


    }
}