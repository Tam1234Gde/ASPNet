namespace HIDE.RestAndRant.Web_Api.Models.v3
{
    /// <summary>
    /// Класс для возврата данных
    /// </summary>
    /// <remarks>
    /// Конструктор 
    /// </remarks>
    /// <param name="Status_Val">Статус OK/Err</param>
    public class Result_v3(string Status_Val)
    {
        /// <summary>
        /// Статус: OK/Err
        /// </summary>
        public string Status { get; private set; } = Status_Val;


        /// <summary>
        /// Код статуса 
        /// </summary>
        public int Code { get; set; }


        /// <summary>
        /// Данные (необязательно)
        /// </summary>
        public object? Data { get; set; }


        /// <summary>
        /// Строка сообщения (необязательно)
        /// </summary>
        public string? Message { get; set; }


        /// <summary>
        /// Вывод всей информации в виде строки
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Status)}: {Status}, {nameof(Code)}: {Code}, {nameof(Data)}: {Data}, {nameof(Message)}: {Message}";
        }
    }
}