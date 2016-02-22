using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECGCATIoTDashboard.Controllers
{
    public class KafkaController : Controller
    {
        // GET: Kafka
        public ActionResult Index()
        {
            
            return View();
        }

        [OutputCache(NoStore =true, Location =System.Web.UI.OutputCacheLocation.Client, Duration =3)]
        public PartialViewResult InitConsumer()
        {
            string time = DateTime.UtcNow.ToString();
            TempData["time"] = time;
            return PartialView("_KafkaData");


        }
    }
}