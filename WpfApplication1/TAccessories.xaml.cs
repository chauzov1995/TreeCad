using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.OleDb;
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


namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для TAccessories.xaml
    /// </summary>
    public partial class TAccessories : Window
    {
        static BD_Connect BD = new BD_Connect();
        public string text_otvet;

        int poisk = 0;
        public static List<todelka> todelka = new List<todelka>();
        public static List<todelka> izmer = new List<todelka>();
        bool zakrit_ok = false;
        // List<Folder> tvv1 = new List<Folder>();
        //    List<combobox> cb1 = new List<combobox>();
        //   List<combobox> cb2 = new List<combobox>();
        List<texnika> gr1 = new List<texnika>();
        List<texnika> gr2 = new List<texnika>();
        int sort_forg1 = 1;
        CollectionViewSource viewSource1;
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();


        List<texnika> gr3 = new List<texnika>();
        List<texnika> gr4 = new List<texnika>();

        public int redakilidob = 0;
        int nom_PP = 0;


        public TAccessories(string path, string text)
        {
            InitializeComponent();
            BD.path = path; //укажем файл бд
            this.text_otvet = text;

            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);

            loadpage();//загрузка полож окна

            //отделка
            OleDbDataReader reader_otd = BD.conn("SELECT Id, Name FROM TOtdelka order by Name ASC");
            todelka.Add(new todelka() { ID = "", nameotd = "" });
            while (reader_otd.Read())
            {
                if (reader_otd["Name"].ToString() != "")
                {

                    todelka.Add(new todelka()
                    {
                        ID = reader_otd["Id"].ToString(),
                        nameotd = reader_otd["Name"].ToString(),

                    });


                }
            }

            otdelka.ItemsSource = todelka;

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

            edizmer.ItemsSource = izmer;



            //аксессуары
            OleDbDataReader reader_GRACC = BD.conn("SELECT TPrice1.Articul, TUnits.UnitsID,  TUnits.UnitsName, TPrice1.MName, TPrice1.MatID,  TPrice1.PriceID,  TPrice1.Price, TMAT.MATNAME,  TPrice1.Visibl FROM (TPrice1  LEFT OUTER JOIN  TMAT ON TPrice1.MatID = TMAT.MATID) LEFT OUTER JOIN  TUnits ON TUnits.UnitsID=TPrice1.UnitsID  ");
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

                gr1.Add(new texnika()
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
                    sort = sort


                });

                sort_forg1++;
            }

            //техника
            OleDbDataReader reader = BD.conn("SELECT TTxPrice1.Article, TTxPrice1.TName, TTxPrice1.TPriceID, TTxPrice1.TexID, TTxPrice1.Price, TTexnics.TexType  FROM TTxPrice1  LEFT OUTER JOIN  TTexnics ON  TTxPrice1.TexID = TTexnics.TexID  ");
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


                    gr1.Add(new texnika()
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
                        sort = sort
                    });
                }
                catch
                {
                    MessageBox.Show("err");
                }
                sort_forg1++;
            }








            //  elem.soder.Add(elem2);





            MenuItem root = new MenuItem() { Title = "Все", type = "at*" };
            MenuItem childItem1 = new MenuItem() { Title = "Аксессуары", type = "a*" };
          
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



            //treeviewformiriov.getall(path, tv1);

            // groups =u TreeViewFORMiriov.GetAll(path);

            // tv1.ItemsSource = groups;


            pars(text);



        }

        private void trw_Products_Expanded(object sender, RoutedEventArgs e)
        {
            /*
                TreeViewItem item = (TreeViewItem)e.OriginalSource;
                item.Items.Clear();
                spisok_treeview dir;
                if (item.Tag is spisok_treeview)
                {
                    spisok_treeview drive = (spisok_treeview)item.Tag;
                    dir = drive.soder;
                }
                else dir = (spisok_treeview)item.Tag;
                try
                {

                    spisok_treeview elem2 = new spisok_treeview();
                    elem2.Name = "frctcc";
                    // elem.soder = elem2;


                    TreeViewItem item1113 = new TreeViewItem();
                    item1113.Tag = dir;
                    item1113.Header = dir.Name;
                    if (dir.soder != null)
                    {
                        item1113.Items.Add("*");
                    }
                    item.Items.Add(item1113);

                }
                catch
                { }
            */
        }


        void pars(string vh_str)
        {
            string[] elems = vh_str.Split(';');
            for (int i = 0; i < elems.Count(); i++)
            {
                if (elems[i] != "")
                {
                    string[] znach = elems[i].Split('~');

                    if (Convert.ToInt32(znach[6]) > nom_PP) nom_PP = Convert.ToInt32(znach[6]);


                    if (znach[0] == "a")
                    {//t-техника a-аксесуар

                        gr3.Add(new texnika()
                        {
                            type = znach[0],//типп аксес или техн
                            Article = znach[1],//Артикул
                            kolvo = Convert.ToSingle(znach[2]),//Колво
                            OTD = znach[3],//отделка
                            Prim = znach[4],//примечание
                            Group = znach[5],//группы
                            nom_pp = Convert.ToInt32(znach[6]),//ид позиция
                            TName = znach[7],//название
                            UnitsName = znach[8],//ед изм
                            priceredak = Convert.ToSingle(znach[9]),//цена ред

                            vived = Convert.ToBoolean((gr1.Find(x => x.Article.Equals(znach[1]))).vived),
                            baseprice = Convert.ToSingle((gr1.Find(x => x.Article.Equals(znach[1]))).baseprice)



                        });
                    }
                    if (znach[0] == "t")
                    {
                        gr4.Add(new texnika()
                        {
                            type = znach[0],//типп аксес или техн
                            Article = znach[1],//Артикул
                            kolvo = Convert.ToSingle(znach[2]),//Колво
                            Prim = znach[3],//примечание
                            Group = znach[4],//группы
                            nom_pp = Convert.ToInt32(znach[5]),//ид позиция
                            TName = znach[6],//название
                                             // TName = znach[7],//всегда 3
                            priceredak = Convert.ToSingle(znach[8]),//цена ред

                            baseprice = Convert.ToSingle((gr2.Find(x => x.Article.Equals(znach[1]))).baseprice)
                        });

                    }
                }
            }





            g3.ItemsSource = gr3;


        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            zakrit_ok = true;
            Close();



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            clear_filtr();
        }
        void clear_filtr()
        {
            tb1.Text = "";
            tb2.Text = "";
            tb1.Focus();
        
        }









        private void b3_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void g1_Loaded(object sender, RoutedEventArgs e)
        {

            //  g1.CanUserSortColumns = true;
            g1.ItemsSource = gr1;

            viewSource1 = new CollectionViewSource();
            viewSource1.Source = gr1;
            viewSource1.Filter += viewSource_Filter1;
            viewSource1.SortDescriptions.Add(new SortDescription("sort", ListSortDirection.Ascending));
            g1.ItemsSource = viewSource1.View;

        }


        void viewSource_Filter1(object sender, FilterEventArgs e)
        {

            try
            {
                e.Accepted = false;
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
                            //   MessageBox.Show((e.Item ).ToString());


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

            Grid_select();

        }






        void loadpage()
        {
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
            string[] kolonki= ps.kolonki.Split(';');
            foreach (string kolonka in kolonki )
            {
        
                MessageBox.Show(otdelka.Width.ToString());
            
            }
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



            ps.Save();



            //pfrhsnbt

            string t = "";
            for (int i = 0; i < gr3.Count; i++)
            {
                texnika otvet_massiv = ((texnika)gr3[i]);
                t += "a~" + otvet_massiv.Article + "~" + otvet_massiv.kolvo + "~" + otvet_massiv.OTD + "~" + otvet_massiv.Prim + "~" + otvet_massiv.Group + "~" + otvet_massiv.nom_pp + "~" + otvet_massiv.TName + "~" + otvet_massiv.UnitsName + "~" + otvet_massiv.priceredak + ";";
                //      MessageBox.Show(text_otvet);
            }
            for (int i = 0; i < gr4.Count; i++)
            {
                texnika otvet_massiv = ((texnika)gr4[i]);
                t += "t~" + otvet_massiv.Article + "~" + otvet_massiv.kolvo + "~" + otvet_massiv.Prim + "~" + otvet_massiv.Group + "~" + otvet_massiv.nom_pp + "~" + otvet_massiv.TName + "~3~" + otvet_massiv.priceredak + ";";
                //   MessageBox.Show(text_otvet);
            }

            if (zakrit_ok)
            {
                //    MessageBox.Show(t);
                this.text_otvet = t;

            }
            else
            {
                if (this.text_otvet != t)
                {
                    if (MessageBox.Show(
        "Вы изменили примечания, хотели бы Вы их сохранить?",
        "Предупреждение",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        //    MessageBox.Show(t);
                        this.text_otvet = t;
                    }

                }

            }
        }
        void save()
        {

            text_otvet = "";
            for (int i = 0; i < gr3.Count; i++)
            {
                texnika otvet_massiv = ((texnika)gr3[i]);
                text_otvet += "a~" + otvet_massiv.Article + "~" + otvet_massiv.kolvo + "~" + otvet_massiv.OTD + "~" + otvet_massiv.Prim + "~" + otvet_massiv.Group + "~" + otvet_massiv.nom_pp + "~" + otvet_massiv.TName + "~" + otvet_massiv.UnitsName + "~" + otvet_massiv.priceredak + ";";
                //      MessageBox.Show(text_otvet);
            }
            for (int i = 0; i < gr4.Count; i++)
            {
                texnika otvet_massiv = ((texnika)gr4[i]);
                text_otvet += "t~" + otvet_massiv.Article + "~" + otvet_massiv.kolvo + "~" + otvet_massiv.Prim + "~" + otvet_massiv.Group + "~" + otvet_massiv.nom_pp + "~" + otvet_massiv.TName + "~3~" + otvet_massiv.priceredak + ";";
                //   MessageBox.Show(text_otvet);
            }
        }

        private void tb1_Loaded(object sender, RoutedEventArgs e)
        {
            tb1.Focus();
        }




        private void g3_delete_Click(object sender, RoutedEventArgs e)
        {
            gr3.Remove((texnika)g3.SelectedItem);
            g3.ItemsSource = null;
            g3.ItemsSource = gr3;
        }



        private void g3_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                zakrit_ok = true;
                Close();
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

        bool g1_KeyUp_bool = false;
        private void g1_KeyUp(object sender, KeyEventArgs e)
        {
            if (g1_KeyUp_bool)
            {
                if (e.Key == Key.Enter)
                {

                    Grid_select();


                }
                g1_KeyUp_bool = false;
            }

        }
        private void g1_KeyDown(object sender, KeyEventArgs e)
        {
            g1_KeyUp_bool = true;
        }


        texnika index_for_poisl ;
        private bool FindComputer(texnika bk)
        {

            if (bk.Article == (g1.SelectedItem as texnika).Article && bk.OTD == "" && bk.Prim == "")
            {//если у сущ поз артикул совп с артик и (прим или отделка есть)
                index_for_poisl = bk;
           
                return true;
            }
            else
            {

                return false;
            }
          
        }
      
        void Grid_select()
        {

            texnika poluch = g1.SelectedItem as texnika;
            List<texnika> kotor_v_gr3 = gr3.FindAll(FindComputer);

            texnika chto_vstavlyaem = new texnika()
            {
                type = poluch.type,
                Article = poluch.Article,
                baseprice = poluch.baseprice,
                Group = poluch.Group,
                GroupName = poluch.GroupName,
                ID = poluch.ID,
                kolvo = poluch.kolvo,
                nom_pp = poluch.nom_pp,
                OTD = poluch.OTD,

                priceredak = poluch.priceredak,
                Prim = poluch.Prim,
                TName = poluch.TName,
                UnitsName = poluch.UnitsName,
                vived = poluch.vived,


            };

            g3.SelectedItem = index_for_poisl;
            if (kotor_v_gr3.Count <= 0)
            {//если такая уже есть

                nom_PP++;
                chto_vstavlyaem.nom_pp = nom_PP;
                gr3.Add(chto_vstavlyaem);
                g3.ItemsSource = null;
                g3.ItemsSource = gr3;
                //   g3.Style = DataGridViewTriState.True;

            }
            else
            {

                dial_for_acctex_danet dial_for_acctex_danet = new dial_for_acctex_danet(this);
                dial_for_acctex_danet.ShowDialog();
               if (redakilidob ==1)
                {
                    //добавить новую строку
                    nom_PP++;
                    chto_vstavlyaem.nom_pp = nom_PP;
                    gr3.Add(chto_vstavlyaem);
                    g3.ItemsSource = null;
                    g3.ItemsSource = gr3;
                }
                if (redakilidob == 2)
                {

                    dial_for_acctex dial_for_acctex1 = new dial_for_acctex((g3.SelectedItem as texnika));
                    dial_for_acctex1.ShowDialog();



                 //   kotor_v_gr3.Last().kolvo++;//увеличиваем на 1
                   g3.ItemsSource = null;
                   g3.ItemsSource = gr3;
                }



            }


        }

        private void g3_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            //   MessageBox.Show(e.EditingElement.);

        }


        private void g3_CellEditEnding_1(object sender, DataGridCellEditEndingEventArgs e)
        {

        }


        private void g3_BeginningEdit_1(object sender, DataGridBeginningEditEventArgs e)
        {


            if ((e.Row.Item as texnika).Article == "***" || (e.Row.Item as texnika).Article == "*" || (e.Row.Item as texnika).Article == "15R***" || (e.Row.Item as texnika).Article == "SAD***")
            {//если 

                string[] slovarb = { "Название", "Примечание", "Отделка", "Ед. изм.", "Кол-во", "Базовая цена", "Цена ред." };
                e.Cancel = isreadonly_forGRID(e, slovarb);
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





        private void g3_MouseUp(object sender, MouseButtonEventArgs e)
        {

            //    texnika element = ((sender as DataGrid).SelectedItem as texnika);

            /*
                        rsktblo1_Copy.Text = element.Article;
                        rsktb1.Text = element.nom_pp.ToString();
                        rsktb2.Text = element.TName;
                        rsktb3.Text = element.kolvo.ToString();
                        combo1.Text = element.OTD;
                        rsktb4.Text = element.Prim;
                        combo2.Text = element.UnitsId;
                        rsktb3_Copy.Text = element.baseprice.ToString();
                        rsktb3_Copy1.Text = element.priceredak.ToString();

                        double mous_x = Mouse.GetPosition(null).X;
                        double mous_y = Mouse.GetPosition(null).Y;

                        if (mous_x + contextmen.Width > ActualWidth)
                        {
                            mous_x -= contextmen.Width;
                        }
                        if (mous_y + contextmen.Height > ActualHeight)
                        {
                            mous_y -= contextmen.Height;
                        }


                        //   (sender as DataGrid).

                        contextmen.Margin = new System.Windows.Thickness { Left = mous_x, Top = mous_y };// Mouse.GetPosition(null).X;
                        // MessageBox.Show(mous_x + contextmen.Width.ToString());

                        //    raskrspis.Visibility = Visibility;
            */

            //  g3.ItemsSource = null;
            //  g3.ItemsSource = gr3;

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (g3.SelectedIndex != -1)
            {
                dial_for_acctex dial_for_acctex1 = new dial_for_acctex((g3.SelectedItem as texnika));
                dial_for_acctex1.ShowDialog();

                g3.SelectedItem = dial_for_acctex1.otvet;
                g3.ItemsSource = null;
                g3.ItemsSource = gr3;
            }
            else
            {
                MessageBox.Show("Сначала выбирите элемент, который необходимо редактировать, затем нажмите эту кнопку снова");

            }


        }

        private void g3_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
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







}