using System.Collections.Generic;
using HIDE.RestAndRant.Business.Definitions;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Models.Menu
{
    /// <summary>
    /// Решение по одному ингредиенту: что с ним делать при удалении группы.
    /// </summary>
    public class IngredientDeleteDecision
    {
        /// <summary>
        /// Код ингредиента
        /// </summary>
        public int IngredientID { get; set; }

        /// <summary>
        /// Действие: "delete" (удалить) или "migrate" (перенести).
        /// По умолчанию — "delete".
        /// </summary>
        public string Action { get; set; } = "delete";

        /// <summary>
        /// Код группы, в которую переносится ингредиент (если Action == "migrate").
        /// </summary>
        public int? TargetGroupID { get; set; }
    }

    /// <summary>
    /// Модель подтверждения удаления группы ингредиентов.
    /// По каждому ингредиенту пользователь выбирает: удалить или перенести.
    /// </summary>
    public class Model_Menu_Ingredient_Group_Del
    {
        /// <summary>
        /// Код удаляемой группы
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Наименование удаляемой группы
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Ингредиенты группы
        /// </summary>
        public List<CMenu_Ingridient> Ingredients { get; set; } = new List<CMenu_Ingridient>();

        /// <summary>
        /// Список других групп, куда можно переносить
        /// </summary>
        public List<CMenu_Ingridient_Group> Target_Groups { get; set; } = new List<CMenu_Ingridient_Group>();

        /// <summary>
        /// Решения по каждому ингредиенту
        /// </summary>
        public List<IngredientDeleteDecision> Decisions { get; set; } = new List<IngredientDeleteDecision>();
    }
}