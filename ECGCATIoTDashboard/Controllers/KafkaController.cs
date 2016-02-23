using ECGCATIoTDashboard.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

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
        public async Task<PartialViewResult> InitConsumer()
        {
            string storedInstance_id = (string)System.Web.HttpContext.Current.Application["instance_id"];
            if (storedInstance_id == null) //need to create an instance on the Kafka server and store the id and url
            {
                var kafkaConsumer = new KafkaConsumerMgr();
                HttpResponseMessage myResponse = await kafkaConsumer.PostCreateConsumerAsync("sensor_instance", "smallest"); //creating instance name "sensor_instance" and setitng the kafka auto index to the earliest time

                if (myResponse.IsSuccessStatusCode == false)
                {
                    string readMe = myResponse.StatusCode.ToString();
                    //Status.Text += myResponse + "\n";
                }
                else
                {
                    string deserializeContent = await myResponse.Content.ReadAsStringAsync(); //get instance_id and base_uri info from HTTP content
                    KafkaRestData kafkaRestData = JsonConvert.DeserializeObject<KafkaRestData>(deserializeContent); //convert to Json and put into objects kafkarestdata.instance_id etc.
                    System.Web.HttpContext.Current.Application["instance_id"] = kafkaRestData.instance_id; //store instance_id
                    System.Web.HttpContext.Current.Application["base_uri"] = kafkaRestData.base_uri; //store base_uri
                    string topicPath = "/topcs/SensorData"; // need to make this generic in the future

                    myResponse = await kafkaConsumer.GetConsumerDataAsync(kafkaRestData.instance_id, kafkaRestData.base_uri + topicPath);
                    deserializeContent = await myResponse.Content.ReadAsStringAsync(); //get Json string back from Kafka
                    //Status.Text += deserializeContent + "\n";

                }
            }
            else //instance exists we need to retrieve the value and refresh the data
            {
                var kafkaConsumer = new KafkaConsumerMgr();
                string storedBase_Uri = (string)System.Web.HttpContext.Current.Application["base_uri"]; //we alrady have instance_id from the top 
                string topicPath = "/topcs/SensorData"; // need to make this generic in the future

                HttpResponseMessage myResponse = await kafkaConsumer.GetConsumerDataAsync(storedInstance_id, storedBase_Uri + topicPath);
                string deserializeContent = await myResponse.Content.ReadAsStringAsync(); //get Json string back from Kafka
            }

            string time = DateTime.UtcNow.ToString();
            TempData["time"] = time;
            return PartialView("_KafkaData");


        }
    }
}