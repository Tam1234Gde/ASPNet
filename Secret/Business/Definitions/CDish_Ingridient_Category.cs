using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HIDE.RestAndRant.Business.Definitions
{
    /// <summary>
    /// Модель - категория ингредиента
    /// </summary>
    public class CDish_Ingridient_Category
    {
        /// <summary>
        /// Код
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = null!;


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
