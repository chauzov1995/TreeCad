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


            var asdasdas = File.ReadAllLines(@"c:\windows\system32\drivers\etc\hosts");
            bool istext = false;
            foreach (string s in asdasdas)
            {
                if (s.Contains("172.16.4.32 ecadrussia.dau.it"))
                {
                    //    MessageBox.Show(s);
                    istext = true;

                }

            }

            if (istext)
            {

                cb1.IsChecked=(true);
            }

        }

        public  bool ElevateToAdmin()
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!isAdmin)
            {
               var asdadad = MessageBox.Show(  "Сейчас программа будет запущена с повышенными правами, необходимо повторить операцию заново. Для продолжения нажмите \"Да\", нажмите \"Нет\" если нужно сохранить заказ.", "Не хватает прав", MessageBoxButton.OKCancel);
                if (asdadad == MessageBoxResult.OK)
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
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ElevateToAdmin())
            {

                var asdasdas = File.ReadAllLines(@"c:\windows\system32\drivers\etc\hosts");
                bool istext = false;
                List<string> list = new List<string>();
                foreach (string s in asdasdas)
                {
                    if (s.Contains("172.16.4.32 ecadrussia.dau.it"))
                    {
                        //    MessageBox.Show(s);
                        istext = true;


                    }
                    else
                    {
                        list.Add(s);
                    }


                }



                if (cb1.IsChecked == true)
                {
                    if (!istext)
                    {
                        list.Add("172.16.4.32 ecadrussia.dau.it");
                        string[] otvet = list.ToArray();
                        File.WriteAllLines(@"c:\windows\system32\drivers\etc\hosts", otvet);
                    }
                }
                else
                {
                    string[] otvet = list.ToArray();
                    File.WriteAllLines(@"c:\windows\system32\drivers\etc\hosts", otvet);
                }


                MessageBox.Show("Перезайдите в программу");
                Close();
            }
        }

       
    }
}
