using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SQLite;
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
using TreeCadN.zenakorp;
using System.Linq;
using System.ComponentModel;

namespace TreeCadN.findprice
{
    /// <summary>
    /// Логика взаимодействия для findprice.xaml
    /// </summary>
    public partial class findprice : Window
    {

        CollectionViewSource viewSource1;
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        List<TARTICLES> Tartcode_for2 = new List<TARTICLES>();

        string path;
        string pred = "", predpred = "";
        public string text_otvet = "";

        public findprice(string path)
        {
            InitializeComponent();







            this.path = path;



            load_spis_art();

            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);

            // firstbukvalb1();
        }
        void firstbukvalb1()
        {
            ta.Text = "";
            SQLiteDataReader myReader = conTreeCadBD.connect("SELECT * FROM TARTCODE order by ARTCODE asc", path);



            List<string> Tartcode_for1 = new List<string>();
            List<string> Tartcode_for2 = new List<string>();
            List<string> Tartcode_for3 = new List<string>();



            while (myReader.Read())
            {



                if (myReader["ARTTYPE"].ToString() == "1")
                {

                    Tartcode_for1.Add(myReader["ARTCODE"].ToString() + " - " + myReader["NAME"].ToString());
                }
                if (myReader["ARTTYPE"].ToString() == "2")
                {

                    Tartcode_for2.Add(myReader["ARTCODE"].ToString() + " - " + myReader["NAME"].ToString());
                }

                if (myReader["ARTTYPE"].ToString() == "3")
                {

                    Tartcode_for3.Add(myReader["ARTCODE"].ToString() + " - " + myReader["NAME"].ToString());
                }





            }
            myReader.Close();



            lb1.ItemsSource = Tartcode_for1;
            lb2.ItemsSource = Tartcode_for2;
            lb3.ItemsSource = Tartcode_for3;

            // formirlb1(ta.Text);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            find(ta.Text);


        }

