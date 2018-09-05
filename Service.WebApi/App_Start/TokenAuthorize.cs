using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;

namespace Service.WebApi
{
    /// <summary>
    /// 重写实现处理授权失败时返回json,避免跳转登录页
    /// </summary>
    public class TokenAuthorize : ActionFilterAttribute
    {
        public string tokenKey { get; set; }
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!string.IsNullOrEmpty(tokenKey)) {
                string mTokenKey = System.Configuration.ConfigurationManager.AppSettings[tokenKey].ToString();

                List<string> str = new List<string>();
                IEnumerable<string> methodOverrideHeader;
                if (actionContext.Request.Headers.TryGetValues("Token", out methodOverrideHeader))
                {
                    string _token = methodOverrideHeader.First();
                    if (_token != mTokenKey)
                    {
                        var response = actionContext.Response = actionContext.Response ?? new HttpResponseMessage();
                        response.StatusCode = HttpStatusCode.Forbidden;
                        response.Content = new StringContent("你没有权限", Encoding.UTF8, "text/html");
                    }
                }
                else {
                    var response = actionContext.Response = actionContext.Response ?? new HttpResponseMessage();
                    response.StatusCode = HttpStatusCode.Forbidden;
                    response.Content = new StringContent("你没有权限", Encoding.UTF8, "text/html");
                }
            }
            else
            {
                var response = actionContext.Response = actionContext.Response ?? new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.Forbidden;
                response.Content = new StringContent("你没有权限", Encoding.UTF8, "text/html");
            }
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}