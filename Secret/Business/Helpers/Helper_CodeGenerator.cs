using HIDE.RestAndRant.Common;

namespace HIDE.RestAndRant.Business.Helpers
{
    /// <summary>
    /// Помощник - Генерация кодов подтверждения
    /// </summary>
    public static class Helper_CodeGenerator
    {
        private const int DEFAULT_CODE_LENGTH = 4;
        private const string DEFAULT_TEST_CODE = "1234";
        private const int START_MULTIPLIER = 1;
        private const int END_MULTIPLIER = 10;


        /// <summary>
        /// Генерация кода для Email
        /// </summary>
        /// <param name="ClientID">Код клиента</param>
        /// <param name="Length">Длина кода</param>
        /// <returns>Сгенерированный код</returns>
        public static string NumGenerate_Email(int ClientID, int Length = DEFAULT_CODE_LENGTH)
        {
            int startnum = START_MULTIPLIER;
            int endnum = END_MULTIPLIER;

            for (int i = 1; i < Length; i++)
            {
                startnum *= END_MULTIPLIER;
                endnum *= END_MULTIPLIER;
            }

            string code = new Random().Next(startnum, endnum - 1).ToString();

            if (!AppConfig.Mail_Send)
            {
                code = DEFAULT_TEST_CODE;
            }

            if (AppConfig.Testers_Code_ClientIDs != null && AppConfig.Testers_Code_ClientIDs.Count != 0 && AppConfig.Testers_Code_ClientIDs.Contains(ClientID))
            {
                code = AppConfig.Testers_Code_Val ?? DEFAULT_TEST_CODE;
            }

            return code;
        }


        /// <summary>
        /// Генерация кода для SMS
        /// </summary>
        /// <param name="ClientID">Код клиента</param>
        /// <param name="Length">Длина кода</param>
        /// <returns>Сгенерированный код</returns>
        public static string NumGenerate_Sms(int ClientID, int Length = DEFAULT_CODE_LENGTH)
        {
            int startnum = START_MULTIPLIER;
            int endnum = END_MULTIPLIER;

            for (int i = 1; i < Length; i++)
            {
                startnum *= END_MULTIPLIER;
                endnum *= END_MULTIPLIER;
            }

            string code = new Random().Next(startnum, endnum - 1).ToString();

            if (!AppConfig.SMS_Send)
            {
                code = DEFAULT_TEST_CODE;
            }

            if (AppConfig.Testers_Code_ClientIDs != null && AppConfig.Testers_Code_ClientIDs.Count != 0 && AppConfig.Testers_Code_ClientIDs.Contains(ClientID))
            {
                code = AppConfig.Testers_Code_Val ?? DEFAULT_TEST_CODE;
            }

            return code;
        }
    }
}