using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TreeCadN.evesync
{
    /// <summary>
    /// Логика взаимодействия для evesync.xaml
    /// </summary>
    public partial class evesync : Window
    {
        yadisk yadisk = new yadisk();
        string path_bd;
        string path_ordini;
        private BackgroundWorker backgroundWorker;
        private int vsegofiles;

        public evesync(string path_bd, string path_ordini)
        {
            InitializeComponent();

            pb1.Visibility = Visibility.Hidden;
            backgroundWorker = (BackgroundWorker)this.FindResource("backgroundWoker");
            this.path_ordini = path_ordini;
            this.path_bd = path_bd;
            yadisk.tokenfromsetting();
            if (yadisk.OAuthp != "")
            {
                tb1.Content = "Авторизован";
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //    MessageBox.Show("Сейчас будет открыто окно для авторизации, после успешного входа просто закройте его");
            yadisk.Authorization();
            yadisk.tokenfromsetting();
            if (yadisk.OAuthp != "")
            {
                tb1.Content = "Авторизован";
            }

        }

        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            tb3.Content = "Синхронизированно " + e.ProgressPercentage.ToString() + " из " + vsegofiles.ToString();
            pb1.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // string pathvspomka = path_ordini + @"\";


            int i = 0;
            BackgroundWorker worker = sender as BackgroundWorker;


            string[] dirs = Directory.GetFiles(path_ordini, "*.eve");
            // vsegofiles = dirs.Length;

            foreach (string dir in dirs)
            {

                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    //string path_ordini = x.ToString();

                    yadisk yadisk1 = new yadisk();
                    yadisk1.tokenfromsetting();
                    yadisk1.combat_zapros("PUT", @"GN_arhiv/" + dir.Split('\\').Last(), dir);
                    i++;
                    worker.ReportProgress(i);

                }
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                this.pb1.Visibility = Visibility.Hidden;
                this.tb3.Content = "Отмена!";
            }

            else if (!(e.Error == null))
            {
                this.pb1.Visibility = Visibility.Hidden;
                this.tb3.Content = ("Ошибка: " + e.Error.Message);
            }

            else
            {
                this.tb3.Content = "Готово! Все заказы успешно синхронизированны.";





                this.pb1.Visibility = Visibility.Hidden;
                //    btnCancel.IsEnabled = false;
                btn1.IsEnabled = true;
                //   Close();
            }



        }





        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            this.pb1.Visibility = Visibility.Visible;

            btn1.IsEnabled = false;

            string[] dirs = Directory.GetFiles(path_ordini, "*.eve");
            vsegofiles = dirs.Length;
            pb1.Maximum = vsegofiles;
            pb1.Minimum = 0;
            tb3.Content = "Подготовка к загрузке..";
            backgroundWorker.RunWorkerAsync();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            YA ps = YA.Default;
            ps.isSave = (bool)cb1.IsChecked;
            if (yadisk.OAuthp == "")
            {
                ps.isSave = false;
            }
            ps.Save();
            Close();

        }

        private void cb1_Loaded(object sender, RoutedEventArgs e)
        {
            YA ps = YA.Default;
            cb1.IsChecked = ps.isSave;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {


            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;


        }
    }





    public class yadisk
    {

        public string OAuthp = "";
        public string Login = "";

        public void Authorization()
        {
            auth auth1 = new auth();
            auth1.ShowDialog();
        }
        public void tokenfromsetting()
        {
            YA ps = YA.Default;

            OAuthp = ps.OAuth;

        }

        public void combat_zapros(string command, string path = "", string localpath = "")
        {
            WebClient webClient = new WebClient();
            string url;

            try
            {
                switch (command)
                {
                    case "DELETE":

                        url = "https://webdav.yandex.ru/" + path;
                        webClient.Headers.Add("Authorization", "OAuth " + OAuthp);
                        webClient.UploadString(url, "DELETE", "");


                        break;
                    case "CREATEDIR":
                        url = "https://webdav.yandex.ru/" + path;
                        webClient.Headers.Add("Authorization", "OAuth " + OAuthp);
                        webClient.UploadString(url, "MKCOL", "");


                        break;
                    case "PUT":
                        url = "https://webdav.yandex.ru/" + path;
                        webClient.Headers.Add("Authorization", "OAuth " + OAuthp);
                        webClient.UploadFile(url, "PUT", localpath);
                        // MessageBox.Show(response);

                        break;

                    case "GET":
                        url = "https://webdav.yandex.ru/" + path;
                        webClient.Headers.Add("Authorization", "OAuth " + OAuthp);


                        webClient.DownloadFile(url, localpath);


                        break;
                    case "PROPFIND":

                        string prop = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                         <propfind xmlns=""DAV:"">
                            
                        </propfind>";
                        url = "https://webdav.yandex.ru/" + path;
                        webClient.Headers.Add("Authorization", "OAuth " + OAuthp);
                        webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        webClient.Headers.Add("Accept", "*/*");
                        webClient.Headers.Add("Depth", "0");

                        var response = webClient.UploadString(url, "PROPFIND", prop);
                        MessageBox.Show(response.ToString());


                        break;
                    case "LOGIN":
                        url = "https://webdav.yandex.ru/?userinfo";
                        webClient.Headers.Add("Authorization", "OAuth " + OAuthp);
                        Login = webClient.UploadString(url, "GET ", "").ToString();
                        MessageBox.Show(Login.ToString());

                        break;

                }

            }
            catch (WebException ex)
            {
                int statusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                //  MessageBox.Show("Ошибка: " + statusCode.ToString());

            }



        }
    }
}
