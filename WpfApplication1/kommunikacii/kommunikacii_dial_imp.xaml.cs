using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace TreeCadN.kommunikacii
{
    /// <summary>
    /// Логика взаимодействия для kommunikacii_dial_imp.xaml
    /// </summary>
    public partial class kommunikacii_dial_imp : Window
    {
        public kommunikacii_dial_imp()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog()==true)
            {
                tb1.Text = openFileDialog1.FileName;
            
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == true)
            {
                tb2.Text = openFileDialog1.FileName;

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string neme_file = tb1.Text.Split('\\').Last();
            string path = Environment.CurrentDirectory + @"\Giulianovars\3DS\importN\"+ neme_file;


            File.Copy(tb1.Text, path);
            MessageBox.Show("Модель добавлена");
        }
    }
}
