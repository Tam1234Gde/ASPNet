using Microsoft.AspNetCore.Mvc;

namespace HIDE.RestAndRant.Web_Api.Auth
{
    /// <summary>
    /// Basic Auth
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BasicAuthAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Basic Auth
        /// </summary>
        /// <param name="realm"></param>
        public BasicAuthAttribute(string realm = @"My Realm") : base(typeof(BasicAuthFilter))
        {
            Arguments = [realm];
        }
    }
}