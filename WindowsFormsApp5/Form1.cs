using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;


namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            int traveltype;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var date = DateTime.Now;
            var datum = DateTime.Now.ToString("yyyy-MM-dd");
            int ora = date.Hour;
           int perc = date.Minute;
            listBox1.Items.Clear();
            int traveltype=0;
            if (comboBox1.SelectedItem == "Busz")
            {
                traveltype = 1;
            }
            if (comboBox1.SelectedItem == "Vonat")
            {
                traveltype = 4;
            }
            if (comboBox1.SelectedItem == "Mindkettő")
            {
                traveltype = 0;
            }
            try
            {
                string webAddr = "https://menetrendek.hu/menetrend/interface/index.php";

                var httpWebRequest = WebRequest.CreateHttp(webAddr);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "{ \"func\" : \"getRoutes\", \"params\" :{\"naptipus\":\"0\",\"datum\":\""+datum+"\",\"honnan\":\"" + honnan.Text + "\",\"hova\":\"" + hova.Text + "\",\"hour\":\"0\",\"min\":\"0\",\"preferencia\":\""+traveltype+ "\",\"hour\":\"" + ora + "\",\"minutes\":\"" + perc + "\"}}";
                    streamWriter.Write(json);
                    streamWriter.Flush();

                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    var responseText = streamReader.ReadToEnd();
                    //   Console.WriteLine(responseText);
                    var data = (JObject)JsonConvert.DeserializeObject(responseText);


                    for (int i = 1; i < responseText.Length; i++)
                    {
                        var indulasihely = data["results"]["talalatok"][$"{i}"]["indulasi_hely"];
                        var erkezesihely = data["results"]["talalatok"][$"{i}"]["erkezesi_hely"];
                        var indulasiido = data["results"]["talalatok"][$"{i}"]["indulasi_ido"];
                        var erkezesiido = data["results"]["talalatok"][$"{i}"]["erkezesi_ido"];
                        var atszallas = data["results"]["talalatok"][$"{i}"]["atszallasok_szama"];
                       // var atszallasinfok = data["results"]["talalatok"][$"{i}"]["atszallasinfok"]["1"]["atszallohely"];
                        var osszido = data["results"]["talalatok"][$"{i}"]["osszido"];

                        listBox1.Items.Add ($"{indulasihely} {indulasiido} - {erkezesihely} {erkezesiido} -- Összidő:{osszido} Átszállás: {atszallas}");



                    }
                }
            }
            catch (WebException ex)
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
       

            comboBox1.Items.Add("Busz");

            comboBox1.Items.Add("Vonat");
  
            comboBox1.Items.Add("Mindkettő");

        }
    }
}
