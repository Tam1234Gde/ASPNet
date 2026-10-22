namespace HIDE.RestAndRant.Web_Api.Models.v4.Menu
{
    /// <summary>
    /// Модель блюда для API v4
    /// </summary>
    public class MenuDishResponse
    {
        public int ID { get; set; }
        public int Menu_SubcategoryID { get; set; }
        public string Name { get; set; } = null!;
        public string? Teaser { get; set; }
        public string? Text { get; set; }
        public int? Cooking_Minutes { get; set; }
        public decimal Price { get; set; }
        public bool Flag_AllowColors { get; set; }
        public bool Flag_Bonus { get; set; }
        public decimal? Bonuses { get; set; }
        public int OrderNum { get; set; }
        public bool FlagNew { get; set; }
        public byte[]? Image { get; set; }
        public DateTime? Image_ChangeDate { get; set; }
        public string? RecommendDishIDs { get; set; }
        public bool FlagNoActive { get; set; }
        public DateTime? New_DateFrom { get; set; }
        public int Menu_OptionStateID { get; set; }
        public List<int>? RecommendDishIDsList { get; set; }
        public List<MenuDishVolumeResponse>? Volumes { get; set; }
        public List<MenuDishIngredientResponse>? Ingredients { get; set; }
    }

    public class MenuDishVolumeResponse
    {
        public int? Vol { get; set; }
        public int? Vol_Price { get; set; }
    }

    public class MenuDishIngredientResponse
    {
        public int IngredientID { get; set; }
        public string IngredientName { get; set; } = null!;
        public int? Price { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; } = null!;
    }
}