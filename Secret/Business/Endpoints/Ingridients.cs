using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Definitions_ForAdmin;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Ингридиенты
    /// </summary>
    public class Ingridients
    {
        private readonly ILogger _log;
        private readonly Ingridient_oper _dl_Ingridient = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Ingridients()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogTrace("Constructor");
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private static CIngridient? Get(Ingridient? Elem)
        {
            if (Elem != null)
            {
                CIngridient item = new()
                {
                    ID = Elem.ID,
                    Name = Elem.Name,
                    Image_ChangeDate = Elem.Image_ChangeDate,
                };
                return item;
            }

            return null;
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem"></param>
        /// <returns></returns>
        private CIngridient_ForAdmin? Get_ForAdmin(Ingridient Elem)
        {
            if (Elem == null) return null;

            return new CIngridient_ForAdmin
            {
                ID = Elem.ID,
                Name = Elem.Name,
                Category_ID = Elem.Category_ID,
                Image = Elem.Image,
                Image_Url = (Elem.Image != null && Elem.Image.Length > 0)
                    ? $"data:image/jpeg;base64,{Convert.ToBase64String(Elem.Image)}"
                    : null,
            };
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CIngridient? Get(int ID)
        {
            return Get(Elem: _dl_Ingridient.Get(ID));
        }


        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CIngridient_ForAdmin? Get_ForAdmin(int ID)
        {
            return Get_ForAdmin(Elem: _dl_Ingridient.Get(ID));
        }


        /// <summary>
        /// Проверка удален ли ингредиент
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Check_Ingridient(int ID)
        {
            return _dl_Ingridient.Get(ID: ID)?.Flag_Del ?? true;
        }


        /// <summary>
        /// Получение изображения ингредиента по ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public byte[]? Image_Get(int ID)
        {
            return _dl_Ingridient.Get(ID: ID)?.Image;
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CIngridient>? GetList(bool? Flag_Del_val = false)
        {
            List<CIngridient> list_result = [];

            foreach (Ingridient elem in _dl_Ingridient.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CIngridient? item = Get(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Name)];
        }


        /// <summary>
        /// Возвращает список
        /// </summary>
        /// <param name="Flag_Del_val"></param>
        /// <returns></returns>
        public List<CIngridient_ForAdmin>? GetList_ForAdmin(bool? Flag_Del_val = false)
        {
            List<CIngridient_ForAdmin> list_result = [];

            foreach (Ingridient elem in _dl_Ingridient.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CIngridient_ForAdmin? item = Get_ForAdmin(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return [.. list_result.OrderBy(x => x.Name)];
        }



        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Image"></param>
        /// <param name="Category_ID"></param>
        /// <returns></returns>
        public int Add(string Name, byte[]? Image, int? Category_ID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Ingridients_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Ingridient.Add(Name: Name, Image: Image, Category_ID: Category_ID);
        }


        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Name_New"></param>
        /// <param name="Image_New"></param>
        /// <param name="CategoryIDNew"></param>
        /// <returns></returns>
        public int Edit(int ID, string Name_New, byte[]? Image_New, int? CategoryIDNew)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Ingridients_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Ingridient.Edit(ID: ID, Name_New: Name_New, Image_New: Image_New, Category_ID_New: CategoryIDNew);
        }


        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Ingridients_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            _bl_Setup.Set(Key: Consts.SetupKey_Dishes_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Ingridient.Edit_FlagDel(ID: ID, Flag_Del_New: true, UserID: UserID);
        }


        /// <summary>
        /// Удаление изображения
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int Del_Image(int ID)
        {
            return _dl_Ingridient.Del_Image(ID: ID);
        }


        /// <summary>
        /// Изменение даты изменения изображения
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int Edit_Image_ChangeData(int ID)
        {
            return _dl_Ingridient.Edit_Image_ChangeData(ID: ID);
        }


        /// <summary>
        /// Изменение изображения
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Ingridient_Image"></param>
        /// <returns></returns>
        public int Edit_Image(int ID, byte[] Ingridient_Image)
        {
            return _dl_Ingridient.Edit_Image(ID: ID, Ingridient_Image: Ingridient_Image);
        }



        /// <summary>
        /// Получить все ингредиенты для админки с информацией о категориях
        /// </summary>
        public List<CIngridient_ForAdmin> GetAll_ForAdmin_WithCategories()
        {
            List<CIngridient_ForAdmin> result = new();

            // Получаем все ингредиенты
            List<Ingridient> ingredients = [.. _dl_Ingridient.GetQuery(Flag_Del_val: false)];

            // Получаем все категории
            Dish_Ingridient_Category_oper categoryOper = new Dish_Ingridient_Category_oper();
            List<Dish_Ingridient_Category> categories = [.. categoryOper.GetQuery()];

            foreach (Ingridient ing in ingredients)
            {
                CIngridient_ForAdmin? item = Get_ForAdmin(ing);
                if (item == null) continue;

                // Добавляем информацию о категории
                if (ing.Category_ID.HasValue)
                {
                    Dish_Ingridient_Category? category = categories.FirstOrDefault(c => c.ID == ing.Category_ID.Value);
                    if (category != null)
                    {
                        item.Category_Name = category.Name;
                        item.Category_IsDeleted = category.Flag_Del == true;
                    }
                    else
                    {
                        item.Category_Name = "Категория не найдена";
                        item.Category_IsDeleted = true;
                    }
                }
                else
                {
                    item.Category_Name = "Без категории";
                    item.Category_IsDeleted = false;
                }

                result.Add(item);
            }

            return [.. result.OrderBy(x => x.Name)];
        }
    }
}