using System.Security.Claims;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Admin_ForMenu.Helpers
{
    public static class Helper_User
    {
        public static int GetCurrentUserID(ClaimsPrincipal User)
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