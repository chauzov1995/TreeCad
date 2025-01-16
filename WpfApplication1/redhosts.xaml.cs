using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
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
    /// Логика взаимодействия для redhosts.xaml
    /// </summary>
    public partial class redhosts : Window
    {
        public redhosts()
        {
            InitializeComponent();

        

            if (checkIsEstbzapSERV())
            {

                cb1.IsChecked = (true);
            }

        }

        public bool checkIsEstbzapSERV()
        {
            var asdasdas = File.ReadAllLines(@"c:\windows\system32\drivers\etc\hosts");
            bool istext = false;
            foreach (string s in asdasdas)
            {
                if (s.Contains("ecadrussia.dau.it"))
                {
                    //    MessageBox.Show(s);
                    istext = true;

                }

            }
            return istext;
        }

        public  bool ElevateToAdmin(string mess )
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            var asdadad = MessageBox.Show(mess, "Не хватает прав", MessageBoxButton.OKCancel);
            if (asdadad == MessageBoxResult.OK)
            {
                if (!isAdmin)
            {
               
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.Verb = "runas";
                    processInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;

                    try
                    {
                        Process.Start(processInfo);
                    }
                    catch (Exception)
                    {
                        // Обработка ошибок, если возникли проблемы при повышении прав
                        MessageBox.Show("Не удалось повысить права до уровня администратора.");
                    }

                    Environment.Exit(0); // Закрытие текущего процесса, так как новый запуск будет с правами администратора
                    return true;
             
            }
            else
            {
                return true;
                }
            }
            else
            {
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ElevateToAdmin("Сейчас программа будет запущена с повышенными правами, необходимо повторить операцию заново. Для продолжения нажмите «ОК», нажмите «Отмена» если нужно сохранить заказ."))
            {

                vklvikl(cb1.IsChecked == true);


                MessageBox.Show("Перезайдите в программу");
                Close();
            }
        }

      public  void vklvikl(bool vkl)
        {
            var asdasdas = File.ReadAllLines(@"c:\windows\system32\drivers\etc\hosts");
            bool istext = false;
            List<string> list = new List<string>();
            foreach (string s in asdasdas)
            {
                if (s.Contains("ecadrussia.dau.it"))
                {
                    //    MessageBox.Show(s);
                    istext = true;


                }
                else
                {
                    list.Add(s);
                }


            }



            if (vkl )
            {
                if (!istext)
                {
                    list.Add("193.124.66.15 ecadrussia.dau.it");
                    string[] otvet = list.ToArray();
                    File.WriteAllLines(@"c:\windows\system32\drivers\etc\hosts", otvet);
                }
            }
            else
            {
                string[] otvet = list.ToArray();
                File.WriteAllLines(@"c:\windows\system32\drivers\etc\hosts", otvet);
            }
            INIManager client_man = new INIManager(Environment.CurrentDirectory + @"\ecadpro.ini");
            client_man.WritePrivateString("giulianovars", "servergn", vkl?"1":"0");
            //    string admin = client_man.GetPrivateString("giulianovars", "3dsadmin");//версия клиента



            ProcessStartInfo processInfo = new ProcessStartInfo();
         //   processInfo.Verb = "runas";
            processInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;

            try
            {
                Process.Start(processInfo);
            }
            catch (Exception)
            {
                // Обработка ошибок, если возникли проблемы при повышении прав
                MessageBox.Show("Не удалось повысить права до уровня администратора.");
            }

            Environment.Exit(0); // Закрытие текущего процесса, так как новый запуск будет с правами администратора


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
             Close();
            /*

          //  string mFolderName = @".\";
            string mFileName = @"C:\1\000006.017.evz";

            if (File.Exists(mFileName))
            {
                try
                {
                    DAUCLib.globale MyClass = new DAUCLib.globale();
                    bool mRes = MyClass.eDecomprimiFile(mFileName, mFileName.Replace(".evz", ".eve"));
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No file: " + mFileName);
            }
            */
        }
    }
}
