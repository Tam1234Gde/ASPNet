using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Ингредиент для блюд
    /// </summary>
    [Table("Ingridients")]
    [Comment("Ингредиенты для блюда")]
    public class Ingridient
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
        /// ID категории, к которой принадлежит
        /// </summary>
        [Comment("ID категории, к которой принадлежит")]
        public int? Category_ID { get; set; }


        /// <summary>
        /// Категория, к которой принадлежит
        /// </summary>
        [Comment("Категория, к которой принадлежит")]
        public Dish_Ingridient_Category? Category { get; set; }


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

    }
}