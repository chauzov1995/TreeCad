using Microsoft.Win32;
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


        //private const string MODEL_PATH = @"C:\!qwerty\TreeCadN\WpfApplication1\bin\Debug\3dsObject\User\Розетки\1_131483728943324771.3ds";

        public kommunikacii_dial_imp(string pathBD, Models3d model, treespis category)
        {

            InitializeComponent();

            this.pathBD = pathBD;
            this.model = model;
            this.category = category;


            if (model != null)
            {

                tb1.Text = model.path;
                if ("/TreeCadN;component/Foto/nofoto.jpg" == model.jpg_path)
                {
                    tb2.Text = "";
                }
                else
                {
                    tb2.Text = model.jpg_path;
                }
                tb3.Text = model.jpg_ugo;
                tb4.Text = model.name;

                tx.Text = model.x;
                ty.Text = model.y;
                tz.Text = model.z;

                ModelVisual3D device3D = new ModelVisual3D();
                Model3D dmodel3ds = Display3d(model.path);

                device3D.Content = dmodel3ds;

           //     viewPort3d.Children.Add(device3D);
        //        viewPort3d.ZoomExtents();
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
            openFileDialog1.Filter = "JPG Files (*.jpg)|*.jpg";
            if (openFileDialog1.ShowDialog() == true)
            {
                tb2.Text = openFileDialog1.FileName;

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {




                if (File.Exists(tb1.Text) && tb1.Text != "")
                {
                    if (File.Exists(tb2.Text) || tb2.Text == "")
                    {
                        if (File.Exists(tb3.Text) || tb3.Text == "")
                        {

                            if (tx.Text != "" || ty.Text != "" || tz.Text != "")
                            {

                                //  string strini = tb1.Text + ";" + tb2.Text + ";" + tb3.Text + ";" + tx.Text + ";" + ty.Text + ";" + tz.Text;
                                string nazv = tb4.Text;
                                if (tb4.Text == "")
                                {
                                    string split = tb1.Text.Split('\\').Last();
                                    nazv = split.Remove(split.Length - 4);

                                }


                                BD_Connect BD = new BD_Connect();
                                BD.path = pathBD; //укажем файл бд
                                if (model == null)
                                {
                                    log.Add("пришла модель должна быть нулл" + model);
                                    DirectoryInfo directory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;

                                    string path_copy = directory + @"\3dsObject\User\" + category.name + @"\";
                                    Directory.CreateDirectory(path_copy);

                                    log.Add("создаём директорию по пути-" + path_copy);
                                    string time = DateTime.Now.ToFileTimeUtc().ToString();

                                    string path3ds = path_copy + nazv + "_" + time + ".3ds";
                                    string pathjpg = path_copy + nazv + "_" + time + ".jpg";
                                    string pathjpgugo = path_copy + nazv + "_" + time + "_ugo.jpg";


                                    File.Copy(tb1.Text, path3ds, true);
                                    log.Add("скопировали какой то файл");
                                    if (File.Exists(tb2.Text))
                                    {
                                        File.Copy(tb2.Text, pathjpg, true);
                                        log.Add("если сущ скопировали какой то файл");
                                    }
                                    else
                                    {
                                        pathjpg = "";
                                    }
                                    if (File.Exists(tb3.Text))
                                    {
                                        log.Add("скопировали какой то файл2");
                                        File.Copy(tb3.Text, pathjpgugo, true);
                                    }
                                    else
                                    {
                                        pathjpgugo = "";
                                    }
                                    log.Add("INSERT INTO `import3ds` (`nazv`, `path3ds`, `pathjpg`, `pathjpgugo`, `x`, `y`, `z`, category) VALUES ('" +
                                        nazv + "','" +
                                        path3ds + "','" +
                                        pathjpg + "','" +
                                        pathjpgugo + "','" +
                                        tx.Text + "','" +
                                        ty.Text + "','" +
                                        tz.Text + "', '" +
                                        category.id + "')");
                                    BD.conn("INSERT INTO `import3ds` (`nazv`, `path3ds`, `pathjpg`, `pathjpgugo`, `x`, `y`, `z`, category) VALUES ('" +
                                        nazv + "','" +
                                        path3ds + "','" +
                                        pathjpg + "','" +
                                        pathjpgugo + "','" +
                                        tx.Text + "','" +
                                        ty.Text + "','" +
                                        tz.Text + "', '" +
                                        category.id + "')");
                                    //  MessageBox.Show("Объект успешно добавлен");
                                }
                                else
                                {
                                    BD.conn("UPDATE `import3ds` SET `nazv`='" + nazv + "',  `path3ds`='" + tb1.Text + "', `pathjpg`='" + tb2.Text + "', `pathjpgugo`='" + tb3.Text + "', `x`='" + tx.Text + "', `y`='" + ty.Text + "', `z`='" + tz.Text + "'  WHERE id=" + model.id);
                                    //   MessageBox.Show("Объект успешно изменён");


                                }
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("Габариты не могут быть пустыми");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ошибка Картинка УГО не существует");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка Картинка не существует");
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка 3ds файл не существует либо не указан");
                }
            }
            catch
            {
              //  MessageBox.Show("ewewewe");
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JPG Files (*.jpg)|*.jpg";
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


        private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                //Adding a gesture here
        //        viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);

                //Import 3D model file
            //    ModelImporter import = new ModelImporter();


                 device =(new HelixToolkit.Wpf.ModelImporter()).Load(model);
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
}
