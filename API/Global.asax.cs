using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace API
{
    public class WebApiApplication :HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(connectionString, MySqlDialect.Provider);

            Application["DbFactory"] = dbFactory;
        }
    }
}
