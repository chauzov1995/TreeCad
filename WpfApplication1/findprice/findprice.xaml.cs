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
        List<TARTICLES> Tartcode_forlb4 = new List<TARTICLES>();

        bool zakrit_ok = false;
        string path;
        string pred = "", predpred = "";
        public string text_otvet = "";
        string modello;

        public findprice(string path, string modello)
        {
            InitializeComponent();



            loadpage();//загрузка полож окна



            this.path = path;
            this.modello = modello;


            load_spis_art();

            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);

            firstbukvalb1();



            ta.Focus();
        }

        void loadpage()
        {

            log.Add("установим размеры окна окна");
            Settings1 ps = Settings1.Default;
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


        }
        void firstbukvalb1()
        {
            ta.Text = "";
            SQLiteDataReader myReader = conTreeCadBD.connect("SELECT * FROM TARTCODE order by ID asc", path);



            List<TARTCODE> Tartcode_for1 = new List<TARTCODE>();
            List<TARTCODE> Tartcode_for2 = new List<TARTCODE>();
            List<TARTCODE> Tartcode_for3 = new List<TARTCODE>();




            int chet = 0;
            while (myReader.Read())
            {



                if (myReader["ARTTYPE"].ToString() == "1")
                {

                    Tartcode_for1.Add(new TARTCODE
                    {
                        ID = myReader["ID"].ToString(),
                        NAME = myReader["NAME"].ToString(),
                        ARTCODE = myReader["ARTCODE"].ToString(),

                    });
                }
                if (myReader["ARTTYPE"].ToString() == "2")
                {

                    Tartcode_for2.Add(new TARTCODE
                    {
                        ID = myReader["ID"].ToString(),
                        NAME = myReader["NAME"].ToString(),
                        ARTCODE = myReader["ARTCODE"].ToString(),

                    });
                }

                if (myReader["ARTTYPE"].ToString() == "3")
                {

                    Tartcode_for3.Add(new TARTCODE
                    {
                        ID = myReader["ID"].ToString(),
                        NAME = myReader["NAME"].ToString(),
                        ARTCODE = myReader["ARTCODE"].ToString(),
                        id_for3 = chet.ToString(),

                    });
                    chet++;
                }





            }
            myReader.Close();


            Tartcode_for1.Sort(delegate (TARTCODE us1, TARTCODE us2)
            { return us1.ARTCODE.CompareTo(us2.ARTCODE); });
            Tartcode_for2.Sort(delegate (TARTCODE us1, TARTCODE us2)
            { return us1.ARTCODE.CompareTo(us2.ARTCODE); });
            Tartcode_for3.Sort(delegate (TARTCODE us1, TARTCODE us2)
            { return us1.ARTCODE.CompareTo(us2.ARTCODE); });

            lb1.ItemsSource = Tartcode_for1;
            lb2.ItemsSource = Tartcode_for2;
            lb3.ItemsSource = Tartcode_for3;

            // formirlb1(ta.Text);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lb1.SelectedIndex = -1;
            lb2.SelectedIndex = -1;
            ta.Text = "";
            tbg.Text = "";
            tbs.Text = "";
            tbv.Text = "";
            topis.Text = "";




        }

        void find(string etalon)
        {

            string findart = etalon;



            // path = @"C:\!qwerty\TreeCadN\WpfApplication1\bin\Debug\GIULIANOVARS\procedure\3CadBase.sqlite";
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

            lb4.ItemsSource = Tartcode_forlb4;

            viewSource1 = new CollectionViewSource();
            viewSource1.Source = Tartcode_forlb4;
            viewSource1.Filter += viewSource_Filter1;
            viewSource1.SortDescriptions.Add(new SortDescription("TARTCODE_ITOG", ListSortDirection.Ascending));
            lb4.ItemsSource = viewSource1.View;

            formor_razmerow();
        }

        private void timerTick(object sender, EventArgs e)
        {
            viewSource1.View.Refresh();


            timer.Stop();
            formor_razmerow();
        }


        void formor_razmerow()
        {

            List<string> v = new List<string>();
            List<string> g = new List<string>();
            List<string> s = new List<string>();
       
            foreach (TARTICLES elem in lb4.Items)
            {
                v.Add(elem.V);
                s.Add(elem.S);
                g.Add(elem.G);
            }


            List<int> v1 = v.ConvertAll<int>(delegate (string i) { return Convert.ToInt32(i); });
            List<int> s1 = s.ConvertAll<int>(delegate (string i) { return Convert.ToInt32(i); });
            List<int> g1 = g.ConvertAll<int>(delegate (string i) { return Convert.ToInt32(i); });
            v1.Sort();
            s1.Sort();
            g1.Sort();


            v = new List<string>();
            s = new List<string>();
            g = new List<string>();
            v.Add("");
            s.Add("");
            g.Add("");
            v.AddRange( v1.ConvertAll<string>(delegate (int i) { return i.ToString(); }));
            s.AddRange(s1.ConvertAll<string>(delegate (int i) { return i.ToString(); }));
            g.AddRange(g1.ConvertAll<string>(delegate (int i) { return i.ToString(); }));

               // v.FirstOrDefault("",true);
               // g.Add("");
                // s.Add("");


            tbv.ItemsSource = v.Distinct();
            tbs.ItemsSource = s.Distinct();
            tbg.ItemsSource = g.Distinct();



        }

        void load_spis_art()
        {


            //  path = @"C:\!qwerty\TreeCadN\WpfApplication1\bin\Debug\GIULIANOVARS\procedure\3CadBase.sqlite";
            SQLiteDataReader myReader = conTreeCadBD.connect("SELECT * FROM TARTICLES WHERE 1", path);



            while (myReader.Read())
            {
                Tartcode_forlb4.Add(new TARTICLES
                {
                    TARTCODE_ITOG = myReader["TARTCODE_ITOG"].ToString(),
                    NAME = myReader["NAME"].ToString(),
                    V = myReader["V"].ToString(),
                    S = myReader["S"].ToString(),
                    G = myReader["G"].ToString(),
                    TARTCODE_ID = myReader["TARTCODE_ID"].ToString(),
                    TARTCODE_ID2 = myReader["TARTCODE_ID2"].ToString(),
                    TARTCODE_STR_3 = myReader["TARTCODE_STR_3"].ToString(),
                    TREECAD_DIS = myReader["TREECAD_DIS"].ToString(),

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

        private void tbg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }





        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            zakrit_ok = true;
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Settings1 ps = Settings1.Default;
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




            ps.Save();




            var elemsel = (lb4.SelectedItem as TARTICLES);

            //   string t = (lb4.SelectedItem as TARTICLES).TREECAD_DIS;

            //1ML2091,, Шкаф - стол, SkafStol; MehOtkr = 02; Otkr = 1; L = 300; A = 355; P = 560; ModFasad = 19.08,1ML,2091,,

            //            t = "1ML2091,,Шкаф-стол,SkafStol;MehOtkr=02;Otkr=1;L=300;A=355;P=560;ModFasad=19.08,1ML,2091,,";
            // string t = modello + elemsel.TARTCODE_ITOG + ",," + elemsel.NAME + "," + elemsel.TREECAD_DIS + ";L = " + elemsel.V + "; A = " + elemsel.S + "; P = " + elemsel.G + "," + modello + "," + elemsel.TARTCODE_ITOG + ",,";


            string str = modello + elemsel.TARTCODE_ITOG + "," + modello + elemsel.TARTCODE_ITOG + "," + elemsel.NAME.Replace(',','.') + "," + elemsel.TREECAD_DIS + ";L = " + elemsel.S + "; A = " + elemsel.V + "; P = " + elemsel.G + "," + modello + "," + elemsel.TARTCODE_ITOG + "," + elemsel.TARTCODE_ITOG + ",";
          
            if (zakrit_ok)
            {
                //    MessageBox.Show(t);
                this.text_otvet = str;
                //   MessageBox.Show(t);
            }



        }


        object predlb1;
        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
       
            predlb1 = lb1.SelectedItem;
            viewSource1.View.Refresh();
            formor_razmerow();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lb4_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            zakrit_ok = true;
            Close();
        }

        private void lb3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var asda = (lb3.SelectedItems as TARTCODE).id_for3;


            foreach (TARTCODE elem in lb3.SelectedItems)


                MessageBox.Show(asda);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            lb1.SelectedIndex = -1;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            lb2.SelectedIndex = -1;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            lb3.SelectedIndex = -1;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            ta.Text = "";
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            topis.Text = "";
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                zakrit_ok = true;
                Close();
            }
            if (e.Key == Key.Escape)
            {

                Close();
            }
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
                            bool lb1bool = false;
                            bool lb2bool = false;
                            bool lb3bool = false;

                            if (lb1.SelectedItem == null)
                            {
                                lb1bool = true;
                            }
                            else
                            {
                                if (((lb1.SelectedItem as TARTCODE).ID) == ((TARTICLES)e.Item).TARTCODE_ID)
                                {
                                    lb1bool = true;
                                }
                            }
                            if (lb2.SelectedItem == null)
                            {
                                lb2bool = true;
                            }
                            else
                            {
                                if (((lb2.SelectedItem as TARTCODE).ID) == ((TARTICLES)e.Item).TARTCODE_ID2)
                                {
                                    lb2bool = true;
                                }
                            }
                            if (lb3.SelectedItem == null)
                            {
                                lb3bool = true;
                            }
                            else
                            {
                                lb3bool = true;
                                string elemea = ((TARTICLES)e.Item).TARTCODE_STR_3.Trim(';');
                                string[] massiv1 = elemea.Split(',');
                                foreach (TARTCODE elem in lb3.SelectedItems)
                                {
                                    if (!massiv1.Contains(elem.id_for3)) lb3bool = false;
                                }
                            }
                            if (lb1bool && lb2bool && lb3bool) e.Accepted = true;
                        }
                    }
                }

            }
            catch (Exception err)
            {
                MessageBox.Show("Исключение " + err.Message);
            }


        }



    }

    class TARTCODE
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string ARTCODE { get; set; }
        public string ARTTYPE { get; set; }
        public string id_for3 { get; set; }
    }

    class TARTICLES
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string TARTCODE_ITOG { get; set; }
        public string TARTCODE_ID { get; set; }
        public string TARTCODE_ID2 { get; set; }
        public string TARTCODE_STR_3 { get; set; }
        public string ARTTYPE { get; set; }
        public string TREECAD_DIS { get; set; }

        public string V { get; set; }
        public string S { get; set; }
        public string G { get; set; }

    }
}