        void find(string etalon)
        {

            string findart = etalon;



            path = @"C:\!qwerty\TreeCadN\WpfApplication1\bin\Debug\GIULIANOVARS\procedure\3CadBase.sqlite";
            SQLiteDataReader myReader = conTreeCadBD.connect("SELECT * FROM TARTICLES WHERE TARTCODE_ITOG LIKE  '" + findart + "%' LIMIT 20", path);


            List<TARTICLES> Tartcode_for2 = new List<TARTICLES>();
            while (myReader.Read())
            {
                Tartcode_for2.Add(new TARTICLES
                {
                    TARTCODE_ITOG = myReader["TARTCODE_ITOG"].ToString(),
                    NAME = myReader["NAME"].ToString(),

                });

            }

            myReader.Close();
            lb4.ItemsSource = Tartcode_for2;

        }

        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            formirlb1(lb1.SelectedItem.ToString()[0].ToString());


        }
        void formirlb1(string etalon)
        {

            List<string> Tartcode_for2 = new List<string>();

            string findelem = formatik(etalon);//сформир артикул //первая буква
            find(findelem);


            int lingthfindelem = findelem.Length;

            SQLiteDataReader myReader = conTreeCadBD.connect("SELECT a.TARTCODE_ITOG, b.ARTTYPE, b.ARTCODE FROM TARTICLES a LEFT JOIN TARTCODE b ON b.ID=a.TARTCODE_ID2 WHERE TARTCODE_ITOG LIKE '" + findelem + "%' ", path);
            string zapros_2s = "";
            string zapros_3s = "";
            bool last = false;
            while (myReader.Read())
            {


                if (myReader["TARTCODE_ITOG"].ToString().Length > lingthfindelem)
                {


                    if (myReader["ARTTYPE"].ToString() == "2")
                    {
                        zapros_2s += "'" + myReader["TARTCODE_ITOG"].ToString()[lingthfindelem] + "',";

                    }
                    else
                    {

                        zapros_3s += "'" + myReader["TARTCODE_ITOG"].ToString()[lingthfindelem] + "',";

                    }

                    if (!Char.IsLetter(myReader["TARTCODE_ITOG"].ToString()[lingthfindelem]))
                    {

                        //   Tartcode_for2.Add(myReader["TARTCODE_ITOG"].ToString().Substring(lingthfindelem));

                    }

                }
            }


            myReader.Close();
            if (zapros_2s.Length > 0)
            {
                zapros_2s = zapros_2s.Substring(0, zapros_2s.Length - 1);
            }
            if (zapros_3s.Length > 0)
            {
                zapros_3s = zapros_3s.Substring(0, zapros_3s.Length - 1);
            }







            myReader = conTreeCadBD.connect("SELECT * FROM TARTCODE WHERE (ARTTYPE=2 and  ARTCODE in (" + zapros_2s + ")) or( ARTTYPE=3 and  ARTCODE in (" + zapros_3s + ")) order by ARTCODE  asc", path);

            while (myReader.Read())
            {
                Tartcode_for2.Add(myReader["ARTCODE"].ToString() + " - " + myReader["NAME"].ToString());
            }




            myReader.Close();
            if (Tartcode_for2.Count != 0)
            {
                lb1.SelectedIndex = -1;
                lb1.ItemsSource = null;
                lb1.ItemsSource = Tartcode_for2;
            }

        }








        string formatik(string etalon)
        {

            string itogart = etalon;



            if (lb1.SelectedItem != null)
            {
                if (!Char.IsLetter(lb1.SelectedItem.ToString()[0]))
                {
                    itogart += lb1.SelectedItem.ToString();
                }
                else
                {

                    itogart += lb1.SelectedItem.ToString()[0];
                }

            }
            ta.Text = itogart;


            return itogart;
        }

        private void ta_TextChanged(object sender, TextChangedEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            lb1.UnselectAll();
            firstbukvalb1();

        }

        private void lb4_Loaded(object sender, RoutedEventArgs e)
        {

            lb4.ItemsSource = Tartcode_for2;

            viewSource1 = new CollectionViewSource();
            viewSource1.Source = Tartcode_for2;
            viewSource1.Filter += viewSource_Filter1;
            viewSource1.SortDescriptions.Add(new SortDescription("TARTCODE_ITOG", ListSortDirection.Ascending));
            lb4.ItemsSource = viewSource1.View;
        }

        private void timerTick(object sender, EventArgs e)
        {
            viewSource1.View.Refresh();
            timer.Stop();
        }

        void load_spis_art()
        {


            path = @"C:\!qwerty\TreeCadN\WpfApplication1\bin\Debug\GIULIANOVARS\procedure\3CadBase.sqlite";
            SQLiteDataReader myReader = conTreeCadBD.connect("SELECT * FROM TARTICLES WHERE 1", path);



            while (myReader.Read())
            {
                Tartcode_for2.Add(new TARTICLES
                {
                    TARTCODE_ITOG = myReader["TARTCODE_ITOG"].ToString(),
                    NAME = myReader["NAME"].ToString(),
                    V = myReader["V"].ToString(),
                    S = myReader["S"].ToString(),
                    G = myReader["G"].ToString(),

                });

            }

            myReader.Close();
            // lb4.ItemsSource = Tartcode_for2;
        }

        private void topis_TextChanged(object sender, TextChangedEventArgs e)
        {

            timer.Stop();
            timer.Start();
        }

        private void tbv_TextChanged(object sender, TextChangedEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        private void tbs_TextChanged(object sender, TextChangedEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        private void tbg_TextChanged(object sender, TextChangedEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {


            string t = (lb4.SelectedItem as TARTICLES).TARTCODE_ITOG;


            t = "1ML2091,,Шкаф-стол,SkafStol;MehOtkr=02;Otkr=1;L=300;A=355;P=560;ModFasad=19.08,1ML,2091,,";
            this.text_otvet = t;


        }

        void viewSource_Filter1(object sender, FilterEventArgs e)
        {

            try
            {
                e.Accepted = false;
                if (((TARTICLES)e.Item).TARTCODE_ITOG.ToLower().IndexOf(ta.Text.ToLower()) >= 0)
                {
                    bool sledetap = true;

                    string[] splitmass = topis.Text.ToLower().Split(' ');
                    for (int w = 0; w < splitmass.Count(); w++)
                    {
                        if (((TARTICLES)e.Item).NAME.ToLower().IndexOf(splitmass[w]) < 0) sledetap = false;

                    }

                    //(
                    if (sledetap)
                    {
                        if (
                            (((TARTICLES)e.Item).V == tbv.Text || tbv.Text == "") &&
                              (((TARTICLES)e.Item).S == tbs.Text || tbs.Text == "") &&
                                  (((TARTICLES)e.Item).G == tbg.Text || tbg.Text == "")
                            )
                        {
                            e.Accepted = true;
                        }


                    }
                }

            }
            catch
            {
                MessageBox.Show("Исключение");
            }


        }



    }

    class TARTCODE
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string ARTCODE { get; set; }
        public string ARTTYPE { get; set; }

    }

    class TARTICLES
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string TARTCODE_ITOG { get; set; }
        public string ARTTYPE { get; set; }
        public string V { get; set; }
        public string S { get; set; }
        public string G { get; set; }

    }
}
