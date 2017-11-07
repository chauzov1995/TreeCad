using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace TreeCadN.evesync
{
    /// <summary>
    /// Логика взаимодействия для saveeve.xaml
    /// </summary>
    public partial class saveeve : Window
    {
        BD_Connect BD = new BD_Connect();
        private BackgroundWorker backgroundWorker;
        List<evevsyncfile> nadosave = new List<evevsyncfile>();
        int vsegofiles;
        int kolvozagr;
        string path_ordini;
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();


        public saveeve(string path_bd, string path_ordini)
        {
            InitializeComponent();

            backgroundWorker = (BackgroundWorker)this.FindResource("backgroundWoker");
            this.path_ordini = path_ordini;
            BD.path = path_bd; //укажем файл бд







            this.Top = System.Windows.SystemParameters.PrimaryScreenHeight - 85;
            this.Left = System.Windows.SystemParameters.PrimaryScreenWidth - 330;


            //  Close();

        }


        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //    tb3.Text = "Синхронизированно " + e.ProgressPercentage.ToString();// + ((vsegofiles - kolvozagr + e.ProgressPercentage).ToString() + " файлов из " + vsegofiles);
            //   pb1.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // string pathvspomka = path_ordini + @"\";


            //  int i = 0;
            BackgroundWorker worker = sender as BackgroundWorker;


            //   for (int i = 0; i < nadosave.Count; i++)
            //   {

            if ((worker.CancellationPending == true))
            {
                e.Cancel = true;
                // break;
            }
            else
            {
                //string path_ordini = x.ToString();
                yadisk yadisk1 = new yadisk();
                yadisk1.tokenfromsetting();
                yadisk1.combat_zapros("PUT", @"GN_arhiv/" + path_ordini.Split('\\').Last(), path_ordini);
                //   worker.ReportProgress(1);

            }
            //  }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                //      this.pb1.Visibility = Visibility.Hidden;
                this.tb3.Text = "Отмена!";
            }

            else if (!(e.Error == null))
            {
                //    this.pb1.Visibility = Visibility.Hidden;
                this.tb3.Text = ("Ошибка: " + e.Error.Message);
            }

            else
            {
                this.tb3.Text = "Готово! Заказ успешно синхронизирован";


            


                //   this.pb1.Visibility = Visibility.Hidden;
                //    btnCancel.IsEnabled = false;
                //   btnStart.IsEnabled = true;
                //   Close();
            }

            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5000);
            timer.Start();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { }
        void sync1file()
        {
            yadisk yadisk1 = new yadisk();
            yadisk1.tokenfromsetting();
            yadisk1.combat_zapros("CREATEDIR", "GN_arhiv/");
            yadisk1.combat_zapros("PUT", @"GN_arhiv/" + path_ordini.Split('\\').Last(), path_ordini);
            Close();

        }
       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // sync1file();
            syncstart();
        }


        void syncstart()
        {

            backgroundWorker.RunWorkerAsync();

        }
        private void timerTick(object sender, EventArgs e)
        {

            timer.Stop();
            Close();
        }



    }
    class evevsyncfile
    {
        public string filename { get; set; }
        public string md5 { get; set; }
        public int id { get; set; }

    }
}
