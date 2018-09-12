using System;
using System.Collections.Generic;
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
using System.Data.OleDb;
using System.Reflection;
using System.IO;

namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {


        static BD_Connect BD = new BD_Connect();
        //   public string vh_func_path = "";
        public string vh_func_str = "", str2 = "";

        public string papka_s_foto;

        private List<Person> idselect = new List<Person>();
        public List<Person> seldopotd = new List<Person>();
        public List<Person> history = new List<Person>();
        private List<Person> GROUPE = new List<Person>();
        public TekOtd TekOtdelka = new TekOtd();
        public List<TekOtd> TekOtdelka_Histor = new List<TekOtd>();
        public List<TekOtd> TekOtdelka_Zakl = new List<TekOtd>();
        List<NCS> ncs = new List<NCS>();
        public string idgroupload_from, razr_group, razr_odinak;
        public int stor_otd, obe_plasti;


        CollectionViewSource viewSource;

        bool reload_text = false;
        object item_forind1, item_forind2;



        public Window1(string path, string str1, string katalog)
        {
            InitializeComponent();

            try
            {
                str2 = str1;
                vh_func_str = str1;
                BD.path = path; //укажем файл бд

                papka_s_foto = Environment.CurrentDirectory + @"\" + katalog + @"\";

                //    TekOtdelka.Add(new TekOtd() { });

                zagr_spis_ncs();


                strrazobr(vh_func_str, true);//разбираем строку
                log.Add("разбираем строку");
                load_group();//загрузка списка групп
                log.Add("отделка загрузка списка групп");
                load_tekst();//загрузка тестуры
                log.Add("отделка загрузка тестуры");
                loadpage();//загрузка полож окна
                log.Add("отделка загрузка полож окна");
                first_def_zagr();//загрузка списка эффект отделки

              

                log.Add("отделка /загрузка списка эффект отделки");
                proverka_uhoda_za_granicu(); //проверка ухода за границу
                log.Add("отделка проверка ухода за границу");
                history_func();//загрузка истории
                log.Add("отделка загрузка истории");
                zakl_func();//загрузка закладок
                log.Add("отделка загрузка закладок");

                listView1_Loaded();
                selectbtn1();//выбор кнопки 1



            }
            catch (Exception err)
            {
                log.Add("отделка" + err.Message);
            }

        }
        void zagr_spis_ncs() //загрузка дополнительных параметров
        {
          

            string path = papka_s_foto+@"foto\Thumb\NCS";
            string[] files = Directory.GetFiles(path, "*.jpg");
            ncs.Add(new NCS() {path="",nazv="" });
            foreach (string file in files)
            {
                ncs.Add(new NCS()
                {
                    path = file,
                    nazv = file.Split('\\').Last().Split('.').First()


                });


            }

            cbncs.ItemsSource = ncs;
        }

        void first_def_zagr() //загрузка дополнительных параметров
        {
            OleDbDataReader reader = BD.conn("SELECT Name, id FROM TOtdelka WHERE IDGroup=25 or IDGroup=26 or IDGroup=41 or IDGroup=47 ORDER BY Name ASC");
            seldopotd.Clear();
            seldopotd.Add(new Person()
            {
                ID = "1",
                NAME = "Нет"
            });
            comboBox1.SelectedIndex = 0;
            int aasssaq = 1;
            while (reader.Read())
            {

                seldopotd.Add(new Person()
                {
                    ID = reader["id"].ToString(),
                    NAME = reader["Name"].ToString(),
                });

                if (TekOtdelka.index6 == reader["id"].ToString())
                {
                    comboBox1.SelectedIndex = aasssaq;
                }
                aasssaq++;
            }

            comboBox1.ItemsSource = seldopotd;



        }
        void load_group()
        {

            string s = "";

            string[] words = TekOtdelka.index10.Split('^');
            for (int x = 0; x < words.Length; x++)
            {

                if (x == 0)
                {
                    s += "WHERE ID=" + words[x];//Индекс Внешней пласти	
                }
                else
                {
                    s += " or ID=" + words[x];//Индекс Внешней пласти
                }

            }


            OleDbDataReader reader = BD.conn("SELECT NAME, ID FROM TOTDELKAGROUP " + s + " order by NAME ASC");

            GROUPE.Add(new Person()
            {
                NAME = "Все",
                ID = "*",
            });


            while (reader.Read())
            {
                GROUPE.Add(new Person()
                {
                    NAME = reader["NAME"].ToString().Trim(),
                    ID = reader["ID"].ToString(),
                });
            }

            comboBox2.ItemsSource = GROUPE;

        }
        void load_tekst()
        {
            string s = "";

            string[] words = TekOtdelka.index10.Split('^');
            for (int x = 0; x <= words.Length - 1; x++)
            {
                if (x == 0)
                {
                    s += "WHERE IDGroup=" + words[x];//Индекс Внешней пласти	
                }
                else
                {
                    s += " or IDGroup=" + words[x];//Индекс Внешней пласти
                }

            }

            OleDbDataReader reader = BD.conn("SELECT Name, id, TEKSTURA, NAPRAVLENIE, IDGroup FROM TOtdelka " + s + "  ORDER BY Name ASC");
            idselect = new List<Person>();
            idselect.Add(new Person()//нулевая отделка
            {
                napravl = "0",
                ID = "0",
                NAME = "Нет отделки",
                Idgroupe = "*",

                textura_otris = @"/TreeCadN;component/Foto/Net_Tekst.jpg",
                textura = "Net_Tekst.jpg"
            });



            while (reader.Read())
            {

                string tekstreader = reader["TEKSTURA"].ToString();
                string textura_otris1 = papka_s_foto + @"FOTO\Thumb\" + tekstreader;
                if (!File.Exists(textura_otris1)) textura_otris1 = @"/TreeCadN;component/Foto/Net_Tekst.jpg";
                if (reader["Name"].ToString() != "")
                {

                    idselect.Add(new Person()
                    {
                        napravl = reader["NAPRAVLENIE"].ToString(),
                        ID = reader["id"].ToString(),
                        NAME = reader["Name"].ToString(),
                        Idgroupe = reader["IDGroup"].ToString(),
                        textura_otris = textura_otris1,
                        textura = tekstreader
                    });


                }


            }

        }
        void settingparse(string vhod, int nom)
        {
            try
            {
                string[] history1word = vhod.Split('|');
                string[] history_Pars_str = history1word[8].Split(';');

                string cheked = "#FFB95F5F";
                if (((Array.IndexOf(razr_group.Split('^'), history1word[6]) != -1)
           && (Array.IndexOf(razr_group.Split('^'), history1word[7]) != -1)) ||
           ((Array.IndexOf(razr_group.Split('^'), history1word[6]) != -1) && (history_Pars_str[1] == "0")) ||
           ((Array.IndexOf(razr_group.Split('^'), history1word[7]) != -1) && (history_Pars_str[0] == "0")) ||
               ((history_Pars_str[0] == "0") && (history_Pars_str[1] == "0"))
               )
                {
                    cheked = "x:Null";
                }


                string textura1_otris1 = papka_s_foto + @"FOTO\Thumb\" + history1word[2];
                string textura2_otris1 = papka_s_foto + @"FOTO\Thumb\" + history1word[3];

                if (!File.Exists(textura1_otris1)) textura1_otris1 = @"/TreeCadN;component/Foto/Net_Tekst.jpg";
                if (!File.Exists(textura2_otris1)) textura2_otris1 = @"/TreeCadN;component/Foto/Net_Tekst.jpg";

                TekOtdelka_Histor.Add(new TekOtd()

                {

                    Name1 = history1word[0],
                    Name2 = history1word[1],
                    textura1 = history1word[2],
                    textura2 = history1word[3],
                    textura1_otris = textura1_otris1,
                    textura2_otris = textura2_otris1,
                    naprav1 = history1word[4],
                    naprav2 = history1word[5],
                    IDGroup1 = history1word[6],
                    IDGroup2 = history1word[7],
                    cheked = cheked,
                    str = history1word[8]

                });


            }
            catch
            {

            }
        }
        void history_func()
        {
            TekOtdelka_Histor.Clear();
            listBox1.SelectedIndex = -1;
            listBox1.ItemsSource = null;
            settingparse(Properties.Settings1.Default.history1, 0);
            settingparse(Properties.Settings1.Default.history2, 1);
            settingparse(Properties.Settings1.Default.history3, 2);
            settingparse(Properties.Settings1.Default.history4, 3);
            settingparse(Properties.Settings1.Default.history5, 4);


            listBox1.ItemsSource = TekOtdelka_Histor;



        }
        void settingparse_zakl(string vhod, int nom)//закладки
        {
            try
            {
                string[] history1word = vhod.Split('|');
                string[] history_Pars_str = history1word[8].Split(';');


                string cheked = "#FFB95F5F";

                if (((Array.IndexOf(razr_group.Split('^'), history1word[6]) != -1)
                && (Array.IndexOf(razr_group.Split('^'), history1word[7]) != -1)) ||
                ((Array.IndexOf(razr_group.Split('^'), history1word[6]) != -1) && (history_Pars_str[1] == "0")) ||
                ((Array.IndexOf(razr_group.Split('^'), history1word[7]) != -1) && (history_Pars_str[0] == "0")) ||
                    ((history_Pars_str[0] == "0") && (history_Pars_str[1] == "0"))
                    )
                {
                    cheked = "x:Null";
                }



                string textura1_otris1 = papka_s_foto + @"FOTO\Thumb\" + history1word[2];
                string textura2_otris1 = papka_s_foto + @"FOTO\Thumb\" + history1word[3];

                if (!File.Exists(textura1_otris1)) textura1_otris1 = @"/TreeCadN;component/Foto/Net_Tekst.jpg";
                if (!File.Exists(textura2_otris1)) textura2_otris1 = @"/TreeCadN;component/Foto/Net_Tekst.jpg";

                TekOtdelka_Zakl.Add(new TekOtd()

                {

                    Name1 = history1word[0],
                    Name2 = history1word[1],
                    textura1 = history1word[2],
                    textura2 = history1word[3],
                    textura1_otris = textura1_otris1,
                    textura2_otris = textura2_otris1,
                    naprav1 = history1word[4],
                    naprav2 = history1word[5],
                    IDGroup1 = history1word[6],
                    IDGroup2 = history1word[7],
                    cheked = cheked,
                    str = history1word[8]

                });


            }
            catch
            {

            }
        }
        void zakl_func()//закладки отрисовка
        {

            TekOtdelka_Zakl.Clear();
            listBox2.SelectedIndex = -1;

            listBox2.ItemsSource = null;
            settingparse_zakl(Properties.Settings1.Default.zakladka1, 0);
            settingparse_zakl(Properties.Settings1.Default.zakladka2, 1);
            settingparse_zakl(Properties.Settings1.Default.zakladka3, 2);
            settingparse_zakl(Properties.Settings1.Default.zakladka4, 3);
            settingparse_zakl(Properties.Settings1.Default.zakladka5, 4);


            listBox2.ItemsSource = TekOtdelka_Zakl;



        }
        public string strsobr()
        {

            string otvet = TekOtdelka.index1 + ";" + TekOtdelka.index2 + ";" + TekOtdelka.index3 + ";" + TekOtdelka.index4 + ";" + TekOtdelka.index5 + ";" + TekOtdelka.index6 + ";" + TekOtdelka.index7 + ";" + TekOtdelka.index8 + ";" + TekOtdelka.index9 + ";" + TekOtdelka.index10;
            return otvet;
        }
        public void pervload()
        {
            razr_odinak = TekOtdelka.index5;
            razr_group = TekOtdelka.index10;


            OleDbDataReader reader = BD.conn("SELECT Name, id, TEKSTURA, NAPRAVLENIE, IDGroup  FROM TOtdelka WHERE id=" + TekOtdelka.index1 + "");

            while (reader.Read())
            {
                if (TekOtdelka.index1 == "0")
                {
                    TekOtdelka.Name1 = "Нет отделки";
                    TekOtdelka.IDGroup1 = "*";

                }
                else
                {
                    TekOtdelka.Name1 = reader["Name"].ToString();
                    TekOtdelka.IDGroup1 = reader["IDGroup"].ToString();
                }
                TekOtdelka.textura1 = reader["TEKSTURA"].ToString();
                TekOtdelka.naprav1 = reader["NAPRAVLENIE"].ToString();

            }
            reader.Close();
            reader = null;//Загружаем фон для текстуры 1

            reader = BD.conn("SELECT Name, id, TEKSTURA, NAPRAVLENIE, IDGroup   FROM TOtdelka WHERE id=" + TekOtdelka.index2 + "");
            while (reader.Read())
            {

                if (TekOtdelka.index2 == "0")
                {
                    TekOtdelka.Name2 = "Нет отделки";
                    TekOtdelka.IDGroup2 = "*";
                }
                else
                {
                    TekOtdelka.Name2 = reader["Name"].ToString();
                    TekOtdelka.IDGroup2 = reader["IDGroup"].ToString();
                }
                TekOtdelka.textura2 = reader["TEKSTURA"].ToString();
                TekOtdelka.naprav2 = reader["NAPRAVLENIE"].ToString();

            }
            NCS polucnxs = ncs.Find(x => x.nazv.Equals(TekOtdelka.index9));
            if (polucnxs != null)
                cbncs.SelectedItem = polucnxs;
            else
                cbncs.SelectedItem = 0;

            

        }
        public void strrazobr(string str, bool perv = false)
        {

            //   TekOtdelka.str = strsobr();//для переопределения инд1 инд2  для выбора текстуры
            string[] words = str.Split(';');


            TekOtdelka.index1 = words[0];//Индекс Внешней пласти
            TekOtdelka.index2 = words[1];//индекс внутренней пласти
            TekOtdelka.index3 = words[2];//Направление текстуры 1- вертик, 2-горизонтально
            TekOtdelka.index4 = words[3];//Направление текстуры Внутренней пласти
            TekOtdelka.index5 = words[4];//Флаг, 1-одинаковая отделака обеих пластей, при этом назначать разную отделку нельзя
            TekOtdelka.index6 = words[5];//Индекс Эффекта
            TekOtdelka.index7 = words[6];//Галочка одинак отделка
            TekOtdelka.index8 = words[7];//нестанд цвет
            TekOtdelka.index9 = words[8];//не исп
            TekOtdelka.index10 = words[9];// спис разреш групп для отделки




            if (perv) pervload();



            brdncs.ToolTip = TekOtdelka.index8;



            TekOtdelka.index5 = razr_odinak;//Флаг, 1-одинаковая отделака обеих пластей, при этом назначать разную отделку нельзя
            TekOtdelka.index10 = razr_group;// спис разреш групп для отделки




            if (TekOtdelka.index5 == "1")
            {
                TekOtdelka.index2 = TekOtdelka.index1;
                TekOtdelka.index4 = TekOtdelka.index3;
                TekOtdelka.index7 = "1";
            }

            //загруж название

            label7.Text = TekOtdelka.Name1;
            label8.Text = TekOtdelka.Name2;



            //загруж направл текстуры для 1 //загруж карт для 1 кнопки
            if (TekOtdelka.naprav1 == "1")//если обладает направлением
            {
                button3.IsEnabled = true;
                if (TekOtdelka.index3 == "1")//если вертикально
                {
                    image3.Source = norotate_loc("naprav.png");
                    image1.Source = norotate(TekOtdelka.textura1);
                    button3.ToolTip = "Вертикальное направление волокон шпона";
                }
                else//если горизонатльно
                {
                    image3.Source = rotate270_loc();
                    image1.Source = rotate270(TekOtdelka.textura1);
                    button3.ToolTip = "Горизонтальное направление волокон шпона";
                }
            }
            else
            {//нет направлений

               
      
                    image1.Source = norotate(TekOtdelka.textura1);
                         image3.Source = norotate_loc("no_naprav.png");
                button3.IsEnabled = false;
                button3.ToolTip = "Не имеет направления волокон шпона";
            }


            //загруж направл текстуры для 2 //загруж карт для 2 кнопки
            if (TekOtdelka.naprav2 == "1")//если обладает направлением
            {
                button4.IsEnabled = true;
                if (TekOtdelka.index4 == "1")//если вертикально
                {
                    image4.Source = norotate_loc("naprav.png");
                    image2.Source = norotate(TekOtdelka.textura2);
                    button4.ToolTip = "Вертикальное направление волокон шпона";
                }
                else//если горизонатльно
                {
                    image4.Source = rotate270_loc();
                    image2.Source = rotate270(TekOtdelka.textura2);
                    button4.ToolTip = "Горизонтальное направление волокон шпона";

                }
            }
            else
            {

  image2.Source = norotate(TekOtdelka.textura2);
                            
                image4.Source = norotate_loc("no_naprav.png");
                button4.IsEnabled = false;
                button4.ToolTip = "Не имеет направления волокон шпона";
            }




            //загруж одинак отделку
            if (TekOtdelka.index5 == "1")
            {

                //заблокир кнопка
                image6.Source = norotate_loc("tek4.png");
                button11.IsEnabled = false;

            }
            else
            {
                button11.IsEnabled = true;
                if (TekOtdelka.index7 == "1")
                {
                    //отделка одинаковая
                    image6.Source = norotate_loc("tek2.png");
                }
                else
                {
                    // отделка разная
                    image6.Source = norotate_loc("tek3.png");
                }


            }
            //загружаем необычный цвет
            
            if (TekOtdelka.index1 == "65" || TekOtdelka.index1 == "66" || TekOtdelka.index2 == "65" || TekOtdelka.index2 == "66")
            {

                cbncs.IsEnabled = true;
                NCS polucnxs = ncs.Find(x => x.nazv.Equals(TekOtdelka.index9));
                if (polucnxs != null)
                {
                    cbncs.SelectedItem = ncs.Find(x => x.nazv.Equals(TekOtdelka.index9));
                    if (TekOtdelka.index9 != "")
                    {
                        if (TekOtdelka.index1 == "65" || TekOtdelka.index1 == "66")
                            image1.Source = new BitmapImage(new Uri((cbncs.SelectedItem as NCS).path));
                        if (TekOtdelka.index2 == "65" || TekOtdelka.index2 == "66")
                            image2.Source = new BitmapImage(new Uri((cbncs.SelectedItem as NCS).path));
                        imgncs.Source = new BitmapImage(new Uri((cbncs.SelectedItem as NCS).path));
                    }
                }
                else
                {
                    cbncs.SelectedIndex = 0;
                  
                }
          

            }
            else
            {


                cbncs.IsEnabled = false;
       
                cbncs.SelectedIndex = 0;
            }
            
            TekOtdelka.str = strsobr();

        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            selectbtn1();
        }
        void selectbtn1()
        {

            if (TekOtdelka.index7 == "1")
            {
                obe_plasti = 1;//значит на обе пласти
            }
            else
            {
                obe_plasti = 0;
            }

            stor_otd = 1;// на внешнюю пласть


            comboBox2.SelectedItem = GROUPE.Find(x => x.ID.Equals(TekOtdelka.IDGroup1));
            if (!reload_text)
            {
                viewSource.View.Refresh();
            }
            reload_text = false;
            listView1.ScrollIntoView(item_forind1);
            listView1.SelectedItem = (item_forind1);

            textBox1.Focus();
            g2.Background = (Brush)new BrushConverter().ConvertFrom("#E5FFFFFF");
            g1.Background = (Brush)new BrushConverter().ConvertFrom("#FFABCDFF");
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {


            if (TekOtdelka.index7 == "1")
            {
                obe_plasti = 1;//значит на обе пласти
            }
            else
            {
                obe_plasti = 0;
            }

            stor_otd = 2;// на внутр пласть

            comboBox2.SelectedItem = GROUPE.Find(x => x.ID.Equals(TekOtdelka.IDGroup2));
            if (!reload_text)
            {
                viewSource.View.Refresh();
            }
            reload_text = false;
            textBox1.Focus();
            listView1.ScrollIntoView(item_forind2);

            listView1.SelectedValue = item_forind2;

            g1.Background = (Brush)new BrushConverter().ConvertFrom("#E5FFFFFF");
            g2.Background = (Brush)new BrushConverter().ConvertFrom("#FFABCDFF");


        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {

            if (TekOtdelka.index3 == "1")
            {//вертик
                TekOtdelka.index3 = "0";
            }
            else
            {//гориз
                TekOtdelka.index3 = "1";
            }

            if (TekOtdelka.index7 == "1")
            {
                TekOtdelka.index4 = TekOtdelka.index3;
            }
            strrazobr(strsobr());


        }
        private void button4_Click(object sender, RoutedEventArgs e)
        {



            if (TekOtdelka.index4 == "1")
            {//вертик
                TekOtdelka.index4 = "0";
            }
            else
            {//гориз
                TekOtdelka.index4 = "1";
            }


            if (TekOtdelka.index7 == "1")
            {
                TekOtdelka.index3 = TekOtdelka.index4;
            }


            strrazobr(strsobr());
        }

        void zakrit_OK()
        {


            try
            {
                TekOtdelka.index6 = seldopotd[(comboBox1.SelectedIndex)].ID;
            }
            catch
            {
                TekOtdelka.index6 = "1";

            }


            TekOtdelka.index9 = cbncs.Text;


            vh_func_str = strsobr();




            string otvetka = TekOtdelka.Name1 + "|" + TekOtdelka.Name2 + "|" + TekOtdelka.textura1 + "|" + TekOtdelka.textura2 + "|" + TekOtdelka.naprav1 + "|" + TekOtdelka.naprav2 + "|" + TekOtdelka.IDGroup1 + "|" + TekOtdelka.IDGroup2 + "|" + vh_func_str;
            Properties.Settings1 ps2 = Properties.Settings1.Default;
            bool prov = false;

            log.Add("Сохранили строку в истории: " + otvetka);

            if (otvetka == ps2.history1)
            {

                prov = true;
            }
            if (otvetka == ps2.history2)
            {
                ps2.history2 = ps2.history1;
                prov = true;
            }
            if (otvetka == ps2.history3)
            {
                ps2.history3 = ps2.history2;
                ps2.history2 = ps2.history1;
                prov = true;
            }
            if (otvetka == ps2.history4)
            {
                ps2.history4 = ps2.history3;
                ps2.history3 = ps2.history2;
                ps2.history2 = ps2.history1;
                prov = true;
            }
            if (otvetka == ps2.history5)
            {
                ps2.history5 = ps2.history4;
                ps2.history4 = ps2.history3;
                ps2.history3 = ps2.history2;
                ps2.history2 = ps2.history1;
                prov = true;
            }

            if (prov == false)
            {

                ps2.history5 = ps2.history4;
                ps2.history4 = ps2.history3;
                ps2.history3 = ps2.history2;
                ps2.history2 = ps2.history1;
            }

            ps2.history1 = otvetka;



            ps2.Save();
            //  history_func();


            str2 = vh_func_str;

            Close();
        }
        private void button5_Click(object sender, RoutedEventArgs e)
        {

            zakrit_OK();
        }
        private void button6_Click(object sender, RoutedEventArgs e)
        {

            Close();
        }
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)//выбор из истоии
        {

            if (listBox1.SelectedIndex != -1)
            {
                if (TekOtdelka_Histor[listBox1.SelectedIndex].cheked == "x:Null")
                {
                    listBox2.SelectedIndex = -1;
                    //  TekOtdelka.Clear();
                    // TekOtdelka.Add(new TekOtd() { });
                    TekOtdelka = TekOtdelka_Histor[listBox1.SelectedIndex];
                    strrazobr(TekOtdelka.str);
                }
                else
                {
                    MessageBox.Show("Группы для вашей детали не допускают такой отделки");
                }
            }

        }
        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)//выбор из закладок
        {

            if (listBox2.SelectedIndex != -1)
            {
                if (TekOtdelka_Zakl[listBox2.SelectedIndex].cheked == "x:Null")
                {
                    listBox1.SelectedIndex = -1;
                    //   TekOtdelka.Clear();
                    //  TekOtdelka.Add(new TekOtd() { });
                    TekOtdelka = TekOtdelka_Zakl[listBox2.SelectedIndex];
                    strrazobr(TekOtdelka.str);
                }
                else
                {
                    MessageBox.Show("Группы для вашей детали не допускают такой отделки");
                }
            }

        }
        private void button7_Click(object sender, RoutedEventArgs e)
        {



            if (comboBox1.SelectedIndex == 0)
            {
                TekOtdelka.index6 = "0";

            }
            else
            {

                TekOtdelka.index6 = seldopotd[(comboBox1.SelectedIndex)].ID;
            }

            TekOtdelka.index9 = cbncs.Text;


            vh_func_str = strsobr();




            string otvetka = TekOtdelka.Name1 + "|" + TekOtdelka.Name2 + "|" + TekOtdelka.textura1 + "|" + TekOtdelka.textura2 + "|" + TekOtdelka.naprav1 + "|" + TekOtdelka.naprav2 + "|" + TekOtdelka.IDGroup1 + "|" + TekOtdelka.IDGroup2 + "|" + vh_func_str;

            Properties.Settings1 ps2 = Properties.Settings1.Default;

            bool prov = false;
            log.Add("Сохранили строку в закладках: " + otvetka);


            if (otvetka == ps2.zakladka1)
            {

                prov = true;
            }
            if (otvetka == ps2.zakladka2)
            {
                ps2.zakladka2 = ps2.zakladka1;
                prov = true;
            }
            if (otvetka == ps2.zakladka3)
            {
                ps2.zakladka3 = ps2.zakladka2;
                ps2.zakladka2 = ps2.zakladka1;
                prov = true;
            }
            if (otvetka == ps2.zakladka4)
            {
                ps2.zakladka4 = ps2.zakladka3;
                ps2.zakladka3 = ps2.zakladka2;
                ps2.zakladka2 = ps2.zakladka1;
                prov = true;
            }
            if (otvetka == ps2.zakladka5)
            {
                ps2.zakladka5 = ps2.zakladka4;
                ps2.zakladka4 = ps2.zakladka3;
                ps2.zakladka3 = ps2.zakladka2;
                ps2.zakladka2 = ps2.zakladka1;
                prov = true;
            }

            if (prov == false)
            {

                ps2.zakladka5 = ps2.zakladka4;
                ps2.zakladka4 = ps2.zakladka3;
                ps2.zakladka3 = ps2.zakladka2;
                ps2.zakladka2 = ps2.zakladka1;
            }

            ps2.zakladka1 = otvetka;





            ps2.Save();
            zakl_func();








        }
        private void button8_Click(object sender, RoutedEventArgs e)
        {

        }
        private void button9_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
"Вы уверены, что хотите очистить весь журнал избранного?",
"Внимание",
MessageBoxButton.YesNo,
MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Properties.Settings1.Default.zakladka5 = "";
                Properties.Settings1.Default.zakladka4 = "";
                Properties.Settings1.Default.zakladka3 = "";
                Properties.Settings1.Default.zakladka2 = "";
                Properties.Settings1.Default.zakladka1 = "";
                Properties.Settings1.Default.Save();
                zakl_func();
            }
        }
        private void button10_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
