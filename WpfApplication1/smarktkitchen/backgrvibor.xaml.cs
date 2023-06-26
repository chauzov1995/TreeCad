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

namespace TreeCadN.smarktkitchen
{
    /// <summary>
    /// Логика взаимодействия для backgrvibor.xaml
    /// </summary>
    public partial class backgrvibor : Window
    {
        public string otvet = "";
        public string nomerzakazafabrik = "";
    

        static  List<sposobupravl> sposuptrall = new List<sposobupravl>()
        {
          new  sposobupravl(){ name="Без дублирующего управления", typedev="none"},
          new  sposobupravl(){ name="Механическая кнопка", typedev="mehbtn"},
          new  sposobupravl(){ name="ИК датчик на взмах", typedev="irsens"},
          new  sposobupravl(){ name="ИК датчик на закрытие дверцы", typedev="irsens2"},
      
        };

        static List<sposobupravl> sposuptzapasnica = new List<sposobupravl>()
        {
          new  sposobupravl(){ name="Без дублирующего управления", typedev="none"},
          new  sposobupravl(){ name="Механическая кнопка", typedev="mehbtn"},
         

        };


        List<typedevice> spistypedevice1= new List<typedevice>()
        {
          new  typedevice(){ typename="Подсветка в запасницу", selectedtype = sposuptrall[0] ,sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 2", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 3", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 4", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Запасница", selectedtype = sposuptzapasnica[0] , sposobupravl=sposuptzapasnica, visibleskyvella="Hidden"},
        };

        List<typedevice> spistypedevice2 = new List<typedevice>()
        {
          new  typedevice(){ typename="Подсветка в ретро-топ", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 2", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 3", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 4", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Ретро-топ (тип 5)",  selectedtype = sposuptzapasnica[0] ,sposobupravl=sposuptzapasnica, visibleskyvella="Hidden"},
        };

        List<typedevice> spistypedevice3 = new List<typedevice>()
        {
          new  typedevice(){ typename="Подсветка 1", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 2", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 3",  selectedtype = sposuptrall[0] ,sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 4", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
         // new  typedevice(){ typename="Запасница", sposobupravl=sposuptzapasnica, visibleskyvella="Hidden"},
        };
        bool konstruktor = false;
        public backgrvibor(string path, string _RIFFABRICA, bool konstruktor)
        {
            InitializeComponent();

           // MessageBox.Show(path);
            this.otvet = path;
            this.konstruktor = konstruktor;
            Title = "Smart Kitchen" + (konstruktor?" конструктор":"");
            nomerzakazafabrik = _RIFFABRICA;
            //MessageBox.Show(nomerzakazafabrik);

            
           // if (!getsavedannie(nomerzakazafabrik))
           if(true)
            {
                if (path == "")
                {
                    lv1.IsEnabled = false;
                    lv2.IsEnabled = false;
                    lv3.IsEnabled = false;
                }
                else
                {
                    try { razobrfunc(path.Replace('^', ',')); } catch (Exception e) { MessageBox.Show(e.Message); }
                }
            }
            

            lv1.ItemsSource = spistypedevice1;
            lv2.ItemsSource = spistypedevice2;
            lv3.ItemsSource = spistypedevice3;
        }

        void razobrfunc(string oldstroka)
        {
           List< Exportcontroller> m = JsonConvert.DeserializeObject<List<Exportcontroller>>(oldstroka);

            cb1.IsChecked = m[0].enable;
            lv1.IsEnabled = m[0].enable;
            for (int i = 0; i < m[0].exportjson.Count; i++) { 
                Exportjson controller=m[0].exportjson[i];

                //new typedevice() { typename = "Подсветка 1", selectedtype = sposuptrall[0], sposobupravl = sposuptrall, visibleskyvella = "Visible" },
                spistypedevice1[i].enabled = controller.type!="none";
                spistypedevice1[i].enabledskyvella = controller.parametr.skyvella;
                if (i == 4) { spistypedevice1[i].selectedtype = sposuptzapasnica.Find(x => x.typedev == controller.parametr.typeupravl); } else
                {
                    spistypedevice1[i].selectedtype = sposuptrall.Find(x => x.typedev == controller.parametr.typeupravl);
                }
              
            }

            cb2.IsChecked = m[1].enable;
            lv2.IsEnabled = m[1].enable;
            for (int i = 0; i < m[1].exportjson.Count; i++)
            {
                Exportjson controller = m[1].exportjson[i];

                //new typedevice() { typename = "Подсветка 1", selectedtype = sposuptrall[0], sposobupravl = sposuptrall, visibleskyvella = "Visible" },
                spistypedevice2[i].enabled = controller.type != "none";
                spistypedevice2[i].enabledskyvella = controller.parametr.skyvella;
                if (i == 4) { spistypedevice2[i].selectedtype = sposuptzapasnica.Find(x => x.typedev == controller.parametr.typeupravl); }
                else
                {
                    spistypedevice2[i].selectedtype = sposuptrall.Find(x => x.typedev == controller.parametr.typeupravl);
                }

            }


            cb3.IsChecked = m[2].enable;
            lv3.IsEnabled = m[2].enable;
            for (int i = 0; i < m[2].exportjson.Count; i++)
            {
                Exportjson controller = m[2].exportjson[i];

                //new typedevice() { typename = "Подсветка 1", selectedtype = sposuptrall[0], sposobupravl = sposuptrall, visibleskyvella = "Visible" },
                spistypedevice3[i].enabled = controller.type != "none";
                spistypedevice3[i].enabledskyvella = controller.parametr.skyvella;
                if (i == 4) { spistypedevice3[i].selectedtype = sposuptzapasnica.Find(x => x.typedev == controller.parametr.typeupravl); }
                else
                {
                    spistypedevice3[i].selectedtype = sposuptrall.Find(x => x.typedev == controller.parametr.typeupravl);
                }

            }
            //  MessageBox.Show(m[0].nomerkontr.ToString());
        }

        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            lv1.IsEnabled =true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            lv1.IsEnabled = false;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            lv2.IsEnabled = true;
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            lv2.IsEnabled = false;
        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            lv3.IsEnabled = true;
        }

