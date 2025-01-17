﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Prim.xaml
    /// </summary>
    public partial class Prim2 : Window
    {

        List<btn_spis> spisbtn = new List<btn_spis>();
        public string text_otvet, PRIM, AUTO, EDIT;
        bool zakrit_ok = false;
        static BD_Connect BD = new BD_Connect();
        List<MyTable> result = new List<MyTable>();
        string TipShk;
        public Prim2(string path, string text, string TipShk)
        {
            InitializeComponent();
            this.TipShk = TipShk;
            BD.path = path; //укажем файл бд
            this.text_otvet = text.Trim();
           // MessageBox.Show(this.text_otvet);
            //PRIM=a2131231232321;AVTO=asdasdasda//


            if (text_otvet.Contains("PRIM=") && text_otvet.Contains(";AVTO=") && text_otvet.Contains(";EDIT="))
            {

                try
                {
                    PRIM = (text_otvet.Split(';'))[0].Substring(5);
                }
                catch
                {
                    PRIM = "";
                }
                try
                {
                    AUTO = (text_otvet.Split(';'))[1].Substring(5).Trim('^');
                }
                catch
                {
                    AUTO = "";
                }
                try
                {
                    EDIT = (text_otvet.Split(';'))[2].Substring(5);
                }
                catch
                {
                    EDIT = "0";
                }
            }
            else
            {
                PRIM = text_otvet;
                AUTO = "";
                EDIT = "0";
            }


            int izmfocus=-1;
            if (PRIM=="" && TipShk=="574")
            {
                PRIM = "Глубина мм.";
                izmfocus = 8;
            }



            if (EDIT.Equals("1")) {
                tb3.IsEnabled = true;
                cb1.IsChecked = true;
            } else {
                tb3.IsEnabled = false;
                cb1.IsChecked = false;
            }
            OleDbDataReader reader = BD.execute("SELECT STCommentD.Name, STCommentD.ID, STCommentDchasto.Chastota FROM STCommentD LEFT JOIN STCommentDchasto ON STCommentD.ID = STCommentDchasto.IDComment ORDER BY STCommentDchasto.Chastota DESC");
            while (reader.Read())
            {
                int Chasto = 0;
                if (reader["Chastota"].ToString() != "") Chasto = Convert.ToInt32(reader["Chastota"]);
                result.Add(new MyTable()
                {
                    ID = reader["ID"].ToString(),
                    Name = reader["Name"].ToString(),
                    Chasto = Chasto
                });

            }

            loadpage();//загрузка полож окна
            proverka_uhoda_za_granicu(); //проверка ухода за границу
           

            string t = PRIM.Replace('#', '"').Replace('$', ';').Replace('@', ',').Replace('№', '/').Replace('^', ' ').Replace('|', ';');

            tb1.Text = t;
            tb3.Text = AUTO;

            splitauto();



            //  lbbutton.ItemsSource = spisbtn;

            if (izmfocus>=0)
            {
                tb1.Focus();
                tb1.CaretIndex = izmfocus;
               
            }
            else
            {
                tb2.Focus();
                tb1.CaretIndex = tb1.Text.Length;
               
            }
            lb2.Content = tb1.Text.Length + " символа(ов)";
        }

        void splitauto()
        {
            
            spisbtn.Clear();
            lbbutton.Items.Clear();
            if (!AUTO.Equals(""))
            {
                string[] split_AUTO = AUTO.Split('^');

              

                foreach (string elem in split_AUTO)
                {
                    var splitelemrech = elem.Split('#');
                    var elemnew = new btn_spis() { name = splitelemrech.Length == 2 ? splitelemrech[1] : elem };
                    spisbtn.Add(elemnew);
                    lbbutton.Items.Add(elemnew);

                }
            }
        }

        public static void Count(object x)
        {
            try
            {
                var asdsa = BD.execute("UPDATE STCommentDchasto SET Chastota = (Chastota+1) WHERE IDComment = " + x);
                if (asdsa.RecordsAffected == 0)
                {
                    BD.execute("INSERT INTO STCommentDchasto (IDComment, Chastota) VALUES (" + x + ",1) ");
                }
            }
            catch
            {
                //   MessageBox.Show(x.ToString());
            }

        }




        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // grid.CanUserSortColumns = true;


            viewSource = new CollectionViewSource();
            viewSource.Source = result;
            viewSource.Filter += viewSource_Filter;
            grid.ItemsSource = viewSource.View;

            log.Add("успешно загружен датагрид");
        }
        CollectionViewSource viewSource;
        void viewSource_Filter(object sender, FilterEventArgs e)
        {

            e.Accepted = ((MyTable)e.Item).Name.ToLower().IndexOf(tb2.Text.ToLower()) >= 0;

        }


        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            string Tag = (sender as GridViewColumnHeader).Tag.ToString();

            if (viewSource.SortDescriptions.Count != 0)
            {
                if (viewSource.SortDescriptions.First() == new SortDescription(Tag, ListSortDirection.Descending))
                {
                    viewSource.SortDescriptions.Clear();
                    viewSource.SortDescriptions.Add(new SortDescription(Tag, ListSortDirection.Ascending));
                }
                else
                {
                    viewSource.SortDescriptions.Clear();
                    viewSource.SortDescriptions.Add(new SortDescription(Tag, ListSortDirection.Descending));
                }
            }
            else
            {
                viewSource.SortDescriptions.Add(new SortDescription(Tag, ListSortDirection.Ascending));
            }




        }

        private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gr1.Width = e.NewSize.Width - 100;
        }

        private void grid_GotFocus(object sender, RoutedEventArgs e)
        {

            if (grid.SelectedIndex == -1)
            {
                grid.SelectedIndex = 0;
            }
        }


        private void b1_Click(object sender, RoutedEventArgs e)
        {
            zakrit_ok = true;
            Close();


        }
        private void otvet(bool zakrit_ok)
        {




        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
"Очистить список часто используемых примечайний?",
"Предупреждение",
MessageBoxButton.YesNo,
MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                BD.execute("DELETE FROM  STCommentDchasto");
                MessageBox.Show("Список успешно очищен, перезапустите окно");
            }


        }
        void loadpage()
        {
            Properties.Settings ps = Properties.Settings.Default;
            if (ps.Top4 == -100)
            {
                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            }
            else
            {
                this.Top = ps.Top4;
                this.Left = ps.Left4;
            }
            if (ps.SizeToContent4 == 1)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.Width = ps.Width4;
                this.Height = ps.Height4;
            }


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings ps = Properties.Settings.Default;
            ps.Top4 = this.Top;
            ps.Left4 = this.Left;


            if (this.WindowState == WindowState.Maximized)
            {
                ps.SizeToContent4 = 1;
            }
            else
            {
                ps.SizeToContent4 = 0;
                ps.Width4 = this.Width;
                ps.Height4 = this.Height;
            }



            ps.Save();


            string autoptim_otvet = "";
            foreach(btn_spis elem in lbbutton.Items)
            {
                autoptim_otvet+=(elem as btn_spis).name+"^";

            }
            autoptim_otvet = autoptim_otvet.Trim('^');
          //  EDIT = "1";

            string t = @"PRIM=" + tb1.Text.Replace('\'', ' ').Replace('"', '#').Replace(';', '|').Replace(',', '@').Replace('/', '№').Replace(':', ' ').Trim() + @";AVTO=" + autoptim_otvet + @";EDIT=" + EDIT;
           // MessageBox.Show(t);
            //pfrhsnbt
            if (zakrit_ok)
            {
              
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
        MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        this.text_otvet = t;
                    }
                }

            }



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



        private void tb2_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewSource.View.Refresh();
        }

        private void b4_Click(object sender, RoutedEventArgs e)
        {
            tb2.Text = "";
            tb2.Focus();
        }

        private void tb1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                //   zakrit_ok = true;
                //   Close();

            }
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            select_grid();
        }
        void select_grid()
        {

            try
            {
                MyTable path = grid.SelectedItem as MyTable;
                tb1.SelectedText = path.Name.ToString();
                tb1.CaretIndex = tb1.SelectionLength + tb1.SelectionStart;
                // tb1.Focus();

                Thread myThread = new Thread(new ParameterizedThreadStart(Count));
                myThread.Start(path.ID);
                tb2.Focus();

            }
            catch
            {
                MessageBox.Show("err");
            }

        }

        private void tb3_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cb1_Checked(object sender, RoutedEventArgs e)
        {

        //    lbbutton.IsEnabled = true;
            rectang.Visibility = Visibility.Collapsed;
            EDIT = "1";

        }

     

        private void Lbbutton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            try
            {
                if (lbbutton.SelectedItem != null)
                {


                    string text = (lbbutton.SelectedItem as btn_spis).name;
                    //    lbbutton.Items.Remove(lbbutton.SelectedItem);

                    //   lbbutton.ItemsSource = null;
                    //  lbbutton.ItemsSource = spisbtn;
                    tb1.SelectedText = text;
                    tb1.CaretIndex = tb1.SelectionLength + tb1.SelectionStart;
                    // tb1.Focus();

                  
                    lbbutton.Items.Remove(lbbutton.SelectedItem);
                    tb1.Focus();
                    // lbbutton.UnselectAll();
                }
            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.Message);
            }


        }

        private void tb1_TextChanged(object sender, TextChangedEventArgs e)
        {
            lb2.Content =tb1.Text.Length+ " символа(ов)";
        }

        private void cb1_Unchecked(object sender, RoutedEventArgs e)
        {
            //    lbbutton.IsEnabled = false;
            rectang.Visibility = Visibility.Visible;
            EDIT = "0";
            splitauto();
        }

        private void grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                select_grid();
            }
            if (e.Key == Key.Tab)
            {
                // tb1.Focus();
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                zakrit_ok = true;
                Close();
            }
        }


    }


    class btn_spis
    {
        public string id { get; set; }
        public bool vibran { get; set; } = true;
        public string imagebg { get; set; } = "/TreeCadN;component/Foto/galka.png";
        public string name { get; set; }

        public void setvibran(bool galka,bool reload=false)
        {
            this.vibran = galka;
            if (reload)
            {
                this.imagebg = "/TreeCadN;component/Foto/reload.png";
            }
            else
            {
                this.imagebg = galka ? "/TreeCadN;component/Foto/galka.png" : "/TreeCadN;component/Foto/crest.png";
            }
        }
      

    }




}
