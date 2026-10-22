using System;
using System.Collections.Generic;
using System.Text;

namespace HIDE.RestAndRant.Common.ENums
{
    /// <summary>
    /// Состояния активности для таблицы настроек нового меню
    /// </summary>
    public enum EMenu_OptionStates
    {
        /// <summary>
        /// Всегда активно
        /// </summary>
        Enabled = 1,


        /// <summary>
        /// Настраивается
        /// </summary>
        Optional = 2,


        /// <summary>
        /// Всегда отключено
        /// </summary>
        Disabled = 3,

    }
}
