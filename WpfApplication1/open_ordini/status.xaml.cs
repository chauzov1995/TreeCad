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
        public string otvet ;
        public status(string path_ini, string umol)
        {
            InitializeComponent();
            otvet = umol;

            if (File.Exists(path_ini))
            {


                // Read the file and display it line by line.  
                System.IO.StreamReader file = new System.IO.StreamReader(path_ini, Encoding.Default);
                List<string> spis = new List<string>();

                while (!file.EndOfStream)
                {
                    spis.Add(file.ReadLine());
                }
                file.Close();

                lb1.ItemsSource = spis;

                 lb1.SelectedIndex=spis.IndexOf(otvet);
                

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
