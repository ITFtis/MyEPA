using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyEPA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //隱藏 ASP.NET MVC 版本
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Headers.Remove("X-AspNet-Version");
            response.Headers.Remove("X-Powered-By");
            response.Headers.Remove("Server");
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpRuntimeSection section = (HttpRuntimeSection)ConfigurationManager.GetSection("system.web/httpRuntime");
            int maxFileSize = section.MaxRequestLength * 1024;

            if (Request.ContentLength > maxFileSize)
            {
                throw new Exception("超過系統可上傳檔案上限");
            }
        }
    }
}
