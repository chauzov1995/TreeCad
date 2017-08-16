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

namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для Proxy.xaml
    /// </summary>
    public partial class Proxy : Window
    {
        string path = Environment.CurrentDirectory + @"\proxy_N.ini";


        public Proxy()
        {
            InitializeComponent();
            FileInfo fileInf = new FileInfo(path);
            if (fileInf.Exists)//если файл существет
            {
                INIManager client_man = new INIManager(path);
                px_l.Text = client_man.GetPrivateString("GN_PROXY", "login");//получ ключ активации
                px_p.Password = client_man.GetPrivateString("GN_PROXY", "pass");//получ ключ активации
                px_s.Text = client_man.GetPrivateString("GN_PROXY", "server");//получ ключ активации
                px_port.Text = client_man.GetPrivateString("GN_PROXY", "port");//получ ключ активации
                string vkl = client_man.GetPrivateString("GN_PROXY", "vkl");//получ ключ активации
                if (vkl == "true") { px_vkl.IsChecked = true; } else { px_vkl.IsChecked = false; }


            }
            else
            {


                fileInf.Create();

            }



            if (px_vkl.IsChecked == true)
            {
                G1.IsEnabled = true;
            }
            else
            {
                G1.IsEnabled = false;
            }
        }

        private void px_ok_Click(object sender, RoutedEventArgs e)
        {


            INIManager client_man = new INIManager(path);
            //string client_ver = client_man.WritePrivateString("GN_UPD", "last_upd");//получ ключ активации
            client_man.WritePrivateString("GN_PROXY", "login", px_l.Text);
            client_man.WritePrivateString("GN_PROXY", "pass", px_p.Password);
            client_man.WritePrivateString("GN_PROXY", "server", px_s.Text);
            client_man.WritePrivateString("GN_PROXY", "port", px_port.Text);
            string vkl = "false";
            if (px_vkl.IsChecked == true) { vkl = "true"; }
            client_man.WritePrivateString("GN_PROXY", "vkl", vkl);
            MessageBox.Show("Прокси сохранён, перезапустите окно");
            this.Close();


        }

        private void px_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void px_vkl_Checked(object sender, RoutedEventArgs e)
        {
            G1.IsEnabled = true;

        }

        private void px_vkl_Unchecked(object sender, RoutedEventArgs e)
        {
            G1.IsEnabled = false;

        }

    }





    class proxy_LPS
    {


        public WebProxy init()
        {
            

            string path = Environment.CurrentDirectory + @"\proxy_N.ini";
            INIManager client_man = new INIManager(path);
            string login_proxy = client_man.GetPrivateString("GN_PROXY", "login");//получ ключ активации
            string pass_proxy = client_man.GetPrivateString("GN_PROXY", "pass");//получ ключ активации
            string address_proxy = client_man.GetPrivateString("GN_PROXY", "server");//получ ключ активации
            string port_proxy = client_man.GetPrivateString("GN_PROXY", "port");//получ ключ активации
            string vkl = client_man.GetPrivateString("GN_PROXY", "vkl");//получ ключ активации
            bool proxy_isp = false;
            log.Add("login_proxy " + login_proxy);
            if (vkl == "true") { proxy_isp = true; }
            WebProxy myProxy = new WebProxy();
            if (proxy_isp)
            {
                log.Add("прокси используется ");
                myProxy.Address = new Uri(@"http://" + address_proxy + ":" + port_proxy);
                myProxy.Credentials = new NetworkCredential(login_proxy, pass_proxy);
            }
            else
            {
                log.Add("прокси НЕ используется ");
                myProxy = null;
            }
            return myProxy;
        }

    }
}
