namespace HIDE.RestAndRant.Common
{
    /// <summary>
    /// Ошибки
    /// </summary>
    public static class Errors
    {
        public static readonly Dictionary<int, string> Dict_Errors = [];


        /// <summary>
        /// Получает строку ошибки по коду ошибки
        /// </summary>
        /// <param name="ErrorVal">Целочисленное значение ошибки</param>
        /// <returns>Строка ошибки</returns>
        public static string GetErrorString(int ErrorVal)
        {
            if (Dict_Errors.Count <= 0)
            {
                Fill_Dict();
            }

            if (Dict_Errors.TryGetValue(ErrorVal, out string? value))
            {
                return value;
            }
            else
            {
                return string.Format("Ошибка #{0}", ErrorVal < 0 ? ErrorVal * (-1) : ErrorVal);
            }
        }


        private static void Fill_Dict()
        {
            Dict_Errors.Add(Err3000, Err3000_str);
            Dict_Errors.Add(Err3001, Err3001_str);
            Dict_Errors.Add(Err3010, Err3010_str);
            Dict_Errors.Add(Err3011, Err3011_str);
            Dict_Errors.Add(Err3012, Err3012_str);
            Dict_Errors.Add(Err3030, Err3030_str);
            Dict_Errors.Add(Err3031, Err3031_str);
            Dict_Errors.Add(Err3032, Err3032_str);
            Dict_Errors.Add(Err3033, Err3033_str);
            Dict_Errors.Add(Err3034, Err3034_str);
            Dict_Errors.Add(Err3035, Err3035_str);
            Dict_Errors.Add(Err3036, Err3036_str);
            Dict_Errors.Add(Err3040, Err3040_str);
            Dict_Errors.Add(Err900, Err900_str);
            Dict_Errors.Add(Err910, Err910_str);
            Dict_Errors.Add(Err200, Err200_str);
            Dict_Errors.Add(Err201, Err201_str);
            Dict_Errors.Add(Err202, Err202_str);
            Dict_Errors.Add(Err203, Err203_str);
            Dict_Errors.Add(Err204, Err204_str);
            Dict_Errors.Add(Err205, Err205_str);
        }


        #region 1XX - Errors_App - общие ошибки 

        /// <summary>
        /// Ошибка #101 - Пароль не верный
        /// </summary>
        public const int Err101 = -101;


        /// <summary>
        /// Ошибка #101 - Пароль не верный
        /// </summary>
        public const string Err101_str = "Ошибка #101 - Пароль не верный";


        /// <summary>
        /// Ошибка #102 - Список состава заказа пустой
        /// </summary>
        public const int Err102 = -102;


        /// <summary>
        /// Ошибка #102 - Список состава заказа пустой
        /// </summary>
        public const string Err102_str = "Ошибка #102 - Список состава заказа пустой";


        /// <summary>
        /// Ошибка #103 - Сумма имеет значение null
        /// </summary>
        public const int Err103 = -103;


        /// <summary>
        /// Ошибка #103 - Сумма имеет значение null
        /// </summary>
        public const string Err103_str = "Ошибка #103 - Сумма имеет значение null";


        /// <summary>
        /// Ошибка #104 - Данные для бронирования столика некорректны
        /// </summary>
        public const int Err104 = -104;


        /// <summary>
        /// Ошибка #104 - Данные для бронирования столика некорректны
        /// </summary>
        public const string Err104_str = "Ошибка #104 - Данные для бронирования столика некорректны";

        #endregion 1XX - Errors_App - общие ошибки 


        #region 2XX - Errors_DB - ошибки базы данных

        /// <summary>
        /// Ошибка #200 - Запись с указанным идентификатором не обнаружена
        /// </summary>
        public const int Err200 = -200;


        /// <summary>
        /// Ошибка #200 - Запись с указанным идентификатором не обнаружена
        /// </summary>
        public const string Err200_str = "Ошибка #200 - Запись с указанным идентификатором не обнаружена";


        /// <summary>
        /// Ошибка #201 - Запись с указанными идентификаторами не обнаружена
        /// </summary>
        public const int Err201 = -201;


        /// <summary>
        /// Ошибка #201 - Запись с указанными идентификаторами не обнаружена
        /// </summary>
        public const string Err201_str = "Ошибка #201 - Запись с указанными идентификаторами не обнаружена";


        /// <summary>
        /// Ошибка #202 - Ошибка добавления записи в БД
        /// </summary>
        public const int Err202 = -202;


        /// <summary>
        /// Ошибка #202 - Ошибка добавления записи в БД
        /// </summary>
        public const string Err202_str = "Ошибка #202 - Ошибка добавления записи в БД";


        /// <summary>
        /// Ошибка #203 - Ошибка изменения записи в БД
        /// </summary>
        public const int Err203 = -203;


        /// <summary>
        /// Ошибка #203 - Ошибка изменения записи в БД
        /// </summary>
        public const string Err203_str = "Ошибка #203 - Ошибка изменения записи в БД";


        /// <summary>
        /// Ошибка #204 - Ошибка удаления записи в БД
        /// </summary>
        public const int Err204 = -204;


        /// <summary>
        /// Ошибка #204 - Ошибка удаления записи в БД
        /// </summary>
        public const string Err204_str = "Ошибка #204 - Ошибка удаления записи в БД";


        /// <summary>
        /// Ошибка #205 - Ошибка группового commit
        /// </summary>
        public const int Err205 = -205;


