using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
        List<treespis> server = new List<treespis>();

        List<Models3d> server_obj = new List<Models3d>();
        string dir3ds;
        string admin;
        public kommunikacii_main(string pathBD)
        {
            InitializeComponent();

            lb4.Visibility = Visibility.Collapsed;
            lb1.Visibility = Visibility.Collapsed;
            stpan.Visibility = Visibility.Collapsed;
            btnserv.Visibility = Visibility.Collapsed;
            label1.Visibility = Visibility.Collapsed;
            stpan1.Visibility = Visibility.Collapsed;

            DirectoryInfo directory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;

            dir3ds = directory + @"\3dsObject\Web\";


            this.pathBD = pathBD;
            BD.path = pathBD; //укажем файл бд

     

            INIManager client_man = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
            admin = client_man.GetPrivateString("giulianovars", "3dsadmin");//версия клиента
            if (admin == "1")
            {
                label1.Visibility = Visibility.Visible;

            }
            else
            {
                lb4.ContextMenu = null;

            }


            loadpage();
            lb3napolnene();
            lb2napolnene();



        }


        void lb3napolnene()
        {
            //загрузка дынных для сервера


            server.Clear();
            //List<treespis> server = new List<treespis>();



            var reader1 = BD.conn("SELECT id, nazv, new, id_server FROM `import3ds_category_server` order by nazv ASC");
            while (reader1.Read())
            {
                string zvetbg = null;
                //string name = reader1["nazv"].ToString();
                if (reader1["new"].ToString() == "1")
                {
                    zvetbg = "#7FEBFF5F";
                    //     name += " !NEW!";

                }

                if (admin == "1" ||  reader1["id_server"].ToString()!="1")
                { 

                    server.Add(new treespis()
                {
                    id = reader1["id"].ToString(),
                    path = "S",
                    name = reader1["nazv"].ToString(),
                    type = "S",
                    zvetbg = zvetbg,
                    id_server = reader1["id_server"].ToString()

                });
                }
            }
            lb3.ItemsSource = null;
            lb3.ItemsSource = (server);
            peremestitb_serv.ItemsSource = null;
            peremestitb_serv.ItemsSource = (server);

        }
        void lb2napolnene()
        {

            ///////////////////////////

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

            //    string path = Treespis.path;



            List<Models3d> modeli = new List<Models3d>();



            var reader = BD.conn("SELECT id, nazv, `path3ds`, `pathjpg`, `pathjpgugo`, `x`, `y`, `z`, category FROM `import3ds` WHERE category='" + Treespis.id + "'");
            while (reader.Read())
            {
                string jpg_path = reader["pathjpg"].ToString();
                if (jpg_path == "")
                {

                    jpg_path = "/TreeCadN;component/Foto/nofoto.jpg";
                }



                modeli.Add(new Models3d
                {
                    id = reader["id"].ToString(),
                    path = reader["path3ds"].ToString(),
                    name = reader["nazv"].ToString(),
                    jpg_path = jpg_path,
                    jpg_ugo = reader["pathjpgugo"].ToString(),
                    x = reader["x"].ToString(),
                    y = reader["y"].ToString(),
                    z = reader["z"].ToString(),
                    category = reader["category"].ToString(),


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


            try
            {

                treespis Treespis = lb3.SelectedItem as treespis;



                //  string path = Treespis.path;


                server_obj.Clear();


                //   var json = (new web_zapros()).load("load_object", "category=" + Treespis.id);

                //  modeli = JsonConvert.DeserializeObject<List<Models3d>>(json);


                var reader = BD.conn("SELECT id, id_server, nazv, `path3ds`, `pathjpg`, `pathjpgugo`, `x`, `y`, `z`, category, new,  loc_3ds, loc_jpg, loc_ugojpg, polzgroup FROM `import3ds_server` WHERE category='" + Treespis.id_server + "' order by new DESC, nazv ASC");
                while (reader.Read())
                {
                    string loc_jpg = reader["loc_jpg"].ToString();

                    if (loc_jpg == "" || !File.Exists(loc_jpg))
                    {

                        loc_jpg = "/TreeCadN;component/Foto/nofoto.jpg";
                    }

                    string zvetbg = null;
                    if (reader["new"].ToString() == "1")
                    {
                        zvetbg = "#7FEBFF5F";
                    }
                    string download = "Collapsed";

                    if (File.Exists(reader["loc_3ds"].ToString()))
                    {
                        download = "Visibility";
                    }


                    server_obj.Add(new Models3d
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
                        id_server = reader["id_server"].ToString(),
                        zvetbg = zvetbg,
                        loc_3ds = reader["loc_3ds"].ToString(),
                        loc_jpg = loc_jpg,
                        loc_ugojpg = reader["loc_ugojpg"].ToString(),
                        download = download,
                        polzgroup = reader["polzgroup"].ToString(),
                    });

                }


                lb4.ItemsSource = null;
                lb4.ItemsSource = server_obj;

                string row_cnt = (new web_zapros()).load("obnov_obj_prov", "category=" + Treespis.id_server);
                if (server_obj.Count.ToString() == row_cnt)
                {

                    btnserv.Visibility = Visibility.Collapsed;
                }
                else
                {

                    btnserv.Content = "Обновить " + server_obj.Count.ToString() + @"/" + row_cnt;
                    btnserv.Visibility = Visibility.Visible;
                }


                try
                {
                 //   MessageBox.Show(lb4_selitem.ToString());
                    object selitem = lb4.Items[lb4_selitem];


                    lb4.ScrollIntoView(selitem);
                    lb4.SelectedItem = (selitem);
                }
                catch(Exception err)
                {
                  //  MessageBox.Show(err.Message);
                    lb4.ScrollIntoView(null);
                    lb4.SelectedItem = (null);
                }
            }
            catch
            {
                //какая то неопознаянная ошибка 
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
                //  var model = (lb2.SelectedItem as treespis).id;
                kommunikacii_dial_imp dial = new kommunikacii_dial_imp(pathBD, null, (lb2.SelectedItem as treespis));
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
            close_server();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            Close();
        }


        void close_server()
        {
            if (lb4.Visibility == Visibility.Visible)
            {


                treespis Treespis = lb3.SelectedItem as treespis;
                var model = (lb4.SelectedItem as Models3d);
                string path_copy = dir3ds + Treespis.name + @"\";
                Directory.CreateDirectory(path_copy);//создадим папку для weб


                string path3ds = "";
                string pathjpg = "";
                string pathjpgugo = "";


                if (model.path != "")
                {

                    path3ds = path_copy + model.path;
                }
                //  MessageBox.Show("asdasd");
                if (model.jpg_path != "")
                {

                    pathjpg = path_copy + model.jpg_path;
                }
                if (model.jpg_ugo != "")
                {
                    pathjpgugo = path_copy + model.jpg_ugo;
                }

                string ecad = "http://ecad.giulianovars.ru/3dmodels/NEW/";


                if (!(File.Exists(path3ds) || path3ds == ""))
                {

                    (new WebClient()).DownloadFile(ecad + model.path, path3ds);//скачиваем картинку

                }
                if (!(File.Exists(pathjpg) || pathjpg == ""))
                {

                    (new WebClient()).DownloadFile(ecad + model.jpg_path, pathjpg);//скачиваем картинку

                }
                if (!(File.Exists(pathjpgugo) || pathjpgugo == ""))
                {

                    (new WebClient()).DownloadFile(ecad + model.jpg_ugo, pathjpgugo);//скачиваем картинку

                }

            }




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
                lb2napolnene();
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
                    lb2napolnene();
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
            ps.lb4sel = lb4.SelectedIndex;
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
            if (e.ChangedButton == MouseButton.Left)
            {
                close_server();
                Close();
            }

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
                string idcategor = ((sender as System.Windows.Controls.MenuItem).Header as treespis).id_server;
                var file = lb4.SelectedValue as Models3d;
                string idmodel_peremech = file.id_server;

                (new web_zapros()).load("peremesh", "id=" + idmodel_peremech + "&category=" + idcategor);
                BD.conn("UPDATE `import3ds_server` SET  `category`='" + idcategor + "' WHERE id_server='" + file.id_server + "'");


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
                lb3napolnene();
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            //подгрузить данные с сервера

            treespis Treespis = lb3.SelectedItem as treespis;
            var json = (new web_zapros()).load("load_object", "category=" + Treespis.id_server);
            List<Models3d> server_load_obj = JsonConvert.DeserializeObject<List<Models3d>>(json);

            //  MessageBox.Show(Treespis.id_server.ToString());
            //  List<treespis> Result = server_load.Where(a => server.Contains(a.name)).ToList();

            List<Models3d> result = new List<Models3d>();


            ///тест









            for (int i = 0; i < server_load_obj.Count(); i++)
            {
                bool estb = true;
                for (int y = 0; y < server_obj.Count(); y++)
                {

                    if (server_load_obj[i].id_server == server_obj[y].id_server)
                    {

                        estb = false;
                    }
                }
                string new_ = "0";
                if (estb)
                {
                    new_ = "1";
                }


                string path_copy = dir3ds + Treespis.name + @"\";
                Directory.CreateDirectory(path_copy);//создадим папку для weб

                //  MessageBox.Show(path_copy + server_load_obj[i].path);
                string path3ds = path_copy + server_load_obj[i].path;
                string pathjpg = "";
                string pathjpgugo = "";

                if (server_load_obj[i].jpg_path != "")
                {

                    pathjpg = path_copy + server_load_obj[i].jpg_path;
                }
                if (server_load_obj[i].jpg_ugo != "")
                {
                    pathjpgugo = path_copy + server_load_obj[i].jpg_ugo;
                }

                string ecad = "http://ecad.giulianovars.ru/3dmodels/NEW/";


                if (!(File.Exists(pathjpg) || pathjpg == ""))
                {

                    (new WebClient()).DownloadFile(ecad + server_load_obj[i].jpg_path, pathjpg);//скачиваем картинку

                }




                result.Add(new Models3d
                {
                    id = server_load_obj[i].id,
                    path = server_load_obj[i].path,
                    name = server_load_obj[i].name,
                    jpg_path = server_load_obj[i].jpg_path,
                    jpg_ugo = server_load_obj[i].jpg_ugo,
                    x = server_load_obj[i].x,
                    y = server_load_obj[i].y,
                    z = server_load_obj[i].z,
                    category = server_load_obj[i].category,
                    id_server = server_load_obj[i].id_server,
                    loc_3ds = path3ds,
                    loc_jpg = pathjpg,
                    loc_ugojpg = pathjpgugo,
                    polzgroup = server_load_obj[i].polzgroup,
                    new_ = new_


                });

            }


            //    MessageBox.Show(result.Count().ToString());


            //  BD.conn("UPDATE `import3ds_server` SET  `new`='0' ");
            BD.conn("DELETE FROM `import3ds_server` WHERE category='" + Treespis.id_server + "'");
            foreach (Models3d elem in result)
            {
                BD.conn("INSERT INTO  `import3ds_server` (id_server, nazv,path3ds,pathjpg,pathjpgugo,x,y,z,category,new, loc_3ds, loc_jpg, loc_ugojpg, polzgroup)  VALUES ('" +
                    elem.id_server + "', '" +
                    elem.name + "', '" +
                    elem.path + "', '" +
                    elem.jpg_path + "', '" +
                    elem.jpg_ugo + "', '" +
                    elem.x + "', '" +
                    elem.y + "', '" +
                    elem.z + "', '" +
                    elem.category + "', '" +
                    elem.new_ + "', '" +
                    elem.loc_3ds + "','" +
                    elem.loc_jpg + "','" +
                    elem.loc_ugojpg + "','" +
                    elem.polzgroup + "')");

            }
            MessageBox.Show("Данные успешно обновлены");
            lb4_napolnenie();


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
                    lb3napolnene();

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

                        (new web_zapros()).load("delete_obj", "id=" + file.id_server);
                        BD.conn("DELETE FROM `import3ds_server` WHERE id_server='" + file.id_server + "'");


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
                //    MessageBox.Show(file.loc_3ds);
                label1.Content = "Группа у пользователя - " + file.polzgroup;
                selectedModel = (file.loc_3ds + @";" + file.name + ";" + file.loc_ugojpg + ";" + file.x + ";" + file.y + ";" + file.z);
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
                 "Вы действительно хотите отправить \"" + file.name + "\" на сервер?", "",
                 MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (File.Exists(file.path))
                        {
                            //    MessageBox.Show(file.path);


                            string time = DateTime.Now.ToFileTimeUtc().ToString();

                            string path3ds = file.name + "_" + time + ".3ds";
                            string pathjpg = "";
                            string pathjpgugo = "";


                            (new FTP()).otprav(file.path, "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + path3ds);
                            if (File.Exists(file.jpg_path))
                            {
                                pathjpg = file.name + "_" + time + ".jpg";
                                (new FTP()).otprav(file.jpg_path, "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + pathjpg);
                            }
                            if (File.Exists(file.jpg_ugo))
                            {
                                pathjpgugo = file.name + "_" + time + "_ugo.jpg";
                                (new FTP()).otprav(file.jpg_ugo, "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + pathjpgugo);
                            }

                            string categoryuser = (lb2.SelectedItem as treespis).name;



                            (new web_zapros()).load("new3ds", "filename=" +
                              file.name + "&filename3ds=" +
                               path3ds + "&filenamejpg=" +
                              pathjpg + "&filenameugojpg=" +
                               pathjpgugo + "&x=" + file.x + "&y=" + file.y + "&z=" + file.z + "&categoryuser=" + categoryuser);
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

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            //подгрузить
            //////подгружаем если есть интернет
            var json = (new web_zapros()).load("load_categor");


            List<treespis> server_load = JsonConvert.DeserializeObject<List<treespis>>(json);


            //  List<treespis> Result = server_load.Where(a => server.Contains(a.name)).ToList();

            List<treespis> result = new List<treespis>();
            for (int i = 0; i < server_load.Count(); i++)
            {
                bool estb = true;
                for (int y = 0; y < server.Count(); y++)
                {
                    if (server_load[i].id_server == server[y].id_server)
                    {
                        estb = false;
                    }
                }
                if (estb)
                {
                    if (admin == "1" || server_load[i].id_server != "1")
                    {
                        result.Add(new treespis { name = server_load[i].name, id_server = server_load[i].id_server });
                    }
                }
            }


            //    MessageBox.Show(result.Count().ToString());


            BD.conn("UPDATE `import3ds_category_server` SET  `new`='0' ");
            foreach (treespis elem in result)
            {
                BD.conn("INSERT INTO  `import3ds_category_server` (nazv, new, id_server)  VALUES ('" + elem.name + "', 1, '" + elem.id_server + "') ");

            }
            MessageBox.Show("Данные успешно обновлены");
            lb3napolnene();
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
              "Вы действительно хотите очистить локальную базу данных?", "",
              MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                BD.conn("DELETE FROM `import3ds_category_server` ");
                BD.conn("DELETE FROM `import3ds_server` ");
                lb3napolnene();
                lb4.ItemsSource = null;
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            lb1_redak();
        }

        private void tab1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tab1.SelectedIndex == 0)
            {
                lb4.Visibility = Visibility.Visible;
                lb1.Visibility = Visibility.Collapsed;
                stpan.Visibility = Visibility.Collapsed;
                stpan1.Visibility = Visibility.Visible;
                //   label1.Visibility = Visibility.Visible;
            }
            else
            {
                lb4.Visibility = Visibility.Collapsed;
                lb1.Visibility = Visibility.Visible;
                stpan.Visibility = Visibility.Visible;
                stpan1.Visibility = Visibility.Collapsed;
                //  label1.Visibility = Visibility.Collapsed;
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
        public string jpg_path { get; set; }
        public string jpg_ugo { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }
        public string id { get; set; }
        public string category { get; set; }
        public string id_server { get; set; }
        public string loc_3ds { get; set; }
        public string loc_jpg { get; set; }
        public string loc_ugojpg { get; set; }
        public string zvetbg { get; set; }
        public string download { get; set; }
        public string polzgroup { get; set; }
        public string new_ { get; set; }

    }

    public class treespis
    {

        public string path { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string id_server { get; set; }
        public string type { get; set; }
        public string zvetbg { get; set; }

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
            string otvet = "";
            try
            {
                if (param != "")
                {
                    param = "&" + param;
                }
                //загрузка дынных для сервера
                WebClient client = new WebClient();
                string url = "http://ecad.giulianovars.ru/php/3dsobject/3ds.php?command=" + comand + param;
                byte[] response = client.DownloadData(url);
                otvet = System.Text.Encoding.UTF8.GetString(response);

            }
            catch (Exception err)
            {
                //  MessageBox.Show(err.Message); 
            }
            return otvet;
        }


    }

}
