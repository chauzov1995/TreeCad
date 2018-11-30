using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TreeCadN.open_ordini
{
    /// <summary>
    /// Логика взаимодействия для srtting.xaml
    /// </summary>
    public partial class srtting : Window
    {
        string path_ordini;
        neqweqe neqqqqq;
        List<spissettings> spiscolumn = new List<spissettings>();
        public srtting(neqweqe _neqqqqq, string _path_ordini)
        {
            InitializeComponent();

            path_ordini = _path_ordini;
            neqqqqq = _neqqqqq;


            Settings1 ps = Settings1.Default;

         
            spiscolumn.Add(new spissettings() { Value = "№ заказа", IsSelected = ps.spisotobrstolb[0] == '1' });
            spiscolumn.Add(new spissettings() { Value = "ФИО клиента", IsSelected = ps.spisotobrstolb[1] == '1' });
            spiscolumn.Add(new spissettings() { Value = "Сумма", IsSelected = ps.spisotobrstolb[2] == '1' });
            spiscolumn.Add(new spissettings() { Value = "Дата изготовления", IsSelected = ps.spisotobrstolb[3] == '1' });
            spiscolumn.Add(new spissettings() { Value = "Номер в салоне", IsSelected = ps.spisotobrstolb[4] == '1' });
            spiscolumn.Add(new spissettings() { Value = "Номер на фабрике", IsSelected = ps.spisotobrstolb[5] == '1' });
            spiscolumn.Add(new spissettings() { Value = "Менеджер в салоне", IsSelected = ps.spisotobrstolb[6] == '1' });
            spiscolumn.Add(new spissettings() { Value = "Салон", IsSelected = ps.spisotobrstolb[7] == '1' });

            lb1.ItemsSource = spiscolumn;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
        "Нажмите \"ОК\", чтобы обновить список заказов",
       "Внимание",
        MessageBoxButton.OKCancel,
        MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                SQLiteConnection m_dbConn = new SQLiteConnection();
                SQLiteCommand m_sqlCmd = new SQLiteCommand();

                string dbFileName = path_ordini + @"\sample.sqlite";

                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;


                string[] files = Directory.GetFiles(path_ordini, "*.eve", SearchOption.TopDirectoryOnly);

                //   Array.Sort(files);

                object xamb = neqqqqq.getParam(neqqqqq.Ambiente, "GetObject", "XAMB");
                object engine = neqqqqq.getParam(neqqqqq.Ambiente, "GetObject", "ENGINE");
                object info = neqqqqq.getParamG(xamb, "INFO");
                object info2 = neqqqqq.getParamG(info, "INFO");


                m_sqlCmd.CommandText = "DELETE FROM ordini";
                m_sqlCmd.ExecuteNonQuery();

                foreach (string file in files)
                {
                    string nomfile = file.Split('\\').Last().Split('.').First();
                    neqqqqq.getParam(xamb, "carica", file);
                    //    string newnum = neqqqqq.getParamI(info, "NuovoNumeroOrdine").ToString();
                    //    neqqqqq.setParamP(info, "Numero", newnum);
                    //    neqqqqq.getParam(info2, "Add", "_NOMEFILEPARETI", nomfile);
                    //    neqqqqq.getParamI(xamb, "salva");//сохраним

                    string FIO = neqqqqq.getParam(info2, "Var", "CLI_1").ToString();
                    string Manager = neqqqqq.getParam(info2, "Var", "Manager").ToString();
                    string orderprice = neqqqqq.getParam(info2, "Var", "orderprice").ToString().Trim();
                    string _RIFFABRICA = neqqqqq.getParam(info2, "Var", "_RIFFABRICA").ToString();
                    string _RIFSALON = neqqqqq.getParam(info2, "Var", "_RIFSALON").ToString();
                    string SROK = neqqqqq.getParam(info2, "Var", "SROK").ToString();
                    string SALON = neqqqqq.getParam(info2, "Var", "SALON").ToString();


                    m_sqlCmd.CommandText = "INSERT OR IGNORE INTO ordini (file_path, nomer_zakaza, FIO, manager, orderprice, _RIFFABRICA, _RIFSALON, SROK, SALON) " +
                        "VALUES ('" + file + "', '" + nomfile + "','" + FIO + "','" + Manager + "', '" + orderprice + "', '" + _RIFFABRICA + "', '" + _RIFSALON + "', '" + SROK + "', '"+ SALON + "')";
                    m_sqlCmd.ExecuteNonQuery();





                    string pathtmp = path_ordini + @"\" + nomfile;
                    string GetFileBitmap = neqqqqq.getParam(xamb, "GetFileBitmap", pathtmp + ".DRG1").ToString();


                    if (GetFileBitmap.ToUpper() == "TRUE")
                    {
                        object imgget = neqqqqq.getParam(neqqqqq.Ambiente, "GetObject", "DauImg");
                        object GetPicture = neqqqqq.getParam(engine, "GetPicture", pathtmp + ".DRG1", "0", "0");
                        imgget.GetType().InvokeMember("SetPicture", BindingFlags.InvokeMethod, null, imgget, new object[] { GetPicture, "0" });
                        neqqqqq.getParam(imgget, "SaveImage", pathtmp + ".JPG", "1");


                    }



                }
         

                m_sqlCmd.Dispose();
                m_dbConn.Close();
                GC.Collect();


                MessageBox.Show("Готово");

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string itog = "";
            foreach(spissettings spiscolum in spiscolumn)
            {
                itog += spiscolum.IsSelected ? '1' :'0';

            }



            Settings1 ps = Settings1.Default;
            ps.spisotobrstolb = itog;
            ps.Save();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    class spissettings
    {
        public string splitid { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }
}
