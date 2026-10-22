using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HIDE.RestAndRant.Database.Definitions_ForMenu
{
    /// <summary>
    /// Блюда меню
    /// </summary>
    [Table("Menu_Dishes")]
    [Comment("Блюда меню")]
    public class Menu_Dish_Def
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }

        /// <summary>
        /// Код Подкатегория
        /// </summary>
        [Comment("Код Подкатегория")]
        public int Menu_SubcategoryID { get; set; }

        /// <summary>
        /// Подкатегория
        /// </summary>
        [Comment("Подкатегория")]
        public Menu_Subcategory_Def Menu_Subcategory { get; set; } = null!;

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
        /// Объем 1
        /// </summary>
        [Comment("Объем 1")]
        public int? Vol1 { get; set; }

        /// <summary>
        /// Объем 1 цена
        /// </summary>
        [Comment("Объем 1 цена")]
        public int? Vol1_Price { get; set; }

        /// <summary>
        /// Объем 2
        /// </summary>
        [Comment("Объем 2")]
        public int? Vol2 { get; set; }

        /// <summary>
        /// Объем 2 цена
        /// </summary>
        [Comment("Объем  цена")]
        public int? Vol2_Price { get; set; }

        /// <summary>
        /// Объем 3
        /// </summary>
        [Comment("Объем 3")]
        public int? Vol3 { get; set; }

        /// <summary>
        /// Объем 3 цена
        /// </summary>
        [Comment("Объем 3 цена")]
        public int? Vol3_Price { get; set; }

        /// <summary>
        /// Объем 4
        /// </summary>
        [Comment("Объем 4")]
        public int? Vol4 { get; set; }

        /// <summary>
        /// Объем 4 цена
        /// </summary>
        [Comment("Объем 4 цена")]
        public int? Vol4_Price { get; set; }

        /// <summary>
        /// Объем 5
        /// </summary>
        [Comment("Объем 5")]
        public int? Vol5 { get; set; }

        /// <summary>
        /// Объем 5 цена
        /// </summary>
        [Comment("Объем 5 цена")]
        public int? Vol5_Price { get; set; }

        /// <summary>
        /// Код Состоянеие активности
        /// </summary>
        [Comment("Код Состоянеие активности")]
        public int Menu_OptionStateID { get; set; }

        /// <summary>
        /// Состоянеие активности
        /// </summary>
        [Comment("Состоянеие активности")]
        public Menu_OptionState_Def Menu_OptionState { get; set; } = null!;

        /// <summary>
        /// Дата и время удаления записи (NULL – активна, не NULL – удалена)
        /// </summary>
        [Comment("Дата и время удаления записи")]
        public DateTime? DeletedAt { get; set; }
    }
}