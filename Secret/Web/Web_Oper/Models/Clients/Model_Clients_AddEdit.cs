using System.ComponentModel.DataAnnotations;

namespace HIDE.RestAndRant.Web_Oper.Models.Clients
{
    /// <summary>
    /// Добавление\Изменение клиента
    /// </summary>
    public class Model_Clients_AddEdit
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Телефон - используется как логин
        /// </summary>
        [Required(ErrorMessage = "Поле 'Телефон' обязательно к заполнению")]
        public string Phone { get; set; } = null!;

        /// <summary>
        /// Имя
        /// </summary>
        [Required(ErrorMessage = "Поле 'Имя' обязательно к заполнению")]
        public string? Name { get; set; } = null!;

        /// <summary>
        /// Данные банковской карты
        /// </summary>
        public string? CardData { get; set; }

        /// <summary>
        /// Количество текущих бонусов у клиента (каждая 10 кружка в подарок)
        /// </summary>
        public int Bonuses { get; set; }

        /// <summary>
        /// Смс код
        /// </summary>
        public string SmsCode { get; set; } = null!;

        /// <summary>
        /// Guid пользователя
        /// </summary>
        public string Guid { get; set; } = null!;

        /// <summary>
        /// Дата последнего входа в приложение
        /// </summary>
        public DateTime Date_LastAction { get; set; }

        /// <summary>
        /// Дата последнего заказа
        /// </summary>
        public DateTime? Date_LastZakaz { get; set; }

        /// <summary>
        /// Флаг пометки на удаление
        /// </summary>
        public bool Flag_Del { get; set; }

        /// <summary>
        /// Дата пометки на удаление
        /// </summary>
        public DateTime? Date_Del { get; set; }

        /// <summary>
        /// Пользователь, который поставил запись на удаление
        /// </summary>
        public int? UserID_Del { get; set; }

    }
}
