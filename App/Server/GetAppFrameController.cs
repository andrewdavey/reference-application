﻿using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using App.Infrastructure;

namespace App.Server
{
    public class GetAppFrameController : ApiController
    {
        public object GetAppFrame()
        {
            return new Page("AppFrame/init")
            {
                Data = new
                {
                    links = Links()
                }
            };
        }

        object Links()
        {
            var links = new List<object>
            {
                new {text = "Dashboard", url = "/"},
                new {text = "Profile", url = "/profile"},
                new {text = "Log Out", url = "#"}
            };
            if (HttpContext.Current != null && HttpContext.Current.IsDebuggingEnabled)
            {
                links.Add(new {text = "Specs", url = "/clientspecs", rel="nohijax"});
            }
            return links;
        }
    }
}