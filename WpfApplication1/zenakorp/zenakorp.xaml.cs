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

namespace TreeCadN.zenakorp
{
    /// <summary>
    /// Логика взаимодействия для zenakorp.xaml
    /// </summary>
    public partial class zenakorp : Window
    {
        public zenakorp()
        {
            InitializeComponent();


            string localBase = "GIULIANOVARS";



            SqlDataReader myReader = conTreeCadBD.connect("SELECT distinct DIML FROM " + localBase + ".dbo.ARTICOLI Where CodiceBarra like '+Xkss308'", localBase);
            List<string> strl = new List<string>();
            while (myReader.Read())
            {

                strl.Add(myReader["DIML"].ToString());
            }
            myReader.Close();

            myReader = conTreeCadBD.connect("SELECT distinct DIMA FROM " + localBase + ".dbo.ARTICOLI Where CodiceBarra like '+Xkss308'", localBase);
            List<string> stra = new List<string>();
            while (myReader.Read())
            {
                stra.Add(myReader["DIMA"].ToString());
            }
            myReader.Close();

            int countl = strl.Count();
            int counta = stra.Count();

            //высота
            for (int j = 0; j < counta; j++)
            {
                gr_left.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
                TextBlock tbTemp = new TextBlock();
                tbTemp.TextAlignment = TextAlignment.Center;
                tbTemp.VerticalAlignment = VerticalAlignment.Center;
                tbTemp.Text = "Выс " + stra[j];
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
                tbTemp.Text = "шир. " + strl[j];
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

            for (int i = 0; i < counta; i++)
            {




                for (int j = 0; j < countl; j++)
                {


                    Button tbTemp = new Button();

                    if (i == 3 && j == 1)
                    {
                        tbTemp.Background = (Brush)new BrushConverter().ConvertFrom("#995FC72F");
                    }

                    tbTemp.Content = stra[i] + " x " + strl[j];
                    gr_centr.Children.Add(tbTemp);
                    Grid.SetRow(tbTemp, i);
                    Grid.SetColumn(tbTemp, j);

                }

            }

            this.Width = ((countl + 1) * 80) + 6;


        }

        private void gr_ob_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.Width.ToString() + " " + gr_ob.ActualWidth);
            //  this.Width = gr_ob.ActualWidth+16;
        }
    }
    class spisdlin
    {
        public string headerleft { get; set; }
        public string diml { get; set; }
        public List<string> dima { get; set; }
    }



    class conTreeCadBD
    {

        public static SqlDataReader connect(string zapros, string bdpath)
        {

            string localBase = bdpath;
            SqlConnection mssql = new SqlConnection();
            mssql.ConnectionString =
            "Data Source=TERMINAL2008\\ECADPRO2008;" +
            "Initial Catalog=" + localBase + ";" +
            "User id=sa;" +
            "Password=eCadPro2008;";



            if (mssql.State == ConnectionState.Closed)
                mssql.Open();


            SqlDataReader myReader = null;
            string sql = zapros;
            SqlCommand myCommand = new SqlCommand(sql, mssql);
            myReader = myCommand.ExecuteReader();


            return myReader;
        }
        public static SqlConnection Bd_connect(string bdpath)
        {

            string localBase = bdpath;
            SqlConnection mssql = new SqlConnection();
            mssql.ConnectionString =
            "Data Source=TERMINAL2008\\ECADPRO2008;" +
            "Initial Catalog=" + localBase + ";" +
            "User id=sa;" +
            "Password=eCadPro2008;";



            if (mssql.State == ConnectionState.Closed)
                mssql.Open();





            return mssql;
        }





    }
}
