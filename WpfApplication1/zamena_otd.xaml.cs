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
using static TreeCadN.neqweqe;

namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для zamena_otd.xaml
    /// </summary>
    public partial class zamena_otd : Window
    {

        object Ambiente;
        object xamb;
        List<string> spisstrok ;
        List<string> arrotd = new List<string>()
        {
           "010", "011", "012", "013", "016", "020", "021", "022", "023", "030", "041", "044", "050", "054", "060", "064", "065", "066", "067", "810", "811", "100"
        };
        neqweqe asdaad;

        public zamena_otd(ref object xAmbiente ,neqweqe asdaad)
        {
            this.Ambiente = xAmbiente;
            this.asdaad = asdaad;

          



            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            spisstrok = new List<string>();


         
            xamb = asdaad.getParam(Ambiente, "GetObject", "XAMB");// Set xamb = Ambiente.GetObject("XAMB")
            int currbox = (int)asdaad.getParamG(xamb, "nBox");//    For i = 0 To xamb.nBox-1 
            for (int i = 0; i < currbox; i++)
            {
                var box = asdaad.getParamG(xamb, "BOX", i.ToString());  // set box  = xamb.BOX(i)
                foreach (string asdasdasd in arrotd)
                {
                 
                    var istriasda = asdaad.getParam(box, "EsisteVarianteRegola", "OTDELKA" + asdasdasd);
                   
                    if ((bool)istriasda)//  if box.EsisteVarianteRegola("OTDELKA010") then
                    {

                        string asdasda = asdaad.getParamG(box, "VarRegola", "OTDELKA" + asdasdasd).ToString();
                        if (!spisstrok.Contains(asdasda))
                        {
                            spisstrok.Add(asdasda);
                            MessageBox.Show(asdasda.ToString());
                            asdaad.setParamP(box, "VarRegola", "OTDELKA" + asdasdasd, "343;0;1;1;0;1;0;;;1^2^3^4^5^6^7^22^33^39^40^37^43^54^56^60^61^62");
                        }
                       
                    }


                    //setParamP(box, "VarRegola", "OTDELKA010", "343;0;1;1;0;1;0;;;1^2^3^4^5^6^7^22^33^39^40^37^43^54^56^60^61^62");//       box.VarRegola("OTDELKA010") = "343;0;1;1;0;1;0;;;1^2^3^4^5^6^7^22^33^39^40^37^43^54^56^60^61^62"
                }
            }
        }
    }
}
