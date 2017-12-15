using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
    /// Логика взаимодействия для uploadPROGR.xaml
    /// </summary>
    public partial class uploadPROGR : Window
    {
        private BackgroundWorker backgroundWorker;
        string path;
        neqweqe neqqqqq;

        public uploadPROGR(string path, neqweqe neqqqqq)
        {

            InitializeComponent();
            this.neqqqqq = neqqqqq;
            this.path = path;

            backgroundWorker = (BackgroundWorker)this.FindResource("backgroundWoker");

            lb1.Content = "Отправка заказа " + path.Split('\\').Last();
            this.pb1.Visibility = Visibility.Hidden;


           this.btn2.Visibility = Visibility.Hidden;


           // DirectoryInfo sdsd = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
        //    log.Add("путь к ини"+ sdsd + @"\ecadpro.ini");
            INIManager client_man = new INIManager(Environment.CurrentDirectory + @"\ecadpro.ini");
            string admin = client_man.GetPrivateString("giulianovars", "3dsadmin");//версия клиента
          //  MessageBox.Show("путь к ини" + sdsd + @"\ecadpro.ini");
         
            if (admin == "1")
            {
                btn2.Visibility = Visibility.Visible;

            }


        }


        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //  lb1.Content = e.ProgressPercentage.ToString();// + ((vsegofiles - kolvozagr + e.ProgressPercentage).ToString() + " файлов из " + vsegofiles);
            pb1.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // string pathvspomka = path_ordini + @"\";


            //  int i = 0;
            BackgroundWorker worker = sender as BackgroundWorker;


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
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    // break;
                }
                else
                {
                    //string path_ordini = x.ToString();





                    i++;

                    bytesRead = fileStream.Read(buffer, 0, 1024);
                    worker.ReportProgress(i);
                    ftpStream.Write(buffer, 0, bytesRead);
                }
            }
            while (bytesRead != 0);
            fileStream.Close();
            ftpStream.Close();


        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                //      this.pb1.Visibility = Visibility.Hidden;
                this.lb1.Content = "Отмена!";
            }

            else if (!(e.Error == null))
            {
                this.pb1.Visibility = Visibility.Hidden;
                this.lb1.Content = ("Ошибка: " + e.Error.Message);
            }

            else
            {
                this.lb1.Content = "Готово! Заказ успешно отправлен";





                this.pb1.Visibility = Visibility.Hidden;
                //    btnCancel.IsEnabled = false;
                btnStart.IsEnabled = true;
                //   Close();
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            FileStream fileStream = File.OpenRead(path);

            pb1.Minimum = 0;
            pb1.Maximum = (fileStream.Length) / 1024;

            this.pb1.Visibility = Visibility.Visible;

            btnStart.IsEnabled = false;
            backgroundWorker.RunWorkerAsync();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dialprogr dial = new dialprogr(neqqqqq);
            dial.ShowDialog();

          
            //    getParam(xamb, "carica", path.ToString());
        }
    }
}
