using System;
using System.Collections.Generic;
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
using System.Reflection;
using System.Data.OleDb;
using System.Reflection;
using System.Collections.ObjectModel;

namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для Basis.xaml
    /// </summary>
    public partial class Basis : Window
    {

        public List<BASIS_class> BASIS = new List<BASIS_class>();
        string nabor;
        neqweqe newneqqw;
        public Basis(neqweqe nneqwq)
        {


            InitializeComponent();

            newneqqw = nneqwq;
            
            tvMain.ItemsSource = WorldArea.GetAll();

        }





        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            newneqqw.Bazis = "Тумба/Редактируемая тумба.js";
            // Close();
        }





        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lv_1.Visibility = Visibility.Collapsed;
            tb_c_1.Visibility = Visibility.Collapsed;
            tvMain.Visibility = Visibility.Visible;
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lv_1.Visibility = Visibility.Visible;
            tb_c_1.Visibility = Visibility.Collapsed;
            tvMain.Visibility = Visibility.Collapsed;

            
            //      MessageBox.Show((sender as Grid).Tag.ToString());
            string treecad = (sender as Grid).Tag.ToString();
            // MessageBox.Show(treecad);
            string vh_func_path = @"C:\BM8.mdb";
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + vh_func_path + "");//подключаемся к базе
            OleDbCommand cmd = new OleDbCommand();//инициализируем запрос
            cmd.Connection = conn;//подключаемся к бд
            conn.Open();//открываем соединение
            OleDbDataReader reader = null;//обьявляем просмотрщик
            reader = null;//Загружаем эффекты отделки
            MessageBox.Show(nabor + treecad);
            cmd.CommandText = ("SELECT BM8_articoli.neutro, BM8_articoli.DIMA, BM8_articoli.DIML, BM8_articoli.DIMP FROM BM8_articoli LEFT JOIN BM8_catalogs ON  BM8_catalogs.COD=BM8_articoli.neutro WHERE (BM8_articoli.Modello='" + nabor + "' AND BM8_catalogs.TIP=" + treecad + "  ) ORDER BY BM8_articoli.DIMA ASC");
          //  cmd.CommandText = ("SELECT BM8_catalog.ITEMNAME, BM8_catalog.CATALOGID FROM BM8_derevo LEFT JOIN BM8_catalog ON  BM8_derevo.CATALOGID=BM8_catalog.CATALOGID WHERE (BM8_derevo.SHOTNAME='" + nabor + "'  ) ");
            reader = cmd.ExecuteReader();//выполняем запрос

            BASIS.Clear();
            while (reader.Read())
            {
                try
                {



                    BASIS.Add(new BASIS_class()
                            {

                              //  ID = reader["derevo"].ToString(),
                                //nabor = reader["Набор"].ToString(),
                                name = "Арт." + reader["neutro"].ToString() + " " + reader["DIMA"].ToString() + "x" + reader["DIML"].ToString() + "x" + reader["DIMP"].ToString(),
                             //   art = reader["Арт"].ToString(),
                               // basis_cod = reader["базис_код"].ToString(),

                            });



                }
                catch { }
            }



            reader.Close();
            conn.Close();

            lv_1.ItemsSource = null;
            lv_1.ItemsSource = BASIS;











        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //string treecad = (sender as TextBlock).Tag.ToString();
            //  MessageBox.Show(treecad);
            newneqqw.Bazis = "Тумба/Редактируемая тумба.js";
            // Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            lv_1.Visibility = Visibility.Collapsed;
            tb_c_1.Visibility = Visibility.Visible;
            tvMain.Visibility = Visibility.Collapsed;



        }

        private void lb_garderob_MouseUp(object sender, MouseButtonEventArgs e)
        {

            lv_1.Visibility = Visibility.Collapsed;
            tb_c_1.Visibility = Visibility.Collapsed;
            tvMain.Visibility = Visibility.Visible;


            nabor = (sender as TextBlock).Tag.ToString();
           tvMain.ItemsSource = WorldArea.GetAll();


        }



    }


    public class BASIS_class
    {
        public string ID { get; set; }
        public string nabor { get; set; }
        public string name { get; set; }
        public string art { get; set; }
        public string basis_cod { get; set; }


    }








    public class WorldArea
    {

        string _name = "";
        ObservableCollection<Country> _countries = null;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }



        public ObservableCollection<Country> Countries
        {
            get
            {
                if (_countries == null) _countries = new ObservableCollection<Country>();
                return _countries;
            }
            set { _countries = value; }
        }



        public static ObservableCollection<WorldArea> GetAll()
        {

            ObservableCollection<WorldArea> listToReturn = new ObservableCollection<WorldArea>();



            WorldArea treeItem = null;

            string vh_func_path = @"C:\BM8.mdb";
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + vh_func_path + "");//подключаемся к базе
            OleDbCommand cmd = new OleDbCommand();//инициализируем запрос
            cmd.Connection = conn;//подключаемся к бд
            conn.Open();//открываем соединение
            OleDbDataReader reader = null;//обьявляем просмотрщик
            reader = null;//Загружаем эффекты отделки
            cmd.CommandText = ("SELECT COD, DES FROM Derevo WHERE 1");
            reader = cmd.ExecuteReader();//выполняем запрос

            string pred = "10000";
            while (reader.Read())
            {
                try
                {
                    string podr1 = reader["COD"].ToString().Substring(0, 2);
                    string podr2 = reader["COD"].ToString().Substring(2, 2);
                    if (pred == "10000")
                    {
                        treeItem = new WorldArea();
                        treeItem.Name = reader["DES"].ToString();
                    }
                    else
                    {
                        if (podr1 == pred)
                        {
                            treeItem.Countries.Add(new Country(reader["DES"].ToString(), reader["COD"].ToString()));
                        }
                        else
                        {
                            listToReturn.Add(treeItem);
                            treeItem = new WorldArea();
                            treeItem.Name = reader["DES"].ToString();
                        }
                    }
                    pred = podr1;
                }
                catch { }
            }



            reader.Close();
            conn.Close();


            return listToReturn;

        }




    }

    public class Country
    {
        public string Name { get; set; }
        public string Cod { get; set; }

        public Country(string name, string cod)
        {
            Name = name;
            Cod = cod;
        }

    }

}