        private void CheckBox_Unchecked_2(object sender, RoutedEventArgs e)
        {
            lv3.IsEnabled = false;
        }

       void sobrfunk(ListView lv1istb, bool enabledctr, int nomerkontr)
        {

         

            List<Exportjson> exp1 = new List<Exportjson>();

            List<typedevice> typedevices = lv1istb.ItemsSource as List<typedevice>;
            int i = 0;
            foreach (typedevice typedevice in typedevices)
            {
                
                i++;
                bool enabled = typedevice.enabled;
                bool enabledskyvella = typedevice.enabledskyvella;
                string typedev = typedevice.selectedtype.typedev;
               
                exp1.Add(new Exportjson() { postfix = "_" + i, type = enabled ? (i >= 5 ? "retrotop_up" : "svet") : "none", parametr = new Parametr() { typeupravl = typedev, skyvella = enabledskyvella } });

            }
            export.Add(new Exportcontroller(){enable= enabledctr, nomerkontr= nomerkontr, exportjson = exp1 });

        }


     public   List<Exportcontroller> export;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
          export = new List<Exportcontroller>();


            

            sobrfunk(lv1,cb1.IsChecked.Value, 1);
            sobrfunk(lv2, cb2.IsChecked.Value, 2);
            sobrfunk(lv3, cb3.IsChecked.Value, 3);




            string json = JsonConvert.SerializeObject(export);

            otvet = json.Replace(',','^');

            if (konstruktor)
            {
                if (nomerzakazafabrik == "")
                {
                    MessageBox.Show("Заказу не присвоен фабричный номер. Состав умной кухни будет сохранён локально.");
                }
                else
                {
                    newzakaz(nomerzakazafabrik, json);
                }
            }
         
     


            //
          //  MessageBox.Show(otvet);
            Close();
        }


         bool getsavedannie(string zakaz)
        {
            if(zakaz == "")return false;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://smart.giulianovars.ru/api/app/v1.0/treecad/getzakaz");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))//ss
            {
                string jsonsend = "{\"zakaz\":" + zakaz + "}";

                streamWriter.Write(jsonsend);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    //    MessageBox.Show(result);

                    if (result == "1")
                    {

                    }
                    else
                    {
                        try { razobrfunc(result); } catch (Exception e) { MessageBox.Show(e.Message); }
                    }

                }
                return true;
            }
            else
            {
                return false;
            }
        }




        static void newzakaz(string zakaz,string json)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://smart.giulianovars.ru/api/app/v1.0/treecad/zakaznew");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))//ss
            {
                string jsonsend = "{\"zakaz\":"+ zakaz + "," +
                              "\"struktura\":" + json + "}";

                streamWriter.Write(jsonsend);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();
                // MessageBox.Show(result);
                if (result == "1")
                {
                    if (MessageBoxResult.OK == MessageBox.Show("Для этого заказа, состав умной кухни был сохранён ранее. Вы хотите изменить состав?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning))
                    {
                        

                        updatezakaz(zakaz, json);
                    }
                }
             
            }
        }

        static void updatezakaz(string zakaz, string json)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://smart.giulianovars.ru/api/app/v1.0/treecad/zakazupdate");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string jsonsend = "{\"zakaz\":" + zakaz + "," +
                               "\"struktura\":" + json + "}";

                streamWriter.Write(jsonsend);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

              //  MessageBox.Show(result);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

   
    }





    public class typedevice
    {
         
        public string typename { get; set; }
    
        public bool enabled { get; set; }
        public bool enabledskyvella { get; set; }
        public List<sposobupravl> sposobupravl { get; set; }
        public sposobupravl selectedtype { get; set; }
        public string visibleskyvella { get; set; }
      


    }
    public class sposobupravl
    {
        public string name { get; set; }
   
            public string typedev { get; set; }


    }


    public partial class Exportcontroller
    {
     
        public int nomerkontr { get; set; }
        public bool enable { get; set; }
        public List<Exportjson> exportjson { get; set; }
    }

    public partial class Exportjson
    {
        public string postfix { get; set; }
        public string type { get; set; }
        public Parametr parametr { get; set; }
    }

    public partial class Parametr
    {
        public string typeupravl { get; set; }
   
        public bool skyvella { get; set; }
    }

}
