using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для licenziidrugie.xaml
    /// </summary>
    /// 

    public partial class licenziidrugie : Window
    {


        List<btn_spis_lic> spisbtn = new List<btn_spis_lic>();

        string id_clienta_root;

        public licenziidrugie(string id_clienta_root)
        {
            InitializeComponent();

            this.id_clienta_root = id_clienta_root;


            btn_spis_lic elemnew = new btn_spis_lic() { idlic = id_clienta_root };

            spisbtn.Add(elemnew);


         //   lb_vibr_tex.Items.Add(elemnew);
        //    lb_vibr_tex.Items.Add(elemnew);
        //    lb_vibr_tex.Items.Add(elemnew);
         //   lb_vibr_tex.Items.Add(elemnew);


          //  firstload("64166");
            firstload(id_clienta_root);

        }
      void   firstload(String idsalon)
        {


            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://ecad.giulianovars.ru/php/salons/dll_getsalon_o.php?id_clienta_root=" + idsalon);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";


            List<jsonneobr> jsonObject;
           var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                jsonObject = JsonConvert.DeserializeObject<List<jsonneobr>>(streamReader.ReadToEnd());
               
            }
            foreach (var obj in jsonObject)
            {
             //   Console.WriteLine(obj.email_root);
                btn_spis_lic elemnew = new btn_spis_lic() { 
                
                    idlic = obj.id_clienta_root,
                    kompany_root = obj.kompany_root,
                    email = obj.email_root,              
                    osnov= "Включено",
                    render = obj.moduli_root[3]=='1'? "Включено" : "",
                    trids = obj.moduli_root[0] == '1' ? "Включено" : "",
                    dwg = obj.moduli_root[1] == '1' ? "Включено" : "",
                    sketchup = obj.moduli_root[8] == '1' ? "Включено" : "",
                    predmeti = obj.moduli_root[11] == '1' ? "Включено" : "",





                };



                lb_vibr_tex.Items.Add(elemnew);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lb_vibr_tex.SelectAllCells();
            lb_vibr_tex.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, lb_vibr_tex);
            lb_vibr_tex.UnselectAllCells();
            String result = (string)Clipboard.GetData(DataFormats.Text);
        //    Clipboard.SetText(result);
/*
            // Configure save file dialog box
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            bool? result2 = dialog.ShowDialog();

            // Process save file dialog box results
            if (result2 == true)
            {
                // Save document
                string filename = dialog.FileName;
                File.WriteAllText(filename, result, UnicodeEncoding.UTF8);
            }
*/
         

        }
    }


    class jsonneobr
    {
        public string id_clienta_root { get; set; }
        public string email_root { get; set; }
        public string moduli_root { get; set; }
        public string abilitato_root { get; set; }
        public string kompany_root { get; set; }




    }

    class btn_spis_lic
    {
        public string idlic { get; set; }
        public string kompany_root { get; set; }
        public string email { get; set; }
        public string osnov { get; set; }
        public string render { get; set; }
        public string trids { get; set; }
        public string dwg { get; set; }
        public string sketchup { get; set; }
        public string predmeti { get; set; }
       


    }
}
