using System;
using System.Collections.Generic;
using System.Text;

namespace HIDE.RestAndRant.Common.Models
{
    /// <summary>
    /// Пара (Клиент - Токен)
    /// </summary>
    public class Model_NotifyClientToken
    {

        /// <summary>
        /// Код клиента
        /// </summary>
        public int ClientID { get; set; }


        /// <summary>
        /// Токен
        /// </summary>
        public string TokenData { get; set; } = null!;

    }
}
