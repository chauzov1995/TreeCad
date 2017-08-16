using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
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
    /// Логика взаимодействия для kommunikacii.xaml
    /// </summary>
    public partial class kommunikacii_main : Window
    {

        public string selectedModel = "";
        public kommunikacii_main()
        {
            InitializeComponent();




            List<Models3d> root = new List<Models3d>();
            root.Add(new Models3d { name = "C сервера" });
            root.Add(new Models3d { name = "Пользовтельские" });
            //    Models3d childItem1 = new Models3d() { path = "C сервера" };

            string path = Environment.CurrentDirectory + @"\Giulianovars\3DS";
            //  MessageBox.Show(path);
            var directories = Directory.GetDirectories(path);
            foreach (string directorie in directories)
            {
                //  MessageBox.Show(file);
                string name = directorie.Split('\\').Last();
                root[0].Items.Add(new Models3d()
                {
                    path = directorie,
                    name = name,
                });
            }
          




            //    root.Items.Add(childItem1);


            tv1.Items.Add(root);
            






         //   lb1_napolnenie();
        }






        void lb1_napolnenie(string path)//наполнение листбокса
        {
            List<Models3d> modeli = new List<Models3d>();
          
            var files = Directory.GetFiles(path, "*.3ds");
            foreach (string file in files)
            {
                string file_split = file.Split('\\').Last();
                modeli.Add(new Models3d
                {
                    path = path,
                    name = file_split,
                    jpg_path = path+ @"\"+file_split.Remove(file_split.Length-3,3)+"jpg",
                    jpg_ugo = path + @"\" + file_split.Remove(file_split.Length - 4, 4) + "ugo.jpg",
                });
            }
 
            lb1.ItemsSource = modeli;

        }
   
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            kommunikacii_dial_imp dial = new kommunikacii_dial_imp();
            dial.ShowDialog();

          //  lb1_napolnenie();

        }

        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //  selectedModel = (lb1.SelectedValue as Models3d).path;
            var file = lb1.SelectedValue as Models3d;
            selectedModel = (file.path + @"\" + file.name+";"+ file.jpg_ugo);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tv1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
           // MessageBox.Show((tv1.SelectedItem as Models3d).path);
            lb1_napolnenie((tv1.SelectedItem as Models3d).path);
        }
    }
    public class Models3d
    {
        public Models3d()
        {
            this.Items = new ObservableCollection<Models3d>();
        }

        public string path { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string jpg_path { get; set; }
        public string jpg_ugo { get; set; }
        
        public ObservableCollection<Models3d> Items { get; set; }
    }
}
