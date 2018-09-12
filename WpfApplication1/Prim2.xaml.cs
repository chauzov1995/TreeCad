using System;
using System.Collections.Generic;
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
        public string text_otvet, PRIM, AUTO;
        bool zakrit_ok = false;
        static BD_Connect BD = new BD_Connect();
        List<MyTable> result = new List<MyTable>();
        public Prim2(string path, string text)
        {
            InitializeComponent();
            BD.path = path; //укажем файл бд
            this.text_otvet = text.Trim();

            //PRIM=a2131231232321;AVTO=asdasdasda
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
                AUTO = (text_otvet.Split(';'))[1].Substring(5);
            }
            catch
            {
                AUTO = "";
            }
       


            OleDbDataReader reader = BD.conn("SELECT STCommentD.Name, STCommentD.ID, STCommentDchasto.Chastota FROM STCommentD LEFT JOIN STCommentDchasto ON STCommentD.ID = STCommentDchasto.IDComment ORDER BY STCommentDchasto.Chastota DESC");
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
            tb2.Focus();

            string t = PRIM.Replace('#', '"').Replace('$', ';').Replace('@', ',').Replace('№', '/').Replace('^', ' ').Replace('|', ';');
            
            tb1.Text = t;
            tb3.Text = AUTO;
            tb1.CaretIndex = tb1.Text.Length;
        }


        public static void Count(object x)
        {
            try
            {
                var asdsa = BD.conn("UPDATE STCommentDchasto SET Chastota = (Chastota+1) WHERE IDComment = " + x);
                if (asdsa.RecordsAffected == 0)
                {
                    BD.conn("INSERT INTO STCommentDchasto (IDComment, Chastota) VALUES (" + x + ",1) ");
                }
            }
            catch
            {
                //   MessageBox.Show(x.ToString());
            }

        }



        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            grid.CanUserSortColumns = true;
         //   grid.ItemsSource = result;

            viewSource = new CollectionViewSource();
            viewSource.Source = result;
            viewSource.Filter += viewSource_Filter;
            grid.ItemsSource = viewSource.View;
        }
        CollectionViewSource viewSource;
        void viewSource_Filter(object sender, FilterEventArgs e)
        {

            e.Accepted = ((MyTable)e.Item).Name.ToLower().IndexOf(tb2.Text.ToLower()) >= 0;

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
                BD.conn("DELETE FROM  STCommentDchasto");
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
            } if (ps.SizeToContent4 == 1)
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





            //pfrhsnbt
            if (zakrit_ok)
            {
                string t = tb1.Text.Replace('\'', ' ').Replace('"', '#').Replace(';', '|').Replace(',', '@').Replace('/', '№');
                //    MessageBox.Show(t);
                this.text_otvet = @"PRIM=" + t.Trim() + @";AVTO=" + tb3.Text;

            }
            else
            {
                if (this.text_otvet != tb1.Text)
                {
                    if (MessageBox.Show(
        "Вы изменили примечания, хотели бы Вы их сохранить?",
        "Предупреждение",
        MessageBoxButton.YesNo,
        MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        string t = tb1.Text.Replace('\'', ' ').Replace('"', '#').Replace(';', '|').Replace(',', '@').Replace('/', '№');
                        //    MessageBox.Show(t);
                        this.text_otvet = @"PRIM=" + t.Trim() + @";AVTO=" + tb3.Text;
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
                tb1.Focus();

                Thread myThread = new Thread(new ParameterizedThreadStart(Count));
                myThread.Start(path.ID);

            }
            catch
            {
            }

        }

        private void grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                select_grid();
            }
            if (e.Key == Key.Tab){
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










}
