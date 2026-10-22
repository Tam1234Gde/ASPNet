using Microsoft.EntityFrameworkCore;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.RestaurantHalls
{
    /// <summary>
    /// Залы в ресторане
    /// </summary>
    public class Model_RestaurantHalls_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Название
        /// </summary>
        [Comment("Название")]
        public string? Name { get; set; }


        /// <summary>
        /// Код ресторана
        /// </summary>
        [Comment("Код ресторана")]
        public int RestaurantID { get; set; }


        /// <summary>
        /// Тип столика - код
        /// </summary>
        [Comment("Тип столика - код")]
        public ERestaurant_Table_Type? Restaurant_Table_TypeID { get; set; }


        /// <summary>
        /// Планировка
        /// </summary>
        [Comment("Планировка")]
        public byte[]? Restaurant_Image { get; set; }


        /// <summary>
        /// Изображение
        /// </summary>
        public IFormFile? UploadImge { get; set; }


        /// <summary>
        /// True - Активен
        /// </summary>
        public bool Flag_Active { get; set; }


        /// <summary>
        /// Список ресторанов
        /// </summary>
        public List<CRestaurant>? List_Restaurants { get; set; }


        /// <summary>
        /// Список типов столика
        /// </summary>
        public List<CCommon>? List_Restaurant_Table_Types { get; set; }

    }
}