"Вы уверены, что хотите очистить всю историю?",
"Внимание",
MessageBoxButton.YesNo,
MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Properties.Settings1.Default.history5 = "";
                Properties.Settings1.Default.history4 = "";
                Properties.Settings1.Default.history3 = "";
                Properties.Settings1.Default.history2 = "";
                Properties.Settings1.Default.history1 = "";
                Properties.Settings1.Default.Save();
                history_func();
            }
        }
        private void button11_Click(object sender, RoutedEventArgs e)
        {

            if (TekOtdelka.index7 == "0")
            {


                if (stor_otd == 2)
                {

                    smena_1to2();


                }
                else
                {
                    smena_2to1();

                }

                image6.Source = norotate_loc("tek2.png");
                TekOtdelka.index7 = "1";
                obe_plasti = 1;
            }
            else
            {
                image6.Source = norotate_loc("tek3.png");
                TekOtdelka.index7 = "0";
                obe_plasti = 0;
            }

            strrazobr(strsobr());
            listView1.SelectedIndex = -1;
        }
        private void button12_Click(object sender, RoutedEventArgs e)
        {


            var perem4 = TekOtdelka.index1;
            var perem5 = TekOtdelka.index3;
            var perem6 = TekOtdelka.textura1;
            var perem7 = TekOtdelka.naprav1;

            var perem9 = TekOtdelka.Name1;
            var perem10 = TekOtdelka.IDGroup1;

            smena_1to2();

            TekOtdelka.index2 = perem4;
            TekOtdelka.index4 = perem5;
            TekOtdelka.textura2 = perem6;
            TekOtdelka.naprav2 = perem7;
            TekOtdelka.Name2 = perem9;
            TekOtdelka.IDGroup2 = perem10;



            strrazobr(strsobr());
            listView1.SelectedIndex = -1;
        }
        private void button13_Click(object sender, RoutedEventArgs e)
        {

            smena_2to1();

            strrazobr(strsobr());
            listView1.SelectedIndex = -1;
        }
        private void button14_Click(object sender, RoutedEventArgs e)
        {


            smena_1to2();

            strrazobr(strsobr());
            listView1.SelectedIndex = -1;

        }
        //ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!
        public BitmapImage rotate270(string url)
        {

            BitmapImage bitmap1 = new BitmapImage();

            if (File.Exists(papka_s_foto + @"FOTO\Thumb\" + url))
            {
                bitmap1.BeginInit();
                bitmap1.UriSource = new Uri(papka_s_foto + @"FOTO\Thumb\" + url);
                bitmap1.Rotation = Rotation.Rotate270;
                bitmap1.EndInit();
            }
            else
            {

                bitmap1 = new BitmapImage(new Uri(@"/TreeCadN;component/Foto/Net_Tekst.jpg", UriKind.RelativeOrAbsolute));
            }

            return bitmap1;
        }
        public BitmapImage norotate(string url)
        {
            BitmapImage bitmap1;
            if (File.Exists(papka_s_foto + @"FOTO\Thumb\" + url))
            {
                bitmap1 = new BitmapImage(new Uri(papka_s_foto + @"FOTO\Thumb\" + url));
            }
            else
            {

                bitmap1 = new BitmapImage(new Uri(@"/TreeCadN;component/Foto/Net_Tekst.jpg", UriKind.RelativeOrAbsolute));
            }

            return bitmap1;
        }
        public BitmapImage rotate270_loc()
        {
            BitmapImage bitmap1 = new BitmapImage();
            bitmap1.BeginInit();
            bitmap1.UriSource = new Uri(@"/TreeCadN;component/Foto/naprav.png", UriKind.RelativeOrAbsolute);
            bitmap1.Rotation = Rotation.Rotate270;
            bitmap1.EndInit();
            return bitmap1;
        }
        public BitmapImage norotate_loc(string url)
        {

            BitmapImage bitmap1 = new BitmapImage(new Uri(@"/TreeCadN;component/Foto/" + url, UriKind.RelativeOrAbsolute));
            return bitmap1;
        }
        //ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!ROTATE!
        void loadpage()
        {
            Properties.Settings ps = Properties.Settings.Default;

            if (ps.Top1 == -100)
            {
                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            }
            else
            {
                this.Top = ps.Top1;
                this.Left = ps.Left1;
            }
            tabControl1.SelectedIndex = ps.tabControl1;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings ps = Properties.Settings.Default;
            ps.Top1 = this.Top;
            ps.Left1 = this.Left;
            ps.tabControl1 = tabControl1.SelectedIndex;
            ps.Save();
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {

                zakrit_OK();
            }
        }
        private void button15_Click(object sender, RoutedEventArgs e)
        {



            switch (listBox2.SelectedIndex)
            {
                case 0:
                    Properties.Settings1.Default.zakladka1 = "";
                    break;
                case 1:
                    Properties.Settings1.Default.zakladka2 = "";
                    break;
                case 2:
                    Properties.Settings1.Default.zakladka3 = "";
                    break;
                case 3:
                    Properties.Settings1.Default.zakladka4 = "";
                    break;
                case 4:
                    Properties.Settings1.Default.zakladka5 = "";
                    break;


            }


            if (Properties.Settings1.Default.zakladka1 == "")
            {
                Properties.Settings1.Default.zakladka1 = Properties.Settings1.Default.zakladka2;
                Properties.Settings1.Default.zakladka2 = "";
            }
            if (Properties.Settings1.Default.zakladka2 == "")
            {
                Properties.Settings1.Default.zakladka2 = Properties.Settings1.Default.zakladka3;
                Properties.Settings1.Default.zakladka3 = "";
            }
            if (Properties.Settings1.Default.zakladka3 == "")
            {
                Properties.Settings1.Default.zakladka3 = Properties.Settings1.Default.zakladka4;
                Properties.Settings1.Default.zakladka4 = "";
            }
            if (Properties.Settings1.Default.zakladka4 == "")
            {
                Properties.Settings1.Default.zakladka4 = Properties.Settings1.Default.zakladka5;
                Properties.Settings1.Default.zakladka5 = "";
            }
            Properties.Settings1.Default.Save();

            zakl_func();
        }
        private void textBox2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 ,.-йфяцычувскамепинртгоьшлбщдюзжэхъqazxswedcvfrtgbnhyujmkiolЙФЯЦЫЧУВСКАМЕПИНРТГОЬШЛБЩДЮЗЖХЭЪQAZWSXEDCRFVTGBYHNUJMIKOLP".IndexOf(e.Text) < 0;

        }
        public void proverka_uhoda_za_granicu()
        {
            //Получаем размеры рабочего пространства (в точках) для текущего видеорежима основного монитора.
            Size size = new Size();

            //Получаем размеры (в точках) для текущего видео 
            //режима основного монитора.
            size.Height = SystemParameters.MaximizedPrimaryScreenHeight;
            size.Width = SystemParameters.MaximizedPrimaryScreenWidth;

            if (this.Top < 0)
            {

                this.Top = 0;
            }
            if (this.Left < 0)
            {
                this.Left = 0;
            }
            if (this.Left + this.Width > size.Width)
            {
                this.Left = size.Width - this.Width;
            }
            if (this.Top + this.Height > size.Height)
            {
                this.Top = size.Height - this.Height;
            }
        }
        private void button16_Click(object sender, RoutedEventArgs e)//кнопка "последняя"
        {

            if (TekOtdelka_Histor[0].cheked == "x:Null")
            {
                listBox2.SelectedIndex = -1;
                // TekOtdelka.Clear();
                // TekOtdelka.Add(new TekOtd() { });
                TekOtdelka = TekOtdelka_Histor[0];
                strrazobr(TekOtdelka.str);


                zakrit_OK();
            }
            else
            {
                MessageBox.Show("Группы для вашей детали не допускают такой отделки");
            }

        }
        private void button17_Click(object sender, RoutedEventArgs e)
        {
            if (stor_otd == 2)
            {
                TekOtdelka.Name2 = "Нет отделки";
                TekOtdelka.index2 = "0";
                TekOtdelka.textura2 = "Net_Tekst.jpg";
                TekOtdelka.naprav2 = "0";
            }
            if (stor_otd == 1)
            {
                TekOtdelka.Name1 = "Нет отделки";
                TekOtdelka.index1 = "0";
                TekOtdelka.textura1 = "Net_Tekst.jpg";
                TekOtdelka.naprav1 = "0";

            }


            strrazobr(strsobr());
            listView1.SelectedIndex = -1;
        }
        private void listView1_Loaded()
        {
            viewSource = new CollectionViewSource();
            viewSource.Source = idselect;
            viewSource.Filter += viewSource_Filter;
            listView1.ItemsSource = viewSource.View;


        }
        void viewSource_Filter(object sender, FilterEventArgs e)
        {

            e.Accepted = false;
            if (((Person)e.Item).NAME.ToLower().IndexOf(textBox1.Text.ToLower()) >= 0)
            {

                if (comboBox2.SelectedItem == null)
                {
                    //   e.Accepted = true;
                }
                else
                {
                    if (((Person)comboBox2.SelectedItem).ID == "*")
                    {

                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = ((Person)comboBox2.SelectedItem).ID == ((Person)e.Item).Idgroupe;
                    }
                    if (((Person)e.Item).Idgroupe == "*")//чтобы выводлиось нет отделки
                    {
                        e.Accepted = true;
                    }


                    if (TekOtdelka.index2 == ((Person)e.Item).ID && stor_otd == 2)
                    {

                        item_forind2 = e.Item;
                    }
                    if (TekOtdelka.index1 == ((Person)e.Item).ID && stor_otd == 1)
                    {
                        item_forind1 = e.Item;
                    }
                }

            }



            reload_text = true;
        }
        private void textBox1_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            viewSource.View.Refresh();
        }
        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewSource.View.Refresh();
        }
        private void button1_Drop(object sender, DragEventArgs e)
        {
            //  ((TextBlock)sender).Text = (string)e.Data.GetData(DataFormats.Text);
            MessageBox.Show("asdasdasd");
        }

        private void button18_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "";
            comboBox2.SelectedIndex = 0;
            textBox1.Focus();
        }
        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {



        }
        void smena_1to2()
        {
            TekOtdelka.Name1 = TekOtdelka.Name2;
            TekOtdelka.index1 = TekOtdelka.index2;
            TekOtdelka.index3 = TekOtdelka.index4;
            TekOtdelka.textura1 = TekOtdelka.textura2;
            TekOtdelka.naprav1 = TekOtdelka.naprav2;
            TekOtdelka.IDGroup1 = TekOtdelka.IDGroup2;

        }//когда 1 текстуре придаем значение 2ой
        void smena_2to1()
        {
            TekOtdelka.IDGroup2 = TekOtdelka.IDGroup1;
            TekOtdelka.index2 = TekOtdelka.index1;
            TekOtdelka.index4 = TekOtdelka.index3;
            TekOtdelka.textura2 = TekOtdelka.textura1;
            TekOtdelka.naprav2 = TekOtdelka.naprav1;
            TekOtdelka.Name2 = TekOtdelka.Name1;
        }//когда 2 текстуре придаем значение 1ой
        private void textBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 ,.-йфяцычувскамепинртгоьшлбщдюзжэхъqazxswedcvfrtgbnhyujmkiolЙФЯЦЫЧУВСКАМЕПИНРТГОЬШЛБЩДЮЗЖХЭЪQAZWSXEDCRFVTGBYHNUJMIKOLP".IndexOf(e.Text) < 0;
        }



        private void listView1_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            if (listView1.SelectedItem != null)
            {
                if (stor_otd == 1)
                {

                    TekOtdelka.IDGroup1 = ((Person)listView1.SelectedItem).Idgroupe;
                    TekOtdelka.Name1 = ((Person)listView1.SelectedItem).NAME;
                    TekOtdelka.index1 = ((Person)listView1.SelectedItem).ID;
                    TekOtdelka.textura1 = ((Person)listView1.SelectedItem).textura;
                    TekOtdelka.naprav1 = ((Person)listView1.SelectedItem).napravl;
                    if (TekOtdelka.naprav1 != "1")
                    {
                        // TekOtdelka.index3 = "0";
                    }
                    if (obe_plasti == 1)
                    {
                        smena_2to1();
                    }
                }
                if (stor_otd == 2)
                {
                    TekOtdelka.IDGroup2 = ((Person)listView1.SelectedItem).Idgroupe;
                    TekOtdelka.Name2 = ((Person)listView1.SelectedItem).NAME;
                    TekOtdelka.index2 = ((Person)listView1.SelectedItem).ID;
                    TekOtdelka.textura2 = ((Person)listView1.SelectedItem).textura;
                    TekOtdelka.naprav2 = ((Person)listView1.SelectedItem).napravl;
                    if (TekOtdelka.naprav2 != "1")
                    {
                        //    TekOtdelka.index4 = "0";
                    }
                    if (obe_plasti == 1)
                    {
                        smena_1to2();
                    }
                }

                strrazobr(strsobr());
            }
        }

        private void comboBox1_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbncs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbncs.SelectedItem != null)
            {
             //   imgncs.Source = new BitmapImage(new Uri((cbncs.SelectedItem as NCS).path));
                TekOtdelka.index9 = (cbncs.SelectedItem as NCS).nazv;
                /*
                if (TekOtdelka.index1 == "65" || TekOtdelka.index1 == "66")
                {

                    image1.Source = new BitmapImage(new Uri((cbncs.SelectedItem as NCS).path));

                }
                if (TekOtdelka.index2 == "65" || TekOtdelka.index2 == "66")
                {
                    image2.Source = new BitmapImage(new Uri((cbncs.SelectedItem as NCS).path));
                }
                */
                strrazobr(strsobr());
            }
     
    }

        private void checkBox3_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void comboBox1_TextInput(object sender, TextCompositionEventArgs e)
        {
            //comboBox1.IsDropDownOpen = true;
        }

        private void comboBox1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // comboBox1.IsDropDownOpen = true;
        }

        private void comboBox1_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //   comboBox1.IsDropDownOpen = true;
        }

        private void comboBox1_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            comboBox1.IsDropDownOpen = true;
        }











    }
    public class TekOtd
    {

        public string textura2 { get; set; }
        public string index3 { get; set; }
        public string naprav2 { get; set; }
        public string index4 { get; set; }
        public string index7 { get; set; }
        public string index5 { get; set; }
        public string index1 { get; set; }
        public string index2 { get; set; }
        public string index6 { get; set; }
        public string index8 { get; set; }
        public string index9 { get; set; }
        public string index10 { get; set; }
        public string Name1 { get; set; }
        public string naprav1 { get; set; }
        public string Name2 { get; set; }
        public string textura1 { get; set; }
        public string IDGroup1 { get; set; }
        public string IDGroup2 { get; set; }
        public string str { get; set; }
        public string cheked { get; set; }
        public string textura2_otris { get; set; }
        public string textura1_otris { get; set; }

    }

    public class Person
    {
        public string textura { get; set; }
        public string napravl { get; set; }
        public string Idgroupe { get; set; }
        public string NAME { get; set; }
        public string ID { get; set; }
        public string textura_otris { get; set; }

    }

    public class NCS
    {
        public string path { get; set; }
        public string nazv { get; set; }
        public BitmapImage bitmap { get; set; }


    }

}
