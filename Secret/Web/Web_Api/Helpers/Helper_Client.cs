using System.Security.Claims;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Api.Helpers
{
    /// <summary>
    /// Хелпер для работы с клиентом
    /// </summary>
    public static class Helper_Client
    {
        /// <summary>
        /// Возвращает код пользователя по данным аунтификации|\
        /// </summary>
        /// <param name="User"></param>
        /// <returns>CCommon, ID=id клиента, Name=guid клиента</returns>
        public static CCommon? GetClient(ClaimsPrincipal User)
        {
            if (User != null)
            {
                Claim? claim_id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                Claim? claim_guid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber);


                if (claim_id != null && claim_guid != null)
                {
                    CCommon item = new()
                    {
                        ID = HIDE_Convert.ToInt_Nullable(claim_id.Value) ?? -1,
                        Name = claim_guid.Value,
                    };

                    return item;
                }
            }
            //TODO: Logs
            return null;
        }

    }
}