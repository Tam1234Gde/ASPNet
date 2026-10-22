using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Database.Definitions;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Категория блюда
    /// </summary>
    public class CDish_Category
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Наименование категории
        /// </summary>
        [DisplayName("Наименование категории")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Количество минут для приготовления блюд в данной категории
        /// </summary>
        [DisplayName("Наименование категории")]
        public int? Cooking_Minutes { get; set; }


        /// <summary>
        /// Флаг того, что в категории у товаров разрешены цвета
        /// </summary>
        [DisplayName("Для товаров разрешены цвета")]
        public bool Flag_AllowColors { get; set; }


        /// <summary>
        /// Порядковый номер
        /// </summary>
        [DisplayName("Порядковый номер")]
        public int OrderNum { get; set; }


        /// <summary>
        /// Часы работы
        /// </summary>
        [Comment("Часы работы")]
        public int[]? Work_Hours { get; set; }


        /// <summary>
        /// Флаг родительской категории
        /// </summary>
        [Comment("Флаг родительской категории")]
        public bool Flag_IsParent { get; set; }


        /// <summary>
        /// Идентификатор родителя
        /// </summary>
        public int? ParentID { get; set; }


        /// <summary>
        /// Родительская категория
        /// </summary>
        [ForeignKey(name: nameof(ParentID))]
        public Dish_Category? Parent { get; set; }


        /// <summary>
        /// Список блюд 
        /// </summary>
        [Comment("Список блюд")]
        public List<CDish>? Dishes { get; set; }


        /// <summary>
        /// Наименование родительской категории
        /// </summary>
        [Comment("Наименование родительской категории")]
        public string? ParentCategory_Name { get; set; }


        /// <summary>
        /// Категория удалена
        /// </summary>
        [Comment("Категория удалена")]
        public bool ParentCategory_IsDeleted { get; set; }


        /// <summary>
        /// Флаг удаления
        /// </summary>
        [Comment("Флаг удаления")]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Флаг - использовать ли общие часы работы из настроек
        /// </summary>
        [Comment("Флаг - использовать ли общие часы работы из настроек")]
        public bool Flag_UseDefaultWorkHours { get; set; }
    }
}