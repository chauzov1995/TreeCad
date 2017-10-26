using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TreeCadN.GNviewer
{
    /// <summary>
    /// Логика взаимодействия для teamv.xaml
    /// </summary>
    public partial class teamv : Window
    {

        public teamv(string path_tv)
        {
            InitializeComponent();


            string otvet = "";
            string patholdtv = "";



            string path = @"C:\evolution\giulianovars\GIULIANOVARS\procedure";
            string[] dirs = Directory.GetFiles(path, "*.exe");


            foreach (string dir in dirs)
            {
                if (dir.Split('\\').Last().Substring(0, 12) == "TeamViewerQS")
                {
                    patholdtv = dir;
                    otvet = path;
                    break;

                }

            }

            WebClient webClient = new WebClient();
            string url;
            url = "https://webapi.teamviewer.com/api/v1/sessions";
            webClient.Headers.Add("Authorization", "Bearer 2875381-8jCmpdsLcQm5FelCc9rv");
            webClient.Headers.Add("Content-Type", "application/json");


            string JSON = webClient.UploadString(url, "POST", @"{""groupid"" : ""g18888010""}");
            session_TV login = JsonConvert.DeserializeObject<session_TV>(JSON);

            string code = login.code.Replace("-", "");
            string newpathtv = otvet + @"\TeamViewerQS-id" + code + ".exe";

            File.Move(patholdtv, newpathtv);
            MessageBox.Show("Успех");
            Process.Start(newpathtv);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    class session_TV
    {

        public string code { get; set; }
        public string end_customer_link { get; set; }
        public string supporter_link { get; set; }
    }
}
