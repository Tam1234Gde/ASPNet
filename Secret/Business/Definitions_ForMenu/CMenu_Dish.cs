using System;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Блюдо меню
    /// </summary>
    public class CMenu_Dish
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Код подкатегории
        /// </summary>
        public int Menu_SubcategoryID { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Краткое описание
        /// </summary>
        public string? Teaser { get; set; }

        /// <summary>
        /// Полное описание
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Время приготовления (минуты)
        /// </summary>
        public int? Cooking_Minutes { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Флаг разрешения цветов
        /// </summary>
        public bool Flag_AllowColors { get; set; }

        /// <summary>
        /// Флаг покупки за бонусы
        /// </summary>
        public bool Flag_Bonus { get; set; }

        /// <summary>
        /// Количество бонусов
        /// </summary>
        public decimal? Bonuses { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int OrderNum { get; set; }

        /// <summary>
        /// Новинка
        /// </summary>
        public bool FlagNew { get; set; }

        /// <summary>
        /// Изображение
        /// </summary>
        public byte[]? Image { get; set; }

        /// <summary>
        /// Дата изменения изображения
        /// </summary>
        public DateTime? Image_ChangeDate { get; set; }

        /// <summary>
        /// Коды рекомендуемых блюд
        /// </summary>
        public string? RecommendDishIDs { get; set; }

        /// <summary>
        /// Отключить отображение
        /// </summary>
        public bool FlagNoActive { get; set; }

        /// <summary>
        /// Дата до которой отображается новинка
        /// </summary>
        public DateTime? New_DateFrom { get; set; }

        /// <summary>
        /// Объем 1
        /// </summary>
        public int? Vol1 { get; set; }

        /// <summary>
        /// Объем 1 цена
        /// </summary>
        public int? Vol1_Price { get; set; }

        /// <summary>
        /// Объем 2
        /// </summary>
        public int? Vol2 { get; set; }

        /// <summary>
        /// Объем 2 цена
        /// </summary>
        public int? Vol2_Price { get; set; }

        /// <summary>
        /// Объем 3
        /// </summary>
        public int? Vol3 { get; set; }

        /// <summary>
        /// Объем 3 цена
        /// </summary>
        public int? Vol3_Price { get; set; }

        /// <summary>
        /// Объем 4
        /// </summary>
        public int? Vol4 { get; set; }

        /// <summary>
        /// Объем 4 цена
        /// </summary>
        public int? Vol4_Price { get; set; }

        /// <summary>
        /// Объем 5
        /// </summary>
        public int? Vol5 { get; set; }

        /// <summary>
        /// Объем 5 цена
        /// </summary>
        public int? Vol5_Price { get; set; }

        /// <summary>
        /// Код состояния активности
        /// </summary>
        public int Menu_OptionStateID { get; set; }


        /// <summary>
        /// Список ингредиентов блюда
        /// </summary>
        public List<CMenu_Dish_Ingridient>? List_Dish_Ingridients { get; set; }
    }
}