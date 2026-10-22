using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Web_Oper.Models.Bookings
{
    /// <summary>
    /// Добавление\Изменение бронирования заказа
    /// </summary>
    public class Model_Bookings_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Тип столика - код
        /// </summary>
        [Comment("Тип столика - код")]
        public ERestaurant_Table_Type? Restaurant_Table_TypeID { get; set; }

        /// <summary>
        /// Дата добавления записи
        /// </summary>
        [DisplayName("Дата добавления записи")]
        public DateTime Date_Action { get; set; }

        /// <summary>
        /// Код клиента
        /// </summary>
        [DisplayName("Код клиента")]
        public int? ClientID { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        [DisplayName("Клиент")]
        public string? Client { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        [DisplayName("Телефон")]
        public string? Phone { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [DisplayName("Телефон")]
        public string? Name { get; set; }

        /// <summary>
        /// VIP - На сколько бронируют
        /// </summary>
        [DisplayName("VIP - На сколько бронируют")]
        public int? Vip_Time { get; set; }

        /// <summary>
        /// Код стола
        /// </summary>
        [DisplayName("Код стола")]
        public int Restaurant_TableID { get; set; }

        /// <summary>
        /// Стол
        /// </summary>
        [DisplayName("Стол")]
        public string? Restaurant_Table { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        [DisplayName("Дата начала")]
        [Required(ErrorMessage = "Поле 'Укажите время' обязательно к заполнению")]
        public string Date_Start { get; set; } = null!;

        /// <summary>
        /// Дата окончания
        /// </summary>
        [DisplayName("Дата окончания")]
        public string? Date_End { get; set; }

        /// <summary>
        /// Планировка помещения
        /// </summary>
        public byte[]? Image { get; set; }

        /// <summary>
        /// Список клиентов
        /// </summary>
        public List<CClient>? List_Clients { get; set; }

        /// <summary>
        /// Список столов
        /// </summary>
        public List<CRestaurant_Table>? List_Restaurant_Tables { get; set; }

        /// <summary>
        /// Список времени
        /// </summary>
        public List<DateTime>? List_Times { get; set; }

        /// <summary>
        /// Список типов столов
        /// </summary>
        public List<CCommon>? List_RestaurantTable_Types { get; set; }

    }
}