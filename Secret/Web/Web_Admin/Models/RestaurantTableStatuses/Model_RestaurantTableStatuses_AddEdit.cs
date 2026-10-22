using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIDE.RestAndRant.Web_Admin.Models.RestaurantTableStatuses;

/// <summary>
/// CRUD - Статусы столиков
/// </summary>
public class Model_RestaurantTableStatuses_AddEdit
{
    /// <summary>
    /// Код
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Comment("Код")]
    public int ID { get; set; }


    /// <summary>
    /// Наименование
    /// </summary>
    [Comment("Наименование")]
    public string Name { get; set; } = null!;


    /// <summary>
    /// Изображение
    /// </summary>
    public IFormFile? UploadImge { get; set; }


    /// <summary>
    /// Изображение стола
    /// </summary>
    [Comment("Изображение стола")]
    public byte[]? RestaurantTable_Image { get; set; }


}