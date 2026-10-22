using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;

namespace HIDE.RestAndRant.Web_Admin.Models.Dishes
{
    /// <summary>
    /// Добавление\Изменение блюда
    /// </summary>
    public class Model_Dishes_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Категория блюда - код
        /// </summary>
        [Required(ErrorMessage = "Поле 'Категория блюда' обязательно к заполнению")]
        public int Dish_CategoryID { get; set; }

        /// <summary>
        /// Категория блюда
        /// </summary>
        public string? Dish_Category { get; set; }

        /// <summary>
        /// Наименование товара
        /// </summary>
        [Required(ErrorMessage = "Поле 'Наименование блюда' обязательно к заполнению")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Краткое описание блюда
        /// </summary>
        [DisplayName("Краткое описание блюда")]
        public string? Teaser { get; set; }

        /// <summary>
        /// Полное описание блюда
        /// </summary>
        [DisplayName("Полное описание блюда")]
        public string? Text { get; set; }

        /// <summary>
        /// Количество минут для приготовления блюда
        /// </summary>
        [DisplayName("Количество минут для приготовления блюда")]
        public int? Cooking_Minutes { get; set; }

        /// <summary>
        /// Коды рекомендуемых блюд(через ;)
        /// </summary>
        [Comment("Коды рекомендуемых блюд")]
        public string? RecommendDishIDs { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        [Required(ErrorMessage = "Поле 'Стоимость' обязательно к заполнению")]
        public decimal Price { get; set; }

        /// <summary>
        /// Флаг того, что этот товар кофе и для него нужно подгрузить шапочку напитка и менять цвет стакана
        /// </summary>
        public bool Flag_AllowColors { get; set; }

        /// <summary>
        /// Флаг того, что блюдо можно купить за бонусы
        /// </summary>
        public bool Flag_Bonus { get; set; }

        /// <summary>
        /// Изменить цену для всех ресторанов
        /// </summary>
        public bool FlagForAll { get; set; }

        /// <summary>
        /// Количество бонусов, начисляемых за покупку
        /// </summary>
        [Precision(8, 2)]
        public decimal? Bonuses { get; set; }

        /// <summary>
        /// Изображение
        /// </summary>
        public IFormFile? UploadImge { get; set; }

        /// <summary>
        /// Изображение товара
        /// </summary>
        public byte[]? Image { get; set; }

        /// <summary>
        /// Ссылка на изображение
        /// </summary>
        public string? Image_Url { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        [Required(ErrorMessage = "Поле 'Порядок отображения' обязательно к заполнению")]
        public int OrderNum { get; set; }

        /// <summary>
        /// Новинка
        /// </summary>
        public bool FlagNew { get; set; }

        /// <summary>
        /// Отключить отображение блюда в основном меню
        /// </summary>
        [DisplayName("Отключить отображение блюда в основном меню")]
        public bool FlagNoActive { get; set; }

        /// <summary>
        /// Список категорий блюд
        /// </summary>
        public List<CDish_Category>? List_Categories { get; set; }

        /// <summary>
        /// Список блюд
        /// </summary>
        public List<CDish_ForAdmin>? List_Dishes { get; set; }


        /// <summary>
        /// Дата до которой будет видна новинка
        /// </summary>
        public DateTime? New_DateFrom { get; set; }
    }
}
