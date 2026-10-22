using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Dish_Volumes
{
    /// <summary>
    /// Добавление\Изменение объёма
    /// </summary>
    public class Model_Dish_Volumes_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        public int DishID { get; set; }


        /// <summary>
        /// Блюдо
        /// </summary>
        public string? Dish { get; set; }


        /// <summary>
        /// Объем
        /// </summary>
        public int VolumeID { get; set; }


        /// <summary>
        /// Объем
        /// </summary>
        public decimal Volume { get; set; }


        /// <summary>
        /// Цена за этот обьем
        /// </summary>
        [Required(ErrorMessage = "Поле 'Стоимость' обязательно к заполнению")]
        public decimal Price { get; set; }


        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int OrderNum { get; set; }


        /// <summary>
        /// Список объемов
        /// </summary>
        public List<CVolume>? List_Volumes { get; set; }

    }
}
