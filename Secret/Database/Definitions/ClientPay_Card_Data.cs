using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Данные карты клиента
    /// </summary>
    [Table("ClientPay_Card_Datas")]
    [Comment("Данные карты клиента")]
    [Index(nameof(ClientID), nameof(CardIdent), IsUnique = true)]
    public class ClientPay_Card_Data
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [Comment("Код")]
        public int ID { get; set; }


        /// <summary>
        /// Клиент - Код
        /// </summary>
        [Comment("Клиент - Код")]
        public int ClientID { get; set; }


        /// <summary>
        /// Клиент
        /// </summary>
        [Comment("Клиент")]
        public Client Client { get; set; } = null!;


        /// <summary>
        /// Идентификатор карты, полученный от платежного сервиса
        /// </summary>
        [Comment("Идентификатор карты, полученный от платежного сервиса")]
        public string CardIdent { get; set; } = null!;


        /// <summary>
        /// Тип карты - код
        /// </summary>
        [Comment("Тип карты - код")]
        public EClientPay_Card_Type? ClientPay_Card_TypeID { get; set; }


        /// <summary>
        /// Тип карты
        /// </summary>
        [Comment("Тип карты")]
        public ClientPay_Card_Type? ClientPay_Card_Type { get; set; }


        /// <summary>
        /// Последние 4 номера карты
        /// </summary>
        [Comment("Последние 4 номера карты")]
        public string CardLastNum { get; set; } = null!;


        /// <summary>
        /// Дата обновления записи
        /// </summary>
        [Comment("Дата обновления записи")]
        public DateTime Date_Update { get; set; }


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

    }
}