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


            WebClient client = new WebClient();
            var url = "http://ecad.giulianovars.ru/zakaz/";
            var response = client.DownloadString(url);
            string[] masssiv = response.Split(new string[] { ".eve\">" }, StringSplitOptions.None);


            foreach (string elem in masssiv)
            {
                string element = elem.Split(new string[] { ".eve</a>" }, StringSplitOptions.None).First();

                if (element.Length <= 6)
                {



                    eve.Add(element + ".eve");

                }
            }


            /*

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ecad.giulianovars.ru/public/zakaz//");

            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential("ecad_ftp", "bFqeNo4Xp2");
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            log.Add("запрос к фтп успех");
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string spisokfiles = reader.ReadToEnd();
            string[] tolb1 = spisokfiles.Split('\n');
            log.Add(spisokfiles);
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
             */


            lb1.ItemsSource = eve;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            skachatb();
        }
        void skachatb()
        {




            //MessageBox.Show("http://ecad.giulianovars.ru/zakaz/" + (lb1.SelectedItem as string));
            WebClient client = new WebClient();
            var url = "http://ecad.giulianovars.ru/zakaz/" + (lb1.SelectedItem as string);
            INIManager client_man = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
            string path_sysdba = client_man.GetPrivateString("Infogen", "percorsoordini");//версия клиента
            string tmppath = Environment.CurrentDirectory + @"\" + path_sysdba + @"\000001.eve";
            log.Add("Путь куда установили " + tmppath);
            // MessageBox.Show(tmppath);
            client.DownloadFile(url, tmppath);//скачаем новую


        

            retzakaz = "1";


            neqqqqq.getParam(neqqqqq.xamb, "carica", tmppath);
            neqqqqq.getParamI(neqqqqq.Ambiente, "bcarica");
            //  MessageBox.Show("Готово, теперь в номере заказа укажите 1");

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
