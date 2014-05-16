using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GenericSearch.Core;

namespace GenericSearch.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ModelBinders.Binders[typeof(AbstractSearch)] = new AbstractSearchModelBinder();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
