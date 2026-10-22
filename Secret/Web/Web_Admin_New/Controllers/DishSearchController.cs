using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Controllers
{
    /// <summary>
    /// Поиск блюд в старой БД для автозаполнения формы добавления.
    /// </summary>
    public class DishSearchController : Controller
    {
        #region Поля и конструктор

        private readonly Dish_oper _dl_Dish = new();
        private readonly Dish_Volume_oper _dl_DishVolume = new();

        #endregion


        #region DTO

        public class SearchResultDto
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? CategoryName { get; set; }
            public decimal? Price { get; set; }
            public string? Teaser { get; set; }
            public bool HasImage { get; set; }
        }

        public class VolumeDto
        {
            public int Slot { get; set; }
            public decimal? Volume { get; set; }
            public decimal? Price { get; set; }
        }

        public class DetailsDto
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Teaser { get; set; }
            public string? Text { get; set; }
            public decimal? Price { get; set; }
            public int? CookingMinutes { get; set; }
            public bool FlagAllowColors { get; set; }
            public bool FlagBonus { get; set; }
            public decimal? Bonuses { get; set; }
            public bool FlagNew { get; set; }
            public bool FlagNoActive { get; set; }
            public DateTime? NewDateFrom { get; set; }
            public string? ImageBase64 { get; set; }
            public List<VolumeDto> Volumes { get; set; } = new();
        }

        #endregion


        /// <summary>
        /// Поиск блюд в старой БД по части наименования.
        /// GET /DishSearch/Search?query=латте&limit=30
        /// </summary>
        [HttpGet]
        public IActionResult Search(string? query, int limit = 30)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(Array.Empty<SearchResultDto>());
            }

            if (limit <= 0) limit = 30;
            if (limit > 100) limit = 100;

            string trimmed = query.Trim();

            List<SearchResultDto> results = _dl_Dish
                .GetQuerySearch(Name: trimmed, Flag_Del_val: false)
                .OrderBy(d => d.Name)
                .Take(limit)
                .Select(d => new SearchResultDto
                {
                    Id = d.ID,
                    Name = d.Name,
                    CategoryName = d.Dish_Category != null ? d.Dish_Category.Name : null,
                    Price = d.Price,
                    Teaser = d.Teaser,
                    HasImage = d.Image != null && d.Image.Length > 0
                })
                .ToList();

            return Json(results);
        }


        /// <summary>
        /// Полный набор данных блюда для автозаполнения формы.
        /// GET /DishSearch/Details?id=123
        /// </summary>
        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                return Json(new { success = false, message = "Некорректный ID" });
            }

            var dish = _dl_Dish.Get(id);
            if (dish == null)
            {
                return Json(new { success = false, message = "Блюдо не найдено" });
            }

            DetailsDto dto = new DetailsDto
            {
                Id = dish.ID,
                Name = dish.Name,
                Teaser = dish.Teaser,
                Text = dish.Text,
                Price = dish.Price,
                CookingMinutes = dish.Cooking_Minutes,
                FlagAllowColors = dish.Flag_AllowColors,
                FlagBonus = dish.Flag_Bonus,
                Bonuses = dish.Bonuses,
                FlagNew = dish.FlagNew,
                FlagNoActive = dish.FlagNoActive,
                NewDateFrom = dish.New_DateFrom,
                ImageBase64 = (dish.Image != null && dish.Image.Length > 0)
                    ? Convert.ToBase64String(dish.Image)
                    : null
            };

            // Объёмы: берём максимум 5 штук, сортируем по OrderNum
            List<VolumeDto> volumes = _dl_DishVolume
                .GetQuery_ByDishID(DishID: dish.ID, Flag_Del_val: false)
                .OrderBy(v => v.OrderNum)
                .Take(5)
                .Select(v => new VolumeDto
                {
                    Volume = v.Volume != null ? (decimal?)v.Volume.Value : null,
                    Price = v.Price
                })
                .ToList();

            for (int i = 0; i < volumes.Count; i++)
            {
                volumes[i].Slot = i + 1;
            }

            dto.Volumes = volumes;

            return Json(new { success = true, data = dto });
        }
    }
}