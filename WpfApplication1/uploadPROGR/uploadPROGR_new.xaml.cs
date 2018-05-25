using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    /// Логика взаимодействия для uploadPROGR.xaml
    /// </summary>
    public partial class uploadPROGR_new : Window
    {
        public string retzakaz;
        string path;
        string nomzakaza;


        public uploadPROGR_new(string path)
        {

            InitializeComponent();

            this.path = path;

            //   backgroundWorker = (BackgroundWorker)this.FindResource("backgroundWoker");

            soder.Title = "Отправка заказа " + path.Split('\\').Last();
            nomzakaza = path.Split('\\').Last().Split('.').First();
            //   MessageBox.Show("");
        }






        void FTPupload(string path, string name)
        {


            // отправляем файл на сервер
            FileInfo toUpload = new FileInfo(path);
            Uri ulr = new Uri("ftp://ecad.giulianovars.ru/public/zakaz/for_spis/" + name);
         
             FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ulr);
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
            



        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            INIManager manager = new INIManager(Environment.CurrentDirectory + @"\ecadpro.ini");
            string authotiz_root = manager.GetPrivateString("giulianovars", "attivazione");//получ ключ активации



            //отрпавка самого фала
            //     FileStream fileStream = File.OpenRead(path);
            //   pb1.Minimum = 0;
            //   pb1.Maximum = (fileStream.Length) / 1024;
            //   pb1.Value = 0;
            btnStart.IsEnabled = false;

            // pb1.Value = result;

            string massivfiles = "";
            string pref = authotiz_root + "_" + nomzakaza + "_";

            massivfiles += pref + nomzakaza + ".eve" + ",";
            FTPupload(path, pref + nomzakaza + ".eve"); //1 закачиваем сам заказ



            foreach (spisfiles elem in tb_dop.Items)
            {
                massivfiles += pref + elem.name + ",";

                FTPupload(elem.path, pref + elem.name); //1 закачиваем сам заказ

            }
            massivfiles = massivfiles.Trim(',');




            WebClient client = new WebClient();
            var url = "http://ecad.giulianovars.ru/php/upload_progr/load_spis.php?command=2";
            client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            client.Encoding = Encoding.UTF8;
            var response = client.UploadString(url, "POST", "text=" + rb1.Text + "&files=" + massivfiles + "&authotiz_root=" + authotiz_root+ "&zakaz="+ nomzakaza);

            MessageBox.Show("Заказ успешно отправлен");
            Close();

        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //прикрепить файлы

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                // List<spisfiles> eleme = new List<spisfiles>();
                int i = 0;
                foreach (var file in openFileDialog1.FileNames)
                {
                    i++;
                    spisfiles eleme = new spisfiles()
                    {
                        path = file,
                        name = file.Split('\\').Last(),
                    };
                    tb_dop.Items.Add(eleme);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (tb_dop.SelectedIndex != -1)
            {
                tb_dop.Items.Remove(tb_dop.SelectedItem);
            }
        }
    }
    class spisfiles
    {

        public string path { get; set; }
        public string name { get; set; }
    }
}
