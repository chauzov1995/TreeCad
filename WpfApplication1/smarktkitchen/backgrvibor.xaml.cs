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

      static  List<sposobupravl> sposuptrall = new List<sposobupravl>()
        {
          new  sposobupravl(){ name="Без дублирующего управления"},
          new  sposobupravl(){ name="Механическая кнопка"},
          new  sposobupravl(){ name="ИК датчик на взмах"},
          new  sposobupravl(){ name="ИК датчик на закрытие дверцы"},
      
        };

        static List<sposobupravl> sposuptzapasnica = new List<sposobupravl>()
        {

          new  sposobupravl(){ name="Механическая кнопка"},


        };


        List<typedevice> spistypedevice1= new List<typedevice>()
        {
          new  typedevice(){ typename="Подсветка 1", sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 2", sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 3", sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 4", sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Запасница", sposobupravl=sposuptzapasnica, visibleskyvella="Hidden"},
        };

        List<typedevice> spistypedevice2 = new List<typedevice>()
        {
          new  typedevice(){ typename="Подсветка 1", sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 2", sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 3", sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 4", sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Ретротоп", sposobupravl=sposuptzapasnica, visibleskyvella="Hidden"},
        };

        List<typedevice> spistypedevice3 = new List<typedevice>()
        {
          new  typedevice(){ typename="Подсветка 1", sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 2", sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 3", sposobupravl=sposuptrall, visibleskyvella="Visible"},
          new  typedevice(){ typename="Подсветка 4", sposobupravl=sposuptrall, visibleskyvella="Visible"},
         // new  typedevice(){ typename="Запасница", sposobupravl=sposuptzapasnica, visibleskyvella="Hidden"},
        };
        public backgrvibor()
        {
            InitializeComponent();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string finalotvet
                = (lv1.ItemsSource as List<typedevice>)[0].enabled.ToString();
            MessageBox.Show(finalotvet);
        }
    }


    public class typedevice
    {
        public string typename { get; set; }
        public bool enabled { get; set; }
        public List<sposobupravl> sposobupravl { get; set; }
        public string visibleskyvella { get; set; }
      


    }
    public class sposobupravl
    {
        public string name { get; set; }
   



    }
}
