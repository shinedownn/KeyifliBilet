using KeyifliBilet.Areas.Yonetim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace KeyifliBilet.Areas.Yonetim.Models
{
    public class KullaniciYetkiler:AuthorizeAttribute
    {
        public static int userId = 0;
        KeyifliBiletEntities entities = new KeyifliBiletEntities();
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;

            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName]; // Cookie yi alıyoruz
            if (authCookie != null) // Cokie boş değil ise
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value); // Cookie şifresini kaldırıyoruz

                string[] roles = authTicket.UserData.Split(new[] { ',' }); // , e göre ayırıyoruz
                userId = Convert.ToInt32(roles[0]);

                //userRole = roles[2]; // Kullanıcı rolü [3]'den geliyor. 2. virgüle göre split ediyoruz 
                string data2 = Roles.ToString();
                var data = entities.Rol.Where(w => w.KullaniciId == userId).FirstOrDefault();
                int rolId = data.RolId;
                var query = entities.Yetki.Where(w => w.RolId == rolId).ToList();

                foreach (var role in query) // Roller içinde foreach ile dönüyoruz
                {
                    if (role.MetodAdi == data2) // Eğer kullanıcının rolü "AdminTestRol" ise 
                    {
                        authorize = true; // Yetkiyi true yapıyoruz ve ActionResult'a erişme hakkı  veriyoruz
                        return authorize;
                    }
                }

            }
            else
            {
                authorize = true;
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary {
                  { "action", "ErişimEngeli" },
                  { "controller", "Panel" }
               }
             );
        }
    }
}