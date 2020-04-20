using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;


namespace TreeCadN.kommunikacii
{
    /// <summary>
    /// Логика взаимодействия для kommunikacii_dial_imp.xaml
    /// </summary>
    public partial class kommunikacii_dial_imp : Window
    {
        public string pathBD = "";
        public Models3d model = new Models3d();
        public treespis category = new treespis();
        string alt_nazv = "";
        kommunikacii_main ctx;
        //private const string MODEL_PATH = @"C:\!qwerty\TreeCadN\WpfApplication1\bin\Debug\3dsObject\User\Розетки\1_131483728943324771.3ds";

        public kommunikacii_dial_imp(kommunikacii_main ctx, string pathBD, Models3d model, treespis category)
        {

            InitializeComponent();

            this.ctx = ctx;
            this.pathBD = pathBD;
            this.model = model;
            this.category = category;


            if (model != null)
            {
                alt_nazv = model.name;

                btn2.IsEnabled = false;
                tb1.IsEnabled = false;

                tb1.Text = model.path;
                if ("/TreeCadN;component/Foto/nofoto.jpg" == model.jpg_path || !File.Exists(model.jpg_path))
                {
                    tb2.Text = "";
                }
                else
                {
                    tb2.Text = model.jpg_path;
                }
                if (!File.Exists(model.jpg_ugo))
                {
                    tb3.Text = "";
                }
                else
                {
                    tb3.Text = model.jpg_ugo;
                }
                tb4.Text = model.name;

                tx.Text = model.x;
                ty.Text = model.y;
                tz.Text = model.z;

                ModelVisual3D device3D = new ModelVisual3D();
                Model3D dmodel3ds = Display3d(model.path);

                device3D.Content = dmodel3ds;

                //     viewPort3d.Children.Add(device3D);
                //        viewPort3d.ZoomExtents();


                string path_copy = Environment.CurrentDirectory + @"\3dsObject\User\" + category.name + @"\" + model.name;
                var asdsadd = Directory.EnumerateFiles(path_copy, "*.*", SearchOption.TopDirectoryOnly)
          .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png"));

                foreach (string elem in asdsadd)
                {
                    spisdop_fiel.Items.Add(elem);
                }
                spisdop_fiel.Items.Refresh();


            }





        }

        ModelVisual3D lastmodel;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "3DS Files (*.3ds)|*.3ds";
            if (openFileDialog1.ShowDialog() == true)
            {
                tb1.Text = openFileDialog1.FileName;











                ModelVisual3D device3D = new ModelVisual3D();
                Model3D dmodel3ds = Display3d(openFileDialog1.FileName);

                device3D.Content = dmodel3ds;
                // Add to view port
                if (lastmodel != null)
                {
                    //       viewPort3d.Children.Remove(lastmodel);
                }

                //    viewPort3d.Children.Add(device3D);
                //     viewPort3d.ZoomExtents();

                tx.Text = dmodel3ds.Bounds.SizeX.ToString("0");
                ty.Text = dmodel3ds.Bounds.SizeZ.ToString("0");
                tz.Text = dmodel3ds.Bounds.SizeY.ToString("0");


                lastmodel = device3D;
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Картинки (*.jpg,*.png)|*.jpg;*.png";



            if (openFileDialog1.ShowDialog() == true)
            {
                tb2.Text = openFileDialog1.FileName;

            }
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Картинки (*.jpg,*.png)|*.jpg;*.png";
            if (openFileDialog1.ShowDialog() == true)
            {
                tb3.Text = openFileDialog1.FileName;

            }
        }

        private void tx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            string pattern = @"[0-9]";
            Regex regex = new Regex(pattern);

