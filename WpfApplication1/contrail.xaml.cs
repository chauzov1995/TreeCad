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

namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для contrail.xaml
    /// </summary>
    public partial class contrail : Window
    {
        public string text_otvet="";
        public contrail(string text)
        {
            InitializeComponent();
            text_otvet = text;
            var olooasd = text_otvet.Split(':');
            if (olooasd.Length == 4)
            {
                l = double.Parse(olooasd[2]);
                a = double.Parse(olooasd[3]);
                countpaz = olooasd[1]==""?0: int.Parse(olooasd[1]);
                selbtb = olooasd[0]==""?1: int.Parse(olooasd[0]);
               
                // MessageBox.Show(text_otvet);

            }
            okrasbtn(selbtb);
            t1.Text = countpaz.ToString();
        }
        int countpaz = 0;
        double l = 0, a = 0;
        int selbtb=1;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        
           text_otvet = selbtb + ":" + countpaz  + ":"+ l + ":" + a;
          
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void t1_TextChanged(object sender, TextChangedEventArgs e)
        {
            textChangegf();
        }
       void textChangegf()
        {
            if (t1.Text != "")
            {
                countpaz = int.Parse(t1.Text);
                if (selbtb <= 2)
                {
                    t2.Text = "Общая длина пазов " + ((countpaz * (selbtb == 2 ? a : l))/1000).ToString("F2") + " м.";
                }
                else { t2.Text = ""; }
            }
        }


        void okrasbtn(int num) {

            b1.Background = null;
            b2.Background = null;
            b3.Background = null;
            b4.Background = null;
            switch (num)
            {
                case 1:
                    b1.Background = Brushes.Yellow;
                    break;
                case 2:
                    b2.Background = Brushes.Yellow;
                    break;
                case 3:
                    b3.Background = Brushes.Yellow;
                    break;
                case 4:
                    b4.Background = Brushes.Yellow;
                    break;

            }

            t1.Focus();
            t1.CaretIndex = t1.Text.Length;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            selbtb = 1;
            okrasbtn(selbtb);
            textChangegf();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            selbtb = 2;
            okrasbtn(selbtb);
            textChangegf();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            selbtb = 3;
            okrasbtn(selbtb);
            textChangegf();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            selbtb = 4;
            okrasbtn(selbtb);
            textChangegf();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешить ввод только цифр
            e.Handled = !char.IsDigit(e.Text, 0);
        }
    }
}
