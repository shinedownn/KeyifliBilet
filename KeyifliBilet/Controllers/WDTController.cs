using KeyifliBilet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace KeyifliBilet.Controllers
{
    public class WDTController : Controller
    {
        public JsonResult Autocomplete(string q)
        {
            string filepath = Server.MapPath("~/Scripts/airports-tr.json");

            string jsonText = System.IO.File.ReadAllText(filepath);
            var titlecase = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(q);
            string bisey = string.Join("", q.Normalize(System.Text.NormalizationForm.FormD).Where(k => char.GetUnicodeCategory(k) != System.Globalization.UnicodeCategory.NonSpacingMark));
            JObject jsonVal = JObject.Parse(jsonText);
            var r = jsonVal["result"].ToString();
            var list= JsonConvert.DeserializeObject<List<Airport>>(r);
            var result = list
            .Where(x => x.name.StartsWith(titlecase)            
            || x.cityName.Contains(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(q)) 
            || x.code.Contains(q.ToUpper())
            || x.cityCode.StartsWith(titlecase.ToUpper())).ToList();

            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}