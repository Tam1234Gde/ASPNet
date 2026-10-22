namespace HIDE.RestAndRant.Web_Api.Helpers
{
    /// <summary>
    /// Помощник для телефонного номера
    /// </summary>
    public static class Helper_Phone
    {
        /// <summary>
        /// Возвращает телефон в формате 7XXXXXXXXXX или null - если телефон не содержит 11 символов и не начинается с 7
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public static string? GetRussianPhone(string? Phone)
        {
            if (string.IsNullOrWhiteSpace(Phone))
            {
                return null;
            }

            string numbers = new([.. Phone.Where(char.IsDigit)]);

            numbers = numbers.Trim();

            if (string.IsNullOrWhiteSpace(numbers))
            {
                return null;
            }

            if (numbers.Length != 11)
            {
                return null;
            }

            if (!numbers.StartsWith('7'))
            {
                return null;
            }

            return numbers;
        }
    }
}