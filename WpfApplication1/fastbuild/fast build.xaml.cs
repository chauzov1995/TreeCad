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
        public fast_build(Process parrent)
        {
            InitializeComponent();

         

            parrent = Process.GetCurrentProcess();



            MessageBox.Show(parrent.ProcessName);
            IntPtr asdasd = parrent.MainWindowHandle;
            

            ShowWindow(asdasd, 6);
            //     parrent.
            //
            //            if (this.WindowState == FormWindowState.Minimized)
            //      {

            //   }
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int cmd);
    }
}
