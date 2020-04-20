using System;
using System.Collections.Generic;
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

namespace TreeCadN.uploadPROGR
{
    /// <summary>
    /// Логика взаимодействия для dialprogr.xaml
    /// </summary>
    public partial class dialprogr : Window
    {
        public string retzakaz;
        neqweqe neqqqqq;
        public dialprogr(neqweqe neqqqqq)
        {
            InitializeComponent();
            this.neqqqqq = neqqqqq;
            log.Add("подключаемся к фтп");


            List<string> eve = new List<string>();


       //     WebClient client = new WebClient();
            var url = "ftp://213.159.210.45/zakaz/";
        



            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("ecad", "UWnlLh3PLy");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string responses = reader.ReadToEnd();

           // MessageBox.Show(responses);

            reader.Close();
            response.Close();




            string[] masssiv = responses.Split('\r');


            foreach (string elem in masssiv)
            {
             //   MessageBox.Show(elem.Trim());
             //   string element = elem.Split(new string[] { ".eve</a>" }, StringSplitOptions.None).First();

                if (elem.Trim().IndexOf("for_spis")==-1 && elem.Trim().Length == 10)
                {

                  

                    eve.Add(elem.Trim());

                }
            }



            lb1.ItemsSource = eve;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            skachatb();
        }
        void skachatb()
        {




          
     
            var url = "ftp://213.159.210.45/zakaz/" + (lb1.SelectedItem as string);
            INIManager client_man = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
            string path_sysdba = client_man.GetPrivateString("Infogen", "percorsoordini");//версия клиента
            string tmppath = Environment.CurrentDirectory + @"\" + path_sysdba + @"\000001.eve";
            log.Add("Путь куда установили " + tmppath);
            // MessageBox.Show(tmppath);
          //  client.DownloadFile(url, tmppath);//скачаем новую




            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("ecad", "UWnlLh3PLy");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string result = reader.ReadToEnd();

            //save file locally on your pc
            using (StreamWriter file = File.CreateText(tmppath))
            {
                file.WriteLine(result);
                file.Close();

            }
            reader.Close();
            response.Close();




            retzakaz = "1";


            neqqqqq.getParam(neqqqqq.xamb, "carica", tmppath);
            neqqqqq.getParamI(neqqqqq.Ambiente, "bcarica");
      

            Close();
        }

        private void lb1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            skachatb();
        }
    }
    class nazveve
    {
        public string name;
        public string path;
    }
}
