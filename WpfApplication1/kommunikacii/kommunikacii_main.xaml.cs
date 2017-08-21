using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        public string pathBD = "";
        public string selectedModel = "";
        BD_Connect BD = new BD_Connect();

        public kommunikacii_main(string pathBD)
        {
            InitializeComponent();


            this.pathBD = pathBD;
            BD.path = pathBD; //укажем файл бд


            lb2lb3napolnene();
            loadpage();


        }


        void lb2lb3napolnene()
        {



            //   lb3.ItemsSource = (server);

            List<treespis> polzovat = new List<treespis>();


            var reader = BD.conn("SELECT id, nazv FROM `import3ds_category` ");
            while (reader.Read())
            {


                polzovat.Add(new treespis()
                {
                    id = reader["id"].ToString(),
                    path = "P",
                    name = reader["nazv"].ToString(),
                });

            }



            lb2.ItemsSource = (polzovat);




        }



        void lb1_napolnenie(treespis Treespis, string type)//наполнение листбокса
        {
            string path = Treespis.path;


            List<Models3d> modeli = new List<Models3d>();

            switch (type)
            {
                case "P":

                    var reader = BD.conn("SELECT id, `path3ds`, `pathjpg`, `pathjpgugo`, `x`, `y`, `z`, category FROM `import3ds` WHERE category='" + Treespis.id + "'");
                    while (reader.Read())
                    {
                        string file_split = reader["path3ds"].ToString().Split('\\').Last();

                        modeli.Add(new Models3d
                        {
                            id = reader["id"].ToString(),
                            path = reader["path3ds"].ToString(),
                            name = file_split,
                            jpg_path = reader["pathjpg"].ToString(),
                            jpg_ugo = reader["pathjpgugo"].ToString(),
                            x = reader["x"].ToString(),
                            y = reader["y"].ToString(),
                            z = reader["z"].ToString(),
                            category = reader["category"].ToString(),

                            type = "P",
                        });

                    }
                    break;
                case "S":
                    break;
                default:

                    var files = Directory.GetFiles(path, "*.3ds");
                    foreach (string file in files)
                    {
                        string file_split = file.Split('\\').Last();


                        string[] gab = file_split.Split('_');
                        if (gab.Count() == 5)
                        {
                            modeli.Add(new Models3d
                            {

                                path = path,
                                name = file_split,
                                jpg_path = path + @"\" + file_split.Remove(file_split.Length - 3, 3) + "jpg",
                                jpg_ugo = path + @"\" + file_split.Remove(file_split.Length - 4, 4) + "ugo.jpg",
                                x = gab[1],
                                y = gab[2],
                                z = gab[3],
                                type = "S",
                            });
                        }
                    }
                    break;
            }
            lb1.ItemsSource = modeli;

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var model = (lb2.SelectedItem as treespis).id;
            kommunikacii_dial_imp dial = new kommunikacii_dial_imp(pathBD, null, model);
            dial.ShowDialog();
            lb1_napolnenie((lb2.SelectedItem as treespis), "P");

            //  lb1_napolnenie();

        }

        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb1.SelectedItem != null)
            {
                //  selectedModel = (lb1.SelectedValue as Models3d).path;
                var file = lb1.SelectedValue as Models3d;
                selectedModel = (file.path + @";" + file.name + ";" + file.jpg_ugo + ";" + file.x + ";" + file.y + ";" + file.z);
                // MessageBox.Show(file.path);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (lb1.SelectedIndex > -1)
            {
                var file = lb1.SelectedValue as Models3d;

                kommunikacii_dial_imp dial = new kommunikacii_dial_imp(pathBD, file, null);
                dial.ShowDialog();
                lb1_napolnenie((lb2.SelectedItem as treespis), "P");
            }
            else
            {
                MessageBox.Show("Сначала выбирите модель для редактирования");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var file = lb1.SelectedValue as Models3d;
            if (MessageBox.Show(
         "Вы действительно хотите удалить \"" + file.name + "\"?", "",
         MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                BD.conn("DELETE FROM `import3ds` WHERE id=" + file.id);
                MessageBox.Show("Модель успешно удалена");
                lb1_napolnenie((lb2.SelectedItem as treespis), "P");

            }
        }

        private void lb3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb3.SelectedIndex > -1)
            {
                lb1_napolnenie((lb3.SelectedItem as treespis), "S");
            }
        }

        private void lb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb2.SelectedIndex > -1)
            {
                lb1_napolnenie((lb2.SelectedItem as treespis), "P");
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string otv = new dialuni().Show("Отмена", null, "Создать", "Добавление новой категории", "Укажите название новой категории", 3, 170);
            if (otv != "CANCEL")
            {
                BD.conn("INSERT INTO  `import3ds_category` (nazv)  VALUES ('" + otv + "')");
                MessageBox.Show("Модель успешно добавлена");
                lb2lb3napolnene();
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {


            var model = (lb2.SelectedItem as treespis);

            var reader = BD.conn("SELECT id FROM `import3ds` WHERE category = '" + model.id + "'");

            if (!reader.HasRows)
            {

                if (MessageBox.Show(
       "Вы действительно хотите удалить \"" + model.name + "\"?", "",
       MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    BD.conn("DELETE FROM `import3ds_category` WHERE id=" + model.id);
                    MessageBox.Show("Категория успешно удалена");
                    lb2lb3napolnene();
                }
            }
            else
            {
                MessageBox.Show("Сначала удалите все модели, удалить категорию нельзя, пока в ней существуют модели");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            kommunikacii_Set ps = kommunikacii_Set.Default;
            ps.Top = this.Top;
            ps.Left = this.Left;


            if (this.WindowState == WindowState.Maximized)
            {
                ps.SizeToContent = 1;
            }
            else
            {
                ps.SizeToContent = 0;
                ps.Width = this.Width;
                ps.Height = this.Height;
            }

            ps.lb2sel = lb2.SelectedIndex;
            ps.lb3sel = lb3.SelectedIndex;
            ps.tab1sel = tab1.SelectedIndex;


            ps.Save();
        }
        void loadpage()
        {
            try
            {
                kommunikacii_Set ps = kommunikacii_Set.Default;
                if (ps.Top == -100)
                {
                    this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                }
                else
                {
                    this.Top = ps.Top;
                    this.Left = ps.Left;
                }
                if (ps.SizeToContent == 1)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.Width = ps.Width;
                    this.Height = ps.Height;
                }

                lb2.SelectedIndex = ps.lb2sel;
                lb3.SelectedIndex = ps.lb3sel;
                tab1.SelectedIndex = ps.tab1sel;
            }
            catch
            {

            }



        }
    }

    public class dialuni
    {
        public string otvet = "";
        public string Show(string sbtn1, string sbtn2, string sbtn3, string stitle, string stext, int param, double heightwin)
        {
            dialog_univi dial = new dialog_univi(this, sbtn1, sbtn2, sbtn3, stitle, stext, param, heightwin);
            dial.ShowDialog();

            return otvet;
        }


    }
    public class Models3d
    {

        public string path { get; set; }
        public string name { get; set; }
        public string type { get; set; }//S-сервера P-пользовательский
        public string jpg_path { get; set; }
        public string jpg_ugo { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }
        public string id { get; set; }
        public string category { get; set; }


    }

    public class treespis
    {

        public string path { get; set; }
        public string name { get; set; }
        public string id { get; set; }

        public string type { get; set; }

    }
}
