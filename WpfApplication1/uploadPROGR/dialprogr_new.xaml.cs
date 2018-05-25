using Microsoft.Win32;
using Newtonsoft.Json;
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
    public partial class dialprogr_new : Window
    {
        public string retzakaz;
        neqweqe neqqqqq;
        string path;
        string admin;
        string authotiz_root;
        public dialprogr_new(string path, neqweqe neqqqqq)
        {
            InitializeComponent();
            this.neqqqqq = neqqqqq;
            this.path = path;



          


            INIManager client_man = new INIManager(Environment.CurrentDirectory + @"\ecadpro.ini");
             admin = client_man.GetPrivateString("giulianovars", "3dsadmin");//версия клиента
             authotiz_root = client_man.GetPrivateString("giulianovars", "attivazione");//получ ключ активации
            loadspis();
            lb2.Content = "Текущий заказ " + path.Split('\\').Last();


            if (admin != "1")
            {
                admbtn.Visibility = Visibility.Collapsed;
                mivip.Visibility = Visibility.Collapsed;
                miarh.Visibility = Visibility.Collapsed;
                mizagr.Visibility = Visibility.Collapsed;

            }

            
        }


        void loadspis()
        {
            WebClient client = new WebClient();
           var url = "http://ecad.giulianovars.ru/php/upload_progr/load_spis.php?command=1&authotiz_root="+ authotiz_root+"&admin="+admin;
            var response = client.DownloadString(url);
            List<zayavki> json = JsonConvert.DeserializeObject<List<zayavki>>(response);
            log.Add("получили данные");
            int i = 0;
            foreach (zayavki elem in json)
            {
                string statispre;
                switch (elem.status) {
                 
                    case "1":
                        statispre = "Выполнено";
                        break;
                    case "2":
                        statispre = "Отменено";
                        break;
                    default:
                        statispre = "Заявка отправлена";
                        break;
                }

                json[i].status_preobr = statispre;
                i++;
            }

            lb1.ItemsSource = json;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();

            //   skachatb();
        }
        private void lb1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            skachatb();
        }
        void skachatb()
        {


            if (admin == "1")//если админ
            {




                WebClient client = new WebClient();
                var url = "http://ecad.giulianovars.ru/zakaz/" + (lb1.SelectedItem as string);
                INIManager client_man = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
                string path_sysdba = client_man.GetPrivateString("Infogen", "percorsoordini");//версия клиента
                string tmppath = Environment.CurrentDirectory + @"\" + path_sysdba + @"\000001.eve";
                log.Add("Путь куда установили " + tmppath);
                client.DownloadFile(url, tmppath);//скачаем новую


                neqqqqq.getParam(neqqqqq.xamb, "carica", neqqqqq.pathordini + @"\000001.eve");
                Close();

                retzakaz = "1";
                MessageBox.Show("Готово, теперь в номере заказа укажите 1");

            }
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            // (sender as Button).IsEnabled = false;

            //2. отправляем файл на сервер
            FileInfo toUpload = new FileInfo(path);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ecad.giulianovars.ru/public/zakaz//" + toUpload.Name);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("ecad_ftp", "bFqeNo4Xp2");
            Stream ftpStream = request.GetRequestStream();
            FileStream fileStream = File.OpenRead(path);
            byte[] buffer = new byte[1024];
            int i = 0;
            int bytesRead = 0;
            do
            {
                i++;
                bytesRead = fileStream.Read(buffer, 0, 1024);
                ftpStream.Write(buffer, 0, bytesRead);
            }
            while (bytesRead != 0);
            fileStream.Close();
            ftpStream.Close();

            MessageBox.Show("Заказ успешно отправлен");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //для админа
            dialprogr dial = new dialprogr(neqqqqq);
            dial.ShowDialog();
            retzakaz = dial.retzakaz;
            Close();
            //    getParam(xamb, "carica", path.ToString());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            uploadPROGR_new asd = new uploadPROGR_new(path);
            asd.ShowDialog();
            loadspis();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //загрузить

            

            WebClient client = new WebClient();
            var url = "http://ecad.giulianovars.ru/zakaz/for_spis/" + (lb1.SelectedItem as zayavki).files.Split(',').First();
            INIManager client_man = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
            string path_sysdba = client_man.GetPrivateString("Infogen", "percorsoordini");//версия клиента
            string tmppath = Environment.CurrentDirectory + @"\" + path_sysdba + @"\000001.eve";
            log.Add("Путь куда установили " + tmppath);
            // MessageBox.Show(tmppath);
            client.DownloadFile(url, tmppath);//скачаем новую


            neqqqqq.getParam(neqqqqq.xamb, "carica", tmppath);
            neqqqqq.getParamI(neqqqqq.Ambiente, "bcarica");

            retzakaz = "1";
         //   MessageBox.Show("Готово, теперь в номере заказа укажите 1");
           
            
           Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //скачть файлы
            SaveFileDialog savenFileDialog1 = new SaveFileDialog();
            savenFileDialog1.CheckFileExists = false;
            savenFileDialog1.RestoreDirectory = true;



            if (savenFileDialog1.ShowDialog() == true)
            {
                // savenFileDialog1.FileName
                var massiv = (lb1.SelectedItem as zayavki).files.Split(',');


                foreach (string elem in massiv)
                {
                    int bytesRead = 0;
                    byte[] buffer = new byte[1024];


                    Uri ulr = new Uri("ftp://ecad.giulianovars.ru/public/zakaz/for_spis/" + elem);

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ulr);
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.Credentials = new NetworkCredential("ecad_ftp", "bFqeNo4Xp2");

                    FileInfo fi = new FileInfo(savenFileDialog1.FileName);
                  
                    Stream reader = request.GetResponse().GetResponseStream();
                    FileStream fileStream = new FileStream(fi.Directory.FullName+"\\"+ elem, FileMode.Create);

                    while (true)
                    {
                        bytesRead = reader.Read(buffer, 0, buffer.Length);

                        if (bytesRead == 0)
                            break;

                        fileStream.Write(buffer, 0, bytesRead);
                    }
                    fileStream.Close();
                }

            }

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //статус выполнено

            WebClient client = new WebClient();
            var url = "http://ecad.giulianovars.ru/php/upload_progr/load_spis.php?command=3&id=" + (lb1.SelectedItem as zayavki).id;
            var response = client.DownloadString(url);
            MessageBox.Show("Статус обновлён");
            loadspis();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            //статус отмен
            WebClient client = new WebClient();
            var url = "http://ecad.giulianovars.ru/php/upload_progr/load_spis.php?command=4&id=" + (lb1.SelectedItem as zayavki).id;
            var response = client.DownloadString(url);
            MessageBox.Show("Статус обновлён");
            loadspis();
        }
    }



    class zayavki
    {
        public string id { get; set; }
        public string nom_zayav { get; set; }
        public string text { get; set; }
        public string files { get; set; }
        public string status { get; set; }
        public string time_sozd { get; set; }
        public string zakaz { get; set; }
        public string status_preobr { get; set; }

       

    }



}
