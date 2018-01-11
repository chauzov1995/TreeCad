using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System;
using System.IO;
using System.Data.SQLite;

namespace TreeCadN.zenakorp
{
    /// <summary>
    /// Логика взаимодействия для zenakorp.xaml
    /// </summary>
    public partial class zenakorp : Window
    {
        string l, a, p, mod;
        string path;
        string[] massiv_korp;
        List<razmzeni> razmerzen = new List<razmzeni>();

        public zenakorp(string path, string param)
        {
            InitializeComponent();

            this.path = path;
            string localBase = "GIULIANOVARS";

            param = param.Remove(0, 1);
            massiv_korp = param.Split('^');

            List<string> korpuskolvo = new List<string>();
            for (int i = 0; i < massiv_korp.Count(); i++)
            {
                korpuskolvo.Add("Корпус " + (i + 1));
            }
            btngrid.ItemsSource = korpuskolvo;


            btngrid.SelectedIndex = 0;
        }
        void otrisgrid(string korpnom)
        {
            gr_centr.Children.Clear();
            gr_centr.RowDefinitions.Clear();
            gr_centr.ColumnDefinitions.Clear();
            gr_bot.ColumnDefinitions.Clear();
            gr_bot.Children.Clear();
            gr_left.RowDefinitions.Clear();
            gr_left.Children.Clear();

            string[] splitpar = korpnom.Split(';');
            l = splitpar[1].Trim();
            a = splitpar[2].Trim();
            p = splitpar[3].Trim();
            mod = splitpar[0].Trim();

            // string path = Environment.CurrentDirectory + @"\12\3CadBase.sqlite";

            //    var m_dbConnection = new SQLiteConnection("Data Source=" + path + "; Version=3;");
            //    m_dbConnection.Open();

            //    string sql = "SELECT distinct DIML FROM ARTICOLI Where CodiceBarra like '+X" + mod + p + "'";
            //    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            //    SQLiteDataReader reader = command.ExecuteReader();

            //    while (reader.Read())
            //    {

            ////        strl.Add(reader["DIML"].ToString());
            //  //  }

            //    gr_ob.Items.Add(new T});

            SQLiteDataReader myReader = conTreeCadBD.connect("SELECT distinct DIML FROM ARTICOLI Where CodiceBarra like '+X" + mod + p + "'  order by DIML asc ", path);
            List<string> strl = new List<string>();
            while (myReader.Read())
            {

                strl.Add(myReader["DIML"].ToString());
            }
            myReader.Close();



            myReader = conTreeCadBD.connect("SELECT distinct DIMA FROM ARTICOLI Where CodiceBarra like '+X" + mod + p + "' order by DIMA asc", path);
            List<string> stra = new List<string>();
            while (myReader.Read())
            {
                stra.Add(myReader["DIMA"].ToString());
            }
            myReader.Close();



            myReader = conTreeCadBD.connect("SELECT COD, DIMA, DIML, DIMP, Price FROM ARTICOLI Where CodiceBarra like '+X" + mod + p + "'", path);

            while (myReader.Read())
            {
                razmerzen.Add(new razmzeni
                {
                    DIMA = myReader["DIMA"].ToString(),
                    DIML = myReader["DIML"].ToString(),
                    DIMP = myReader["DIMP"].ToString(),
                    COD = myReader["COD"].ToString(),
                    Price = myReader["Price"].ToString(),
                });

            }
            myReader.Close();

            if (razmerzen.Count() == 0) MessageBox.Show("В базе данных записей не обнаружено");




            int countl = strl.Count();
            int counta = stra.Count();

            //высота
            for (int j = 0; j < counta; j++)
            {
                gr_left.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                TextBlock tbTemp = new TextBlock();
                tbTemp.TextAlignment = TextAlignment.Center;
                tbTemp.VerticalAlignment = VerticalAlignment.Center;
                tbTemp.Text = "Выс. " + stra[j];
                gr_left.Children.Add(tbTemp);

                Grid.SetRow(tbTemp, j);
            }
            //ширина
            for (int j = 0; j < countl; j++)
            {
                gr_bot.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
                TextBlock tbTemp = new TextBlock();
                tbTemp.TextAlignment = TextAlignment.Center;
                tbTemp.VerticalAlignment = VerticalAlignment.Center;
                tbTemp.Text = "Шир. " + strl[j];
                gr_bot.Children.Add(tbTemp);

                Grid.SetColumn(tbTemp, j);
            }


            //наполянем список
            for (int j = 0; j < counta; j++)
            {
                gr_centr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            }

            for (int j = 0; j < countl; j++)
            {

                gr_centr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });

            }
            bool boola = false, booll = false, zakr = true;
            for (int i = 0; i < counta; i++)
            {




                for (int j = 0; j < countl; j++)
                {


                    Button tbTemp = new Button();
                    var zena_mass = (razmerzen.Find(x => x.DIMA.Equals(stra[i]) && x.DIML.Equals(strl[j])));
                    if (zakr)
                    {
                        if (Convert.ToInt32(stra[i]) >= Convert.ToInt32(a)) { boola = true; }
                        else
                        {
                            boola = false;

                        }
                        if (Convert.ToInt32(strl[j]) >= Convert.ToInt32(l)) { booll = true; }
                        else
                        {
                            booll = false;
                        }
                        if (boola == true && booll == true)
                        {
                            tbTemp.Background = (Brush)new BrushConverter().ConvertFrom("#99E4D220");

                            // MessageBox.Show( (sender as Button).Tag.ToString());


                            tb1.Text = "Арт. " + zena_mass.COD + " (" + zena_mass.DIMA + "x" + zena_mass.DIML + "x" + zena_mass.DIMP + ") $" + zena_mass.Price;

                            //   boola = false;
                            // booll = false;
                            zakr = false;
                        }
                    }
                    //в случае если всё таки размер совпадёт то прекрасит на зелёный

                    if (stra[i] == a && strl[j] == l)
                    {
                        tbTemp.Background = (Brush)new BrushConverter().ConvertFrom("#995FC72F");
                    }



                    // MessageBox.Show(booll.ToString());
                    //
                    string zena = "";


                    if (zena_mass == null)
                    {
                        zena = "";
                        tbTemp.IsEnabled = false;
                        tbTemp.ToolTip = "Нет цены";
                    }
                    else
                    {
                        zena = zena_mass.Price;
                        tbTemp.Tag = zena_mass.COD;

                        tbTemp.Click += new RoutedEventHandler(Onb2Click);
                        tbTemp.ToolTip = "Арт. " + zena_mass.COD + " (" + stra[i] + " x " + strl[j] + ")";

                    }


                    //stra[i] + " x " + strl[j] + 
                    if (zena == "") zena = "нет цены";
                    tbTemp.Content = "$" + zena;
                    gr_centr.Children.Add(tbTemp);
                    Grid.SetRow(tbTemp, i);
                    Grid.SetColumn(tbTemp, j);

                }

            }
            float width = ((countl + 1) * 80);
            if (width > 500)
            {
                this.Width = width;
            }

        }

        private void btngrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // MessageBox.Show("asdasd");
            otrisgrid(massiv_korp[btngrid.SelectedIndex]);
        }

        void Onb2Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show( (sender as Button).Tag.ToString());
            string artik = (sender as Button).Tag.ToString();
            var obj = razmerzen.Find(x => x.COD.Equals(artik));
            string zena = obj.Price;
            if (zena == "") zena = "нет цены";
            tb1.Text = "Арт. " + artik + " (" + obj.DIML + "x" + obj.DIMA + "x" + obj.DIMP + ") $" + zena;
            //    tb1.Foreground = (sender as Button).Background;
        }





        private void gr_ob_Loaded(object sender, RoutedEventArgs e)
        {
            //   MessageBox.Show(this.Width.ToString() + " " + gr_ob.ActualWidth);
            //  this.Width = gr_ob.ActualWidth+16;
        }
    }

    class razmzeni
    {
        public string DIMA { get; set; }
        public string DIML { get; set; }
        public string DIMP { get; set; }
        public string Price { get; set; }
        public string COD { get; set; }
    }

    class spisdlin
    {
        public string headerleft { get; set; }
        public string diml { get; set; }
        public List<string> dima { get; set; }
    }



    class conTreeCadBD
    {
        
        public static SQLiteDataReader connect(string zapros, string path)
        {



            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + path + "; Version=3;");

            if (m_dbConnection.State == ConnectionState.Closed)
                m_dbConnection.Open();

            string sql = zapros;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

          //  m_dbConnection.Close();
            return reader;



        }
        public static SqlConnection Bd_connect(string bdpath)
        {

            string localBase = bdpath;
            SqlConnection mssql = new SqlConnection();
            mssql.ConnectionString =
            "Data Source=3CAD\\ECADPRO2008;" +
            "Initial Catalog=" + localBase + ";" +
            "User id=sa;" +
            "Password=eCadPro2008;";



            if (mssql.State == ConnectionState.Closed)
                mssql.Open();





            return mssql;
        }





    }
}
