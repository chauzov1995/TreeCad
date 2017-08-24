using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
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

namespace TreeCadN.kommunikacii
{
    /// <summary>
    /// Логика взаимодействия для kommunikacii.xaml
    /// </summary>
    public partial class kommunikacii_main : Window
    {
        public string pathBD = "";
        public string selectedModel = "";
        BD_Connect BD = new BD_Connect();
        int lb1_selitem, lb4_selitem;


        public kommunikacii_main(string pathBD)
        {
            InitializeComponent();

            lb4.Visibility = Visibility.Collapsed;
            lb1.Visibility = Visibility.Collapsed;
            stpan.Visibility = Visibility.Collapsed;
            btnserv.Visibility = Visibility.Collapsed;

            this.pathBD = pathBD;
            BD.path = pathBD; //укажем файл бд
            loadpage();

            lb2lb3napolnene();



        }


        void lb2lb3napolnene()
        {
            //загрузка дынных для сервера

            var json = (new web_zapros()).load("load_categor");

            List<treespis> server = JsonConvert.DeserializeObject<List<treespis>>(json);
            lb3.ItemsSource = (server);
            peremestitb_serv.ItemsSource = (server);

            List<treespis> polzovat = new List<treespis>();


            var reader = BD.conn("SELECT id, nazv FROM `import3ds_category` order by nazv ASC");
            while (reader.Read())
            {


                polzovat.Add(new treespis()
                {
                    id = reader["id"].ToString(),
                    path = "P",
                    name = reader["nazv"].ToString(),
                    type = "P"
                });

            }



            lb2.ItemsSource = (polzovat);
            peremestitb.ItemsSource = (polzovat);



        }



        void lb1_napolnenie()//наполнение листбокса
        {
            treespis Treespis = lb2.SelectedItem as treespis;

            string path = Treespis.path;



            List<Models3d> modeli = new List<Models3d>();



            var reader = BD.conn("SELECT id, nazv, `path3ds`, `pathjpg`, `pathjpgugo`, `x`, `y`, `z`, category FROM `import3ds` WHERE category='" + Treespis.id + "'");
            while (reader.Read())
            {
                string file_split = reader["path3ds"].ToString().Split('\\').Last();

                modeli.Add(new Models3d
                {
                    id = reader["id"].ToString(),
                    path = reader["path3ds"].ToString(),
                    name = reader["nazv"].ToString(),
                    jpg_path = reader["pathjpg"].ToString(),
                    jpg_ugo = reader["pathjpgugo"].ToString(),
                    x = reader["x"].ToString(),
                    y = reader["y"].ToString(),
                    z = reader["z"].ToString(),
                    category = reader["category"].ToString(),

                    type = "P",
                });

            }


            lb1.ItemsSource = modeli;

            try
            {
                object selitem = lb1.Items[lb1_selitem];


                lb1.ScrollIntoView(selitem);
                lb1.SelectedItem = (selitem);
            }
            catch
            {
                lb1.ScrollIntoView(null);
                lb1.SelectedItem = (null);
            }

        }
        void lb4_napolnenie()//наполнение листбокса
        {
            treespis Treespis = lb3.SelectedItem as treespis;
            string path = Treespis.path;


            List<Models3d> modeli = new List<Models3d>();


            var json = (new web_zapros()).load("load_object", "category=" + Treespis.id);

            modeli = JsonConvert.DeserializeObject<List<Models3d>>(json);



            lb4.ItemsSource = modeli;


            try
            {
                object selitem = lb4.Items[lb4_selitem];


                lb4.ScrollIntoView(selitem);
                lb4.SelectedItem = (selitem);
            }
            catch
            {
                lb4.ScrollIntoView(null);
                lb4.SelectedItem = (null);
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            dob_new3ds();
        }
        void dob_new3ds()
        {
            if (lb2.SelectedIndex > -1)
            {
                var model = (lb2.SelectedItem as treespis).id;
                kommunikacii_dial_imp dial = new kommunikacii_dial_imp(pathBD, null, model);
                dial.ShowDialog();
                lb1_napolnenie();
            }
            else
            {
                MessageBox.Show("Сначала выберите категорию");
            }
            //  lb1_napolnenie();



        }

        private void lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb1.SelectedItem != null)
            {
                //  selectedModel = (lb1.SelectedValue as Models3d).path;
                var file = lb1.SelectedValue as Models3d;
                selectedModel = (file.path + @";" + file.name + ";" + file.jpg_ugo + ";" + file.x + ";" + file.y + ";" + file.z);
                // MessageBox.Show(file.path);
                lb1_selitem = lb1.SelectedIndex;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            lb1_redak();
        }

