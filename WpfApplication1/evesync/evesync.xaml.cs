using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
        private BackgroundWorker backgroundWorker;

        public evesync()
        {
            InitializeComponent();
            backgroundWorker = (BackgroundWorker)this.FindResource("backgroundWoker");

            yadisk.tokenfromsetting();
            token.Content = yadisk.OAuthp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            yadisk.Authorization();
            yadisk.tokenfromsetting();
            token.Content = yadisk.OAuthp;
        }




        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            yadisk.combat_zapros("CREATEDIR", tb1.Text, tb2.Text);

        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            yadisk.combat_zapros("PUT", tb1.Text, tb2.Text);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            yadisk.combat_zapros("GET", tb1.Text, tb2.Text);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            yadisk.combat_zapros("DELETE", tb1.Text, tb2.Text);
        }



        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            txtProgress.Content = (e.ProgressPercentage.ToString() + "%");
            pb1.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string[] dirs = Directory.GetFiles(@"C:\evolution\giulianovars\_ecadpro\ordini", "*.eve");


            int i = 0;
            BackgroundWorker worker = sender as BackgroundWorker;


            foreach (string dir in dirs)
            {
                i++;
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    yadisk yadisk1 = new yadisk();
                    yadisk1.tokenfromsetting();
                    yadisk1.combat_zapros("PUT", @"GN_arhiv/" + dir.Split('\\').Last(), dir);

                    worker.ReportProgress(i);
                }
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                this.pb1.Visibility = Visibility.Hidden;
                this.txtProgress.Content = "Отмена!";
            }

            else if (!(e.Error == null))
            {
                this.pb1.Visibility = Visibility.Hidden;
                this.txtProgress.Content = ("Ошибка: " + e.Error.Message);
            }

            else
            {
                this.txtProgress.Content = "Готово!";
                this.pb1.Visibility = Visibility.Hidden;
                //    btnCancel.IsEnabled = false;
                //   btnStart.IsEnabled = true;
            }

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string[] dirs = Directory.GetFiles(@"C:\evolution\giulianovars\_ecadpro\ordini", "*.eve");

            pb1.Minimum = 0;
            pb1.Maximum = dirs.Length;
            pb1.Value = 0;
            pb1.Visibility = Visibility.Visible;
            backgroundWorker.RunWorkerAsync();



        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            saveeve saveeve = new saveeve();
            saveeve.ShowDialog();
        }
    }

    public class yadisk
    {

        public string OAuthp = "";

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

        public void combat_zapros(string command, string path, string localpath = "")
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


                }

            }
            catch (WebException ex)
            {
                int statusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                MessageBox.Show("Ошибка: " + statusCode.ToString());

            }



        }
    }
}
