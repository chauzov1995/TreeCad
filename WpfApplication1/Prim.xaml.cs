using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public partial class Prim : Window
    {
        public string text_otvet;
        bool zakrit_ok = false;
        static BD_Connect BD = new BD_Connect();
        List<MyTable> result = new List<MyTable>();
        public Prim(string path, string text)
        {
            InitializeComponent();




            BD.path = path; //укажем файл бд
            this.text_otvet = text.Trim();




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
            log.Add("Успешно положение окна");
            proverka_uhoda_za_granicu(); //проверка ухода за границу
            tb2.Focus();

            string t = text.Replace('#', '"').Replace('$', ';').Replace('@', ',').Replace('№', '/').Replace('^', ' ').Replace('|', ';');

            tb1.Text = t;

            tb1.CaretIndex = tb1.Text.Length;
            log.Add("Успешно включен диалог");
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




        private void b1_Click(object sender, RoutedEventArgs e)
        {
            zakrit_ok = true;
            Close();


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





            //pfrhsnbt

            string t = tb1.Text.Replace('\'', ' ').Replace('"', '#').Replace(';', '|').Replace(',', '@').Replace('/', '№');


            if (zakrit_ok)
            {
                //    MessageBox.Show(t);
                this.text_otvet = t;

            }
            else
            {
                if (this.text_otvet != t)
                {
                    var ms = MessageBox.Show(
        "Вы изменили примечания, хотели бы Вы их сохранить?",
        "Предупреждение",
        MessageBoxButton.YesNoCancel,
        MessageBoxImage.Warning);

                    if (ms == MessageBoxResult.Yes)
                    {
                        //    MessageBox.Show(t);
                        this.text_otvet = t;
                    }
                    if (ms == MessageBoxResult.Cancel)
                    {
                        //    MessageBox.Show(t);
                        e.Cancel = true;
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

        private void grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                select_grid();
            }

        }



        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                zakrit_ok = true;
                Close();
            }
            if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                tb2.Focus();
            }

        }

        private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gr1.Width = e.NewSize.Width - 100;
        }

        private void tb2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                grid.Focus();
            }
        }

        private void grid_GotFocus(object sender, RoutedEventArgs e)
        {

            if (grid.SelectedIndex == -1)
            {
                grid.SelectedIndex = 0;
            }
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



        private void tb2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tb2.Text))
            {
                tb2.SelectionStart = 0;
                tb2.SelectionLength = tb2.Text.Length;
            }
        }
    }

    class MyTable
    {

        public string ID { get; set; }
        public string Name { get; set; }
        public int Chasto { get; set; }
    }



    public class BD_Connect
    {

        public string path;
        public OleDbDataReader conn(string zapros)
        {
            try
            {
                log.Add("Путь к бд- " + path);
                log.Add("строка подключ к бд- " + @"Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=" + path + @";Mode=Share Deny None;Jet OLEDB:System database=\System.mdw;");
                OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=" + path + @";Mode=Share Deny None;Jet OLEDB:System database=\System.mdw;");//подключаемся к базе
                OleDbCommand cmd = new OleDbCommand();//инициализируем запрос
                cmd.Connection = conn;//подключаемся к бд
                conn.Open();//открываем соединение
                cmd.CommandText = (zapros);
                OleDbDataReader reader = cmd.ExecuteReader();//выполняем запрос

                return reader;
            }
            catch (Exception exx)
            {
                log.Add(exx.Message);
                return null;
            }
        }


    }



    public class Phone
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public int Price { get; set; }
    }


}