            // Получаем совпадения в экземпляре класса Match
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;

            }

        }

        private void tx_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }

        }

        private void ty_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void tz_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void ty_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string pattern = @"[0-9]";
            Regex regex = new Regex(pattern);

            // Получаем совпадения в экземпляре класса Match
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;

            }
        }

        private void tz_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string pattern = @"[0-9]";
            Regex regex = new Regex(pattern);

            // Получаем совпадения в экземпляре класса Match
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;

            }
        }


        public Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                //Adding a gesture here
                //        viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);

                //Import 3D model file
                //    ModelImporter import = new ModelImporter();


                device = (new HelixToolkit.Wpf.ModelImporter()).Load(model);
                //Load the 3D model file
                //  device = import.Load(model);




            }
            catch (Exception e)
            {
                // Handle exception in case can not find the 3D model file
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }
            return device;
        }

        private void tx_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (cb_1.IsChecked == true && (tx.Text != "" && ty.Text != "" && tz.Text != "") && (sender as TextBox).IsFocused)
            {

                double koef = Convert.ToDouble(tx.Text) / pre_x;
                ty.Text = (pre_y * koef).ToString("0");
                tz.Text = (pre_z * koef).ToString("0");
            }
        }
        private void ty_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (cb_1.IsChecked == true && (tx.Text != "" && ty.Text != "" && tz.Text != "") && (sender as TextBox).IsFocused)
            {

                double koef = Convert.ToDouble(ty.Text) / pre_y;
                tx.Text = (pre_x * koef).ToString("0");
                tz.Text = (pre_z * koef).ToString("0");
            }
        }
        private void tz_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (cb_1.IsChecked == true && (tx.Text != "" && ty.Text != "" && tz.Text != "") && (sender as TextBox).IsFocused)
            {

                double koef = Convert.ToDouble(tz.Text) / pre_z;
                tx.Text = (pre_x * koef).ToString("0");
                ty.Text = (pre_y * koef).ToString("0");

            }
        }

        double pre_x, pre_y, pre_z;


        List<spis_dopfile> spisok = new List<spis_dopfile>();

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (spisdop_fiel.SelectedItem != null)
            {


                // if (File.Exists(spisdop_fiel.SelectedItem.ToString())) File.Delete(spisdop_fiel.SelectedItem.ToString());
                spisdop_fiel.Items.Remove(spisdop_fiel.SelectedItem);
            }
            spisdop_fiel.Items.Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                string nazv = tb4.Text;
                log.Add("название " + nazv);
                if (!(File.Exists(tb1.Text) && tb1.Text != "")) { MessageBox.Show("Ошибка 3d файла не существует"); return; }
                if (!(File.Exists(tb2.Text) || tb2.Text == "")) { MessageBox.Show("Ошибка Картинка не существует"); return; }
                if (!(File.Exists(tb3.Text) || tb3.Text == "")) { MessageBox.Show("Ошибка Картинка УГО не существует"); return; }
                if (!(tx.Text != "" || ty.Text != "" || tz.Text != "")) { MessageBox.Show("Габариты не могут быть пустыми"); return; }

                if (nazv.Equals("")) { nazv = tb1.Text.Split('\\').Last().Split('.').First(); }
                if (model == null)
                {
                    alt_nazv = nazv;
                }

                string pathcategory = Environment.CurrentDirectory + @"\3dsObject\User\" + category.name;
                string path_copy = pathcategory + @"\" + alt_nazv;

                Directory.CreateDirectory(path_copy + @"\teh");
                log.Add("создаём директорию по пути1-" + path_copy);

                string path3ds = path_copy + @"\" + tb1.Text.Split('\\').Last();
                string pathjpg = path_copy + @"\teh\prev." + tb2.Text.Split('.').Last();
                string pathjpgugo = path_copy + @"\teh\ugo." + tb3.Text.Split('.').Last();
                string pathconf = path_copy + @"\teh\conf.json";

                //копируем все файлы
                if (File.Exists(tb1.Text))
                {
                    if (!tb1.Text.Equals(path3ds))
                    {
                        File.Copy(tb1.Text, path3ds, true);
                        log.Add("скопировали какой то файл");
                    }
                }
                else
                {
                    path3ds = "";
                }
                if (File.Exists(tb2.Text))
                {
                    if (!tb2.Text.Equals(pathjpg))
                    {
                        File.Copy(tb2.Text, pathjpg, true);
                        log.Add("если сущ скопировали какой то файл");
                    }
                }
                else
                {
                    pathjpg = "";
                }
                if (File.Exists(tb3.Text))
                {
                    if (!tb3.Text.Equals(pathjpgugo))
                    {
                        log.Add("скопировали какой то файл2");
                        File.Copy(tb3.Text, pathjpgugo, true);
                    }
                }
                else
                {
                    pathjpgugo = "";
                }


                foreach (string elem in spisdop_fiel.Items)
                {
                    string namefile = elem.Split('\\').Last();
                    if (File.Exists(elem) && !elem.Equals(path_copy + @"\" + namefile))
                    {
                        File.Copy(elem, path_copy + @"\" + namefile, true);
                    }
                }



                //генерируем JSON файл
                Models3dJSON modeljson = new Models3dJSON(tx.Text, ty.Text, tz.Text);
                string output = JsonConvert.SerializeObject(modeljson);
                File.Delete(pathconf);
                using (FileStream fs = new FileStream(pathconf, FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты
                    byte[] array = System.Text.Encoding.Default.GetBytes(output);
                    // запись массива байтов в файл

                    fs.Write(array, 0, array.Length);
                }


                //удалим все реаннее сгенерированные файлы для обновления модели
                var asdsadd = Directory.EnumerateFiles(path_copy, "*.*", SearchOption.AllDirectories)
              .Where(s => s.EndsWith(".DR3D") || s.EndsWith(".DRG1"));
                foreach (string elem in asdsadd)
                {
                    if (File.Exists(elem))
                    {
                        File.Delete(elem);
                    }
                }


                if (model != null && !path_copy.Equals(pathcategory + @"\" + nazv))
                {
                    //  MessageBox.Show(path_copy+"  "+ pathcategory + @"\" + nazv);
                    Directory.Move(path_copy, pathcategory + @"\" + nazv);
                }

                /*
                                if (model == null)
                                {

                                    BD.conn("INSERT INTO `import3ds` (`nazv`, `path3ds`, `pathjpg`, `pathjpgugo`, `x`, `y`, `z`, category) VALUES ('" +
                                    nazv + "','" +
                                    path3ds + "','" +
                                    pathjpg + "','" +
                                    pathjpgugo + "','" +
                                    tx.Text + "','" +
                                    ty.Text + "','" +
                                    tz.Text + "', '" +
                                    category.id + "')");

                                   // MessageBox.Show("Объект успешно добавлен");
                                }
                                else
                                {
                                    BD.conn("UPDATE `import3ds` SET `nazv`='" + nazv + "',  `path3ds`='" + path3ds + "', `pathjpg`='" + pathjpg + "', `pathjpgugo`='" + pathjpgugo + "', `x`='" + tx.Text + "', `y`='" + ty.Text + "', `z`='" + tz.Text + "'  WHERE id=" + model.id);
                                  //  MessageBox.Show("Объект успешно изменён");

                                }
                                */



                ctx.lb1_napolnenie();

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.TargetSite);
                MessageBox.Show(ex.Message + ", попробуйте указать другое имя модели");
            }



  
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Картинки (*.jpg,*.png)|*.jpg;*.png";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == true)
            {
                foreach (string elem in openFileDialog1.FileNames)
                {
                    spisdop_fiel.Items.Add(elem);
                }
                spisdop_fiel.Items.Refresh();

            }


        }

        private void cb_1_Checked(object sender, RoutedEventArgs e)
        {
            if (cb_1.IsChecked == true)
            {
                pre_x = Convert.ToDouble(tx.Text);
                pre_y = Convert.ToDouble(ty.Text);
                pre_z = Convert.ToDouble(tz.Text);
            }
        }


    }

    class spis_dopfile
    {

        public string name { get; set; }

    }


}
