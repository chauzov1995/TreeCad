using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TreeCadN.kommunikacii;

namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для dialog_univi.xaml
    /// </summary>
    public partial class dialog_univi : Window
    {
        dialuni otvet;
        public dialog_univi(dialuni otv, string sbtn1, string sbtn2, string sbtn3, string stitle, string stext, int param, double heightwin)
        {
            InitializeComponent();
            /*param 
            1-текст
            2-текстбокс
            3-текс и текстбокс
            
    */

            otvet = otv;
            if (sbtn1 != null) { btn1.Content = sbtn1; btn1.Visibility = Visibility.Visible; }
            else
            {
                btn1.Visibility = Visibility.Collapsed;
            }
            if (sbtn2 != null)
            {
                btn2.Content = sbtn2; btn2.Visibility = Visibility.Visible;
            }
            else
            {
                btn2.Visibility = Visibility.Collapsed;
            }
            if (sbtn3 != null ){ btn3.Content = sbtn3; btn3.Visibility = Visibility.Visible; }
            else
            {
                btn3.Visibility = Visibility.Collapsed;
            }
            tblock1.Text = stext;
            win.Title = stitle;
            win.Height = heightwin;


            switch (param)
            {
                case 1:
                    tb1.Visibility = Visibility.Collapsed;

                    break;
                case 2:
                    tb1.Focus();
                    tblock1.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    tb1.Focus();
                    break;



            }



        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
          
            Close();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            otvet.otvet = "NO";
            close=true;
            Close();
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text == "")
            {
                otvet.otvet = "YES";
            }
            else
            {
                otvet.otvet = tb1.Text;
            }
            close = true;
            Close();
        }
        bool close=false;
        private void win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (close == false)
            {
                otvet.otvet = "CANCEL";
            }
        }
    }
}
