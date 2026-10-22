using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using HIDE.RestAndRant.Business;
using HIDE.RestAndRant.Business.Definitions;
using HIDE.Units_Core;

namespace HIDE.RestAndRant.Web_Api.Auth
{
    //Source: https://codeburst.io/adding-basic-authentication-to-an-asp-net-core-web-api-project-5439c4cf78ee
    //Git: https://github.com/dotnet-labs/ApiAuthDemo

    /// <summary>
    /// BasicAuthFilter
    /// </summary>
    public class BasicAuthFilter : IAuthorizationFilter
    {
        private readonly string _realm;
        private readonly BL_Context _bl;

        /// <summary>
        /// BasicAuthFilter
        /// </summary>
        /// <param name="realm"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BasicAuthFilter(string realm)
        {
            _bl = new();
            _realm = realm;
            if (string.IsNullOrWhiteSpace(_realm))
            {
                throw new ArgumentNullException(nameof(realm), @"Please provide a non-empty realm value.");
            }
        }


        /// <summary>
        /// Событие при авторизации
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                Endpoint? endpoint = context.HttpContext.GetEndpoint();
                if (endpoint != null && endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null)
                {
                    //Разрешено анонимное подключение
                    return;
                }

                string? authHeader = context.HttpContext.Request.Headers.Authorization;
                if (authHeader != null)
                {
                    var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                    if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty)).Split(separator: ':', count: 2);
                        if (credentials.Length == 2)
                        {
#if DEBUG
                            int clientid = HIDE_Convert.ToInt(credentials[0]);

                            CClient? client_data = _bl.Clients.Get(ID: clientid);
                            CCommon? client;

                            if (client_data != null)
                            {
                                client = _bl.Clients.Login_Api(ID: client_data.ID.ToString(), Guid: client_data.Guid, Ip: context.HttpContext.Connection.RemoteIpAddress?.ToString());
                            }
                            else
                            {
                                throw new Exception($"Не найден клиент с ID:{clientid}");
                            }

#else
                            CCommon? client = _bl.Clients.Login_Api(ID: credentials[0], Guid: credentials[1], Ip: context.HttpContext.Connection.RemoteIpAddress?.ToString());
#endif
                            if (client != null)
                            {
                                List<Claim> claims =
                                [
                                    new Claim(ClaimTypes.Sid, client.ID.ToString()),
                                    new Claim(ClaimTypes.SerialNumber, client.Name)
                                ];

                                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Basic "));
                                context.HttpContext.User = principal;
                                return;
                            }
                        }
                    }
                }


                ReturnUnauthorizedResult(context); //Если дошли сюда, то авторизация не сработала
            }
            catch (FormatException)
            {
                ReturnUnauthorizedResult(context);
            }
        }

        private void ReturnUnauthorizedResult(AuthorizationFilterContext context)
        {
            // Return 401 and a basic authentication challenge (causes browser to show login dialog)
            context.HttpContext.Response.Headers.WWWAuthenticate = $"Basic realm=\"{_realm}\"";
            context.Result = new UnauthorizedResult();
        }
    }
}