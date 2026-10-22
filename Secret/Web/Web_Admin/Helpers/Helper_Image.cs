using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Drawing;
using HIDE.RestAndRant.Common.Consts;

namespace HIDE.RestAndRant.Web_Admin.Helpers
{
    /// <summary>
    /// Помощник для изображений
    /// </summary>
    public static class Helper_Image
    {
        public static bool IsJpeg(string filename)
        {
            bool result = Path.GetExtension(filename).Equals(".jpg", StringComparison.InvariantCultureIgnoreCase) || Path.GetExtension(filename).Equals(".jpeg", StringComparison.InvariantCultureIgnoreCase);

            if (!result)
            {
                return false;
            }

            //https://stackoverflow.com/questions/772388/c-sharp-how-can-i-test-a-file-is-a-jpeg
            using BinaryReader br = new(File.Open(filename, FileMode.Open, FileAccess.Read));
            ushort soi = br.ReadUInt16();  // Start of Image (SOI) marker (FFD8)
            ushort marker = br.ReadUInt16(); // JFIF marker (FFE0) or EXIF marker(FFE1)

            return soi == 0xd8ff && (marker & 0xe0ff) == 0xe0ff;
        }

        /// <summary>
        /// Добавление изображения
        /// </summary>
        /// <param name="dir">Путь</param>
        /// <param name="CurrentFile">Файл</param>
        private static byte[] Add(string dir, IFormFile CurrentFile)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string path = Path.Combine(dir, CurrentFile.FileName); // безопасное объединение

            if (string.IsNullOrWhiteSpace(path))
            {
                return [];
            }

            // Сохраняем оригинальный файл на диск
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                CurrentFile.CopyTo(fileStream);
            }

            try
            {
                using SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(path);
                if (image.Width > 500)
                {
                    int newHeight = (int)((500.0 / image.Width) * image.Height);
                    image.Mutate(x => x.Resize(500, newHeight));
                }
                image.Save(path);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Ошибка обработки изображения: {ex.Message}");
            }

