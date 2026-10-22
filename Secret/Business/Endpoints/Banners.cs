using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Business.Endpoints;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;
using HIDE.RestAndRant.Database.Definitions;
using HIDE.RestAndRant.Database.Opers;

namespace HIDE.RestAndRant.Business
{

    /// <summary>
    /// Банеры
    /// </summary>
    public class Banners
    {

        private readonly ILogger _log;
        private readonly Banner_oper _dl_Banner = new();
        private readonly City_oper _dl_City = new();
        private readonly Setups _bl_Setup = new();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Banners()
        {
            _log = AppLog.CreateLogger(GetType());

        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem">Объект</param>
        /// <returns></returns>
        private static CBanner? Get(Banner? Elem)
        {
            if (Elem == null) return null;

            CBanner item = new()
            {
                ID = Elem.ID,
                Name = Elem.Name,
                Image_Big = Elem.Image_Big,
                Image_Small = Elem.Image_Small,
                CityID = Elem.CityID,
                Category_ID = Elem.Category_ID,
                Product_ID = Elem.Product_ID,
                Date_Start = Elem.Date_Start,
                Date_End = Elem.Date_End,
                Date_Create = Elem.Date_Create,
                OrderNum = Elem.OrderNum
            };
            return item;
        }

        /// <summary>
        /// Получение данных 
        /// </summary>
        /// <param name="Elem">Объект</param>
        /// <returns></returns>
        public CBanner? GetShort(Banner? Elem)
        {
            if (Elem == null) return null;

            City? city = _dl_City.Get(Elem.CityID);

            CBanner item = new()
            {
                ID = (int)Elem.ID,
                Name = Elem.Name,
                CityID = Elem.CityID,
                City = city,
                Category_ID = Elem.Category_ID,
                Product_ID = Elem.Product_ID,
                Date_Start = Elem.Date_Start,
                Date_End = Elem.Date_End,
                Date_Create = Elem.Date_Create,
                OrderNum = Elem.OrderNum,
            };
            return item;
        }

        /// <summary>
        /// Найти по ID 
        /// </summary>
        /// <param name="ID">Объект</param>
        /// <returns></returns>
        public CBanner? Get(int ID)
        {
            return Get(Elem: _dl_Banner.Get(ID));
        }


        /// <summary>
        /// Получение баннера по ID без изображений
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CBanner_Light? Get_Light(int ID)
        {
            CBanner? elem = Get(ID: ID);

            if (elem != null)
            {
                CBanner_Light result = new()
                {
                    ID = elem.ID,
                    Name = elem.Name,
                    CityID = elem.CityID,
                    Category_ID = elem.Category_ID,
                    Product_ID = elem.Product_ID,
                    Date_Start = elem.Date_Start,
                    Date_End = elem.Date_End,
                    Date_Create = elem.Date_Create,
                    OrderNum = elem.OrderNum,
                };

                return result;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Возвращает список всех баннеров
        /// </summary>
        /// <param name="Flag_Del_val">True - Помеченные как удаленные</param>
        /// <returns></returns>
        public List<CBanner> GetList_Full(bool? Flag_Del_val = false)
        {
            List<CBanner> list_result = [];
            foreach (Banner elem in _dl_Banner.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CBanner? item = GetShort(Elem: elem);
                if (item != null)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

        /// <summary>
        /// Возвращает список без картинок
        /// </summary>
        /// <param name="Flag_Del_val">True - Помеченные как удаленные</param>
        /// <returns></returns>
        public List<CBanner> GetList(bool? Flag_Del_val = false)
        {
            List<CBanner> list_result = [];

            foreach (Banner elem in _dl_Banner.GetQuery(Flag_Del_val: Flag_Del_val).ToList())
            {
                CBanner? item = GetShort(Elem: elem);

                if (item != null && item.Date_Start < DateTime.Now && DateTime.Now < item.Date_End)
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }

        /// <summary>
        /// Возвращает список для города без картинок
        /// </summary>
        /// <param name="Flag_Del_val">True - Помеченные как удаленные</param>
        /// <param name="CityID">Номер города</param>
        /// <returns></returns>
        public List<CBanner> GetList_ByCity(int CityID, bool? Flag_Del_val = false)
        {
            List<CBanner> list_result = [];

            foreach (Banner elem in _dl_Banner.GetQuery_ByCity(CityID: CityID, Flag_Del_val: Flag_Del_val).ToList())
            {
                CBanner? item = GetShort(Elem: elem);

                if ((item != null && item.Date_Start < DateTime.Now && DateTime.Now < item.Date_End) ||
                    (item != null && item.Date_Start < DateTime.Now && item.Date_End == null))
                {
                    list_result.Add(item);
                }
            }
            return list_result;
        }


        /// <summary>
        /// Возвращает список ID и DateEdit для города
        /// </summary>
        /// <param name="CityID">Город</param>
        /// <param name="Flag_Del_val">Флаг удаления</param>
        /// <returns></returns>
        public List<CBanner_IDs> GetListIDs_ByCity(int CityID, bool? Flag_Del_val = false)
        {
            List<CBanner_IDs> list_result = [];

            foreach (Banner elem in _dl_Banner.GetQuery_ByCity(CityID: CityID, Flag_Del_val: Flag_Del_val).ToList())
            {

                if ((elem != null && elem.Date_Start < DateTime.Now && DateTime.Now < elem.Date_End) ||
                    (elem != null && elem.Date_Start < DateTime.Now && elem.Date_End == null))
                {
                    CBanner_IDs item = new()
                    {
                        ID = elem.ID,
                        Date_Edit = elem.Date_Edit,
                    };

                    list_result.Add(item);
                }
            }
            return list_result;
        }

        /// <summary>
        /// Получение изображения баннера большого
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public byte[]? ImageBig_Get(int ID)
        {
            return _dl_Banner.Get(ID: ID)?.Image_Big;
        }

        /// <summary>
        /// Изменение изображения большого
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Banner_Image_Big"></param>
        /// <returns></returns>
        public int Edit_Image_Big(int ID, byte[] Banner_Image_Big)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Banners_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Banner.Edit_Image_Big(ID: ID, Banner_Image_Big: Banner_Image_Big);
        }

        /// <summary>
        /// Изменение изображения маленького
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Banner_Image_Small"></param>
        /// <returns></returns>
        public int Edit_Image_Small(int ID, byte[] Banner_Image_Small)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Banners_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Banner.Edit_Image_Small(ID: ID, Banner_Image_Small: Banner_Image_Small);
        }

        /// <summary>
        /// Получение изображения баннера большого
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public byte[]? ImageSmall_Get(int ID)
        {
            return _dl_Banner.Get(ID: ID)?.Image_Small;
        }

        /// <summary>
        /// Удаление изображения
        /// </summary>
        /// <param name="ID">Номер картинки</param>
        /// <param name="Big">Тип картинки</param>
        /// <returns></returns>
        public int Del_Image(int ID, bool Big)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Banners_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Banner.Del_Image(ID: ID, Big: Big);
        }

        /// <summary>
        /// Получение даты последнего редактирования баннеров
        /// </summary>
        /// <returns></returns>
        public DateTime? GetMaxLastEditDate()
        {
            return _bl_Setup.Get_Date(Key: Consts.SetupKey_Banners_DateChange);
        }

        /// <summary>
        /// Добавление
        /// </summary>
        /// <param name="Name">Баннер большой</param>
        /// <param name="Image_Big">Баннер большой</param>
        /// <param name="Image_Small">Баннер маленький</param>
        /// <param name="CityID">ID города</param>
        /// <param name="Category_ID">ID категории</param>
        /// <param name="Product_ID">ID товара</param>
        /// <param name="Date_Start">Дата начала показа </param>
        /// <param name="Date_End">Дата окончания показа </param>
        /// <param name="OrderNum"> Порядок cортировки </param>
        /// <returns></returns>
        public int Add(string? Name, byte[]? Image_Big, byte[]? Image_Small, int CityID, int? Category_ID, int? Product_ID, DateTime? Date_Start, DateTime? Date_End, int OrderNum)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Banners_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Banner.Add(Name: Name, Image_Big: Image_Big, Image_Small: Image_Small, CityID: CityID, Category_ID: Category_ID, Product_ID: Product_ID, Date_Start: Date_Start, Date_End: Date_End, OrderNum: OrderNum);
        }

        /// <summary>
        /// Изменение
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="Name">Баннер большой</param>
        /// <param name="Image_Big">Баннер большой</param>
        /// <param name="Image_Small">Баннер маленький</param>
        /// <param name="CityID">ID города</param>
        /// <param name="Category_ID">ID категории</param>
        /// <param name="Product_ID">ID товара</param>
        /// <param name="Date_Start">Дата начала показа </param>
        /// <param name="Date_End">Дата окончания показа </param>
        /// <param name="OrderNum"> Порядок cортировки </param>
        /// <returns></returns>
        public int Edit(int ID, string? Name, byte[]? Image_Big, byte[]? Image_Small, int CityID, int? Category_ID, int? Product_ID, DateTime? Date_Start, DateTime? Date_End, int OrderNum)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Banners_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Banner.Edit(ID: ID, Name: Name, Image_Big: Image_Big, Image_Small: Image_Small, CityID: CityID, Category_ID: Category_ID, Product_ID: Product_ID, Date_Start: Date_Start, Date_End: Date_End, OrderNum: OrderNum);
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="ID">Код</param>
        /// <param name="UserID">Скрыт</param>
        /// <returns></returns>
        public int Del(int ID, int UserID)
        {
            _bl_Setup.Set(Key: Consts.SetupKey_Banners_DateChange, Val_New: DateTime.Now.ToString()); //Обновление даты последнего обновления
            return _dl_Banner.Edit_FlagDel(ID: ID, UserID: UserID, Flag_Del: true);
        }

    }
}
