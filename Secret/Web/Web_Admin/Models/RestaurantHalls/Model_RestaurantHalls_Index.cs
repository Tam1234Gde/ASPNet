using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin.Models.RestaurantHalls;

/// <summary>
/// Залы в ресторане
/// </summary>
public class Model_RestaurantHalls_Index
{
    /// <summary>
    /// Ресторан
    /// </summary>
    public CRestaurant? Restaurant { get; set; }

    /// <summary>
    /// Список залов в ресторане
    /// </summary>
    public List<CRestaurant_Hall>? List_Restaurant_Halls { get; set; }

}