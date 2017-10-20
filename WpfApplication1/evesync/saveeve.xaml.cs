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

        public saveeve()
        {
            InitializeComponent();

            backgroundWorker = (BackgroundWorker)this.FindResource("backgroundWoker");
            BD.path = pathBD; //укажем файл бд








        }


        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            tb3.Text = (e.ProgressPercentage.ToString() + "%");
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
                this.tb3.Text = "Отмена!";
            }

            else if (!(e.Error == null))
            {
                this.pb1.Visibility = Visibility.Hidden;
                this.tb3.Text = ("Ошибка: " + e.Error.Message);
            }

            else
            {
                this.tb3.Text = "Готово!";
                this.pb1.Visibility = Visibility.Hidden;
                //    btnCancel.IsEnabled = false;
                //   btnStart.IsEnabled = true;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //создаём файл со списком файлом и хэшей разделяем  @
           string inifile_path = @"C:\evolution\giulianovars\_ecadpro\ordini\yandexsync.ini";
            if (!File.Exists(inifile_path))
            {
                File.Create(inifile_path);
            //    string save_log = Environment.NewLine + DateTime.Now + " - " + str;
            //    File.AppendAllText(path, save_log, System.Text.Encoding.UTF8);
            }


            var reader1 = BD.conn("SELECT id, nazv, new, id_server FROM `import3ds_category_server` order by nazv ASC");


            string[] dirs = Directory.GetFiles(@"C:\evolution\giulianovars\_ecadpro\ordini", "*.eve");

            foreach (string dir in dirs)
            {
                MD5 myRIPEMD160 = MD5.Create();
                FileStream tmppathStream = File.OpenRead(dir);
                byte[] hashValue = myRIPEMD160.ComputeHash(tmppathStream);
                tmppathStream.Close();
                string hash = BitConverter.ToString(hashValue).Replace("-", String.Empty);

                findstr = dir + "@" + hash;
            }




            pb1.Minimum = 0;
            pb1.Maximum = dirs.Length;
            pb1.Value = 0;
            pb1.Visibility = Visibility.Visible;
          //  backgroundWorker.RunWorkerAsync();

        }
    }
}
