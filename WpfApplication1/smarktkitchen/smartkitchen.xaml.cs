
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TreeCadN.smarktkitchen
{
    /// <summary>
    /// Логика взаимодействия для smartkitchen.xaml
    /// </summary>
    public partial class smartkitchen : Window
    {

        string[] DevicesSettings = { "svet", "retrotop_up", "none" };
        string[] images = { "svet", "retrotop_up", "none" };
        string[] spisplat = { "GN_f008d15e8850",
"GN_f008d15e8884",
"GN_f008d15e885c",
"GN_e8db84034b20",
"GN_e8db84034b30",
"GN_e8db84034dd4",
"GN_f008d15e886c",
"GN_f008d15e8868"};


        public string otvet ="";
        List<tabcontr> asdssgbbvbdasd;

        public smartkitchen(string vhod)
        {
            InitializeComponent();
            /*

                        var asdsadasd = new List<DirectoryListing>() { 

                               new DirectoryListing() { Name = "Только голосом", Path =  @"/TreeCadN;component/Foto/gns_no.jpg"      , typebtn="none"     },
                               new DirectoryListing() { Name = "Кнопка", Path =  @"/TreeCadN;component/Foto/gns_btn.jpg"       , typebtn="mehbtn"   },
                               new DirectoryListing() { Name = "ИК сенсор", Path =  @"/TreeCadN;component/Foto/gns_ir.jpg"    , typebtn="irsens"       },

                        };
                        var asdsadasd_skaf = new List<DirectoryListing>() {

                               new DirectoryListing() { Name = "Только голосом", Path =  @"/TreeCadN;component/Foto/gns_no.jpg"  , typebtn="none"      },
                               new DirectoryListing() { Name = "Кнопка", Path =  @"/TreeCadN;component/Foto/gns_btn.jpg"   , typebtn="mehbtn"         },


                        };
                       asdasdasdasdasdasd = new List<HIGT>()
                        {
                            new HIGT(){num=1, nazv="Подсветка 1",Name=asdsadasd},
                            new HIGT(){num=2,nazv="Подсветка 2",Name=asdsadasd},
                            new HIGT(){num=3,nazv="Подсветка 3",Name=asdsadasd},
                            new HIGT(){num=4,nazv="Подсветка 4",Name=asdsadasd},
                            new HIGT(){num=5,nazv="Ретротоп",Name=asdsadasd_skaf},



                        };*/

            string vstr = "1^1^Подсветка 1^mehbtn^svet;2^0^Подсветка 2^none^svet;3^0^Подсветка 3^none^svet;4^1^Подсветка 4^mehbtn^svet;5^0^Ретротоп^mehbtn^retrotop_up";
            if (vhod == "")
            {
                vstr = "1^0^Подсветка 1^none^svet;2^0^Подсветка 2^none^svet;3^0^Подсветка 3^none^svet;4^0^Подсветка 4^none^svet;5^0^Ретротоп^none^retrotop_up";
            }
            else
            {
                vstr = vhod;
            }
            otvet = vstr;

            List<HIGT> asdasdasdasdasdasd = raborstroki(vstr);



        asdssgbbvbdasd = new List<tabcontr>() { 
            new tabcontr(){name= "dev2222",valuies=asdasdasdasdasdasd }
            
            };


            tabbb2.ItemsSource = asdssgbbvbdasd;
          //  lb1.ItemsSource = asdasdasdasdasdasd;

            //lbspisplat.ItemsSource = spisplat;



       


            //   lbspisplat.Items.Refresh();

        }


        
        List<HIGT> raborstroki(string eleeee)
        {






        

            List<HIGT> sssssss = new List<HIGT>();
            /*    {
                    new HIGT(){num=1, nazv="Подсветка 1",Name=asdsadasd},
                    new HIGT(){num=2,nazv="Подсветка 2",Name=asdsadasd},
                    new HIGT(){num=3,nazv="Подсветка 3",Name=asdsadasd},
                    new HIGT(){num=4,nazv="Подсветка 4",Name=asdsadasd},
                    new HIGT(){num=5,nazv="Ретротоп",Name=asdsadasd_skaf},



                };*/


            foreach (string sssdsd in eleeee.Split(';'))
            {

           

                string[] aasss = sssdsd.Split('^');

                var asdsadasd = new List<DirectoryListing>() {

                   new DirectoryListing() { Name = "Только голосом", Path =  @"/TreeCadN;component/Foto/gns_no.jpg"      , typebtn="none"  , selected= aasss[3]=="none"?true:false   },
                   new DirectoryListing() { Name = "Кнопка", Path =  @"/TreeCadN;component/Foto/gns_btn.jpg"       , typebtn="mehbtn", selected= aasss[3]=="mehbtn"?true:false     },
                   new DirectoryListing() { Name = "ИК сенсор", Path =  @"/TreeCadN;component/Foto/gns_ir.jpg"    , typebtn="irsens"   , selected= aasss[3]=="irsens"?true:false      },

            };
                var asdsadasd_skaf = new List<DirectoryListing>() {

                   new DirectoryListing() { Name = "Только голосом", Path =  @"/TreeCadN;component/Foto/gns_no.jpg"  , typebtn="none"  , selected= aasss[3]=="none"?true:false      },
                   new DirectoryListing() { Name = "Кнопка", Path =  @"/TreeCadN;component/Foto/gns_btn.jpg"   , typebtn="mehbtn"    , selected= aasss[3]=="mehbtn"?true:false       },


            };


                if (aasss[4] == "svet")
                {
                    sssssss.Add(new HIGT() { num = aasss[0], selected = aasss[1] == "1" ? true : false, nazv = aasss[2], Name = asdsadasd, type = aasss[4] });
                }
                else if (aasss[4] == "retrotop_up")
                {
                    sssssss.Add(new HIGT() { num = aasss[0], selected = aasss[1] == "1" ? true : false, nazv = aasss[2], Name = asdsadasd_skaf, type = aasss[4] });
                }
            }





            return sssssss;
        }





        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string nomzakaza = nomzak.Text.Trim();
            if(nomzakaza.Length<5 || nomzakaza.Length>6)
            {
                MessageBox.Show("Номер заказа заполнен неверно" +nomzakaza);
                return;
            }
     



          

            string itogstr = sborstroki();

            MessageBox.Show(itogstr);


            var httpWebRequest = WebRequest.Create("https://europe-west1-giulia-novars-smart.cloudfunctions.net/getConfig/konstrSetZakaz/" + nomzakaza);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"dev1\":\""+ itogstr + "\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }






            otvet = itogstr;
            Close();

            //  MessageBox.Show(itogstr);
            //  sss.Text = itogstr;
        }



        string sborstroki()
        {
            

            string itogstr = "";
            /*
            foreach (var elems in lb1.Items)
            {


                ListBoxItem item = (ListBoxItem)lb1.ItemContainerGenerator.ContainerFromItem(elems);
                ListBox sublist = FindVisualChild<ListBox>(item);
                // MessageBox.Show(((DirectoryListing)sublist.SelectedItem).typebtn+"");

                var elem = (HIGT)elems;
                DirectoryListing asdadasdadksd = (DirectoryListing)sublist.SelectedItem;
                string type = asdadasdadksd == null ? "none" : asdadasdadksd.typebtn;// elem.Name.

                itogstr += elem.num + "^" + (item.IsSelected ? "1" : "0") + "^" + elem.nazv + "^" + type + "^" + elem.type + ";";

                // 1^0^Подсветка 1^none;2^0^Подсветка 2^none;3^0^Подсветка 3^none;4^1^Подсветка 4^none;5^0^Ретротоп^none
            }
            itogstr = itogstr.Trim(';');
            */
            return itogstr;
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj)
    where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private void lb1_Loaded(object sender, RoutedEventArgs e)
        {
          //  MessageBox.Show("loadedd");

            var lb111111 = sender as ListBox;


            foreach (var elem in lb111111.Items)
            {
                ListBoxItem item = (ListBoxItem)lb111111.ItemContainerGenerator.ContainerFromItem(elem);
                var elemss = (HIGT)elem;
                item.IsSelected =  elemss.selected;
                ListBox sublist = FindVisualChild<ListBox>(item);
                sublist.SelectedIndex = elemss.Name.FindIndex(ex=>ex.selected==true);
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var  dfgdf = raborstroki("1^1^Подсветка 1^none^svet;2^0^Подсветка 2^none^svet;3^0^Подсветка 3^none^svet;4^0^Подсветка 4^none^svet;5^0^Ретротоп^none^retrotop_up");
            int index = asdssgbbvbdasd.Count+1;
            asdssgbbvbdasd.Add(new tabcontr() { name = "dev"+ index, valuies= dfgdf });
            tabbb2.Items.Refresh();
        }

        


   
    }

    public class DirectoryListing
    {
        public string Name { get; set; }
        public bool selected { get; set; }
        public string Path { get; set; }
        public string typebtn { get; set; }
    }
    public class HIGT
    {
        public string num { get; set; }
        public bool selected { get; set; }
        public string nazv { get; set; }
        public string type { get; set; }
        public List<DirectoryListing> Name { get; set; }

    }

    public class tabcontr
    {
        public string name { get; set; }
        public List<HIGT> valuies { get; set; }


    }


}
