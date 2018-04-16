using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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


namespace TreeCadN.fastbuild
{
    /// <summary>
    /// Логика взаимодействия для fast_build.xaml
    /// </summary>
    public partial class fast_build : Window
    {


        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();


        public fast_build()
        {
            InitializeComponent();

        





        }

      

        const int SW_RESTORE = 9;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process parrent = Process.GetProcessesByName("giulianovars")[0];
            MessageBox.Show(parrent.ProcessName);
            IntPtr hWnd = parrent.MainWindowHandle;

            if (IsZoomed(hWnd))
            {
               // ShowWindow(hWnd, SW_RESTORE);
                MessageBox.Show("Свёрнуто");
            }
            else
            {
                MessageBox.Show("Развернуто");
            }
        }
    }
}
