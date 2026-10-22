using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Бизнес логика для категорий ингредиентов
    /// </summary>
    public class Dish_IngridientCategory
    {

        /// <summary>
        /// Логер
        /// </summary>
        private readonly ILogger _log;


        /// <summary>
        /// Контекст Базы данных
        /// </summary>
        private readonly Dish_Ingridient_Category_oper dl_dish_Ingridient_Category_Oper = new();


        /// <summary>
        /// Конструктор
        /// </summary>
        public Dish_IngridientCategory()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }


        /// <summary>
        /// Категория ингредиента
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        public CDish_Ingridient_Category? Get(Dish_Ingridient_Category? Elem)
        {
            if(Elem == null)
            {
                return null;
            }

            return new CDish_Ingridient_Category()
            {
                ID = Elem.ID,
                Name = Elem.Name,
                Flag_Del = Elem.Flag_Del,
                Date_Del = Elem.Date_Del,
                UserID_Del = Elem.UserID_Del
            };
        }


        /// <summary>
        /// Получить ингредиент
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CDish_Ingridient_Category? GetIngridient(int ID)
        {
            return Get(dl_dish_Ingridient_Category_Oper.Get(ID: ID));
        }


        /// <summary>
        /// Получить все ингредиенты
        /// </summary>
        /// <returns></returns>
        public List<CDish_Ingridient_Category?> GetAllIngridients()
        {
            List<Dish_Ingridient_Category>? listCategories = [.. dl_dish_Ingridient_Category_Oper.GetQuery(Flag_Del_val: null)];

            List<CDish_Ingridient_Category?> result = [];

            foreach(Dish_Ingridient_Category? elem in listCategories)
            {
                CDish_Ingridient_Category? item = Get(elem);

                result.Add(item);
            }

            return result;
        }



        /// <summary>
        /// Добавление 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public int Add(string Name)
        {
            return dl_dish_Ingridient_Category_Oper.Add(Name: Name);
        }


        /// <summary>
        /// Редактирование 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name_New"></param>
        /// <returns></returns>
        public int Edit(int ID, string Name_New)
        {
            return dl_dish_Ingridient_Category_Oper.Edit(ID: ID, Name_New: Name_New);
        }


        /// <summary>
        /// Редактирование 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name_New"></param>
        /// <returns></returns>
        public int Delete(int ID)
        {
            return dl_dish_Ingridient_Category_Oper.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: 1);
        }


        /// <summary>
        /// Восстановление категории (снятие флага удаления)
        /// </summary>
        /// <param name="ID">ID категории</param>
        /// <returns>Количество затронутых записей</returns>
        public int Restore(int ID)
        {
            return dl_dish_Ingridient_Category_Oper.Edit_FlagDel(ID: ID, Flag_Del_New: false, UserID: 1, Flag_Del_val: null);
        }


        /// <summary>
        /// Получить все КАТЕГОРИИ ингредиентов
        /// </summary>
        /// <returns>Список категорий ингредиентов</returns>
        public List<CDish_Ingridient_Category?> GetAllCategories()
        {
            List<Dish_Ingridient_Category>? listCategories = [.. dl_dish_Ingridient_Category_Oper.GetQuery(Flag_Del_val: null)];

            List<CDish_Ingridient_Category?> result = new List<CDish_Ingridient_Category?>();

            foreach (Dish_Ingridient_Category? elem in listCategories)
            {
                CDish_Ingridient_Category? item = Get(elem);
                result.Add(item);
            }

            return result;
        }
    }
}
