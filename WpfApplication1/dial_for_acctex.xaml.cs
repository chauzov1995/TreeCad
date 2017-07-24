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
    /// Логика взаимодействия для dial_for_acctex.xaml
    /// </summary>
    public partial class dial_for_acctex : Window
    {
        public texnika otvet = new texnika();
        bool zakrit_ok = false;
        public dial_for_acctex(texnika item)
        {
            InitializeComponent();


            otvet = item;

            //   MessageBox.Show(otvet.OTD);

            combo1.ItemsSource = TAccessories.todelka;
            combo2.ItemsSource = TAccessories.izmer;


            rsktblo1_Copy.Text = otvet.Article;
            rsktb1.Text = otvet.nom_pp.ToString();
            rsktb2.Text = otvet.TName;
            rsktb3.Text = otvet.kolvo.ToString();

            rsktb4.Text = otvet.Prim;

            rsktb3_Copy.Text = otvet.baseprice.ToString();
            rsktb3_Copy1.Text = otvet.priceredak.ToString();

            combo1.SelectedIndex = (TAccessories.todelka.FindIndex(x => x.ID.Equals(otvet.OTD)));
            combo2.SelectedIndex = (TAccessories.izmer.FindIndex(x => x.ID.Equals(otvet.UnitsName)));




            if (otvet.Article == "***" || otvet.Article == "*" || otvet.Article == "15R***" || otvet.Article == "SAD***")
            {//если 


            }
            else
            {
                rsktb2.IsReadOnly = true;
                rsktb2.Foreground = Brushes.Gray;
                rsktb3_Copy.IsReadOnly = true;
                rsktb3_Copy.Foreground = Brushes.Gray;
                combo2.IsEnabled = false;
            }

        }

        private void combo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //  MessageBox.Show((combo1.SelectedItem as todelka).nameotd.ToString());
        }

        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            zakrit_ok = true;
            Close();
        }


        private void rsktb1_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {


            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void rsktb3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (otvet.type == "a")
            {
                if ((sender as TextBox).Text.IndexOf('.') < 0 && e.Text == ".")
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = "0123456789".IndexOf(e.Text) < 0;
                }
            }
            if (otvet.type == "t") e.Handled = "0123456789".IndexOf(e.Text) < 0;

        }



        private void rsktb3_Copy_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if ((sender as TextBox).Text.IndexOf('.') < 0 && e.Text == ".")
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = "0123456789".IndexOf(e.Text) < 0;
            }
        }

        private void rsktb3_Copy1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if ((sender as TextBox).Text.IndexOf('.') < 0 && e.Text == ".")
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = "0123456789".IndexOf(e.Text) < 0;
            }
        }

        private void rsktb3_Copy_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void rsktb3_Copy1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void rsktb3_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void rsktb1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                zakrit_ok = true;
                Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (zakrit_ok)
            {
                try
                {
                    otvet.Article = rsktblo1_Copy.Text;
                    otvet.nom_pp = Convert.ToInt32(rsktb1.Text);
                    otvet.TName = rsktb2.Text;
                    otvet.kolvo = Convert.ToSingle(rsktb3.Text);
                    otvet.OTD = (combo1.SelectedItem as todelka).ID;
                    otvet.Prim = rsktb4.Text;
                    otvet.UnitsName = (combo2.SelectedItem as todelka).ID;
                    otvet.baseprice = Convert.ToSingle(rsktb3_Copy.Text);
                    otvet.priceredak = Convert.ToSingle(rsktb3_Copy1.Text);

                    //Close();
                }
                catch
                {
                    MessageBox.Show("Некорректные данные");
                    e.Cancel = true;
                }
            }



        }
    }
}
