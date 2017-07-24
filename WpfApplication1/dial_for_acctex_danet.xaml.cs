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

namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для dial_for_acctex_danet.xaml
    /// </summary>
    public partial class dial_for_acctex_danet : Window
    {
        TAccessories otvet ;
        public dial_for_acctex_danet(TAccessories main)
        {
            InitializeComponent();
            otvet= main;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            otvet.redakilidob = 1;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            otvet.redakilidob = 2;
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
