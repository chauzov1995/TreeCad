using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


namespace TreeCadN.open_ordini
{
    /// <summary>
    /// Логика взаимодействия для status.xaml
    /// </summary>
    public partial class status : Window
    {
        public string otvet;
        public string path_ini;
        List<string> spis;
        public status(string path_ini, string umol)
        {
            InitializeComponent();
            otvet = umol;
            this.path_ini = path_ini;


            naolnenie();


        }

       void naolnenie()
        {

            if (File.Exists(path_ini))
            {


                // Read the file and display it line by line.  
                System.IO.StreamReader file = new System.IO.StreamReader(path_ini, Encoding.Default);
                spis = new List<string>();

                while (!file.EndOfStream)
                {
                    spis.Add(file.ReadLine());
                }
                file.Close();

                lb1.ItemsSource = spis;

                lb1.SelectedIndex = spis.IndexOf(otvet);


            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lb1.SelectedItem != null)
            {
                otvet = lb1.SelectedItem as string;
            }
            else
            {
                otvet = "";
            }
            Close();
        }

        private void Lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Lb1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lb1.SelectedItem != null)
            {
                otvet = lb1.SelectedItem as string;
            }
            else
            {
                otvet = "";
            }
            Close();
        }



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //  DialogHost.Show(DialogHost1);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // DialogHost.Show(viewOrModel);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            using (StreamWriter sw = new StreamWriter(path_ini, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(AnimalTextBox.Text);
                //sw.Write(4.5);
            }
            naolnenie();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (lb1.SelectedItem == null) return;






            using (StreamWriter sw = new StreamWriter(path_ini, false, System.Text.Encoding.Default))
            {
                int i = 0;
                foreach (var elem in spis)
                {
                    if (i== lb1.SelectedIndex)
                        {

                        }
                        else
                        {
                            sw.WriteLine(elem);
                        }
                    i++;
                }

            }

            naolnenie();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AnimalTextBox.Text = "";
        }
        /*
private void Lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
if (lb1.SelectedItem != null) { }
foreach (string zapis in lb1.Items)
{
if (lb1.SelectedItem.Equals(zapis))
{
return;
}
else {
lb1.SelectedItem
}


}
}}
*/
    }
}
