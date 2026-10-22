using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace HIDE.RestAndRant.Business.Definitions_ForAdmin
{
    /// <summary>
    /// Категория блюд
    /// </summary>
    public class CDish_Category_ForAdmin
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Наименование категории
        /// </summary>
        [DisplayName("Наименование категории")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Количество минут для приготовления блюд в данной категории
        /// </summary>
        [DisplayName("Количество минут для приготовления")]
        public int? Cooking_Minutes { get; set; }


        /// <summary>
        /// Флаг того, что в категории у товаров разрешены цвета
        /// </summary>
        [DisplayName("Разрешены цвета")]
        public bool Flag_AllowColors { get; set; }


        /// <summary>
        /// Порядковый номер
        /// </summary>
        [DisplayName("Порядковый номер")]
        public int OrderNum { get; set; }


        /// <summary>
        /// Список блюд
        /// </summary>
        [Comment("Список блюд")]
        [DisplayName("Список блюд")]
        public List<CDish_ForAdmin>? Dishes { get; set; }
    }
}