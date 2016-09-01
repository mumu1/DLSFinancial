using System.Web.Mvc;

namespace BEYON.Web.Areas.App
{
    public class AppRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "App";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "App_default",
                "App/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "BEYON.Web.Areas.App.Controllers" }
            );
        }
    }
}