        /// <summary>
        /// Ошибка #205 - Ошибка группового commit
        /// </summary>
        public const string Err205_str = "Ошибка #205 - Ошибка группового commit";

        #endregion 2XX - Errors_DB - ошибки базы данных


        #region 9XX - Ошибки работы с картой

        /// <summary>
        /// Ошибка #900 - Указанный идентификатор карты уже использован
        /// </summary>
        public const int Err900 = -900;


        /// <summary>
        /// Ошибка #900 - Указанный идентификатор карты уже использован
        /// </summary>
        public const string Err900_str = "Ошибка #900 - Указанный идентификатор карты уже использован";


        /// <summary>
        /// Ошибка #910 - Не удалось выполнить возврат
        /// </summary>
        public const int Err910 = -910;


        /// <summary>
        /// Ошибка #910 - Не удалось выполнить возврат
        /// </summary>
        public const string Err910_str = "Ошибка #910 - Не удалось выполнить возврат";

        #endregion 9XX - Ошибки работы с картой


        #region Ошибки API

        /// <summary>
        /// Ошибка #3000 - данные на вход не переданы
        /// </summary>
        public const int Err3000 = -3000;


        /// <summary>
        /// Ошибка #3000 - данные на вход не переданы
        /// </summary>
        public const string Err3000_str = "Ошибка #3000 - данные на вход не переданы";


        /// <summary>
        /// Ошибка #3000 - Формат входного JSON ошибочен
        /// </summary>
        public const int Err3001 = -3001;


        /// <summary>
        /// Ошибка #3000 - Формат входного JSON ошибочен
        /// </summary>
        public const string Err3001_str = "Ошибка #3001 - Формат входного JSON ошибочен";


        /// <summary>
        /// Ошибка #3010 - Не передан телефон
        /// </summary>
        public const int Err3010 = -3010;


        /// <summary>
        /// Ошибка #3010 - Неверно передан телефон
        /// </summary>
        public const string Err3010_str = "Ошибка #3010 - Неверно передан телефон";


        /// <summary>
        /// Ошибка #3011 - Неверно передан телефон или СМС-код
        /// </summary>
        public const int Err3011 = -3011;


        /// <summary>
        /// Ошибка #3011 - Неверно передан телефон или СМС-код
        /// </summary>
        public const string Err3011_str = "Ошибка #3011 - Неверно передан телефон или СМС-код";


        /// <summary>
        /// Ошибка #3012 - Неверно передан GUID клиента
        /// </summary>
        public const int Err3012 = -3012;


        /// <summary>
        /// Ошибка #3012 - Неверно передан GUID клиента
        /// </summary>
        public const string Err3012_str = "Ошибка #3012 - Неверно передан GUID клиента";


        /// <summary>
        /// Ошибка #3030 - Не передан код клиента
        /// </summary>
        public const int Err3030 = -3030;


        /// <summary>
        /// Ошибка #3030 - Не передан код клиента
        /// </summary>
        public const string Err3030_str = "Ошибка #3030 - Не передан код клиента";


        /// <summary>
        /// Ошибка #3031 - Неверно передан токен клиента
        /// </summary>
        public const int Err3031 = -3031;


        /// <summary>
        /// Ошибка #3031 - Неверно передан токен клиента
        /// </summary>
        public const string Err3031_str = "Ошибка #3031 - Неверно передан токен клиента";


        /// <summary>
        /// Ошибка #3032 - Не передан вид оплаты
        /// </summary>
        public const int Err3032 = -3032;


        /// <summary>
        /// Ошибка #3032 - Не передан вид оплаты
        /// </summary>
        public const string Err3032_str = "Ошибка #3032 - Не передан вид оплаты";


        /// <summary>
        /// Ошибка #3033 - Не передан код ресторана
        /// </summary>
        public const int Err3033 = -3033;


        /// <summary>
        /// Ошибка #3033 - Не передан код ресторана
        /// </summary>
        public const string Err3033_str = "Ошибка #3033 - Не передан код ресторана";


        /// <summary>
        /// Ошибка #3034 - Не передан флаг с собой
        /// </summary>
        public const int Err3034 = -3034;


        /// <summary>
        /// Ошибка #3034 - Не передан флаг с собой
        /// </summary>
        public const string Err3034_str = "Ошибка #3034 - Не передан флаг с собой";


        /// <summary>
        /// Ошибка #3035 - Не передан флаг оплаты
        /// </summary>
        public const int Err3035 = -3035;


        /// <summary>
        /// Ошибка #3035 - Не передан флаг оплаты
        /// </summary>
        public const string Err3035_str = "Ошибка #3035 - Не передан флаг оплаты";


        /// <summary>
        /// Ошибка #3036 - Не передан флаг оплаты или не передан код платежки
        /// </summary>
        public const int Err3036 = -3036;


        /// <summary>
        /// Ошибка #3036 - Не передан флаг оплаты или не передан код платежки
        /// </summary>
        public const string Err3036_str = "Ошибка #3036 - Не передан флаг оплаты или не передан код платежки";


        /// <summary>
        /// Ошибка #3040 - Не передано содержимое заказа
        /// </summary>
        public const int Err3040 = -3040;


        /// <summary>
        /// Ошибка #3040 - Не передано содержимое заказа
        /// </summary>
        public const string Err3040_str = "Ошибка #3040 - Не передано содержимое заказа";

        #endregion Ошибки API
    }
}