using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Блюдо
    /// </summary>
    [Table("Dishes")]
    [Comment("Блюда>")]
    public class Dish
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Категория блюда - код
        /// </summary>
        [Comment("Категория блюда - код")]
        public int Dish_CategoryID { get; set; }


        /// <summary>
        /// Категория блюда
        /// </summary>
        [Comment("Категория блюда")]
        public Dish_Category Dish_Category { get; set; } = null!;


        /// <summary>
        /// Наименование товара
        /// </summary>
        [Comment("Наименование товара")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Краткое описание блюда
        /// </summary>
        [Comment("Краткое описание блюда")]
        public string? Teaser { get; set; }


        /// <summary>
        /// Полное описание блюда
        /// </summary>
        [Comment("Полное описание блюда")]
        public string? Text { get; set; }


        /// <summary>
        /// Количество минут для приготовления блюда
        /// </summary>
        [Comment("Количество минут для приготовления блюда")]
        public int? Cooking_Minutes { get; set; }


        /// <summary>
        /// Стоимость
        /// </summary>
        [Precision(8, 2)]
        [Comment("Стоимость")]
        public decimal Price { get; set; }


        /// <summary>
        /// Флаг того, что этот товар кофе и для него нужно подгрузить шапочку напитка и менять цвет стакана
        /// </summary>
        [Comment("Флаг того, что этот товар кофе и для него нужно подгрузить шапочку напитка и менять цвет стакана")]
        public bool Flag_AllowColors { get; set; }


        /// <summary>
        /// Флаг того, что блюдо можно купить за бонусы
        /// </summary>
        [Comment("Флаг того, что блюдо можно купить за бонусы")]
        public bool Flag_Bonus { get; set; }


        /// <summary>
        /// Количество бонусов, начисляемых за покупку
        /// </summary>
        [Precision(8, 2)]
        [Comment("Количество бонусов, начисляемых за покупку")]
        public decimal? Bonuses { get; set; }


        /// <summary>
        /// Порядковый номер
        /// </summary>
        [Comment("Порядковый номер")]
        public int OrderNum { get; set; }


        /// <summary>
        /// Новинка
        /// </summary>
        [Comment("Новинка")]
        public bool FlagNew { get; set; }


        /// <summary>
        /// Объемы блюда с ценой
        /// </summary>
        [Comment("Объемы блюда с ценой")]
        public List<Dish_Volume>? Dish_Volumes { get; set; } //TODO: MAE - проверить точно ли тут нужен Nullable


        /// <summary>
        /// Ингридиенты блюда
        /// </summary>
        [Comment("Ингридиенты блюда")]
        public List<Dish_Ingridient>? Dish_Ingridients { get; set; }//TODO: MAE - проверить точно ли тут нужен Nullable


        /// <summary>
        /// Изображение блюда
        /// </summary>
        [Comment("Изображение блюда")]
        public byte[]? Image { get; set; }


        /// <summary>
        /// Дата изменения изображения
        /// </summary>
        [Comment("Дата изменения изображения")]
        public DateTime? Image_ChangeDate { get; set; }


        /// <summary>
        /// Коды рекомендуемых блюд(через ;)
        /// </summary>
        [Comment("Коды рекомендуемых блюд")]
        public string? RecommendDishIDs { get; set; }


        /// <summary>
        /// Отключить отображение блюда в основном меню
        /// </summary>
        [Comment("Отключить отображение блюда в основном меню")]
        public bool FlagNoActive { get; set; }


        /// <summary>
        /// Дата до которой отображается новинка
        /// </summary>
        [Comment("Дата до которой отображается новинка")]
        public DateTime? New_DateFrom { get; set; }


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