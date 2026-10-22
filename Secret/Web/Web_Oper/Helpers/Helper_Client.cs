using System.Security.Claims;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Oper.Helpers
{
    public static class Helper_Client
    {
        public static int GetCurrentClientID(ClaimsPrincipal User)
        {
            if (User != null)
            {
                var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (claim != null)
                {
                    return HIDE_Convert.ToInt(claim.Value);
                }
            }
            return -1;
        }
    }
}