using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIDE.RestAndRant.Common.ENums;

namespace HIDE.RestAndRant.Database.Definitions
{
    /// <summary>
    /// Тип карты: MasterCard, Visa, Mir, UnionPay, JCB, AmericanExpress, DinersClub,...
    /// </summary>
    public class ClientPay_Card_Type
    {
        /// <summary>
        /// Код
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] //Отключена автоинкрементация поля
        public EClientPay_Card_Type ID { get; set; }


        /// <summary>
        /// Наименование карты для отображения
        /// </summary>
        public string Name { get; set; } = null!;


        /// <summary>
        /// Системное имя карты 
        /// (будет возвращаться от платежного сервиса, например: MasterCard, Visa, Mir, UnionPay, JCB, AmericanExpress, DinersClub,...)
        /// </summary>
        public string SName { get; set; } = null!;

    }
}