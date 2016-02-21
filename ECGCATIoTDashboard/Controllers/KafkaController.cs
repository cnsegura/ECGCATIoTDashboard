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

        public PartialViewResult InitConsumer()
        {
            string time = DateTime.UtcNow.ToString();
            return PartialView("_KafkaData", time);


        }
    }
}