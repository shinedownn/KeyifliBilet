using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyifliBilet.Controllers
{
    public class trController : Controller
    {
        [Route("tr/iptal-iade",Name ="iptaliade")]
        public ActionResult IptalIade()
        {
            return View();
        }

        [Route("tr/online-check-in",Name ="onlinecheckin")]
        public ActionResult OnlineCheckIn()
        {
            return View();
        }
        [Route("tr/iletisim",Name ="iletisim")]
        public ActionResult Iletisim()
        {
            return View();
        }
    }
}