using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.RestAndRant.Web_Site_Menu.Models.Clients;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Site_Menu.Helpers
{
    /// <summary>
    /// Помощник для клиента
    /// </summary>
    public static class Helper_Client
    {
        /// <summary>
        /// Получить ID клиента
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        public static int GetCurrentClientID(ClaimsPrincipal Client)
        {
            if (Client != null)
            {
                Claim claim = Client.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (claim != null)
                {
                    return HIDE_Convert.ToInt(claim.Value);
                }
            }
            return -1;
        }

        /// <summary>
        /// Возвращает клиента по данным аутентификации
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        public static Model_Client? GetClient(ClaimsPrincipal? Client)
        {
            if (Client != null)
            {
                Claim claim_id = Client.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);

                if (claim_id != null)
                {
                    CClient? client = new Clients().Get(ID: HIDE_Convert.ToInt_Nullable(claim_id.Value) ?? -1);

                    if (client == null)
                    {
                        return null;
                    }

                    Model_Client item = new Model_Client()
                    {
                        Guid = client.Guid,
                    };
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Вспомогательный класс для контроллера (определение пользователя)
        /// </summary>
        public class BaseControllerWithUser : Controller
        {
            /// <summary>
            /// Получение ID авторизованного пользователя
            /// </summary>
            protected virtual int Client_ID
            {
                get { return GetCurrentClientID(HttpContext.User); }
            }

            /// <summary>
            /// Получение ID, FIO и Flag_Admin авторизованного пользователя
            /// </summary>
            protected virtual new Model_Client? User
            {
                get { return GetClient(HttpContext.User); }
            }

            /// <summary>
            /// Возвращает пользователя по данным аутентификации
            /// </summary>
            /// <param name="Client"></param>
            /// <returns></returns>
            public Model_Client? GetClient(ClaimsPrincipal? Client)
            {
                if (Client != null)
                {
                    Claim claim_id = Client.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                    Claim claim_guid = Client.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Hash);

                    if (claim_id != null)
                    {
                        CClient? client = new Clients().Get(ID: HIDE_Convert.ToInt_Nullable(claim_id.Value) ?? -1);

                        if (client == null)
                        {
                            return null;
                        }

                        Model_Client item = new Model_Client()
                        {
                            ID = client.ID,
                            Guid = claim_guid?.Value ?? string.Empty,
                        };

                        return item;
                    }
                }
                return null;
            }

            /// <summary>
            /// Класс для вызова в View
            /// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0120?f1url=%3FappId%3Droslyn%26k%3Dk(CS0120)
            /// </summary>
            /// <param name="Client"></param>
            /// <returns></returns>
            public static Model_Client? GetUserMain(ClaimsPrincipal? Client)
            {
                BaseControllerWithUser class_client = new BaseControllerWithUser();
                Model_Client? client = class_client.GetClient(Client: Client);
                return client;
            }
        }
    }
}