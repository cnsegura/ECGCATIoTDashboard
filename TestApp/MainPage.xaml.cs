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
            string myResponse = await kafkaConsumer.CreateConsumerAsync("sensor_instance", "smallest"); //creating instance name "sensor_instance" and setitng the kafka auto index to the earliest time
            string readMe = myResponse.ToString();
            Status.Text += myResponse + "\n";
            //if (myResponse != null)
            //{
            //    string jsonData = kafkaConsumer.GetConsumerData(Id)
            //}
        }
    }
}
