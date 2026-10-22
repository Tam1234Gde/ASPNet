using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;

namespace HIDE.RestAndRant.Web_Admin.Models.Dish_Categories
{
    /// <summary>
    /// Добавление\Изменение категории блюд
    /// </summary>
    public class Model_Dish_Categories_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование категории
        /// </summary>
        [Required(ErrorMessage = "Поле 'Наименование категории' обязательно к заполнению")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Количество минут для приготовления блюд в данной категории
        /// </summary>
        [DisplayName("Количество минут для приготовления блюд в данной категории")]
        public int? Cooking_Minutes { get; set; }

        /// <summary>
        /// Флаг того, что в категории у товаров разрешены цвета
        /// </summary>
        public bool Flag_AllowColors { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        [Required(ErrorMessage = "Поле 'Порядок отображения' обязательно к заполнению")]
        public int OrderNum { get; set; }

        /// <summary>
        /// Часы работы
        /// </summary>
        [Comment("Часы работы")]
        public int[]? Work_Hours { get; set; } = null;

        /// <summary>
        /// Флаг того, что эта категория родительская
        /// </summary>
        [Comment("Родительская категория")]
        public bool Flag_IsParent { get; set; }

        /// <summary>
        /// Идентификатор родителя
        /// </summary>
        public int? ParentID { get; set; }


        /// <summary>
        /// Список родительских категорий
        /// </summary>
        public List<CDish_Category>? List_ParentCategories { get; set; }


        /// <summary>
        /// Использовать время работы из общих настроек
        /// </summary>
        public bool Flag_UseDefaultWorkHours { get; set; }
    }
}
