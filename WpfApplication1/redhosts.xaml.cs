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

        

            if (CheckIsEstbzapSERV())
            {

                cb1.IsChecked = (true);
            }

        }

        public bool CheckIsEstbzapSERV()
        {
            string hostsPath = @"C:\Windows\System32\drivers\etc\hosts";

            if (!File.Exists(hostsPath))
                return false;

            var lines = File.ReadAllLines(hostsPath);

            foreach (var rawLine in lines)
            {
                string line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 2 &&
                    parts[1].Equals("ecadrussia.dau.it", StringComparison.OrdinalIgnoreCase))
                {
                    return parts[0] == "194.32.243.194";
                }
            }

            return false;
        }

        public bool ElevateToAdmin(string mess)
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

            // Если уже есть права администратора — просто продолжаем
            if (isAdmin)
                return true;

            var result = MessageBox.Show(
                mess,
                "Не хватает прав",
                MessageBoxButton.OKCancel
            );

            if (result != MessageBoxResult.OK)
                return false;

            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.Verb = "runas";
            processInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
            processInfo.UseShellExecute = true;

            try
            {
                Process.Start(processInfo);
                Environment.Exit(0); // текущий процесс закрываем только если новый реально стартовал
                return true;
            }
            catch
            {
                // сюда попадём если пользователь нажал "Отмена" в UAC
                // или если запуск не удался
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool canContinue = ElevateToAdmin(
                "Сейчас программа будет запущена с повышенными правами, необходимо повторить операцию заново. Для продолжения нажмите «ОК», нажмите «Отмена», если нужно сохранить заказ."
            );

            if (!canContinue)
                return;

            try
            {
                vklvikl(cb1.IsChecked == true);
              //  MessageBox.Show("Перезайдите в программу");
                Close();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Нет прав для изменения файла hosts.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при изменении сервера: " + ex.Message);
            }
        }

        public void vklvikl(bool vkl)
        {
            string hostsPath = @"C:\Windows\System32\drivers\etc\hosts";

            string[] lines;
            if (File.Exists(hostsPath))
                lines = File.ReadAllLines(hostsPath);
            else
                lines = new string[0];

            List<string> list = new List<string>();

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    list.Add(rawLine);
                    continue;
                }

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 2 &&
                    parts[1].Equals("ecadrussia.dau.it", StringComparison.OrdinalIgnoreCase))
                {
                    continue; // удаляем любую запись для этого домена
                }

                list.Add(rawLine);
            }

            if (vkl)
            {
                list.Add("194.32.243.194 ecadrussia.dau.it");
            }

            File.WriteAllLines(hostsPath, list.ToArray());

            INIManager client_man = new INIManager(Environment.CurrentDirectory + @"\ecadpro.ini");
            client_man.WritePrivateString("giulianovars", "servergn", vkl ? "1" : "0");
        }

        private bool IsRunningAsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private bool RestartAsAdministrator()
        {
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = Process.GetCurrentProcess().MainModule.FileName,
                    UseShellExecute = true,
                    Verb = "runas"
                };

                Process.Start(processInfo);

                // если новый процесс стартовал — закрываем текущий
                Environment.Exit(0);
                return true;
            }
            catch
            {
                // сюда попадём, если пользователь нажал "Отмена"
                // или если запуск не удался
                return false;
            }
        }

        private void RestartApplication()
        {
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = Process.GetCurrentProcess().MainModule.FileName,
                    UseShellExecute = true
                };

                Process.Start(processInfo);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось перезапустить приложение: " + ex.Message);
            }
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
