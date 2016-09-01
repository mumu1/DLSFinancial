﻿using System.Web.Mvc;

namespace BEYON.Web.Areas.Common
{
    public class CommonAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Common";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Common_default",
                "Common/{controller}/{action}/{id}",
                new {controller="Login", action = "Index", id = UrlParameter.Optional },
                new string[] { "BEYON.Web.Areas.Common.Controllers" }
            );
        }
    }
}