            byte[] imgdata = File.ReadAllBytes(path);
            return imgdata;
        }


        /// <summary>
        /// Изменение изображения
        /// </summary>
        /// <param name="dir">Путь к папке</param>
        /// <param name="CurrentFile">Файл</param>
        private static void Edit(string dir, IFormFile CurrentFile)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // Удалить все существующие файлы в папке
            DirectoryInfo di = new(dir);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            string path = Path.Combine(dir, CurrentFile.FileName); // безопасное объединение

            if (string.IsNullOrWhiteSpace(path))
                return;

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                CurrentFile.CopyTo(fileStream);
            }

            try
            {
                using SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(path);

                if (image.Width > 500)
                {
                    int newHeight = (int)((500.0 / image.Width) * image.Height);
                    image.Mutate(x => x.Resize(500, newHeight));
                    image.Save(path); // перезаписываем
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки: {ex.Message}");
            }
        }


        /// <summary>
        /// Добавление изображения блюда
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="CurrentFile"></param>
        /// <param name="ContentRootPath"></param>
        public static byte[] Add_Dish(int DishID, IFormFile CurrentFile, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Dishes}/{DishID}/";

            byte[] image = Add(dir: dir, CurrentFile: CurrentFile);

            return image;
        }

        /// <summary>
        /// Изменение изображения блюда
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="CurrentFile"></param>
        /// <param name="ContentRootPath"></param>
        public static void Edit_Dish(int DishID, IFormFile CurrentFile, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Dishes}/{DishID}/";

            Edit(dir: dir, CurrentFile: CurrentFile);
        }

        /// <summary>
        /// Удаление изображения блюда
        /// </summary>
        /// <param name="DishID"></param>
        /// <param name="ContentRootPath"></param>
        public static void Del_Dish(int DishID, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Dishes}/{DishID}/";
            Del(dir: dir);
        }

        /// <summary>
        /// Добавление изображения ингридиента
        /// </summary>
        /// <param name="IngridientID"></param>
        /// <param name="CurrentFile"></param>
        /// <param name="ContentRootPath"></param>
        public static byte[] Add_Ingridient(int IngridientID, IFormFile CurrentFile, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Ingridients}/{IngridientID}/";

            byte[] image = Add(dir: dir, CurrentFile: CurrentFile);

            return image;
        }

        /// <summary>
        /// Изменение изображения ингридиента
        /// </summary>
        /// <param name="IngridientID"></param>
        /// <param name="CurrentFile"></param>
        /// <param name="ContentRootPath"></param>
        public static void Edit_Ingridient(int IngridientID, IFormFile CurrentFile, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Ingridients}/{IngridientID}/";

            Edit(dir: dir, CurrentFile: CurrentFile);
        }

        /// <summary>
        /// Удаление изображения ингридиента
        /// </summary>
        /// <param name="IngridientID"></param>
        /// <param name="ContentRootPath"></param>
        public static void Del_Ingridient(int IngridientID, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Ingridients}/{IngridientID}/";
            Del(dir: dir);
        }

        /// <summary>
        /// Добавление изображения клиента
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="CurrentFile"></param>
        /// <param name="ContentRootPath"></param>
        public static byte[] Add_Client(int ClientID, IFormFile CurrentFile, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Clients}/{ClientID}/";

            byte[] image = Add(dir: dir, CurrentFile: CurrentFile);

            return image;
        }

        /// <summary>
        /// Изменение изображения клиента
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="CurrentFile"></param>
        /// <param name="ContentRootPath"></param>
        public static void Edit_Client(int ClientID, IFormFile CurrentFile, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Clients}/{ClientID}/";

            Edit(dir: dir, CurrentFile: CurrentFile);
        }

        /// <summary>
        /// Удаление изображения клиента
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="ContentRootPath"></param>
        public static void Del_Client(int ClientID, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Clients}/{ClientID}/";
            Del(dir: dir);
        }

        /// <summary>
        /// Добавление изображения баннера большого
        /// </summary>
        /// <param name="BannerID"></param>
        /// <param name="CurrentFile"></param>
        /// <param name="ContentRootPath"></param>
        public static byte[] Add_Banner_Big(int BannerID, IFormFile CurrentFile, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Banners}/{BannerID}/big/";

            byte[] image = Add(dir: dir, CurrentFile: CurrentFile);

            return image;
        }

        /// <summary>
        /// Изменение изображения банера большого
        /// </summary>
        /// <param name="BannerID"></param>
        /// <param name="CurrentFile"></param>
        /// <param name="ContentRootPath"></param>
        public static void Edit_Banner_Big(int BannerID, IFormFile CurrentFile, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Banners}/{BannerID}/big/";

            Edit(dir: dir, CurrentFile: CurrentFile);
        }

        /// <summary>
        /// Удаление изображения банера большого
        /// </summary>
        /// <param name="BannerID"></param>
        /// <param name="ContentRootPath"></param>
        public static void Del_Banner_Big(int BannerID, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Banners}/{BannerID}/big/";
            Del(dir: dir);
        }

        /// <summary>
        /// Добавление изображения баннера большого
        /// </summary>
        /// <param name="BannerID"></param>
        /// <param name="CurrentFile"></param>
        /// <param name="ContentRootPath"></param>
        public static byte[] Add_Banner_Small(int BannerID, IFormFile CurrentFile, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Banners}/{BannerID}/small/";

            byte[] image = Add(dir: dir, CurrentFile: CurrentFile);

            return image;
        }

        /// <summary>
        /// Изменение изображения банера большого
        /// </summary>
        /// <param name="BannerID"></param>
        /// <param name="CurrentFile"></param>
        /// <param name="ContentRootPath"></param>
        public static void Edit_Banner_Small(int BannerID, IFormFile CurrentFile, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Banners}/{BannerID}/small/";

            Edit(dir: dir, CurrentFile: CurrentFile);
        }

        /// <summary>
        /// Удаление изображения банера маленького
        /// </summary>
        /// <param name="BannerID"></param>
        /// <param name="ContentRootPath"></param>
        public static void Del_Banner_Small(int BannerID, string ContentRootPath)
        {
            string dir = $"{ContentRootPath}/{Consts.ImageFolder_Banners}/{BannerID}/small/";
            Del(dir: dir);
        }

        /// <summary>
        /// Удаление изображения
        /// </summary>
        /// <param name="dir">Путь</param>
        private static void Del(string dir)
        {
            if (Directory.Exists(dir))
            {
                DirectoryInfo di = new(dir);
                foreach (FileInfo file in di.GetFiles()) //Удалить все существующие файлы
                {
                    file.Delete();
                }
            }
        }

    }
}