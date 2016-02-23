using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ECGCATIoTDashboard.Models
{
    public class KafkaConsumerMgr
    {
        public async Task<HttpResponseMessage> PostCreateConsumerAsync(string _instanceName, string _offsetPosition)
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
            HttpResponseMessage postResponse = await httpClient.PostAsync(topicUri, postBody);

            return postResponse;
        }
        public async Task<HttpResponseMessage> GetConsumerDataAsync(string _instanceId, string _baseUri)
        {

            var baseFilter = new HttpClientHandler();
            baseFilter.AutomaticDecompression = System.Net.DecompressionMethods.None; //turn off all compression methods
            HttpClient httpClient = new HttpClient(baseFilter);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.kafka.json.v1+json")); //Add Accept: application/vnd.kafka.json.vl+json, application... header )
            HttpResponseMessage postResponse = await httpClient.GetAsync(_baseUri);

            return postResponse;
        }

        public async Task<HttpResponseMessage> DeleteConsumerAsync(string _baseUri)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage postResponse = await httpClient.DeleteAsync(_baseUri);

            return postResponse;
        }


    }
}