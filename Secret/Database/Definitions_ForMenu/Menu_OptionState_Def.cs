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
    /// Словарь состояния активности для таблицы настроек нового меню
    /// </summary>
    [Table("Menu_OptionStates")]
    [Comment("Словарь состояния активности для таблицы настроек нового меню")]
    public class Menu_OptionState_Def
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Значение
        /// </summary>
        public EMenu_OptionStates Value { get; set; }
    }
}
