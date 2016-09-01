using System.Web.Mvc;

namespace BEYON.Web.Areas.Common
{
    public class BasicDataManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BasicDataManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BasicDataManagement_default",
                "BasicDataManagement/{controller}/{action}/{id}",
                new {controller="Login", action = "Index", id = UrlParameter.Optional },
                new string[] { "BEYON.Web.Areas.BasicDataManagement.Controllers" }
            );
        }
    }
}
