using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace KeyifliBilet.Areas.Yonetim.Models
{
    public class MyFilterAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.RequestContext.HttpContext.Session["KullaniciId"] == null)
            {

                filterContext.HttpContext.Response.Redirect("~/Yonetim/Panel/Login");
            }
        }
    }
}