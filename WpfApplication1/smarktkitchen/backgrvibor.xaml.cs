using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        static  List<sposobupravl> sposuptrall = new List<sposobupravl>()
        {
          new  sposobupravl(){ name="Без дублирующего управления", typedev="none"},
          new  sposobupravl(){ name="Механическая кнопка", typedev="mehbtn"},
          new  sposobupravl(){ name="ИК датчик на взмах", typedev="irsens"},
          new  sposobupravl(){ name="ИК датчик на закрытие дверцы", typedev="irsens2"},
      
        };

        static List<sposobupravl> sposuptzapasnica = new List<sposobupravl>()
        {

          new  sposobupravl(){ name="Механическая кнопка", typedev="mehbtn"},


        };


        List<typedevice> spistypedevice1= new List<typedevice>()
        {
          new  typedevice(){ typename="Подсветка 1", selectedtype = sposuptrall[0] ,sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 2", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 3", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 4", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Запасница", selectedtype = sposuptzapasnica[0] , sposobupravl=sposuptzapasnica, visibleskyvella="Hidden"},
        };

        List<typedevice> spistypedevice2 = new List<typedevice>()
        {
          new  typedevice(){ typename="Подсветка 1", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 2", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 3", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 4", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Ретротоп",  selectedtype = sposuptzapasnica[0] ,sposobupravl=sposuptzapasnica, visibleskyvella="Hidden"},
        };

        List<typedevice> spistypedevice3 = new List<typedevice>()
        {
          new  typedevice(){ typename="Подсветка 1", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 2", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 3",  selectedtype = sposuptrall[0] ,sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 4", selectedtype = sposuptrall[0] , sposobupravl=sposuptrall, visibleskyvella="Visible"},
         // new  typedevice(){ typename="Запасница", sposobupravl=sposuptzapasnica, visibleskyvella="Hidden"},
        };
        public backgrvibor(string path)
        {
            InitializeComponent();
            MessageBox.Show(path);

            lv1.ItemsSource = spistypedevice1;
            lv2.ItemsSource = spistypedevice2;
            lv3.ItemsSource = spistypedevice3;
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

       void sobrfunk(ListView lv1istb)
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

                exp1.Add(new Exportjson() { postfix = "_" + i, type = enabled ? (i >= 5 ? "retrotop" : "svet") : "none", parametr = new Parametr() { typeupravl = typedev, skyvella = enabledskyvella } });

            }
            export.Add(exp1);

        }
        List<List<Exportjson>> export;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
          export = new List<List<Exportjson>>();


            sobrfunk(lv1);
            sobrfunk(lv2);
            sobrfunk(lv3);




            string json = JsonConvert.SerializeObject(export);

            otvet = json;
            MessageBox.Show(json);
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
