using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KafkaConsumer
{
    public class KafkaConsumerMgr
    {
        public async Task<HttpResponseMessage> CreateConsumer(string _instanceName, bool _fromBeginning) 
        {
            //string topicString = "/SensorData";
            //UriBuilder u1 = new UriBuilder();
            ////u1.Host = "localhost"; //DEBUG
            //u1.Host = "wssccatiot.westus.cloudapp.azure.com";
            //u1.Port = 8082;
            //u1.Path = "topics" + topicString;
            //u1.Scheme = "http";
            //Uri topicUri = u1.Uri;
            string topicUri = "http://wssccatiot.westus.cloudapp.azure.com:8082/consumers/json_consumer";
            //Currently focused on REST API surface for Confluent.io Kafka deployment. We can make this more generic in the future
            string jsonHeader = ("{\"name\": "); //same as above, fixing string for Server requirements
            string jsonBody = "\"" + _instanceName + "\", \"format\": \"json\", "; 
            string jsonFooter = ("}]}"); //ditto
            string json = jsonHeader + jsonBody + jsonFooter;

            var baseFilter = new HttpClientHandler();
            baseFilter.AutomaticDecompression = System.Net.DecompressionMethods.None; //turn off all compression methods
            HttpClient httpClient = new HttpClient(baseFilter);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.kafka.json.v1+json")); //Add Accept: application/vnd.kafka.json.vl+json, application... header )
            HttpContent postBody = new StringContent(json);
            postBody.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.kafka.json.v1+json"); //set Content-Type header
            HttpResponseMessage postResponse = await httpClient.PostAsync(topicUri,postBody);
            return postResponse;
        }

    }
}
