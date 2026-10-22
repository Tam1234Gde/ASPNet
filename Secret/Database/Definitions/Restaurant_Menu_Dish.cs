using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Блюда меню ресторана
    /// </summary>
    [Table("Restaurant_Menu_Dishes")]
    [Comment("Меню ресторана>")]
    public class Restaurant_Menu_Dish
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Код меню
        /// </summary>
        [Comment("Код меню")]
        public int Restaurant_MenuID { get; set; }


        /// <summary>
        /// Меню
        /// </summary>
        [Comment("Меню")]
        public Restaurant_Menu Restaurant_Menu { get; set; } = null!;


        /// <summary>
        /// Код блюда
        /// </summary>
        [Comment("Код блюда")]
        public int DishID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        [Comment("Блюдо")]
        public Dish Dish { get; set; } = null!;


        /// <summary>
        /// Стоимость блюда
        /// </summary>
        [Comment("Стоимость блюда")]
        [Precision(8, 2)]
        public decimal? Price { get; set; }


        /// <summary>
        /// Флаг доступности
        /// </summary>
        [Comment("Флаг доступности")]
        public bool Flag_Available { get; set; }


        /// <summary>
        /// Новинка
        /// </summary>
        [Comment("Новинка")]
        public bool FlagNew { get; set; }


        /// <summary>
        /// Порядковый номер
        /// </summary>
        [Comment("Порядковый номер")]
        public int OrderNum { get; set; }


        /// <summary>
        /// Объемы блюда с ценой
        /// </summary>
        [Comment("Объемы блюда с ценой")]
        public List<Restaurant_Menu_Dish_Volume>? Restaurant_Menu_Dish_Volumes { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable


        /// <summary>
        /// Ингридиенты блюда
        /// </summary>
        [Comment("Ингридиенты блюда")]
        public List<Restaurant_Menu_Dish_Ingredient>? Restaurant_Menu_Dish_Ingredients { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable


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