using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Path = System.IO.Path;
using System.IO.Compression;
using System.Threading.Tasks;

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


        List<Models3d> server_obj = new List<Models3d>();
        string dir3ds, dir3ds_user;
        string admin;
        public kommunikacii_main(string pathBD)
        {
            InitializeComponent();
            log.Add("старт диалог моделей");
            lb4.Visibility = Visibility.Collapsed;
            lb1.Visibility = Visibility.Collapsed;
            //  stpan.Visibility = Visibility.Collapsed;
            //btnserv.Visibility = Visibility.Collapsed;



            dir3ds = Environment.CurrentDirectory + @"\3dsObject\Web";
            dir3ds_user = Environment.CurrentDirectory + @"\3dsObject\User";

            Directory.CreateDirectory(dir3ds);
            Directory.CreateDirectory(dir3ds_user);

            this.pathBD = pathBD;
            BD.path = pathBD; //укажем файл бд



            INIManager client_man = new INIManager(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\ecadpro.ini");
            admin = client_man.GetPrivateString("Infogen", "3dsadmin");//версия клиента
            if (admin == "1")
            {
                label1.Visibility = Visibility.Visible;
                allnaserver.Visibility = Visibility.Visible;

            }
            else
            {
                lb4.ContextMenu = null;
                label1.Visibility = Visibility.Collapsed;
                allnaserver.Visibility = Visibility.Collapsed;
            }


            move_to_new_41();

            loadpage();
            log.Add("lb3napolnene");
            lb3napolnene();
            lb2napolnene();



        }


        void lb3napolneneeeee()
        {

        }
        void lb2napolnene()
        {



            List<treespis> spisvategor = new List<treespis>();

            foreach (string elem in Directory.GetDirectories(dir3ds_user, "*", SearchOption.TopDirectoryOnly))
            {
                spisvategor.Add(new treespis() { name = elem.Split('\\').Last() });
            }

            lb2.ItemsSource = spisvategor;
            //  peremestitb.ItemsSource = spisvategor;
        }



        public void lb1_napolnenie()//наполнение листбокса
        {
            try
            {

                log.Add("lb1_napolnenie старт");
                treespis Treespis = lb2.SelectedItem as treespis;
                List<Models3d> modeli = new List<Models3d>();
                log.Add("Преобразовали в класс");
                foreach (string elem in Directory.GetDirectories(dir3ds_user + "\\" + Treespis.name, "*", SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        string name = elem.Split('\\').Last();
                        log.Add("парсим элемент " + name);
                        var asdasd = Directory.GetFiles(elem, "*.3ds", SearchOption.TopDirectoryOnly);
                        if (!(asdasd.Length > 0)) return;
                        string path = asdasd.First();
                        string jpg_path = elem + "\\teh\\prev.jpg";
                        string jpg_ugo = elem + "\\teh\\ugo.jpg";
                        string pathcategory = elem;
                        string confjson = elem + "\\teh\\conf.json";



                        Models3dJSON zyx;


                        if (File.Exists(confjson))
                        {
                            using (StreamReader fstream = new StreamReader(confjson, System.Text.Encoding.Default))
                            {
                                zyx = JsonConvert.DeserializeObject<Models3dJSON>(fstream.ReadToEnd());
                            }
                        }
                        else
                        {



                            try
                            {

                                Model3D dmodel3ds = (new HelixToolkit.Wpf.ModelImporter()).Load(path);

                                zyx = new Models3dJSON()
                                {
                                    x = dmodel3ds.Bounds.SizeX.ToString("0"),
                                    z = dmodel3ds.Bounds.SizeY.ToString("0"),
                                    y = dmodel3ds.Bounds.SizeZ.ToString("0")
                                };

                            }
                            catch
                            {
                                zyx = new Models3dJSON() { x = "100", y = "100", z = "100" };
                            }





                            //генерируем JSON файл
                            string output = JsonConvert.SerializeObject(zyx);
                            using (FileStream fs = new FileStream(confjson, FileMode.OpenOrCreate))
                            {
                                // преобразуем строку в байты
                                byte[] array = System.Text.Encoding.Default.GetBytes(output);
                                // запись массива байтов в файл

                                fs.Write(array, 0, array.Length);
                            }
                        }





                        BitmapImage btmap;







                        if (!File.Exists(jpg_path))
                        {
                            jpg_path = "/TreeCadN;component/Foto/nofoto.jpg";

                            btmap = new BitmapImage(new Uri(jpg_path, UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            btmap = new BitmapImage();
                            btmap.BeginInit();
                            btmap.UriSource = new Uri(jpg_path);
                            btmap.CacheOption = BitmapCacheOption.OnLoad;
                            btmap.EndInit();
                        }



                        // img.Source = btmap;



                        modeli.Add(new Models3d
                        {
                            name = name,
                            path = path,
                            jpg_path = jpg_path,
                            jpg_ugo = jpg_ugo,
                            pathcategory = pathcategory,
                            x = zyx.x,
                            y = zyx.y,
                            z = zyx.z,
                            bitmap_prev = btmap

                        });
                    }
                    catch (Exception ex)
                    {
                        log.Add("фатал ошибка: " + ex.Message);
                    }
                }
                log.Add("Закончили цикл");

                //test
                lb1.ItemsSource = modeli;
                lb1.Items.Refresh();

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
            catch (Exception ex)
            {
                log.Add("фатал ошибка: " + ex.Message);
            }
        }

        List<treespis> server;

        async void lb3napolnene()
        {

            var json = await Task.Run(() => loadsinch("load_categor"));                            // выполняется асинхронно

            server = JsonConvert.DeserializeObject<List<treespis>>(json);





            foreach (treespis drive in server)
            {
                TreeViewItem item = new TreeViewItem();
                item.Tag = drive;
                item.Header = drive.name;

                if (server.FindAll(x => x.owner.Equals(drive.id)).Count > 0 && drive.owner.Equals("0"))
                {
                    item.Items.Add("*");
                }
                lb3.Items.Add(item);
            }








        }

        private void trw_Products_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            item.Items.Clear();

            //MessageBox.Show((item.Tag as treespis).name.ToString());


            List<treespis> asdjasdaa = server.FindAll(x => x.owner.Equals((item.Tag as treespis).id));



            try
            {
                foreach (treespis subDir in asdjasdaa)
                {
                    TreeViewItem newItem = new TreeViewItem();
                    newItem.Tag = subDir;
                    newItem.Header = subDir.name;
                    if (server.FindAll(x => x.owner.Equals(subDir.id)).Count > 0)
                    {
                        newItem.Items.Add("*");
                    }
                    item.Items.Add(newItem);
                }
            }
            catch
            { }
        }



        async void lb4_napolnenie()//наполнение листбокса
        {
            log.Add("наполнение старт");
            treespis Treespis = (lb3.SelectedItem as TreeViewItem).Tag as treespis;
            var json = await Task.Run(() => loadsinch("load_object", "category=" + Treespis.id_server));                            // выполняется асинхронно
            log.Add("получили джсон ");
            List<Models3d> server = JsonConvert.DeserializeObject<List<Models3d>>(json);


            log.Add("преобразовани жсон в класс ");
            foreach (var elam in server)
            {

                if (Uri.IsWellFormedUriString(elam.jpg_path, UriKind.Absolute))
                {

                    BitmapImage bi3 = new BitmapImage();
                    bi3.BeginInit();
                    bi3.UriSource = new Uri(elam.jpg_path);
                    bi3.EndInit();

                    elam.jpgsource = bi3;
                    //   img_inp.Source =
                }
                else
                {
                    elam.jpgsource = null;
                }

            }

            log.Add("закончили цикл");


            lb4.ItemsSource = null;
            lb4.ItemsSource = (server);

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
                    var file = lb1.SelectedItem as Models3d;

                    kommunikacii_dial_imp dial = new kommunikacii_dial_imp(this, pathBD, file, (lb2.SelectedItem as treespis));
                    dial.ShowDialog();

                    //  lb1_napolnenie();
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



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            dob_new3ds();


   
        }
        void dob_new3ds()
        {
            if (lb2.SelectedIndex > -1)
            {
                //  var model = (lb2.SelectedItem as treespis).id;
                kommunikacii_dial_imp dial = new kommunikacii_dial_imp(this, pathBD, null, (lb2.SelectedItem as treespis));
                dial.ShowDialog();
                //     MessageBox.Show("Модель успешно создана");
                //lb1_napolnenie();
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
                selectedModel = (file.path + @"^" + file.name + "^" + file.jpg_ugo + "^" + file.x + "^" + file.y + "^" + file.z);
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




            if (item1.IsSelected)
            {

                var model = (lb4.SelectedItem as Models3d);


                //    var json = await Task.Run(() => loadsinch("load_categor"));                            // выполняется асинхронно

                string remoteUri = "https://ecad.giulianovars.ru/php/3dsobject/3ds.php?command=getZip&id=" + model.id;
                WebClient myWebClient = new WebClient();
                string url_zip = myWebClient.DownloadString(remoteUri);
                File.Delete(Path.GetTempPath() + "3ds_load_TreeCadN.zip");
                myWebClient.DownloadFile(url_zip, Path.GetTempPath() + "3ds_load_TreeCadN.zip");
                string zelev_path = dir3ds + "\\" + model.category + "\\" + model.name;
                Directory.CreateDirectory(zelev_path);
                string[] file3dssservera_arr = Directory.GetFiles(zelev_path, "*.3ds", SearchOption.TopDirectoryOnly);
                if (file3dssservera_arr.Length == 0)
                {
                    ZipFile.ExtractToDirectory(Path.GetTempPath() + "3ds_load_TreeCadN.zip", zelev_path);
                    file3dssservera_arr = Directory.GetFiles(zelev_path, "*.3ds", SearchOption.TopDirectoryOnly);
                }
                string file3dssservera = file3dssservera_arr.First();
                selectedModel = (file3dssservera + @"^" + model.name + "^" + model.loc_ugojpg + "^" + model.x + "^" + model.y + "^" + model.z);
            }
        }





        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            lb1_del();
        }

        void lb1_del()
        {

            if (!(lb2.SelectedIndex > -1)) { MessageBox.Show("Сначала выберите категорию"); return; }
            if (!(lb1.SelectedIndex > -1)) { MessageBox.Show("Сначала выберите объект для редактирования"); return; }

            var file = lb1.SelectedValue as Models3d;
            if (MessageBox.Show(
         "Вы действительно хотите удалить \"" + file.name + "\"?", "",
         MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //  BD.conn("DELETE FROM `import3ds` WHERE id=" + file.id);
                //   MessageBox.Show("Объект успешно удалён");

                Directory.Delete(file.pathcategory, true);
                lb1_napolnenie();

            }



        }

        private void Lb3_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {



            log.Add("чандж");

            lb4_napolnenie();


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
                //    BD.conn("INSERT INTO  `import3ds_category` (nazv)  VALUES ('" + otv + "')");
                //   MessageBox.Show("Категория успешно создана");
                Directory.CreateDirectory(dir3ds_user + "\\" + otv);

                lb2napolnene();
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(lb2.SelectedIndex > -1)) { MessageBox.Show("Выберите категорию для удаления"); return; }

            var Treespis = (lb2.SelectedItem as treespis);
            if (Directory.GetDirectories(dir3ds_user + "\\" + Treespis.name, "*", SearchOption.TopDirectoryOnly).Length > 0)
            { MessageBox.Show("Сначала удалите все модели, удалить категорию нельзя, пока в ней существуют модели"); return; }
            if (MessageBox.Show(
   "Вы действительно хотите удалить категорию \"" + Treespis.name + "\"?", "Удаление категории",
   MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Directory.Delete(dir3ds_user + "\\" + Treespis.name);
                lb2napolnene();
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
                    // lb3.SelectedIndex = ps.lb3sel;
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



            string idmodel_peremech = (lb1.SelectedItem as Models3d).pathcategory;//из
            string idcategor = dir3ds_user + "\\" + ((sender as System.Windows.Controls.MenuItem).Header as treespis).name + "\\" + (lb1.SelectedItem as Models3d).name;//куда
            if (idcategor.Equals(idmodel_peremech)) return;
            Directory.Move(idmodel_peremech, idcategor);
            lb1.UnselectAll();
            //    MessageBox.Show("asd");
            lb1_napolnenie();

        }

        private void peremestitb_serv_event(object sender, RoutedEventArgs e)
        {

            try
            {
                string idcategor = ((sender as System.Windows.Controls.MenuItem).Header as treespis).id_server;
                var file = lb4.SelectedValue as Models3d;
                string idmodel_peremech = file.id_server;

                (new web_zapros()).load("peremesh", "id=" + idmodel_peremech + "&category=" + idcategor);
                BD.execute("UPDATE `import3ds_server` SET  `category`='" + idcategor + "' WHERE id_server='" + file.id_server + "'");


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

                string ecad = "https://ecad.giulianovars.ru/3dmodels/NEW/";


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
            BD.execute("DELETE FROM `import3ds_server` WHERE category='" + Treespis.id_server + "'");
            foreach (Models3d elem in result)
            {
                BD.execute("INSERT INTO  `import3ds_server` (id_server, nazv,path3ds,pathjpg,pathjpgugo,x,y,z,category,new, loc_3ds, loc_jpg, loc_ugojpg, polzgroup)  VALUES ('" +
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
            /*
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
                        */
        }

        //
        private void lb4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb4.SelectedItem != null)
            {

                //  selectedModel = (lb1.SelectedValue as Models3d).path;
                var file = lb4.SelectedValue as Models3d;
                //    MessageBox.Show(file.loc_3ds);
                label1.Content = "Группа у пользователя - " + file.polzgroup;

                // MessageBox.Show(file.path);
                lb4_selitem = lb4.SelectedIndex;
            }
        }


        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            if (!(lb2.SelectedIndex > -1)) { MessageBox.Show("Сначала выберите категорию"); return; }
            if (!(lb1.SelectedIndex > -1)) { MessageBox.Show("Сначала выберите объект"); return; }
            var file = lb1.SelectedValue as Models3d;
            if (!(MessageBox.Show(
         "Вы действительно хотите отправить \"" + file.name + "\" на сервер?", "",
         MessageBoxButton.YesNo) == MessageBoxResult.Yes)) return;

            if (!(File.Exists(file.path))) { MessageBox.Show("3ds файл на Вашем компьютере не обнаружен"); return; }
                      
            string startPath = file.pathcategory;
            string zipPath = Path.GetTempPath() + "3ds_to_server_TreeCadN.zip";
          
            if (File.Exists(zipPath)) File.Delete(zipPath);
            log.Add("zipPath - " + zipPath);
            ZipFile.CreateFromDirectory(startPath, zipPath);

            string time = DateTime.Now.ToFileTimeUtc().ToString();
            string path3ds = Tr2(file.name).Replace(' ', '_') + "_" + time + ".zip";
            string pathjpg = Tr2(file.name).Replace(' ', '_') + "_" + time + ".jpg";


            log.Add("Отправляем заказ на фабрику: " + path3ds + "  " + pathjpg);


            using (var client = new System.Net.WebClient())
            {
                MessageBox.Show(zipPath);
                //   client.Headers.Add("Content-Type", "multipart/form-data");
                var asdasdasd = client.UploadFile(@"https://ecad.giulianovars.ru/zakaz/uploadzakaz.php", "POST", zipPath);
                string download = Encoding.UTF8.GetString(asdasdasd);

                  MessageBox.Show(download);

            }
            
            //(new FTP()).otprav(zipPath, "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + path3ds);
            if (File.Exists(file.jpg_path))
            {
                //  (new FTP()).otprav(file.jpg_path, "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + pathjpg);

              

            }
           

            string categoryuser = (lb2.SelectedItem as treespis).name;


            //Создание объекта, для работы с файлом
            INIManager manager = new INIManager(GetEcadProIni());
            string authotiz_root = manager.GetPrivateString("giulianovars", "attivazione");//получ ключ активации



            (new web_zapros()).load("new3ds",
                "filename=" + file.name +
                "&filename3ds=" + path3ds +
                "&filenamejpg=" + pathjpg +
                "&idclienta=" + authotiz_root +
                "&x=" + file.x +
                "&y=" + file.y +
                "&z=" + file.z +
                "&categoryuser=" + categoryuser);
            MessageBox.Show("Объект успешно отправлен");




        }

        string GetEcadProIni()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\ecadpro.ini";
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            dob_new3ds();
        }








        public static string loadsinch(string comand, string param = "")
        {
            string otvet = "";
            try
            {
                if (param != "")
                {
                    param = "&" + param;
                }
                //загрузка дынных для сервера
                log.Add("/загрузка дынных для сервера");
                WebClient client = new WebClient();
                
                string url = "https://ecad.giulianovars.ru/php/3dsobject/3ds.php?command=" + comand + param;
                log.Add(url);
                byte[] response = client.DownloadData(url);
                otvet = Encoding.UTF8.GetString(response);
                log.Add("успех");
            }
            catch (Exception err)
            {
                //  MessageBox.Show(err.Message); 
            }
            return otvet;
        }






        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
              "Вы действительно хотите очистить локальную базу данных?", "",
              MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                BD.execute("DELETE FROM `import3ds_category_server` ");
                BD.execute("DELETE FROM `import3ds_server` ");
                lb3napolnene();
                lb4.ItemsSource = null;
            }
        }

        private void move_to_new_41()
        {

            string pathcategory = Environment.CurrentDirectory + @"\3dsObject\User";
            string[] dirs = Directory.GetDirectories(pathcategory);

            bool msgpokaz = true;

            foreach (string dir in dirs)
            {

                foreach (string file3ds in Directory.GetFiles(dir, "*.3ds", SearchOption.TopDirectoryOnly))
                {
                    if (msgpokaz)
                    {
                        MessageBox.Show("Восстановление старых объектов, нажмите ОК и подождите до завершения");
                        msgpokaz = false;
                    }
                    string namefile = file3ds.Split('\\').Last().Split('_').First();
                    string path3ds_new = dir + @"\" + namefile + @"\" + namefile + ".3ds";
                    string pathjpg_new = dir + @"\" + namefile + @"\teh\prev.jpg";

                    Directory.CreateDirectory(dir + @"\" + namefile + @"\teh");

                    if (File.Exists(path3ds_new)) File.Delete(path3ds_new);
                    File.Move(file3ds, path3ds_new);
                    string pathjpg = file3ds.Replace(".3ds", ".jpg");
                    if (File.Exists(pathjpg))
                    {

                        if (File.Exists(pathjpg_new)) File.Delete(pathjpg_new);
                        File.Move(pathjpg, pathjpg_new);
                    }
                }
            }

            if (msgpokaz == false) MessageBox.Show("Готово");



            //   MessageBox.Show("Готово");
            //   lb2napolnene();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            //отправить все на фабрику





            if (!(lb2.SelectedIndex > -1)) { MessageBox.Show("Сначала выберите категорию"); return; }
            //       if (!(lb1.SelectedIndex > -1)) { MessageBox.Show("Сначала выберите объект"); return; }


            if (!(MessageBox.Show(
"Вы действительно хотите отправить всю категорию на сервер?", "",
MessageBoxButton.YesNo) == MessageBoxResult.Yes)) return;

            foreach (Models3d file in lb1.Items)
            {





                if (!(File.Exists(file.path))) { MessageBox.Show("3ds файл на Вашем компьютере не обнаружен"); return; }

                string startPath = file.pathcategory;
                string zipPath = Path.GetTempPath() + "3ds_to_server_TreeCadN.zip";

                if (File.Exists(zipPath)) File.Delete(zipPath);
                log.Add("zipPath - " + zipPath);
                ZipFile.CreateFromDirectory(startPath, zipPath);

                string time = DateTime.Now.ToFileTimeUtc().ToString();
                string path3ds = Tr2(file.name).Replace(' ', '_') + "_" + time + ".zip";
                string pathjpg = Tr2(file.name).Replace(' ', '_') + "_" + time + ".jpg";


                log.Add("Отправляем заказ на фабрику: " + path3ds + "  " + pathjpg);


                (new FTP()).otprav(zipPath, "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + path3ds);
                if (File.Exists(file.jpg_path))
                {
                    (new FTP()).otprav(file.jpg_path, "ftp://ecad.giulianovars.ru/public/3dmodels/NEW/" + pathjpg);
                }


                string categoryuser = (lb2.SelectedItem as treespis).name;


                //Создание объекта, для работы с файлом
                INIManager manager = new INIManager(GetEcadProIni());
                string authotiz_root = manager.GetPrivateString("giulianovars", "attivazione");//получ ключ активации



                (new web_zapros()).load("new3ds",
                    "filename=" + file.name +
                    "&filename3ds=" + path3ds +
                    "&filenamejpg=" + pathjpg +
                    "&idclienta=" + authotiz_root +
                    "&x=" + file.x +
                    "&y=" + file.y +
                    "&z=" + file.z +
                    "&categoryuser=" + categoryuser);
             //   MessageBox.Show("Объект успешно отправлен");






            }
            MessageBox.Show("Объект успешно отправлен");










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
                //    stpan.Visibility = Visibility.Collapsed;

                //   label1.Visibility = Visibility.Visible;
            }
            else
            {
                lb4.Visibility = Visibility.Collapsed;
                lb1.Visibility = Visibility.Visible;
                //  stpan.Visibility = Visibility.Visible;

                //  label1.Visibility = Visibility.Collapsed;
            }

        }
        private static string Tr2(string s)
        {
            StringBuilder ret = new StringBuilder();
            string[] rus = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й",
          "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц",
          "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
            string[] eng = { "A", "B", "V", "G", "D", "E", "E", "ZH", "Z", "I", "Y",
          "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "KH", "TS",
          "CH", "SH", "SHCH", null, "Y", null, "E", "YU", "YA" };

            for (int j = 0; j < s.Length; j++)
                for (int i = 0; i < rus.Length; i++)
                    if (s.Substring(j, 1) == rus[i]) ret.Append(eng[i]);
            return ret.ToString();
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
        public string pathcategory { get; set; }
        public BitmapImage bitmap_prev { get; set; }
        public BitmapImage jpgsource { get; set; }

    }


    public class Models3dJSON
    {

        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }

        public Models3dJSON()
        {
        }
        public Models3dJSON(string x, string y, string z)
        {
            this.x = x;
            this.y = y;
            this.z = z;

        }


    }

    public class treespis
    {

        //     public string path { get; set; }
        public string path_dir { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string id_server { get; set; }
        //   public string type { get; set; }
        public string zvetbg { get; set; }

        public string owner { get; set; }
        public ObservableCollection<treespis> Nodes { get; set; }
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
                string url = "https://ecad.giulianovars.ru/php/3dsobject/3ds.php?command=" + comand + param;
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
