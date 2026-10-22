using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace HIDE.RestAndRant.Web_Api.Controllers
{

    /// <summary>
    /// Контроллер версии проекта
    /// </summary>
    public class VersionController : Controller
    {
        /// <summary>
        /// Отображает версию проекта
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Version")]
        public string Index()
        {
            string? ver = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileInfo fileInfo = new(assembly.Location);
            DateTime lastModified = fileInfo.LastWriteTime;


            return $"HIDE Version: {ver}\n" +
                   $"Date File: {lastModified}";
        }

    }
}
