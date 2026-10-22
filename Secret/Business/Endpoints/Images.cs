using Microsoft.Extensions.Logging;
using HIDE.RestAndRant.Common;
using HIDE.RestAndRant.Common.Consts;

namespace HIDE.RestAndRant.Business
{
    /// <summary>
    /// Работа с изображениями
    /// </summary>
    public class Images
    {
        private readonly ILogger _log;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Images()
        {
            _log = AppLog.CreateLogger(GetType());
            _log.LogDebug("Constructor");
        }

        /// <summary>
        /// Получение изображения
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="dir_file"></param>
        /// <returns></returns>
        private string? Get(string dir, string dir_file)
        {
            List<string> list_Images = [];
            if (Directory.Exists(dir))
            {

                foreach (string? file in Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".jpg") ||
                                                                                                             s.EndsWith(".jpeg") ||
                                                                                                             s.EndsWith(".png") ||
                                                                                                             s.EndsWith(".svg")))
                {
                    list_Images.Add($"{dir_file}/{Path.GetFileName(file)}");
                }
            }

            if (list_Images.Count > 1)
            {
                _log.LogWarning("В папке изображений более одного файла");
            }

            return list_Images.FirstOrDefault();
        }


        /// <summary>
        /// Получение изображения блюда
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="ContentRootPath"></param>
        /// <param name="Url_Prefix"></param>
        /// <returns></returns>
        public string? Get_Dish(int DishID, string ContentRootPath, string Url_Prefix)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Dishes}/{DishID}/";
            string dir_file = $"{Url_Prefix}/{Consts.ImageFolder_Dishes}/{DishID}/";
            return Get(dir: dir, dir_file: dir_file);
        }


        /// <summary>
        /// Получение изображения ингридиента
        /// </summary>
        /// <param name="IngridientID"></param>
        /// <param name="ContentRootPath"></param>
        /// <param name="Url_Prefix"></param>
        /// <returns></returns>
        public string? Get_Ingridient(int IngridientID, string ContentRootPath, string Url_Prefix)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Ingridients}/{IngridientID}/";
            string dir_file = $"{Url_Prefix}/{Consts.ImageFolder_Ingridients}/{IngridientID}/";
            return Get(dir: dir, dir_file: dir_file);
        }


        /// <summary>
        /// Получение изображения клиента
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="ContentRootPath"></param>
        /// <param name="Url_Prefix"></param>
        /// <returns></returns>
        public string? Get_Client(int ClientID, string ContentRootPath, string Url_Prefix)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Clients}/{ClientID}/";
            string dir_file = $"{Url_Prefix}/{Consts.ImageFolder_Clients}/{ClientID}/";
            return Get(dir: dir, dir_file: dir_file);
        }

        /// <summary>
        /// Получение изображения баннера большого
        /// </summary>
        /// <param name="BannerID"></param>
        /// <param name="ContentRootPath"></param>
        /// <param name="Url_Prefix"></param>
        /// <returns></returns>
        public string? Get_Banner_Big(int BannerID, string ContentRootPath, string Url_Prefix)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Banners}/{BannerID}/big/";
            string dir_file = $"{Url_Prefix}/{Consts.ImageFolder_Banners}/{BannerID}/big/";
            return Get(dir: dir, dir_file: dir_file);
        }

        /// <summary>
        /// Получение изображения баннера маленького
        /// </summary>
        /// <param name="BannerID"></param>
        /// <param name="ContentRootPath"></param>
        /// <param name="Url_Prefix"></param>
        /// <returns></returns>
        public string? Get_Banner_Small(int BannerID, string ContentRootPath, string Url_Prefix)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Banners}/{BannerID}/small/";
            string dir_file = $"{Url_Prefix}/{Consts.ImageFolder_Banners}/{BannerID}/small/";
            return Get(dir: dir, dir_file: dir_file);
        }

    }
}