namespace HIDE.RestAndRant.Business.Helpers
{
    /// <summary>
    /// Помощник - Общие функции
    /// </summary>
    public static class Helper_Common
    {
        private const byte DEFAULT_EMPTY_BYTE_VALUE = 0xFF;


        /// <summary>
        /// Меняет элементы списка местами
        /// </summary>
        /// <typeparam name="T">Generic параметр списка</typeparam>
        /// <param name="list">Список T моделей</param>
        /// <param name="index1">Индекс 1</param>
        /// <param name="index2">Индекс 2</param>
        public static void Swap<T>(this List<T> list, int index1, int index2)
        {
            (list[index2], list[index1]) = (list[index1], list[index2]);
        }


        /// <summary>
        /// Проверка на пустой byte[]
        /// </summary>
        /// <param name="source">Массив байт</param>
        /// <param name="value">Значение для проверки</param>
        /// <returns>True если массив пустой</returns>
        public static bool ByteIsEmpty(byte[] source, byte value = DEFAULT_EMPTY_BYTE_VALUE)
        {
            return source == null || source.All(x => x == value);
        }
    }
}