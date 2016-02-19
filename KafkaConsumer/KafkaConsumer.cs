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
        public async Task<HttpResponseMessage> CreateConsumer(string _instanceName, string _offsetPosition) 
        {
            string topicUri = "http://wssccatiot.westus.cloudapp.azure.com:8082/consumers/json_consumer";
            //Currently focused on REST API surface for Confluent.io Kafka deployment. We can make this more generic in the future
            string jsonHeader = ("{\"name\": "); //same as above, fixing string for Server requirements
            string jsonBody = "\"" + _instanceName + "\", \"format\": \"json\", ";
            string jsonFooter = ("\"auto.offset.reset\": \"") + _offsetPosition + "\"}";
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
        public async Task<string> GetConsumerData(string _instanceId, string _baseUri)
        {
            var baseFilter = new HttpClientHandler();
            baseFilter.AutomaticDecompression = System.Net.DecompressionMethods.None; //turn off all compression methods
            HttpClient httpClient = new HttpClient(baseFilter);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.kafka.json.v1+json")); //Add Accept: application/vnd.kafka.json.vl+json, application... header )
            HttpResponseMessage postResponse = await httpClient.GetAsync(_baseUri);
            string postResponseString = postResponse.Content.ToString();

            return postResponseString;
        }



    }
}
