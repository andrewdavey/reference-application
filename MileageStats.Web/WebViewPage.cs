/*  
Copyright Microsoft Corporation

Licensed under the Apache License, Version 2.0 (the "License"); you may not
use this file except in compliance with the License. You may obtain a copy of
the License at 

http://www.apache.org/licenses/LICENSE-2.0 

THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED 
WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
MERCHANTABLITY OR NON-INFRINGEMENT. 

See the Apache 2 License for the specific language governing permissions and
limitations under the License. */

using MileageStats.Web.Helpers;

namespace MileageStats.Web
{
    // This is a custom version of the WebViewPage that
    // enables us to reference the Mustache helper 
    // directly in our views. 
    // We've modified the web.config inside the ~/Views
    // folder to reference this class.

    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public override void InitHelpers()
        {
            base.InitHelpers();
            Mustache = new MustacheHelper<TModel>(Html);
        }

        public override string Layout
        {
            get
            {
                if (Html.IsRenderingForMustache())
                {
                    return null;
                }
                return base.Layout;
            }
            set
            {
                base.Layout = value;
            }
        }

        public MustacheHelper<TModel> Mustache
        {
            get;
            private set;
        }
    }
}