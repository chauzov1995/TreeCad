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
    /// Логика взаимодействия для filtero.xaml
    /// </summary>
    public partial class filtero : Window
    {
        public filtero()
        {
            InitializeComponent();


            List<ordini> sssss = new List<ordini>();
            sssss.Add(new ordini() { name = "asdsadasdasdasd" });
            sssss.Add(new ordini() { name = "asdsadasdasdasd" });
            sssss.Add(new ordini() { name = "asdsadasdasdasd" }); sssss.Add(new ordini() { name = "asdsadasdasdasd" }); sssss.Add(new ordini() { name = "asdsadasdasdasd" }); sssss.Add(new ordini() { name = "asdsadasdasdasd" }); sssss.Add(new ordini() { name = "asdsadasdasdasd" }); sssss.Add(new ordini() { name = "asdsadasdasdasd" }); sssss.Add(new ordini() { name = "asdsadasdasdasd" }); sssss.Add(new ordini() { name = "asdsadasdasdasd" }); sssss.Add(new ordini() { name = "asdsadasdasdasd" }); sssss.Add(new ordini() { name = "asdsadasdasdasd" }); sssss.Add(new ordini() { name = "asdsadasdasdasd" });


            fsds.ItemsSource = sssss;
        }
    }
    class ordini
    {
        public string name { get; set; }

    }
}