        void lb1_redak()
        {
            if (lb2.SelectedIndex > -1)
            {
                if (lb1.SelectedIndex > -1)
                {
                    var file = lb1.SelectedValue as Models3d;

                    kommunikacii_dial_imp dial = new kommunikacii_dial_imp(pathBD, file, null);
                    dial.ShowDialog();
                    lb1_napolnenie();
                }
                else
                {
                    MessageBox.Show("Сначала выберите объект для редактирования");
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите категорию");
            }

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            lb1_del();
        }

        void lb1_del()
        {

            if (lb2.SelectedIndex > -1)
            {
                if (lb1.SelectedIndex > -1)
                {
                    var file = lb1.SelectedValue as Models3d;
                    if (MessageBox.Show(
                 "Вы действительно хотите удалить \"" + file.name + "\"?", "",
                 MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        BD.conn("DELETE FROM `import3ds` WHERE id=" + file.id);
                        MessageBox.Show("Объект успешно удалён");
                        lb1_napolnenie();

                    }
                }
                else
                {
                    MessageBox.Show("Сначала выберите объект для редактирования");
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите категорию");
            }

        }

        private void lb3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb3.SelectedIndex > -1)
            {
                lb4_napolnenie();
            }
        }

        private void lb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb2.SelectedIndex > -1)
            {
                lb1_napolnenie();
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string otv = new dialuni().Show("Отмена", null, "Создать", "Добавление новой категории", "Укажите название новой категории", 3, 170);
            if (otv != "CANCEL")
            {
                BD.conn("INSERT INTO  `import3ds_category` (nazv)  VALUES ('" + otv + "')");
                MessageBox.Show("Категория успешно создана");
                lb2lb3napolnene();
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {


            var model = (lb2.SelectedItem as treespis);

            var reader = BD.conn("SELECT id FROM `import3ds` WHERE category = '" + model.id + "'");

            if (!reader.HasRows)
            {

                if (MessageBox.Show(
       "Вы действительно хотите удалить \"" + model.name + "\"?", "",
       MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    BD.conn("DELETE FROM `import3ds_category` WHERE id=" + model.id);
                    MessageBox.Show("Категория успешно удалена");
                    lb2lb3napolnene();
                }
            }
            else
            {
                MessageBox.Show("Сначала удалите все модели, удалить категорию нельзя, пока в ней существуют модели");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            kommunikacii_Set ps = kommunikacii_Set.Default;
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
            ps.lb1sel = lb1.SelectedIndex;
            ps.lb2sel = lb2.SelectedIndex;
            ps.lb3sel = lb3.SelectedIndex;
            ps.tab1sel = tab1.SelectedIndex;


            ps.Save();
        }
        void loadpage()
        {
            try
            {
                kommunikacii_Set ps = kommunikacii_Set.Default;

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

                tab1.SelectedIndex = ps.tab1sel;
                if (ps.tab1sel == 0)
                {
                    lb3.SelectedIndex = ps.lb3sel;
                    //    lb4.Visibility = Visibility.Visible;
                    //    stpan.Visibility = Visibility.Visible;
                }
                else
                {
                    lb2.SelectedIndex = ps.lb2sel;
                    //   lb1.Visibility = Visibility.Visible;
                    //    btnserv.Visibility = Visibility.Visible;
                }

                lb1_selitem = ps.lb1sel;
                lb4_selitem = ps.lb4sel;
            }
            catch
            {

            }



        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            lb1_redak();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            lb1_del();
        }

        private void lb1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }




        private void PolygonShapesMenu_OnClick(object sender, RoutedEventArgs e)
        {

            try
            {
                string idcategor = ((sender as System.Windows.Controls.MenuItem).Header as treespis).id;
                string idmodel_peremech = (lb1.SelectedItem as Models3d).id;
                BD.conn("UPDATE `import3ds` SET  `category`='" + idcategor + "' WHERE id=" + idmodel_peremech);
                MessageBox.Show("Объект успешно перемещён");
                lb1_napolnenie();
            }
            catch (Exception err)
            {
                //  MessageBox.Show(err.Message.ToString());
            }
        }

        private void peremestitb_serv_event(object sender, RoutedEventArgs e)
        {

            try
            {
                string idcategor = ((sender as System.Windows.Controls.MenuItem).Header as treespis).id;
                string idmodel_peremech = (lb4.SelectedItem as Models3d).id;



                (new web_zapros()).load("peremesh", "id=" + idmodel_peremech + "&category=" + idcategor);

                MessageBox.Show("Объект успешно перемещён");
                lb4_napolnenie();
            }
            catch (Exception err)
            {
                //   MessageBox.Show(err.Message.ToString());
            }
        }
        //добавление новой категории на сервер
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {



            string otv = new dialuni().Show("Отмена", null, "Создать", "Добавление новой категории НВ СЕРВЕР", "Укажите название новой категории", 3, 170);
            if (otv != "CANCEL")
            {

                (new web_zapros()).load("create_categ", "nazv=" + otv);
                MessageBox.Show("Категория успешно создана");
                lb2lb3napolnene();
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {

            var model = (lb3.SelectedItem as treespis);

            if (MessageBox.Show(
"Вы действительно хотите удалить \"" + model.name + "\"?", "",
MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {



                var json = (new web_zapros()).load("delete_group", "delidcategor=" + model.id);



                if (json != "FAIL")
                {
                    MessageBox.Show("Категория успешно удалена");
                    lb2lb3napolnene();

                }
                else
                {
                    MessageBox.Show("Сначала удалите все модели, удалить категорию нельзя, пока в ней существуют модели");
                }
            }


        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)//удалить объект
        {

            if (lb3.SelectedIndex > -1)
            {
                if (lb4.SelectedIndex > -1)
                {
                    var file = lb4.SelectedValue as Models3d;
                    if (MessageBox.Show(
                 "Вы действительно хотите удалить \"" + file.name + "\"?", "",
                 MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {

                        (new web_zapros()).load("delete_obj", "id=" + file.id);


                        MessageBox.Show("Объект успешно удалён");
                        lb4_napolnenie();


                    }
                }
                else
                {
                    MessageBox.Show("Сначала выберите объект для редактирования");
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите категорию");
            }
        }

        private void lb4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb4.SelectedItem != null)
            {
                //  selectedModel = (lb1.SelectedValue as Models3d).path;
                var file = lb4.SelectedValue as Models3d;
                selectedModel = (file.path + @";" + file.name + ";" + file.jpg_ugo + ";" + file.x + ";" + file.y + ";" + file.z);
                // MessageBox.Show(file.path);
                lb4_selitem = lb4.SelectedIndex;
            }
        }


        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            if (lb2.SelectedIndex > -1)
            {
                if (lb1.SelectedIndex > -1)
                {
                    var file = lb1.SelectedValue as Models3d;
                    if (MessageBox.Show(
                 "Вы действительно хотите отправить файл на сервер \"" + file.name + "\"?", "",
                 MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (File.Exists(file.path))
                        {
                            (new FTP()).otprav(file.path, "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + file.name + ".3ds");
                            (new FTP()).otprav(file.jpg_path, "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + file.name + ".jpg");
                            (new FTP()).otprav(file.jpg_ugo, "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + file.name + "ugo.jpg");


                            string categoryuser = (lb2.SelectedItem as treespis).name;
                            (new web_zapros()).load("new3ds", "filename=" + file.name + "&x=" + file.x + "&y=" + file.y + "&z=" + file.z + "&categoryuser=" + categoryuser);
                            MessageBox.Show("Объект успешно отправлен");
                        }
                        else
                        {
                            MessageBox.Show("3ds файл на Вашем компьютере не обнаружен");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Сначала выберите объект");
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите категорию");
            }
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            dob_new3ds();
        }

        private void tab1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tab1.SelectedIndex == 0)
            {
                lb4.Visibility = Visibility.Visible;
                lb1.Visibility = Visibility.Collapsed;
                stpan.Visibility = Visibility.Collapsed;
                btnserv.Visibility = Visibility.Visible;
            }
            else
            {
                lb4.Visibility = Visibility.Collapsed;
                lb1.Visibility = Visibility.Visible;
                stpan.Visibility = Visibility.Visible;
                btnserv.Visibility = Visibility.Collapsed;
            }

        }
    }

    public class dialuni
    {
        public string otvet = "";
        public string Show(string sbtn1, string sbtn2, string sbtn3, string stitle, string stext, int param, double heightwin)
        {
            dialog_univi dial = new dialog_univi(this, sbtn1, sbtn2, sbtn3, stitle, stext, param, heightwin);
            dial.ShowDialog();

            return otvet;
        }


    }
    public class Models3d
    {

        public string path { get; set; }
        public string name { get; set; }
        public string type { get; set; }//S-сервера P-пользовательский
        public string jpg_path { get; set; }
        public string jpg_ugo { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }
        public string id { get; set; }
        public string category { get; set; }


    }

    public class treespis
    {

        public string path { get; set; }
        public string name { get; set; }
        public string id { get; set; }

        public string type { get; set; }

    }
    public class FTP
    {

        public void otprav(string filename, string path)
        {
            //ftp://ecad.giulianovars.ru/public/
            FileInfo toUpload = new FileInfo(filename);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(path);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("ecad_ftp", "bFqeNo4Xp2");
            Stream ftpStream = request.GetRequestStream();
            FileStream fileStream = File.OpenRead(filename);
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            do
            {
                bytesRead = fileStream.Read(buffer, 0, 1024);
                ftpStream.Write(buffer, 0, bytesRead);
            }
            while (bytesRead != 0);
            fileStream.Close();
            ftpStream.Close();
        }

    }
    class web_zapros
    {
        //вызов
        //(new web_zapros()).load("new3ds","asdasdsa");
        public string load(string comand, string param = "")
        {
            if (param != "")
            {
                param = "&" + param;
            }
            //загрузка дынных для сервера
            WebClient client = new WebClient();
            string url = "http://ecad.giulianovars.ru/php/3dsobject/3ds.php?command=" + comand + param;
            byte[] response = client.DownloadData(url);
            string otvet = System.Text.Encoding.UTF8.GetString(response);

            return otvet;
        }


    }
}
