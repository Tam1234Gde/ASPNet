using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Категория блюда
    /// </summary>
    [Table("Dish_Categories")]
    [Comment("Категории блюд")]
    public class Dish_Category
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Наименование категории
        /// </summary>
        [Comment("Наименование категории")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Количество минут для приготовления блюд в данной категории
        /// </summary>
        [Comment("Количество минут для приготовления блюд в данной категории")]
        public int? Cooking_Minutes { get; set; }


        /// <summary>
        /// Порядковый номер
        /// </summary>
        [Comment("Порядковый номер")]
        public int OrderNum { get; set; }


        /// <summary>
        /// Флаг того, что в категории у товаров разрешены цвета
        /// </summary>
        [Comment("Флаг того, что в категории у товаров разрешены цвета")]
        public bool Flag_AllowColors { get; set; }


        /// <summary>
        /// Часы работы
        /// </summary>
        [Comment("Часы работы")]
        public int[]? Work_Hours { get; set; }


        /// <summary>
        /// Флаг - Родитель
        /// </summary>
        [Comment("Флаг - Родитель")]
        public bool Flag_IsParent { get; set; }


        /// <summary>
        /// Идентификатор родителя
        /// </summary>
        [Comment("Идентификатор родителя")]
        public int? ParentID { get; set; }


        /// <summary>
        /// Родительская категория
        /// </summary>
        [Comment("Родительская категория")]
        [ForeignKey(name: nameof(ParentID))]
        public Dish_Category? Parent { get; set; }


        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        [Comment("Флаг пометки на удаление")]
        public bool Flag_Del { get; set; }


        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        [Comment("Дата пометки на удаление")]
        public DateTime? Date_Del { get; set; }


        /// <summary>
        /// Пользователь, который поставил запись на удаление
        /// </summary>
        [Comment("Пользователь, который поставил запись на удаление")]
        public int? UserID_Del { get; set; }


        /// <summary>
        /// Список блюд 
        /// </summary>
        [Comment("Список блюд")]
        public List<Dish>? Dishes { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable


        /// <summary>
        /// Флаг - использовать ли общие часы работы из настроек
        /// </summary>
        [Comment("Флаг - использовать ли общие часы работы из настроек")]
        public bool Flag_UseDefaultWorkHours { get; set; }
    }
}