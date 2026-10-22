using System.ComponentModel;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.Clients;

/// <summary>
/// Клиент
/// </summary>
public class Model_Client : CClient
{
    /// <summary>
    /// Бонусы
    /// </summary>
    [DisplayName("Бонусы")]
    public decimal? Bonuses { get; set; }

}