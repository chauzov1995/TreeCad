using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public string pathBD = "";
        public Models3d model;
        public string category;

        public kommunikacii_dial_imp(string pathBD, Models3d model, string category)
        {

            InitializeComponent();
            this.pathBD = pathBD;
            this.model = model;
            this.category = category;
            

            if (model != null)
            {


                tb1.Text = model.path;
                tb2.Text = model.jpg_path;
                if (model.jpg_ugo == "")
                {
                    chb1.IsChecked = false;
                }
                else
                {
                    chb1.IsChecked = true;
                    tb3.IsEnabled = true;
                    btn1.IsEnabled = true;
                    tb3.Text = model.jpg_ugo;
                }

                tx.Text = model.x;
                ty.Text = model.y;
                tz.Text = model.z;


            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "3DS Files (*.3ds)|*.3ds";
            if (openFileDialog1.ShowDialog() == true)
            {
                tb1.Text = openFileDialog1.FileName;

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JPG Files (*.jpg)|*.jpg";
            if (openFileDialog1.ShowDialog() == true)
            {
                tb2.Text = openFileDialog1.FileName;

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (File.Exists(tb1.Text) && File.Exists(tb2.Text) &&
               ((File.Exists(tb3.Text) && chb1.IsChecked == true) || (chb1.IsChecked == false))
                )
            {

                if (tx.Text == "" || ty.Text == "" || tz.Text == "")
                {
                    MessageBox.Show("Габариты не могут быть пустыми");
                }
                else
                {
                    if (chb1.IsChecked == false)
                    {
                        tb3.Text = "";
                    }
                    string strini = tb1.Text + ";" + tb2.Text + ";" + tb3.Text + ";" + tx.Text + ";" + ty.Text + ";" + tz.Text;
                    BD_Connect BD = new BD_Connect();
                    BD.path = pathBD; //укажем файл бд
                    if (model == null)
                    {
                        BD.conn("INSERT INTO `import3ds` (`path3ds`, `pathjpg`, `pathjpgugo`, `x`, `y`, `z`, category) VALUES ('" + tb1.Text + "','" + tb2.Text + "','" + tb3.Text + "','" + tx.Text + "','" + ty.Text + "','" + tz.Text + "', '"+ category + "')");
                        MessageBox.Show("Модель успешно добавлена");
                    }
                    else
                    {
                        BD.conn("UPDATE `import3ds` SET  `path3ds`='" + tb1.Text + "', `pathjpg`='"+ tb2.Text + "', `pathjpgugo`='" + tb3.Text + "', `x`='" + tx.Text + "', `y`='" + ty.Text + "', `z`='" + tz.Text + "'  WHERE id="+model.id);
                        MessageBox.Show("Модель успешно изменена");
                        Close();

                    }
                }
            }
            else
            {
                MessageBox.Show("Указанного файла не существует");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JPG Files (*.jpg)|*.jpg";
            if (openFileDialog1.ShowDialog() == true)
            {
                tb3.Text = openFileDialog1.FileName;

            }
        }

        private void tx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            string pattern = @"[0-9]";
            Regex regex = new Regex(pattern);

            // Получаем совпадения в экземпляре класса Match
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;

            }

        }

        private void tx_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }

        }

        private void ty_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void tz_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void ty_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string pattern = @"[0-9]";
            Regex regex = new Regex(pattern);

            // Получаем совпадения в экземпляре класса Match
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;

            }
        }

        private void tz_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string pattern = @"[0-9]";
            Regex regex = new Regex(pattern);

            // Получаем совпадения в экземпляре класса Match
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;

            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            tb1.Text = "";
            tb2.Text = "";
            tb3.Text = "";
            tx.Text = "";
            ty.Text = "";
            tz.Text = "";
        }


        private void chb1_Click(object sender, RoutedEventArgs e)
        {
            if (chb1.IsChecked == true)
            {
                tb3.IsEnabled = true;
                btn1.IsEnabled = true;
            }
            else
            {
                tb3.IsEnabled = false;
                btn1.IsEnabled = false;

            }
        }
    }
}
