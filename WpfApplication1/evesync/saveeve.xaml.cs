using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
using System.Security.Cryptography;

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
        void syncstart2()
        {
            log.Add("Старт синхронизации");

            List<evevsyncfile> syncfile = new List<evevsyncfile>();
            var reader1 = BD.conn("SELECT * FROM `evesync` WHERE 1");

            while (reader1.Read())
            {
                syncfile.Add(new evevsyncfile()
                {
                    filename = reader1["filename"].ToString(),
                    md5 = reader1["md5"].ToString(),
                });

            }
            log.Add("Запрос из бд выполнен");

            log.Add("Где смотрим папки - " + path_ordini);
            string[] dirs = Directory.GetFiles(path_ordini, "*.eve");
            vsegofiles = dirs.Length;
            log.Add("Получили число файлов eve - " + vsegofiles);
            foreach (string dir in dirs)
            {
                MD5 myRIPEMD160 = MD5.Create();
                FileStream tmppathStream = File.OpenRead(dir);
                byte[] hashValue = myRIPEMD160.ComputeHash(tmppathStream);
                tmppathStream.Close();
                string hash = BitConverter.ToString(hashValue).Replace("-", String.Empty);

                string filename = dir.Split('\\').Last();
                // MessageBox.Show(filename);

                string md5bd = "";

                try
                {
                    md5bd = (syncfile.Find(x => x.filename.Equals(filename))).md5;
                }
                catch { }
                if (md5bd == hash)
                {

                }
                else
                {
                    if (md5bd == "")
                    {

                        BD.conn("INSERT INTO evesync (filename, md5) VALUES ('" + filename + "','" + hash + "') ");

                    }
                    else
                    {
                        //   MessageBox.Show(md5bd + " " + hash);
                        BD.conn("UPDATE evesync SET md5='" + hash + "' WHERE filename='" + filename + "' ");

                    }
                    nadosave.Add(new evevsyncfile
                    {
                        filename = filename,
                        md5 = hash

                    });
                }

                //   BD.conn("INSERT INTO evesync (filename, md5) VALUES ('" + filename + "','" + hash + "') ");
            }
            log.Add("Цикл выполнен");
            kolvozagr = nadosave.Count;

            //pb1.Minimum = 0;
            //pb1.Maximum = nadosave.Count;
            //pb1.Value = 0;
            //pb1.Visibility = Visibility.Visible;

            if (nadosave.Count == 0)
            {

                Close();
            }
            else
            {
                backgroundWorker.RunWorkerAsync();
            }

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
