using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Site_Menu.Helpers
{
    public static class Helper_ToArray
    {
        /// <summary>
        /// Преобразовывает в список int значения из строки, где разделителем служит ';'
        /// </summary>
        /// <param name="StringArr"></param>
        /// <returns></returns>
        public static List<int> StringArr_ToList(string StringArr)
        {
            List<int> list_result = new List<int>();

            foreach (string str in StringArr.Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                int? elem = HIDE_Convert.ToInt_Nullable(str);
                if (elem.HasValue)
                {
                    list_result.Add(elem.Value);
                }
            }

            return list_result;
        }

        /// <summary>
        /// Преобразовывает в список int значения из строки, где разделителем служит ','
        /// </summary>
        /// <param name="StringArr"></param>
        /// <returns></returns>
        public static List<int> StringArr2_ToList(string StringArr)
        {
            List<int> list_result = new List<int>();

            foreach (string str in StringArr.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                int? elem = HIDE_Convert.ToInt_Nullable(str);
                if (elem.HasValue)
                {
                    list_result.Add(elem.Value);
                }
            }

            return list_result;
        }

        /// <summary>
        /// Преобразовывает в список string значения из строки, где разделителем служит ';'
        /// </summary>
        /// <param name="StringArr"></param>
        /// <returns></returns>
        public static List<string> StringArr_ToListString(string StringArr)
        {
            List<string> list_result = new List<string>();

            foreach (string str in StringArr.Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                list_result.Add(str);
            }

            return list_result;
        }
    }
}