using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
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
using FirebirdSql.Data.FirebirdClient;


namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для TAccessories.xaml
    /// </summary>
    public partial class TAccessories : Window
    {

        static BD_Connect BD = new BD_Connect();
        public string text_otvet;
        public static List<todelka> otdelka_array;
        public static List<todelka> izmer = new List<todelka>();
        bool zakrit_ok = false;
        List<texnika> array_spis_tex = new List<texnika>();
        List<texnika> gr2 = new List<texnika>();
        int sort_forg1 = 1;
        CollectionViewSource viewSource1;
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        String ismanager;
        List<texnika> array_vibr_tex = new List<texnika>();
        public int redakilidob = 0;
        int nom_PP = 0;
        bool g1_KeyUp_bool = false;
        texnika index_for_poisl;
        neqweqe neqqqqq;

        public TAccessories(string path, string text, neqweqe _neqqqqq)
        {

            InitializeComponent();
            this.neqqqqq = _neqqqqq;
            this.Title = "Аксессуары/Техника БД:"+ path;
            BD.path = path; //укажем файл бд
            this.text_otvet = text;
            log.Add("Запуск окна");
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            log.Add("Запуск таймера");
            loadpage();//загрузка полож окна

            INIManager client_man = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
            ismanager = client_man.GetPrivateString("giulianovars", "ismanager");//версия клиента
            if (ismanager == "1")
            {
                btnimp.Visibility = Visibility.Visible;
            }
            else
            {
                btnimp.Visibility = Visibility.Collapsed;
            }

            //отделка
            log.Add("Подключимся к бд");
            OleDbDataReader reader_otd = BD.conn("SELECT Id, Name FROM TOtdelka order by Name ASC");

            otdelka_array = new List<todelka>();
            otdelka_array.Add(new todelka() { ID = "", nameotd = "" });
            while (reader_otd.Read())
            {
                if (reader_otd["Name"].ToString() != "")
                {

                    otdelka_array.Add(new todelka()
                    {
                        ID = reader_otd["Id"].ToString(),
                        nameotd = reader_otd["Name"].ToString(),

                    });


                }
            }
            log.Add("установим отделку");
            otdelka.ItemsSource = otdelka_array;
            //.ItemsSource = todelka;
            //ед изм
            OleDbDataReader reader_izmer = BD.conn("SELECT UnitsID, UnitsName FROM TUnits order by UnitsName ASC");

            while (reader_izmer.Read())
            {
                if (reader_izmer["UnitsName"].ToString() != "")
                {

                    izmer.Add(new todelka()
                    {
                        ID = reader_izmer["UnitsID"].ToString(),
                        nameotd = reader_izmer["UnitsName"].ToString(),

                    });


                }
            }
            log.Add("установим ед изм");
            edizmer.ItemsSource = izmer;


            log.Add("загрузим аксссуары");
            //аксессуары
            OleDbDataReader reader_GRACC = BD.conn("SELECT TPrice1.GRAFIKA, TPrice1.Articul, TUnits.UnitsID, TPrice1.TKOEFGROUP_ID, TUnits.UnitsName, TPrice1.MName, TPrice1.MatID,  TPrice1.PriceID,  TPrice1.Price, TMAT.MATNAME,  TPrice1.Visibl FROM (TPrice1  LEFT OUTER JOIN  TMAT ON TPrice1.MatID = TMAT.MATID) LEFT OUTER JOIN  TUnits ON TUnits.UnitsID=TPrice1.UnitsID  ");//WHERE TPrice1.GRAFIKA=0
            //LEFT OUTER JOIN  TUnits ON TUnits.UnitsID=TPrice1.UnitsID
            while (reader_GRACC.Read())
            {



                float baseprice = 0;
                if (reader_GRACC["Price"].ToString() != "") baseprice = Convert.ToSingle(reader_GRACC["Price"].ToString());

                bool vived = true;
                if (reader_GRACC["Visibl"].ToString() == "1") vived = false;


                int sort = 0;
                if (reader_GRACC["Articul"].ToString() == "***" || reader_GRACC["Articul"].ToString() == "15R***" || reader_GRACC["Articul"].ToString() == "SAD***")
                {

                }
                else
                {
                    sort = sort_forg1;

                }

                array_spis_tex.Add(new texnika()
                {
                    type = "a",
                    ID = reader_GRACC["PriceID"].ToString(),
                    TName = reader_GRACC["MName"].ToString(),
                    Group = reader_GRACC["MatID"].ToString(),
                    baseprice = baseprice,
                    priceredak = baseprice,
                    UnitsId = reader_GRACC["UnitsName"].ToString(),
                    GroupName = reader_GRACC["MATNAME"].ToString(),
                    UnitsName = reader_GRACC["UnitsID"].ToString(),
                    Article = reader_GRACC["Articul"].ToString(),
                    Prim = "",
                    kolvo = 1,
                    vived = vived,
                    OTD = "",
                    sort = sort,
                    GROUP_dlyaspicif = reader_GRACC["TKOEFGROUP_ID"].ToString(),
                    GRAFIKA = reader_GRACC["GRAFIKA"].ToString(),


                });

                sort_forg1++;
            }
            log.Add("Загрузим технику");
            //техника
            OleDbDataReader reader = BD.conn("SELECT TTxPrice1.Article, TTxPrice1.TName, TTxPrice1.TKOEFGROUP_ID, TTxPrice1.TPriceID, TTxPrice1.TexID, TTxPrice1.Price, TTexnics.TexType  FROM TTxPrice1  LEFT OUTER JOIN  TTexnics ON  TTxPrice1.TexID = TTexnics.TexID  ");
            while (reader.Read())
            {
                try
                {


                    float baseprice = 0;
                    if (reader["Price"].ToString() != "") baseprice = Convert.ToSingle(reader["Price"].ToString());

                    int sort = 0;
                    if (reader["Article"].ToString() == "*")
                    {

                    }
                    else
                    {
                        sort = sort_forg1;
                    }


                    array_spis_tex.Add(new texnika()
                    {

                        type = "t",
                        ID = reader["TPriceID"].ToString(),
                        TName = reader["TName"].ToString(),
                        Article = reader["Article"].ToString(),
                        UnitsId = "шт",
                        baseprice = baseprice,
                        priceredak = baseprice,
                        UnitsName = "3",
                        GroupName = reader["TexType"].ToString(),
                        Group = reader["TexID"].ToString(),
                        Prim = "",
                        kolvo = 1,
                        OTD = "",
                        sort = sort,
                        GROUP_dlyaspicif = reader["TKOEFGROUP_ID"].ToString(),
                        GRAFIKA = "0"
                    });
                }
                catch
                {
                    MessageBox.Show("err");
                }
                sort_forg1++;
            }





            MenuItem root = new MenuItem() { Title = "Все", type = "at*" };
            MenuItem childItem1 = new MenuItem() { Title = "Аксессуары", type = "a*" };
            log.Add("Загрузим матид");
            OleDbDataReader readertreeeacc = BD.conn("select T.MATID,MATNAME from TMAT T, TPrice1 T1 where T1.MATID = T.MATID  group by T.MATID,T.MATNAME ORDER BY T.MATNAME ASC");

            while (readertreeeacc.Read())
            {
                childItem1.Items.Add(new MenuItem()
                {
                    Title = readertreeeacc["MATNAME"].ToString(),
                    ID = readertreeeacc["MATID"].ToString(),
                    type = "a",
                });
            }


            root.Items.Add(childItem1);
            childItem1 = new MenuItem() { Title = "Техника", type = "t*" };
            log.Add("загрузим ттехникс");
            OleDbDataReader reader_texCB = BD.conn("select T.TexID,TexType from TTexnics T, TTxPrice1 T1 where T1.TexID=T.TexID  group by T.TEXID,T.TexType ORDER BY T.TexType ASC ");

            while (reader_texCB.Read())
            {
                childItem1.Items.Add(new MenuItem()
                {
                    Title = reader_texCB["TexType"].ToString(),
                    ID = reader_texCB["TexID"].ToString(),
                    type = "t",
                });
            }
            root.Items.Add(childItem1);

            tv1.Items.Add(root);

            log.Add("парсим строкуs");
            pars(text);



        }

        void pars(string vh_str)
        {

            array_vibr_tex = new List<texnika>();
            string[] elems = vh_str.Split(';');

            log.Add("запуск цикла парсим");
            log.Add("входная строка " + vh_str);
            for (int i = 0; i < elems.Count(); i++)
            {
                if (elems[i] != "")
                {
                    log.Add("строка " + elems[i]);
                    string[] znach = elems[i].Split('~');

                    string name;

                    if (Convert.ToInt32(znach[6]) > nom_PP) nom_PP = Convert.ToInt32(znach[6]);

                    texnika poluch = new texnika();
                    try
                    {

                        string article = znach[1].Replace('@', ',').Replace('$', ';');
                        if (article == "***" || article == "*" || article == "15R***" || article == "SAD***")
                        {
                            name = znach[7].Replace('@', ',').Replace('$', ';');

                        }
                        else
                        {
                            name = (array_spis_tex.Find(x => x.Article.Equals(znach[1].Replace('@', ',').Replace('$', ';')))).TName;

                        }
                        //t-техника a-аксесуар




                        texnika poluch1 = new texnika();
                        poluch1 = (array_spis_tex.Find(x => x.Article.Equals(article.Replace('@', ',').Replace('$', ';'))));




                        poluch.Article = poluch1.Article;
                        poluch.baseprice = poluch1.baseprice;
                        poluch.Group = poluch1.Group;
                        poluch.GroupName = poluch1.GroupName;
                        poluch.GROUP_dlyaspicif = poluch1.GROUP_dlyaspicif;
                        poluch.ID = poluch1.ID;
                        poluch.kolvo = poluch1.kolvo;
                        poluch.nom_pp = poluch1.nom_pp;
                        poluch.OTD = poluch1.OTD;
                        poluch.priceredak = poluch1.priceredak;
                        // poluch.Prim = poluch1.Prim;
                        poluch.sort = poluch1.sort;
                        //   poluch.TName = poluch1.TName;
                        poluch.type = poluch1.type;
                        poluch.UnitsId = poluch1.UnitsId;
                        poluch.UnitsName = poluch1.UnitsName;
                        poluch.vived = poluch1.vived;
                        poluch.TName = name;
                        // poluch.Prim = znach[4].Replace('@', ',').Replace('$', ';');//примечание
                        poluch.colortext = "x:Null";

                    }
                    catch
                    {
                        poluch.TName = znach[7].Replace('@', ',').Replace('$', ';');
                        poluch.Article = znach[1].Replace('@', ',').Replace('$', ';');
                        // poluch.Prim = "!Артикула нет в базе!".Replace('@', ',').Replace('$', ';');//примечание
                        //  poluch.Prim = znach[4].Replace('@', ',').Replace('$', ';');//примечание
                        poluch.colortext = "#FFDE0606";
                        poluch.GROUP_dlyaspicif = "";
                        poluch.UnitsName = "";
                    }
                    if (znach.Length > 4)
                    {
                        poluch.Prim = znach[4].Replace('@', ',').Replace('$', ';');//примечание
                    }
                    else
                    {
                        poluch.Prim = "";
                    }
                    poluch.OTD = znach[3].Replace('@', ',').Replace('$', ';');//отделка
                    poluch.type = znach[0].Replace('@', ',').Replace('$', ';');//типп аксес или техн
                    poluch.kolvo = Convert.ToSingle(znach[2].Replace('@', ',').Replace('.', ',').Replace('$', ';'));//Колво
                    poluch.priceredak = Convert.ToSingle(znach[9].Replace('@', ',').Replace('.', ',').Replace('$', ';'));//цена ред


                    // poluch.GROUP_dlyaspicif = znach[5].Replace('@', ',');//группа специфик
                    if (array_vibr_tex.Find(x => x.nom_pp.Equals(Convert.ToInt32(znach[6].Replace('@', ',').Replace('$', ';')))) == null)
                    {
                        poluch.nom_pp = Convert.ToInt32(znach[6].Replace('@', ',').Replace('$', ';'));//ид позиция
                    }
                    else
                    {
                        nom_PP++;
                        poluch.nom_pp = nom_PP;

                    }

                    array_vibr_tex.Add(poluch);

                }
            }
            log.Add("закончили парсить установим в гр3");
            lb_vibr_tex.ItemsSource = null;
            lb_vibr_tex.ItemsSource = array_vibr_tex;


        }

        void Closinger()
        {



            this.Hide();

            string t = str_sobr(array_vibr_tex);


            if (zakrit_ok)
            {

                this.text_otvet = t;

            }
            else
            {
                if (this.text_otvet != t)
                {
                    if (MessageBox.Show(
        "Есть изменения, хотели бы Вы их сохранить?",
        "Предупреждение",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        //    MessageBox.Show(t);
                        this.text_otvet = t;
                    }

                }

            }




            GC.Collect();


            Close();

        }

        string str_sobr(List<texnika> array_vibr_tex)
        {

            string t = "";
            for (int i = 0; i < array_vibr_tex.Count; i++)
            {
                texnika otvet_massiv = (array_vibr_tex[i]);

                string name = "";

                if (otvet_massiv.Article == "***" || otvet_massiv.Article == "15R***" || otvet_massiv.Article == "SAD***" || otvet_massiv.Article == "*") name = otvet_massiv.TName.Replace(',', '@').Replace(';', '$');



                t += otvet_massiv.type.Replace(',', '@').Replace(';', '$') + "~" +
                otvet_massiv.Article.Replace(',', '@').Replace(';', '$') + "~" +
                otvet_massiv.kolvo.ToString().Replace(',', '.').Replace(';', '$') + "~" +
                otvet_massiv.OTD.Replace(',', '@').Replace(';', '$') + "~" +
                otvet_massiv.Prim.Replace(',', '@').Replace(';', '$') + "~" +
                otvet_massiv.GROUP_dlyaspicif.Replace(',', '@').Replace(';', '$') + "~" +
                otvet_massiv.nom_pp.ToString().Replace(',', '@').Replace(';', '$') + "~" +
                name + "~" +
                otvet_massiv.UnitsName.Replace(',', '@').Replace(';', '$') + "~" +
                otvet_massiv.priceredak.ToString().Replace(',', '.').Replace(';', '$') + ";";
                //      MessageBox.Show(text_otvet);

            }
            return t;

        }

        void clear_filtr()
        {
            tb1.Text = "";
            tb2.Text = "";
            tb1.Focus();

        }

        void loadpage()
        {

            log.Add("установим размеры окна окна");
            Properties.Settings_AT ps = Properties.Settings_AT.Default;
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
            /// string[] kolonki = ps.kolonki.Split(';');

           // st1.Width.Value= ps.st1;
            st1.Width = new DataGridLength(ps.st1, DataGridLengthUnitType.Pixel);// = ;
            st2.Width = new DataGridLength(ps.st2, DataGridLengthUnitType.Pixel);// = ;
            st3.Width = new DataGridLength(ps.st3, DataGridLengthUnitType.Pixel);// = ;
            st4.Width = new DataGridLength(ps.st4, DataGridLengthUnitType.Pixel);// = ;
            otdelka.Width = new DataGridLength(ps.st5, DataGridLengthUnitType.Pixel);// = ;
            edizmer.Width = new DataGridLength(ps.st6, DataGridLengthUnitType.Pixel);// = ;
            st7.Width = new DataGridLength(ps.st7, DataGridLengthUnitType.Pixel);// = ;
            st8.Width = new DataGridLength(ps.st8, DataGridLengthUnitType.Pixel);// = ;
            st9.Width = new DataGridLength(ps.st9, DataGridLengthUnitType.Pixel);// = ;
            st11.Width = ps.st11;
            st12.Width = ps.st12;
            st13.Width = ps.st13;
            st14.Width = ps.st14;

        }

        private bool FindComputer(texnika bk)
        {

            if (bk.Article == (Lb_spis_tex.SelectedItem as texnika).Article && bk.OTD == "" && bk.Prim == "")
            {//если у сущ поз артикул совп с артик и (прим или отделка есть)
                index_for_poisl = bk;

                return true;
            }
            else
            {

                return false;
            }

        }

        void Grid_select(texnika poluch1)
        {
            if (poluch1.GRAFIKA != "0")
            {
                MessageBox.Show(poluch1.Article + " " + poluch1.TName + " можно добавить только в графическом исполнении (искать в дереве объектов)");

                return;
            }

            List<texnika> kotor_v_gr3 = array_vibr_tex.FindAll(FindComputer);


            texnika poluch = new texnika();
            poluch.Article = poluch1.Article;
            poluch.baseprice = poluch1.baseprice;
            poluch.Group = poluch1.Group;
            poluch.GroupName = poluch1.GroupName;
            poluch.GROUP_dlyaspicif = poluch1.GROUP_dlyaspicif;
            poluch.ID = poluch1.ID;
            poluch.kolvo = poluch1.kolvo;
            poluch.nom_pp = poluch1.nom_pp;
            poluch.OTD = poluch1.OTD;
            poluch.priceredak = poluch1.priceredak;
            poluch.Prim = poluch1.Prim;
            poluch.sort = poluch1.sort;
            poluch.TName = poluch1.TName;
            poluch.type = poluch1.type;
            poluch.UnitsId = poluch1.UnitsId;
            poluch.UnitsName = poluch1.UnitsName;
            poluch.vived = poluch1.vived;
            poluch.colortext = poluch1.colortext;



            lb_vibr_tex.SelectedItem = index_for_poisl;
            if (kotor_v_gr3.Count <= 0)
            {//если такой нет
                //добавить новую строку
                nom_PP++;
                poluch.nom_pp = nom_PP;
                array_vibr_tex.Add(poluch);
                lb_vibr_tex.ItemsSource = null;
                lb_vibr_tex.ItemsSource = array_vibr_tex;

                //   g3.Style = DataGridViewTriState.True;

            }
            else
            {//если такая уже есть

                dial_for_acctex_danet dial_for_acctex_danet = new dial_for_acctex_danet(this);
                dial_for_acctex_danet.ShowDialog();
                if (redakilidob == 1)
                {
                    //добавить новую строку
                    nom_PP++;
                    poluch.nom_pp = nom_PP;
                    array_vibr_tex.Add(poluch);
                    lb_vibr_tex.ItemsSource = null;
                    lb_vibr_tex.ItemsSource = array_vibr_tex;
                }
                if (redakilidob == 2)
                {

                    dial_for_acctex dial_for_acctex1 = new dial_for_acctex((lb_vibr_tex.SelectedItem as texnika));
                    dial_for_acctex1.ShowDialog();



                    //   kotor_v_gr3.Last().kolvo++;//увеличиваем на 1
                    lb_vibr_tex.ItemsSource = null;
                    lb_vibr_tex.ItemsSource = array_vibr_tex;
                }



            }


        }

        bool isreadonly_forGRID(DataGridBeginningEditEventArgs e, string[] slovarb_Stolbikov)
        {

            if (Array.IndexOf(slovarb_Stolbikov, e.Column.Header.ToString()) == -1)
            {//если мы выбираемый столюик есть в списке разреш для редкат столбов то разрешим редактирование

                return true;
            }
            else
            {
                return false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            zakrit_ok = true;
            Closinger();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            clear_filtr();
        }

        private void b3_Click(object sender, RoutedEventArgs e)
        {
            Closinger();
        }

        private void g1_Loaded(object sender, RoutedEventArgs e)
        {

            Lb_spis_tex.ItemsSource = array_spis_tex;

            viewSource1 = new CollectionViewSource();
            viewSource1.Source = array_spis_tex;
            viewSource1.Filter += viewSource_Filter1;
            viewSource1.SortDescriptions.Add(new SortDescription("sort", ListSortDirection.Ascending));
            Lb_spis_tex.ItemsSource = viewSource1.View;

        }

        void viewSource_Filter1(object sender, FilterEventArgs e)
        {

            try
            {
                e.Accepted = false;
                if (!((texnika)e.Item).vived)
                {
                    if (((texnika)e.Item).Article.ToLower().IndexOf(tb1.Text.ToLower()) >= 0)
                    {

                        bool sledetap = true;

                        string[] splitmass = tb2.Text.ToLower().Split(' ');
                        for (int w = 0; w < splitmass.Count(); w++)
                        {
                            if (((texnika)e.Item).TName.ToLower().IndexOf(splitmass[w]) < 0) sledetap = false;

                        }

                        //(
                        if (sledetap)
                        {

                            //поиск по трееее
                            if (tv1.SelectedItem == null || (tv1.SelectedItem as MenuItem).type == "at*")//если пока ничегоне выбрано
                            {
                                e.Accepted = true;
                            }
                            else
                            {

                                string name, id = "";//id в триивиф для фильтрации

                                if ((tv1.SelectedItem as MenuItem).type == "t*" ||
                                    (tv1.SelectedItem as MenuItem).type == "a*")
                                {//если выбрали в названии


                                    if ((e.Item as texnika).type == "a" && (tv1.SelectedItem as MenuItem).type == "a*") e.Accepted = true;

                                    if ((e.Item as texnika).type == "t" && (tv1.SelectedItem as MenuItem).type == "t*") e.Accepted = true;


                                }
                                else
                                {
                                    //если выбрали в псол раскр списке

                                    id = (tv1.SelectedItem as MenuItem).ID;




                                    if (((texnika)e.Item).Article == "***" || ((texnika)e.Item).Article == "*" || ((texnika)e.Item).Article == "15R***" || ((texnika)e.Item).Article == "SAD***")
                                    {



                                        if (((e.Item as texnika).Article == "***" || ((texnika)e.Item).Article == "15R***" || ((texnika)e.Item).Article == "SAD***") && (tv1.SelectedItem as MenuItem).type == "a")
                                        {
                                            e.Accepted = true;
                                        }

                                        if ((e.Item as texnika).Article == "*" && (tv1.SelectedItem as MenuItem).type == "t")
                                        {
                                            e.Accepted = true;
                                        }


                                    }
                                    else
                                    {
                                        if (((texnika)e.Item).Group == id && ((texnika)e.Item).type == (tv1.SelectedItem as MenuItem).type)
                                        {

                                            e.Accepted = true;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Исключение");
            }


        }

        private void tv1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            viewSource1.View.Refresh();
        }

        private void tb1_TextChanged(object sender, TextChangedEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        private void tb2_TextChanged(object sender, TextChangedEventArgs e)
        {


            timer.Stop();
            timer.Start();

        }

        private void timerTick(object sender, EventArgs e)
        {
            viewSource1.View.Refresh();
            timer.Stop();
        }

        private void c1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            viewSource1.View.Refresh();
        }

        private void c2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            viewSource1.View.Refresh();
        }

        private void g1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            texnika poluch = new texnika();
            poluch = Lb_spis_tex.SelectedItem as texnika;
            Grid_select(poluch);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            Properties.Settings_AT ps = Properties.Settings_AT.Default;
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

            ps.st1 = st1.ActualWidth;
            ps.st2 = st2.ActualWidth;
            ps.st3 = st3.ActualWidth;
            ps.st4 = st4.ActualWidth;
            ps.st5 = otdelka.ActualWidth;
            ps.st6 = edizmer.ActualWidth;
            ps.st7 = st7.ActualWidth;
            ps.st8 = st8.ActualWidth;
            ps.st9 = st9.ActualWidth;

            ps.st11 = st11.ActualWidth;
            ps.st12 = st12.ActualWidth;
            ps.st13 = st13.ActualWidth;
            ps.st14 = st14.ActualWidth;
            //MessageBox.Show(st14.Width.ToString());
            /*
            ps.kolonki =
st1.Width.ToString() + ";" +
st2.Width.ToString() + ";" +
st3.Width.ToString() + ";" +
st4.Width.ToString() + ";" +
otdelka.Width.ToString() + ";" +
edizmer.Width.ToString() + ";" +
st7.Width.ToString() + ";" +
st8.Width.ToString() + ";" +
st9.Width.ToString() + ";" +
st11.Width.ToString() + ";" +
st12.Width.ToString() + ";" +
st13.Width.ToString() + ";" +
st14.Width.ToString() + ";";
*/


            ps.Save();



            //pfrhsnbt


        }

        private void tb1_Loaded(object sender, RoutedEventArgs e)
        {
            tb1.Focus();
        }

        private void g3_delete_Click(object sender, RoutedEventArgs e)
        {
            array_vibr_tex.Remove((texnika)lb_vibr_tex.SelectedItem);
            lb_vibr_tex.ItemsSource = null;
            lb_vibr_tex.ItemsSource = array_vibr_tex;
        }

        private void g3_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                zakrit_ok = true;
                Closinger();
            }

            if (e.Key == Key.F1)
            {
                if (lb_vibr_tex.SelectedIndex != -1)
                {
                    dial_for_acctex dial_for_acctex1 = new dial_for_acctex((lb_vibr_tex.SelectedItem as texnika));
                    dial_for_acctex1.ShowDialog();

                    lb_vibr_tex.SelectedItem = dial_for_acctex1.otvet;
                    lb_vibr_tex.ItemsSource = null;
                    lb_vibr_tex.ItemsSource = array_vibr_tex;
                }
                else
                {
                    MessageBox.Show("Сначала выберите элемент, который необходимо редактировать, затем нажмите эту кнопку снова");

                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            tb1.Text = "";
            tb1.Focus();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            tb2.Text = "";
            tb2.Focus();
        }

        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            string Tag = (sender as GridViewColumnHeader).Tag.ToString();

            if (viewSource1.SortDescriptions.Count != 0)
            {
                if (viewSource1.SortDescriptions.First() == new SortDescription(Tag, ListSortDirection.Descending))
                {
                    viewSource1.SortDescriptions.Clear();
                    viewSource1.SortDescriptions.Add(new SortDescription(Tag, ListSortDirection.Ascending));
                }
                else
                {
                    viewSource1.SortDescriptions.Clear();
                    viewSource1.SortDescriptions.Add(new SortDescription(Tag, ListSortDirection.Descending));
                }
            }
            else
            {
                viewSource1.SortDescriptions.Add(new SortDescription(Tag, ListSortDirection.Ascending));
            }
        }

        private void g1_KeyUp(object sender, KeyEventArgs e)
        {
            if (g1_KeyUp_bool)
            {
                if (e.Key == Key.Enter)
                {
                    texnika poluch = new texnika();
                    poluch = Lb_spis_tex.SelectedItem as texnika;
                    Grid_select(poluch);


                }
                g1_KeyUp_bool = false;
            }

        }

        private void g1_KeyDown(object sender, KeyEventArgs e)
        {
            g1_KeyUp_bool = true;
        }

        private void g3_CellEditEnding_1(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void g3_BeginningEdit_1(object sender, DataGridBeginningEditEventArgs e)
        {


            if ((e.Row.Item as texnika).Article == "***" || (e.Row.Item as texnika).Article == "*" || (e.Row.Item as texnika).Article == "15R***" || (e.Row.Item as texnika).Article == "SAD***")
            {//если 
                if ((e.Row.Item as texnika).Article == "*")
                {
                    string[] slovarb = { "Название", "Примечание", "Ед. изм.", "Кол-во", "Базовая цена", "Цена ред." };

                    e.Cancel = isreadonly_forGRID(e, slovarb);

                }
                else
                {
                    string[] slovarb = { "Название", "Примечание", "Отделка", "Ед. изм.", "Кол-во", "Базовая цена", "Цена ред." };
                    e.Cancel = isreadonly_forGRID(e, slovarb);
                }
            }
            else
            {
                if ((e.Row.Item as texnika).type == "a")
                {
                    string[] slovarb = { "Примечание", "Отделка", "Кол-во", "Цена ред." };
                    e.Cancel = isreadonly_forGRID(e, slovarb);
                }
                if ((e.Row.Item as texnika).type == "t")
                {
                    string[] slovarb = { "Примечание", "Кол-во", "Цена ред." };
                    e.Cancel = isreadonly_forGRID(e, slovarb);

                }
            }



        }

        private void g3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool otvet = true;
            bool integer = "0123456789".IndexOf(e.Text) < 0;



            bool floate = @"0123456789.".IndexOf(e.Text) < 0;
            string b = (sender as DataGrid).CurrentCell.Column.Header.ToString();

            if (b == "Название" || b == "Примечание") otvet = false;
            if (b == "Базовая цена" || b == "Цена ред.") otvet = floate;
            if (((sender as DataGrid).CurrentItem as texnika).type == "a" && b == "Кол-во") otvet = floate;
            if (((sender as DataGrid).CurrentItem as texnika).type == "t" && b == "Кол-во") otvet = integer;

            //  MessageBox.Show((((sender as DataGrid).CurrentItem as texnika).type == "a" && b == "Кол - во.").ToString()+ b);


            e.Handled = otvet;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (lb_vibr_tex.SelectedIndex != -1)
            {
                dial_for_acctex dial_for_acctex1 = new dial_for_acctex((lb_vibr_tex.SelectedItem as texnika));
                dial_for_acctex1.ShowDialog();

                lb_vibr_tex.SelectedItem = dial_for_acctex1.otvet;
                lb_vibr_tex.ItemsSource = null;
                lb_vibr_tex.ItemsSource = array_vibr_tex;
            }
            else
            {
                MessageBox.Show("Сначала выбирите элемент, который необходимо редактировать, затем нажмите эту кнопку снова");






            }


        }

        private void g3_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string t = "";
            for (int i = 0; i < array_vibr_tex.Count; i++)
            {
                texnika otvet_massiv = ((texnika)array_vibr_tex[i]);

                string name = "";

                if (otvet_massiv.Article == "***" || otvet_massiv.Article == "15R***" || otvet_massiv.Article == "SAD***" || otvet_massiv.Article == "*") name = otvet_massiv.TName.Replace(',', '@').Replace(';', '$');



                t += otvet_massiv.type.Replace(',', '@').Replace(';', '$') + "~" +
                otvet_massiv.Article.Replace(',', '@').Replace(';', '$') + "~" +
                otvet_massiv.kolvo.ToString().Replace(',', '.').Replace(';', '$') + "~" +
                otvet_massiv.OTD.Replace(',', '@').Replace(';', '$') + "~" +
                otvet_massiv.Prim.Replace(',', '@').Replace(';', '$') + "~" +
                otvet_massiv.GROUP_dlyaspicif.Replace(',', '@').Replace(';', '$') + "~" +
                otvet_massiv.nom_pp.ToString().Replace(',', '@').Replace(';', '$') + "~" +
                name + "~" +
                otvet_massiv.UnitsName.Replace(',', '@').Replace(';', '$') + "~" +
                otvet_massiv.priceredak.ToString().Replace(',', '.').Replace(';', '$') + ";";
                //      MessageBox.Show(text_otvet);

            }



            Clipboard.SetText(t);







            MessageBox.Show("Теперь зайдите в кубик в другом проекте и нажмите кнопку \"2.Вставить\"");
            Closinger();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                pars(Clipboard.GetText());
            }
            catch
            {
                MessageBox.Show("Строка неверного формата");
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {


            if (MessageBox.Show(
             "Сейчас будет загружена техника, принадлежащая заказу. Все раннее выбранные аксессуары и техника будут очищены. Продолжить?",
            "Важно",
             MessageBoxButton.YesNo,
             MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {


                object xamb = neqqqqq.getParam(neqqqqq.Ambiente, "GetObject", "XAMB");
                object info = neqqqqq.getParamG(xamb, "INFO");
                object info2 = neqqqqq.getParamG(info, "INFO");
                string _RIFFABRICA = neqqqqq.getParam(info2, "Var", "_RIFFABRICA").ToString();
                if (_RIFFABRICA == "")
                {
                    MessageBox.Show("Заказу не присвоен фабричный номер");
                }
                else
                {

                    log.Add("номер заказа на фабрике = " + _RIFFABRICA);
                    nom_PP = 0;
                    List<texnika> importlist = new List<texnika>();

                    FbConnectionStringBuilder fb_con = new FbConnectionStringBuilder();
                    fb_con.Charset = "WIN1251"; //используемая кодировка
                    fb_con.UserID = "sysdba"; //логин
                    fb_con.Password = "TukTuk"; //пароль
                    fb_con.Database = "172.16.6.155:/usr/interbase/db/resurs.gdb"; //путь к файлу базы данных
                    fb_con.ServerType = 0; //указываем тип сервера (0 - "полноценный Firebird" (classic или super server), 1 - встроенный (embedded))

                    //создаем подключение
                    var fb = new FbConnection(fb_con.ToString()); //передаем нашу строку подключения объекту класса FbConnection

                    fb.Open(); //открываем БД




                    if (fb.State == ConnectionState.Closed)
                        fb.Open();

                    FbTransaction fbt = fb.BeginTransaction(); //стартуем транзакцию; стартовать транзакцию можно только для открытой базы (т.е. мутод Open() уже был вызван ранее, иначе ошибка)

                    FbCommand SelectSQL = new FbCommand("SELECT * FROM CCUSTOMTEXNICS WHERE CUSTOMID=" + _RIFFABRICA + " order by POZ asc", fb); //задаем запрос на выборку

                    SelectSQL.Transaction = fbt; //необходимо проинициализить транзакцию для объекта SelectSQL
                    FbDataReader reader = SelectSQL.ExecuteReader(); //для запросов, которые возвращают результат в виде набора данных надо использоваться метод ExecuteReader()


                    while (reader.Read()) //пока не прочли все данные выполняем...
                    {

                        float baseprice = 0;
                        if (reader["WPRICE"].ToString() != "") baseprice = Convert.ToSingle(reader["WPRICE"].ToString());

                        int nompp = Convert.ToInt32(reader["POZ"].ToString());

                        //MessageBox.Show(reader["SALONNUMBER"].ToString());
                        importlist.Add(new texnika()
                        {

                            type = "t",
                            ID = reader["TPriceID"].ToString(),
                            TName = reader["NAME"].ToString(),
                            Article = reader["ARTICLE"].ToString(),
                            UnitsId = "шт",
                            baseprice = baseprice,
                            priceredak = baseprice,
                            UnitsName = "3",
                            Prim = "",
                            kolvo = Convert.ToInt32(reader["CNT"].ToString()),
                            OTD = "",
                            nom_pp = nompp,


                            //                    GroupName = reader["TexType"].ToString(),
                            //                  Group = reader["TexID"].ToString(),
                            // sort = sort,
                            GROUP_dlyaspicif = "",// reader["TKOEFGROUP_ID"].ToString(),


                        });
                        // MessageBox.Show(reader["CNT"].ToString()+""+ reader["WPRICE"].ToString());

                        if (nom_PP < nompp) nom_PP = nompp;

                        //  Grid_select(texnika);
                    }




                    SelectSQL.Dispose(); //в документации написано, что ОЧЕНЬ рекомендуется убивать объекты этого типа, если они больше не нужны
                    fb.Close();


                    //  MessageBox.Show(str_sobr(importlist));
                    pars(str_sobr(importlist));
                }
            }
        }

        private void Lb_vibr_tex_CurrentCellChanged(object sender, EventArgs e)
        {

        }
    }

    public class texnika
    {

        public string type { get; set; }
        public string ID { get; set; }
        public int nom_pp { get; set; }
        public string TName { get; set; }
        public string Article { get; set; }
        public string Group { get; set; }
        public string GroupName { get; set; }
        public float baseprice { get; set; }
        public float priceredak { get; set; }
        public string UnitsId { get; set; }
        public string UnitsName { get; set; }
        public string Prim { get; set; }
        public string OTD { get; set; }
        public float kolvo { get; set; }
        public bool vived { get; set; }
        public int sort { get; set; }
        public string GROUP_dlyaspicif { get; set; }
        public string colortext { get; set; }
        public string GRAFIKA { get; set; }
    }

    public class todelka
    {
        public string ID { get; set; }
        public string nameotd { get; set; }
        public string Gender { get; set; }
    }

    public class MenuItem
    {
        public MenuItem()
        {
            this.Items = new ObservableCollection<MenuItem>();
        }

        public string ID { get; set; }
        public string Title { get; set; }
        public string type { get; set; }

        public ObservableCollection<MenuItem> Items { get; set; }
    }

    class AgeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Все проверки для краткости выкинул
            return (string)value == "#FFDE0606" ?
                new SolidColorBrush(Colors.Red)
                : new SolidColorBrush(Colors.Black);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }


    [ValueConversion(typeof(object), typeof(int))]
    public class NumberToPolarValueConverter : IValueConverter
    {
        public object Convert(         object value, Type targetType,         object parameter, CultureInfo culture)
        {
            // Все проверки для краткости выкинул
            return (string)value != "0" ?
                new SolidColorBrush(Colors.Gray)
                : new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(         object value, Type targetType,         object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack not supported");
        }
    }

}

