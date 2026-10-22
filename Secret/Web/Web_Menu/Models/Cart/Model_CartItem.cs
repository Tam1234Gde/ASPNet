using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Site_Menu.Models.Cart;

/// <summary>
/// Корзина - Вывод результатов
/// </summary>
public class Model_CartItem
{
    /// <summary>
    /// Код
    /// </summary>
    public int ID { get; set; }


    /// <summary>
    /// Код блюда
    /// </summary>
    public int DishID { get; set; }


    /// <summary>
    /// Стоимость блюда
    /// </summary>
    public decimal Dish_Price { get; set; }


    /// <summary>
    /// Название блюда
    /// </summary>
    public string Dish_Name { get; set; } = null!;


    /// <summary>
    /// Количество блюд
    /// </summary>
    public decimal Dish_Count { get; set; }


    /// <summary>
    /// Изображение блюда
    /// </summary>
    public byte[]? Dish_Image { get; set; }


    /// <summary>
    /// Код выбранного объёма
    /// </summary>
    public int? Dish_VolumeID { get; set; }


    /// <summary>
    /// Выбранные ингредиенты блюда
    /// </summary>
    public List<int>? Dish_Ingredient_IDs { get; set; }


    /// <summary>
    /// Список объемов у блюда
    /// </summary>
    [DisplayName("Список объёмов")]
    public List<CDish_Volume>? List_Dish_Volumes { get; set; }


    /// <summary>
    /// Общая стоимость(с учетом объёма и ингредиентов)
    /// </summary>
    public decimal? Sum { get; set; }


    /// <summary>
    /// Количество минут для приготовления блюда
    /// </summary>
    [Comment("Количество минут для приготовления блюда")]
    public int? Dish_Cooking_Minutes { get; set; }

}