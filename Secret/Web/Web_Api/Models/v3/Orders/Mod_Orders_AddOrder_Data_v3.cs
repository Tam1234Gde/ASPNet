namespace HIDE.RestAndRant.Web_Api.Models.v3.Orders
{
    /// <summary>
    /// Данные заказа
    /// </summary>
    public class Mod_Orders_AddOrder_Data_v3
    {
        /// <summary>
        /// Код товара
        /// </summary>
        public int DishID { get; set; }


        /// <summary>
        /// Код обьема заказа
        /// </summary>
        public int? Dish_VolumeID { get; set; }


        /// <summary>
        /// Количество товара
        /// </summary>
        public decimal Count { get; set; }


        /// <summary>
        /// Цена за единицу товара
        /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// Общая сумма за указанное количество товара (Count*Price)
        /// </summary>
        public decimal Summ { get; set; }


        /// <summary>
        /// Код цвета товара
        /// </summary>
        public int Dish_ColorID { get; set; }

    }
}