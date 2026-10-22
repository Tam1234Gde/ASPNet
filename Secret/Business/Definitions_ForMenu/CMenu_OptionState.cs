using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Состояние активности меню
    /// </summary>
    public class CMenu_OptionState
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Значение состояния (enum)
        /// </summary>
        public EMenu_OptionStates Value { get; set; }

        /// <summary>
        /// Человекочитаемое название состояния на русском
        /// </summary>
        public string DisplayName
        {
            get
            {
                return Value switch
                {
                    EMenu_OptionStates.Enabled => "Включено",
                    EMenu_OptionStates.Optional => "Необязательно",
                    EMenu_OptionStates.Disabled => "Отключено",
                    _ => "Неизвестно"
                };
            }
        }

        /// <summary>
        /// CSS-класс бейджа для отображения состояния в списках
        /// </summary>
        public string BadgeClass
        {
            get
            {
                return Value switch
                {
                    EMenu_OptionStates.Enabled => "badge-success",
                    EMenu_OptionStates.Optional => "badge-warning",
                    EMenu_OptionStates.Disabled => "badge-danger",
                    _ => "badge-secondary"
                };
            }
        }
    }
}