using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindSpeedFrom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void  button1_Click(object sender, EventArgs e)
        {
            string city = textBox2.Text;
            string state = textBox3.Text;
            string url = $"http://api.aerisapi.com/observations/{city},{state}?client_id=Um44SiagDc91Dj153tZDQ&client_secret=bE65GvB4SSkxvvO0Cca18exgbmloGhS0iIRT8P5Q";

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                
                    var jsonString = await response.Content.ReadAsStringAsync();
                    JObject resultJson = (JObject)JsonConvert.DeserializeObject<object>(jsonString);
                string ac = (string)resultJson["success"];
                if ( (string)resultJson["success"] == "True")
                { 
                    int windSpeed = (int)resultJson["response"]["ob"]["windSpeedKPH"];

                    if (windSpeed > 8)
                    {
                        textBox1.Text = $"Wind speed - {windSpeed} KPH \n The weather in {city} is too windy for bycicling";
                    }
                    else
                    {
                        textBox1.Text = $"Wind speed - {windSpeed} KPH \n The weather in {city} is perfect for bycicling";
                    }
                }
                else 
                {
                    textBox1.Text = "Sorry, the is no data about your city";
                }
            }
        }
    }
}
