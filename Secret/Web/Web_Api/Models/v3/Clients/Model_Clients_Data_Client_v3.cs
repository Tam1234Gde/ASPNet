using Microsoft.EntityFrameworkCore;

namespace HIDE.RestAndRant.Web_Api.Models.v3.Clients
{
    /// <summary>
    /// Модель данных клиента
    /// </summary>
    public class Model_Clients_Data_Client_v3
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Guid клиента
        /// </summary>
        public string Guid { get; set; } = null!;


        /// <summary>
        /// Телефон клиента
        /// </summary>
        public string Phone { get; set; } = null!;


        /// <summary>
        /// Имя клиента
        /// </summary>
        public string? Name { get; set; } = null!;


        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? Date_Birth { get; set; }


        /// <summary>
        /// Количество текущих бонусов у клиента (каждая 10 кружка в подарок)
        /// </summary>
        public int Bonuses { get; set; }


        /// <summary>
        /// Дата регистрации
        /// </summary>
        public DateTime? Date_Registration { get; set; }


        /// <summary>
        /// Политика принята
        /// </summary>
        public bool Flag_IsPolicy { get; set; }


        /// <summary>
        /// Имя - Другому человеку
        /// </summary>
        [Comment("Имя - Другому человеку")]
        public string? Name_Another { get; set; }


        /// <summary>
        /// Телефон - Другому человеку
        /// </summary>
        [Comment("Телефон - Другому человеку")]
        public string? Phone_Another { get; set; }


        /// <summary>
        /// Марка машины
        /// </summary>
        [Comment("Марка машины")]
        public string? Car_Brand { get; set; }


        /// <summary>
        /// Госномера машины
        /// </summary>
        [Comment("Госномера машины")]
        public string? Car_RegNumber { get; set; }


        /// <summary>
        /// Цвет машины
        /// </summary>
        [Comment("Цвет машины")]
        public string? Car_Color { get; set; }


        /// <summary>
        /// Адрес доставки
        /// </summary>
        [Comment("Адрес доставки")]
        public string? Delivery_Address { get; set; }


        /// <summary>
        /// Квартира
        /// </summary>
        [Comment("Квартира")]
        public string? Delivery_Apartment { get; set; }


        /// <summary>
        /// Домофон
        /// </summary>
        [Comment("Домофон")]
        public string? Delivery_EntranceCode { get; set; }


        /// <summary>
        /// Номер подъезда
        /// </summary>
        [Comment("Номер подъезда")]
        public string? Delivery_EntranceNumber { get; set; }


        /// <summary>
        /// Номер этажа
        /// </summary>
        [Comment("Номер этажа")]
        public string? Delivery_Floor { get; set; }

    }
}