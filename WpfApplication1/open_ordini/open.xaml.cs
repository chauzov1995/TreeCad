using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace TreeCadN.open_ordini
{
    /// <summary>
    /// Логика взаимодействия для open.xaml
    /// </summary>
    public partial class open : Window
    {


        String dbFileName;
        string FIO, nomer, path;
        string path_ordini;
        neqweqe neqqqqq;
        CollectionViewSource viewSource1 = new CollectionViewSource();

        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        public open(neqweqe _neqqqqq, string _FIO, string _nomer, string _path, string _path_ordini)
        {
            InitializeComponent();
            FIO = _FIO;
            nomer = _nomer;
            path = _path;
            path_ordini = _path_ordini;
            this.neqqqqq = _neqqqqq;
     
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
         
            string percorsoordini = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\_ecadpro\ordini";
            string[] files = Directory.GetFiles(percorsoordini, "*.eve", SearchOption.TopDirectoryOnly);
            if (files.Length == 0 || percorsoordini == (Environment.CurrentDirectory + @"\_ecadpro\ordini"))
            {
                btn_share.Visibility = Visibility.Collapsed;
            }

       
            loadpage();




         




            firstload();
        

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

        //    lb1.SelectedIndex = ps.last_vibor;



        }

        void firstload()
        {
            Settings1 dds = Settings1.Default;





            SQLiteConnection m_dbConn = new SQLiteConnection();
            SQLiteCommand m_sqlCmd = new SQLiteCommand();

            dbFileName = path_ordini + @"\sample.sqlite";
            //    LB.Content = dbFileName;
            //   path_ordini = dds.path;




            if (!File.Exists(dbFileName))
                SQLiteConnection.CreateFile(dbFileName);
            //   MessageBox.Show(dbFileName);
            try
            {
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;

                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS ordini (id INTEGER PRIMARY KEY AUTOINCREMENT, file_path TEXT, FIO TEXT, nomer_zakaza TEXT UNIQUE, last_upd TEXT, manager TEXT)";
                m_sqlCmd.ExecuteNonQuery();

            }
            catch (SQLiteException ex)
            {

                MessageBox.Show("Error: " + ex.Message);
            }
            m_dbConn.Close();
            GC.Collect();
                      xapis_nev();
  
            select();
        }
        void select()
        {


            SQLiteConnection m_dbConn = new SQLiteConnection();
            SQLiteCommand m_sqlCmd = new SQLiteCommand();


            m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
            m_dbConn.Open();
            m_sqlCmd.Connection = m_dbConn;

        
            m_sqlCmd.CommandText = "SELECT id, file_path, FIO, nomer_zakaza, last_upd, manager FROM ordini order by nomer_zakaza DESC";

            List<ordini> spisok = new List<ordini>();
            var reader = m_sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                spisok.Add(new ordini
                {
                    file_path = reader["file_path"].ToString(),
                    FIO_klienta = reader["FIO"].ToString(),
                    nomer_zakaza = reader["nomer_zakaza"].ToString(),
                    Date_last = reader["last_upd"].ToString(),
                    manager_salons= reader["manager"].ToString(),
                    
                });
            }


            //


            //  lb1.ItemsSource = spisok;
            m_dbConn.Close();
            GC.Collect();


            viewSource1.Source = spisok;
            lb1.ItemsSource = viewSource1.View;
            viewSource1.View.Refresh();
        }



        void xapis_nev()
        {
            string evefile = "000000".Substring(0, 6 - nomer.Length) + nomer + ".eve";


            SQLiteConnection m_dbConn = new SQLiteConnection();
            SQLiteCommand m_sqlCmd = new SQLiteCommand();

            m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
            m_dbConn.Open();
            m_sqlCmd.Connection = m_dbConn;


            // MessageBox.Show(FIO);
            string time = File.GetLastWriteTime(path_ordini + @"\" + evefile).ToString("dd MMM HH:mm:ss");


            m_sqlCmd.CommandText = "UPDATE ordini SET FIO='" + FIO + "', last_upd='" + time + "' where nomer_zakaza ='" + evefile + "'";
            m_sqlCmd.ExecuteNonQuery();


            m_dbConn.Close();
            GC.Collect();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
        "Нажмите \"ОК\", чтобы подгрузить заказы которых нет в списке",
       "Внимание",
        MessageBoxButton.OKCancel,
        MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                SQLiteConnection m_dbConn = new SQLiteConnection();
                SQLiteCommand m_sqlCmd = new SQLiteCommand();

                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;


                var files = Directory.GetFiles(path_ordini, "*.eve");
                foreach (string file in files)
                {
                    string mass = file.Split('\\').Last().Split('.').First();
                    m_sqlCmd.CommandText = "INSERT OR IGNORE INTO ordini (file_path, nomer_zakaza) VALUES ('" + file + "', '" + mass + "')";
                    m_sqlCmd.ExecuteNonQuery();
                }
                MessageBox.Show("Готово");

                m_dbConn.Close();
                GC.Collect();

                firstload();
               
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            neqqqqq.getParam(neqqqqq.xamb, "carica", (lb1.SelectedItem as ordini).file_path);
            neqqqqq.getParamI(neqqqqq.Ambiente, "bcarica");
            Close();
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
                firstload();
              
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
            viewSource1.SortDescriptions.Add(new SortDescription("sort", ListSortDirection.Ascending));

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
                            e.Accepted = true;
                        }
                    }
                }
            }
            catch { }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show(
   "Удалить выбранный заказ",
  "Внимание",
   MessageBoxButton.OKCancel,
   MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                ordini delete = lb1.SelectedItem as ordini;

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
                firstload();
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //  ddd
            ordini duplicate = lb1.SelectedItem as ordini;

            object info = neqqqqq.getParamG(neqqqqq.xamb, "INFO");
            object info2 = neqqqqq.getParamG(info, "INFO");
            neqqqqq.getParam(neqqqqq.xamb, "carica", duplicate.file_path);
            string newnum = neqqqqq.getParamI(info, "NuovoNumeroOrdine").ToString();
            neqqqqq.setParamP(info, "Numero", newnum);
            neqqqqq.getParam(info2, "Add", "_NOMEFILEPARETI", duplicate.nomer_zakaza);
            neqqqqq.getParamI(neqqqqq.xamb, "salva");//сохраним

            SQLiteConnection m_dbConn = new SQLiteConnection();
            SQLiteCommand m_sqlCmd = new SQLiteCommand();
            m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
            m_dbConn.Open();
            m_sqlCmd.Connection = m_dbConn;



            string pattern = "000000";
            string nom_form = pattern.Remove(0, newnum.Length) + newnum;
            m_sqlCmd.CommandText = "INSERT OR IGNORE INTO ordini(file_path, nomer_zakaza, FIO, manager) VALUES('" + duplicate.file_path + "', '" + nom_form + "', '" + duplicate.FIO_klienta + "', '"+ duplicate.manager_salons + "')";
            m_sqlCmd.ExecuteNonQuery();

            m_dbConn.Close();
            GC.Collect();

            MessageBox.Show("Готово");


            firstload();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings1 ps = Settings1.Default;
            ps.Top = this.Top;
            ps.Left = this.Left;
            ps.last_vibor = ( lb1.SelectedItem as ordini).nomer_zakaza;

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
        }




    }


    class ordini
    {
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

    }



}
