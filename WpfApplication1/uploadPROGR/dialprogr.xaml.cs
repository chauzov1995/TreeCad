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
        neqweqe neqqqqq;
        public dialprogr(neqweqe neqqqqq)
        {
            InitializeComponent();
          this.neqqqqq = neqqqqq;
          
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ecad.giulianovars.ru/public/zakaz//");

            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential("ecad_ftp", "bFqeNo4Xp2");
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();


            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string spisokfiles = reader.ReadToEnd();
            string[] tolb1 = spisokfiles.Split('\n');

            for (int i = 0; i < tolb1.Length; i++)
            {
                tolb1[i] = tolb1[i].Split(' ').Last();

            }

            lb1.ItemsSource = tolb1;



            //  MessageBox.Show(reader.ReadToEnd());

            reader.Close();
            responseStream.Close();
            response.Close();
            Console.Read();



            List<nazveve> eve = new List<nazveve>();
            eve.Add(new nazveve
            {
                name = "",
                path = ""
            });
            //  lb1.ItemsSource();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("http://ecad.giulianovars.ru/zakaz/" + (lb1.SelectedItem as string));
            WebClient client = new WebClient();
            var url = "http://ecad.giulianovars.ru/zakaz/" + (lb1.SelectedItem as string);

            INIManager client_man = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
            string path_sysdba = client_man.GetPrivateString("Infogen", "percorsoordini");//версия клиента

            string tmppath = Environment.CurrentDirectory + @"\" + path_sysdba + @"\000001.eve";

           // MessageBox.Show(tmppath);
            client.DownloadFile(url, tmppath);//скачаем новую


            neqqqqq.getParam(neqqqqq.xamb, "carica", neqqqqq.pathordini + @"\000001.eve");
            Close();
        }
    }
    class nazveve
    {
        public string name;
        public string path;
    }
}
