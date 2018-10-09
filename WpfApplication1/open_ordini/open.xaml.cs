using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace TreeCadN.open_ordini
{
    /// <summary>
    /// Логика взаимодействия для open.xaml
    /// </summary>
    public partial class open : Window
    {
        private int BD_VARSION = 3;

        string dbFileName;
        string nomer, path_ordini;
        neqweqe neqqqqq;
        CollectionViewSource viewSource1 = new CollectionViewSource();
        int kolvo_stolb = 8;


        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        public open(neqweqe _neqqqqq, string _path_ordini)
        {
            InitializeComponent();

            path_ordini = _path_ordini;
            this.neqqqqq = _neqqqqq;

            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);

            string percorsoordini = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\_ecadpro\ordini";
            log.Add("percorsoordini= " + percorsoordini);
            log.Add("percorsoordiniif= " + Environment.CurrentDirectory + @"\_ecadpro\ordini");
            string[] files = Directory.GetFiles(percorsoordini, "*.eve", SearchOption.TopDirectoryOnly);
            if (files.Length == 0 || percorsoordini.ToUpper() == (Environment.CurrentDirectory + @"\_ecadpro\ordini").ToUpper())
            {
                btn_share.Visibility = Visibility.Collapsed;
            }
            Title = "Заказы | Папка с заказами: " + path_ordini;

            loadpage();









            createOpenBD();

            updateTekZakaz();

            select(true);


            //lb1.ScrollIntoView(lb1.SelectedItem);
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

        public void createOpenBD()
        {



            SQLiteConnection m_dbConn = new SQLiteConnection();
            SQLiteCommand m_sqlCmd = new SQLiteCommand();

            dbFileName = path_ordini + @"\sample.sqlite";




            if (!File.Exists(dbFileName))
                SQLiteConnection.CreateFile(dbFileName);


            try
            {
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;
                //при создании
                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS ordini (id INTEGER PRIMARY KEY AUTOINCREMENT, file_path TEXT, FIO TEXT, nomer_zakaza TEXT UNIQUE, last_upd TEXT, manager TEXT, orderprice TEXT, _RIFSALON TEXT, _RIFFABRICA TEXT, SROK TEXT, SALON TEXT)";
                m_sqlCmd.ExecuteNonQuery();
                //при создании !конец
                m_sqlCmd.CommandText = "PRAGMA user_version";
                var reader = m_sqlCmd.ExecuteReader();
                int oldVersion = 0;
                while (reader.Read())
                {
                    oldVersion = Convert.ToInt32(reader["user_version"]);
                }
                reader.Close();

                if (oldVersion == 0)
                {
                    oldVersion = BD_VARSION;
                    m_sqlCmd.CommandText = "PRAGMA user_version=" + oldVersion;
                    m_sqlCmd.ExecuteNonQuery();
                }
                while (oldVersion < BD_VARSION)
                {
                    //при изменении

                    switch (oldVersion)
                    {
                        case 1:

                            try
                            {
                                m_sqlCmd.CommandText = "ALTER TABLE ordini ADD COLUMN `_RIFSALON` TEXT ";
                                m_sqlCmd.ExecuteNonQuery();
                                m_sqlCmd.CommandText = "ALTER TABLE ordini ADD COLUMN `_RIFFABRICA` TEXT ";
                                m_sqlCmd.ExecuteNonQuery();
                                m_sqlCmd.CommandText = "ALTER TABLE ordini ADD COLUMN `SROK` TEXT ";
                                m_sqlCmd.ExecuteNonQuery();
                            }
                            catch
                            {

                                oldVersion = 1;
                            }
                            break;
                        case 2:

                            try
                            {
                                m_sqlCmd.CommandText = "ALTER TABLE ordini ADD COLUMN `SALON` TEXT ";
                                m_sqlCmd.ExecuteNonQuery();

                            }
                            catch
                            {
                                oldVersion = 2; //это чтобы убрать ошибки старые , для новых записей не надо
                            }
                            break;
                    }

                    //при изменении !КОНЕЦ
                    oldVersion++;
                    m_sqlCmd.CommandText = "PRAGMA user_version=" + oldVersion;
                    m_sqlCmd.ExecuteNonQuery();

                }






            }
            catch (SQLiteException ex)
            {

                MessageBox.Show("Error: " + ex.Message);
            }
            m_dbConn.Close();
            GC.Collect();



        }
        void select(bool first = false)
        {
            //   MessageBox.Show(nomer);
            if (!first)
                save_setting();


            SQLiteConnection m_dbConn = new SQLiteConnection();
            SQLiteCommand m_sqlCmd = new SQLiteCommand();


            m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
            m_dbConn.Open();
            m_sqlCmd.Connection = m_dbConn;


            m_sqlCmd.CommandText = "SELECT * FROM ordini order by nomer_zakaza DESC";

            List<ordini> spisok = new List<ordini>();
            var reader = m_sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                string[] date_split = reader["SROK"].ToString().Split('.');
                string date_sorted = "";
                if (date_split.Length == 3)
                {
                    date_sorted = date_split[2] + "." + date_split[1] + "." + date_split[0];
                }

                string[] salon_pre = reader["SALON"].ToString().Split(',');
                spisok.Add(new ordini
                {
                    file_path = reader["file_path"].ToString(),
                    FIO_klienta = reader["FIO"].ToString(),
                    nomer_zakaza = reader["nomer_zakaza"].ToString(),
                    Date_last = reader["last_upd"].ToString(),
                    manager_salons = reader["manager"].ToString(),
                    orderprice = reader["orderprice"].ToString().Trim(),
                    _RIFFABRICA = reader["_RIFFABRICA"].ToString(),
                    _RIFSALON = reader["_RIFSALON"].ToString(),
                    SROK = reader["SROK"].ToString(),
                    SALON = salon_pre.Length >= 3 ? salon_pre[1] + " " + salon_pre[2] : "",
                    date_sorted = date_sorted//,
                                             // papka_zakaza = PapkaZakaz(reader["nomer_zakaza"].ToString(), reader["FIO"].ToString())
                });
            }
            reader.Close();

            //


            //  lb1.ItemsSource = spisok;
            m_dbConn.Close();
            GC.Collect();


            viewSource1.Source = spisok;
            lb1.ItemsSource = viewSource1.View;
            viewSource1.View.Refresh();



            //выбор при открытии
            string evefile = "000000".Substring(0, 6 - nomer.Length) + nomer;
            ordini polucnxs = spisok.Find(x => x.nomer_zakaza.Equals(evefile));
            lb1.SelectedItem = polucnxs;



            visiblecolumns();

        }

        void visiblecolumns()
        {
            Settings1 ps = Settings1.Default;
            for (int i = 0; i < kolvo_stolb; i++)
            {

                if (i >= ps.spisotobrstolb.Length)
                {
                    ps.spisotobrstolb += "1";
                    ps.spisindex += ";" + (i);
                    ps.spiswidth += ";1";
                    ps.Save();
                }

                lb1.Columns[i].Visibility = ps.spisotobrstolb[i] == '1' ? Visibility.Visible : Visibility.Collapsed;
                lb1.Columns[i].DisplayIndex = Convert.ToInt32(ps.spisindex.Split(';')[i]);
                lb1.Columns[i].Width = new DataGridLength(double.Parse(ps.spiswidth.Split(';')[i], CultureInfo.InvariantCulture), DataGridLengthUnitType.Star);

            }



        }



        string PapkaZakaz(string numer, string fioclient)
        {

            string InPath = @"\\Win2008filesser\d\Основное производство\Заказы";


            if (fioclient != "")
                fioclient = " " + fioclient;
            string DT = numer[0].ToString(); // десятки тысяч
            string T = numer[1].ToString();  // тысячи
            string S = numer[2].ToString();  // сотни
            string Path = InPath + @"\" + DT + @"0000\" + DT + T + @"000\" + DT + T + S + @"00\"; // папка с фимилией
                                                                                                  // string Papka = ScanDir(Path, numer);
            if (Directory.Exists(Path + numer + fioclient))
            {
                return Path + numer + fioclient;
            }
            else
            {
                if (Directory.Exists(Path + numer))
                {
                    return Path + numer;
                }
                else
                {
                    return "";
                }
            }
        }


        public void updateTekZakaz()
        {
            object xamb = neqqqqq.getParam(neqqqqq.Ambiente, "GetObject", "XAMB");
            object info = neqqqqq.getParamG(xamb, "INFO");
            object info2 = neqqqqq.getParamG(info, "INFO");


            nomer = neqqqqq.getParam(info, "Numero").ToString();

            string evefile = "000000".Substring(0, 6 - nomer.Length) + nomer;


            SQLiteConnection m_dbConn = new SQLiteConnection();
            SQLiteCommand m_sqlCmd = new SQLiteCommand();

            m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
            m_dbConn.Open();
            m_sqlCmd.Connection = m_dbConn;


            // MessageBox.Show(FIO);
            string file_path_load1 = path_ordini + @"\" + evefile + ".eve";
            string time = File.GetLastWriteTime(file_path_load1).ToString("dd MMM HH:mm:ss");


            m_sqlCmd.CommandText = "SELECT * FROM ordini where nomer_zakaza ='" + evefile + "' limit 1";


            var reader = m_sqlCmd.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                i++;
            }
            reader.Close();






            string nomfile = file_path_load1.Split('\\').Last().Split('.').First();
            //neqqqqq.getParam(xamb, "carica", file_path_load1);


            string FIO = neqqqqq.getParam(info2, "Var", "CLI_1").ToString();
            string Manager = neqqqqq.getParam(info2, "Var", "Manager").ToString();
            string orderprice = neqqqqq.getParam(info2, "Var", "orderprice").ToString().Trim();
            string _RIFFABRICA = neqqqqq.getParam(info2, "Var", "_RIFFABRICA").ToString();
            string _RIFSALON = neqqqqq.getParam(info2, "Var", "_RIFSALON").ToString();
            string SROK = neqqqqq.getParam(info2, "Var", "SROK").ToString();
            string SALON = neqqqqq.getParam(info2, "Var", "SALON").ToString();


            if (i > 0)
            {

                m_sqlCmd.CommandText = "UPDATE ordini SET " +
                    "file_path='" + file_path_load1 + "', " +
                    "nomer_zakaza='" + nomfile + "', " +
                    "FIO='" + FIO + "', " +
                    "manager='" + Manager + "', " +
                    "orderprice='" + orderprice + "', " +
                    "_RIFFABRICA='" + _RIFFABRICA + "', " +
                    "_RIFSALON='" + _RIFSALON + "', " +
                    "SROK='" + SROK + "', " +
                        "SALON='" + SALON + "' " +
                    " where nomer_zakaza ='" + evefile + "'";
                m_sqlCmd.ExecuteNonQuery();

            }
            else
            {

                // object xamb = neqqqqq.getParam(neqqqqq.Ambiente, "GetObject", "XAMB");
                // neqqqqq.getParamI(neqqqqq.xamb, "salva");//сохраним
                if (File.Exists(file_path_load1))
                {
                    m_sqlCmd.CommandText = "INSERT INTO ordini (file_path, nomer_zakaza, FIO, manager, orderprice, _RIFFABRICA, _RIFSALON, SROK) " +
                        "VALUES ('" + file_path_load1 + "', '" + nomfile + "','" + FIO + "','" + Manager + "', '" + orderprice + "', '" + _RIFFABRICA + "', '" + _RIFSALON + "', '" + SROK + "')";
                    m_sqlCmd.ExecuteNonQuery();
                }

            }

            m_dbConn.Close();
            GC.Collect();

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Closinger();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
           "Сейчас начнётся перекачка заказов на сервер, операция может выполняться длительное время, в зависимости от количества ваших заказов, нажмите \"OK\", чтобы начать",
          "Внимание",
           MessageBoxButton.OKCancel,
           MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                neqqqqq.copy_to_share();
                btn_share.Visibility = Visibility.Collapsed;
                MessageBox.Show("Готово");
                select();

            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            t1.Text = "";
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            t2.Text = "";
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            t3.Text = "";
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            t1.Text = "";
            t2.Text = "";
            t3.Text = "";
            t4.Text = "";
            t5.Text = "";
        }

        private void t1_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            timer.Stop();
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            viewSource1.View.Refresh();
            timer.Stop();
        }

        private void lb1_Loaded(object sender, RoutedEventArgs e)
        {

            //     lb1.ItemsSource = spisok;

            viewSource1.Filter += viewSource_Filter1;

        }


        void viewSource_Filter1(object sender, FilterEventArgs e)
        {

            try
            {
                e.Accepted = false;
                if (((ordini)e.Item).nomer_zakaza.ToLower().IndexOf(t1.Text.ToLower()) >= 0)
                {
                    if (((ordini)e.Item).FIO_klienta.ToLower().IndexOf(t2.Text.ToLower()) >= 0)
                    {
                        if (((ordini)e.Item).manager_salons.ToLower().IndexOf(t3.Text.ToLower()) >= 0)
                        {
                            if (((ordini)e.Item)._RIFSALON.ToLower().IndexOf(t4.Text.ToLower()) >= 0)
                            {
                                if (((ordini)e.Item)._RIFFABRICA.ToLower().IndexOf(t5.Text.ToLower()) >= 0)
                                {
                                    e.Accepted = true;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            ordini delete = lb1.SelectedItem as ordini;
            if (MessageBox.Show(
    "Удалить заказ " + delete.nomer_zakaza + "?",
    "Внимание",
    MessageBoxButton.OKCancel,
    MessageBoxImage.Warning) == MessageBoxResult.OK)
            {


                SQLiteConnection m_dbConn = new SQLiteConnection();
                SQLiteCommand m_sqlCmd = new SQLiteCommand();

                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;




                m_sqlCmd.CommandText = "DELETE FROM ordini where nomer_zakaza='" + delete.nomer_zakaza + "'";
                m_sqlCmd.ExecuteNonQuery();

                m_dbConn.Close();
                GC.Collect();

                File.Delete(delete.file_path);
                MessageBox.Show("Готово");
                select();
            }

        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {

        }

        private void lb1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lb1.SelectedItem != null)
            {
                string pathtmp = path_ordini + @"\";
                //  MessageBox.Show(pathtmp);
                ordini selected_item = (lb1.SelectedItem as ordini);
                object xamb = neqqqqq.getParam(neqqqqq.Ambiente, "GetObject", "XAMB");
                object engine = neqqqqq.getParam(neqqqqq.Ambiente, "GetObject", "ENGINE");
                neqqqqq.getParam(xamb, "carica", selected_item.file_path);
                string GetFileBitmap = neqqqqq.getParam(xamb, "GetFileBitmap", pathtmp + selected_item.nomer_zakaza + ".DRG1").ToString();


                if (GetFileBitmap.ToUpper() == "TRUE")
                {
                    object imgget = neqqqqq.getParam(neqqqqq.Ambiente, "GetObject", "DauImg");
                    object GetPicture = neqqqqq.getParam(engine, "GetPicture", pathtmp + selected_item.nomer_zakaza + ".DRG1", "0", "0");
                    imgget.GetType().InvokeMember("SetPicture", BindingFlags.InvokeMethod, null, imgget, new object[] { GetPicture, "0" });
                    neqqqqq.getParam(imgget, "SaveImage", pathtmp + selected_item.nomer_zakaza + ".JPG", "1");
                    img.Source = new BitmapImage(new Uri(pathtmp + selected_item.nomer_zakaza + ".JPG"));

                }
                else
                    img.Source = new BitmapImage(new Uri(@"/TreeCadN;component/Foto/nofoto.jpg", UriKind.RelativeOrAbsolute));
            }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            object xamb = neqqqqq.getParam(neqqqqq.Ambiente, "GetObject", "XAMB");
            string GetFileBitmap = neqqqqq.getParam(xamb, "GetFileBitmap", @"C:\\PREV.DRG1").ToString();
            if (GetFileBitmap.ToUpper() == "TRUE")
            {
                //  string GetPicture = neqqqqq.getParam(neqqqqq.engine, "GetPicture", @"C:\\PREV.DRG1", "0", "0").ToString();


                object GetPicture = neqqqqq.engine.GetType().InvokeMember("GetPicture", BindingFlags.InvokeMethod, null, neqqqqq.engine, new object[] { @"C:\PREV.DRG1", "0", "0" });

                //        MyImage = new Bitmap("d:\\PREV.jpg");
                //  img.Image = (Image)MyImage;
                img.Source = (BitmapImage)(GetPicture);// new BitmapImage(new Uri("C:\\PREV.DRG1"));
                MessageBox.Show("123" + GetFileBitmap);
            }
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            t4.Text = "";
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            t5.Text = "";
        }

        private void lb1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Closinger();
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Closinger();
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ordini duplicate = lb1.SelectedItem as ordini;
            if (MessageBox.Show(
    "Дублировать заказ?",
    "Внимание",
    MessageBoxButton.OKCancel,
    MessageBoxImage.Warning) == MessageBoxResult.OK)
            {













                object xamb = neqqqqq.getParam(neqqqqq.Ambiente, "GetObject", "XAMB");
                object info = neqqqqq.getParamG(xamb, "INFO");
                object info2 = neqqqqq.getParamG(info, "INFO");
                neqqqqq.getParam(xamb, "carica", duplicate.file_path);
                string newnum = neqqqqq.getParamI(info, "NuovoNumeroOrdine").ToString();
                neqqqqq.setParamP(info, "Numero", newnum);
                neqqqqq.getParam(info2, "Add", "_NOMEFILEPARETI", duplicate.nomer_zakaza);
                neqqqqq.getParamI(xamb, "salva");//сохраним

                SQLiteConnection m_dbConn = new SQLiteConnection();
                SQLiteCommand m_sqlCmd = new SQLiteCommand();
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;



                string pattern = "000000";
                string nom_form = pattern.Remove(0, newnum.Length) + newnum;
                m_sqlCmd.CommandText = "INSERT OR IGNORE INTO ordini(file_path, nomer_zakaza) VALUES('" + path_ordini + "\\" + nom_form + ".eve', '" + nom_form + "')";
                m_sqlCmd.ExecuteNonQuery();

                m_dbConn.Close();
                GC.Collect();

                MessageBox.Show("Новый заказ имеет номер " + nom_form);


                select();

            }
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            (new srtting(neqqqqq, path_ordini)).ShowDialog();

            select();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            save_setting();
        }
        void save_setting()
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



            //запомним порядок колонок
            string spisindex = "";
            string spiswidth = "";

            foreach (var sad in lb1.Columns)
            {
                spisindex += sad.DisplayIndex + ";";
                spiswidth += (sad.Width.ToString().Equals("*") ? "1" : sad.Width.ToString().Trim('*')) + ";";
            }
            spisindex = spisindex.Trim(';');
            spiswidth = spiswidth.Trim(';');
            ps.spiswidth = spiswidth;
            ps.spisindex = spisindex;
            ps.Save();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        void Closinger()
        {



            this.Hide();




            if (lb1.SelectedIndex != -1)
            {
                object xamb = neqqqqq.getParam(neqqqqq.Ambiente, "GetObject", "XAMB");

                neqqqqq.getParam(xamb, "carica", (lb1.SelectedItem as ordini).file_path);
                neqqqqq.getParamI(neqqqqq.Ambiente, "bcarica");

            }



            Close();

        }






    }


    class ordini
    {
        public string orderprice { get; set; }

        public string id_zakaza { get; set; }
        public string nomer_zakaza { get; set; }
        public string FIO_klienta { get; set; }
        public string manager_salons { get; set; }
        public string Summa { get; set; }
        public string Kurs { get; set; }
        public string Koef_po_meb { get; set; }
        public string Koef_po_aks { get; set; }
        public string Date_last { get; set; }
        public string file_path { get; set; }
        public string _RIFSALON { get; set; }
        public string _RIFFABRICA { get; set; }
        public string SROK { get; set; }
        public string date_sorted { get; set; }
        public string SALON { get; set; }
        public string papka_zakaza { get; set; }

    }



}
