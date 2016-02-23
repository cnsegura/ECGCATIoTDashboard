using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Newtonsoft.Json;
using KafkaConsumer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var kafkaConsumer = new KafkaConsumerMgr();
            HttpResponseMessage myResponse = await kafkaConsumer.PostCreateConsumerAsync("sensor_instance", "smallest"); //creating instance name "sensor_instance" and setitng the kafka auto index to the earliest time

            if (myResponse.IsSuccessStatusCode == false)
            {
                string readMe = myResponse.StatusCode.ToString();
                Status.Text += myResponse + "\n";
            }
            else
            {
                string deserializeContent = await myResponse.Content.ReadAsStringAsync(); //get instance_id and base_uri info from HTTP content
                KafkaRestData kafkaRestData = JsonConvert.DeserializeObject<KafkaRestData>(deserializeContent); //convert to Json and put into objects kafkarestdata.instance_id etc.
                string topicPath = "/topcs/SensorData"; // need to make this generic in the future, will pass into the method from webpage

                myResponse = await kafkaConsumer.GetConsumerDataAsync(kafkaRestData.instance_id, kafkaRestData.base_uri + topicPath);
                deserializeContent = await myResponse.Content.ReadAsStringAsync(); //get Json string back from Kafka
                Status.Text += deserializeContent + "\n";

                myResponse = await kafkaConsumer.DeleteConsumerAsync(kafkaRestData.base_uri);
            }

        }
    }
}
