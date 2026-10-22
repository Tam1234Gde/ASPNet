using System;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Ингредиент меню
    /// </summary>
    public class CMenu_Ingridient
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Изображение
        /// </summary>
        public byte[]? Image { get; set; }

        /// <summary>
        /// Дата изменения изображения
        /// </summary>
        public DateTime? Image_ChangeDate { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public int? Price { get; set; }

        /// <summary>
        /// Флаг: цена была переопределена вручную
        /// </summary>
        public bool Flag_Price_Overridden { get; set; }

        /// <summary>
        /// Код меню
        /// </summary>
        public int MenuID { get; set; }

        /// <summary>
        /// Код группы ингредиентов
        /// </summary>
        public int Menu_Ingridient_GroupID { get; set; }

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public int OrderNum { get; set; }

        /// <summary>
        /// Код состояния активности
        /// </summary>
        public int Menu_OptionStateID { get; set; }
    }
}