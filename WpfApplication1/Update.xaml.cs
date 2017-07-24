using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using System.Net;

namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для Update.xaml
    /// </summary>


    public partial class Update : Window
    {
        string last_upd_ROOT, client_ver_ROOT;
        public List<UpdateUPD> UPdate1 = new List<UpdateUPD>();
        string krit1 = "0";
        public Update(string last_upd, string client_ver, string krit, WebClient client, string ver)
        {
            InitializeComponent();
            last_upd_ROOT = last_upd;
            

            WB1.Source = new Uri(Path.GetTempPath() + "GN_upd.html");
            lb5.Content = "Текущяя версия " + ver;
            krit1 = krit;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }




        private void b2_Click(object sender, RoutedEventArgs e)
        {
            if (krit1 == "1")
            {
                MessageBox.Show(
              "У вас имееется критическое обновление, данное диалоговое окно будет показываться до тех пор, пока Вы не обновите программу",
             "Внимание",
              MessageBoxButton.OK,
              MessageBoxImage.Warning);

            }
            Properties.Update.Default.poslproch = last_upd_ROOT;
            Properties.Update.Default.Save();
            Close();
        }

        private void ___DispatcherUnhandledException_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;

        }
    }




    public class UpdateUPD
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string krit { get; set; }
        public string time { get; set; }

    }
}
