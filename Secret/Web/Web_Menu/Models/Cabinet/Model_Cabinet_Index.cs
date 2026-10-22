using HIDE.RestAndRant.Business.Definitions_ForAdmin;

namespace HIDE.RestAndRant.Web_Site_Menu.Models.Cabinet;

/// <summary>
/// Личный кабинет
/// </summary>
public class Model_Cabinet_Index
{
    /// <summary>
    /// Активные заказы
    /// </summary>
    public List<COrder_ForAdmin>? List_Orders { get; set; }

}