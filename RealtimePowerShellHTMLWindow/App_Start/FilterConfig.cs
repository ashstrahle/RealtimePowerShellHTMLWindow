﻿using System.Web.Mvc;

namespace RealtimePowerShellHTMLWindow
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
