using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Collections.Specialized;
using System.Net;
using System.Xml.Linq;
using System.Runtime.InteropServices;

namespace TreeCadN
{
    [ComVisible(true)]
    public class neqweqe
    {
        public string path1 = "";
        public string str1 = "", str2 = "";

        public string papka_s_foto = Environment.CurrentDirectory + @"\";
        public string index1idotd, index2idotd;
        public string Bazis;


        public string GNOTD(string path, string str)
        {

            str1 = str;
            str2 = str;
            path1 = path;


            if (prov_vh_str_na_prigodnostb(str1) == 9)
            {
                zagruz();

            }
            else
            {
                if (str1 == "")
                {
                    str1 = "0;0;0;0;0;0;0;;;1^2^3^4^5^6^7^10^11^12^21^22^33^34^35^36^37^39^40";
                    zagruz();
                }
                else
                {
                    if (MessageBox.Show(
              "Входная строка имеет недопустимые значения, использовать значения по умолчанию?",
             "Ошибка",
              MessageBoxButton.YesNo,
              MessageBoxImage.Error) == MessageBoxResult.Yes)
                    {
                        str1 = "0;0;0;0;0;0;0;;;1^2^3^4^5^6^7^10^11^12^21^22^33^34^35^36^37^39^40";
                        zagruz();

                    }
                    else
                    {
                        MessageBox.Show("Извините, программа не может работать по таким значениям, Исправьте их вручную и попробуйте заново \r\nФормат по умолчанию 0;0;0;0;0;0;0;;;0");

                    }
                }

            }






            return str2;
        }


        public string BASIS()
        {
            Basis f_Basis = new Basis(this);
            f_Basis.ShowDialog();


            return Bazis;
        }
        public string GNPrimN(string path, int filtr, ref string text)
        {
            Prim f_prim = new Prim(path + @"\system.mdb", text);
            f_prim.ShowDialog();
            return f_prim.text_otvet;
        }
        public string GNPrimNAUTO(string path, int filtr, ref string text)
        {
            Prim2 f_prim = new Prim2(path + @"\system.mdb", text);
            f_prim.ShowDialog();
            return f_prim.text_otvet;
        }

        public string TAccess(string path, int filtr, ref string text)
        {
            TAccessories f_TAccess = new TAccessories(path + @"\system.mdb", text);
            f_TAccess.ShowDialog();
            return f_TAccess.text_otvet;
        }



        public List<UpdateUPD> UPdate = new List<UpdateUPD>();
        public void UPDATE()
        {
            log log = new log();

            //log.Add("asdsada");
            log.Add("Старт обновления!", true);

            try
            {
                WebClient client = new WebClient();
                WebProxy myProxy = new proxy_LPS().init();
                client.Proxy = myProxy;
                string path = Environment.CurrentDirectory + @"\giulianovars\procedure\updN.ini";
                FileInfo fileInf = new FileInfo(path);
                if (fileInf.Exists)//если файл существет
                {
                    log.Add("updN.ini файл существет");
                    string url;
                    System.Byte[] response;
                    string last_upd = "";
                    INIManager client_man = new INIManager(path);
                    string client_ver = client_man.GetPrivateString("GN_UPD", "last_upd");//версия клиента
                    log.Add("версия клиента" + client_ver);
                    if (client_ver != "") //если в файле есть запись о последнем обновлении
                    {
                        log.Add("если в файле есть запись о последнем обновлении");
                        //получим код авторизации
                        INIManager manager = new INIManager(Environment.CurrentDirectory + @"\ecadpro.ini");
                        string authotiz_root = manager.GetPrivateString("giulianovars", "attivazione");//получ ключ активации

                        url = "http://ecad.giulianovars.ru/php/upd/dll_prov_ver.php?upd_time=" + client_ver + "&attivazione=" + authotiz_root;
                        response = client.DownloadData(url);
                        last_upd = System.Text.Encoding.UTF8.GetString(response);
                        response = null;
                        log.Add(url);



                        if (last_upd.Length != 0)//если требуется обновление
                        {
                            log.Add("если требуется обновление last_upd=" + last_upd);
                            string krit = last_upd.Remove(1, last_upd.Length - 1);
                            log.Add("критическое -" + krit);
                            if (Properties.Update.Default.poslproch != last_upd || krit == "1")//если обновление критическое или не нажата кнопка показывать снова
                            {
                                log.Add("если обновление критическое или не нажата кнопка показывать снова");
                                string ver = last_upd.Split('=')[1];
                                response = client.DownloadData("http://ecad.giulianovars.ru/php/upd/dll_load.php?upd_time=" + client_ver);
                                log.Add("http://ecad.giulianovars.ru/php/upd/dll_load.php?upd_time=" + client_ver);
                                string save_html = System.Text.Encoding.UTF8.GetString(response);
                                File.WriteAllText(Path.GetTempPath() + "GN_upd.html", save_html, System.Text.Encoding.UTF8);

                                Update f_Update = new Update(last_upd, client_ver, krit, client, ver);
                                f_Update.ShowDialog();
                            }
                        }
                    }
                }


            }
            catch (Exception e)
            {
                //  MessageBox.Show(e.Message);
            }
        }








        public void GNLICENSE()
        {
            try
            {
                string URL_zapros = @"http://ecadrussia.alk.hr/ecadsv/RichiesteAdmin.aspx";
                string id_clienta_root, email_root, authotiz_root, moduli_root, abilitato_root;
                WebClient client = new WebClient();
                WebProxy myProxy = new proxy_LPS().init();
                client.Proxy = myProxy;

                //Создание объекта, для работы с файлом
                INIManager manager = new INIManager(Environment.CurrentDirectory + @"\ecadpro.ini");
                authotiz_root = manager.GetPrivateString("giulianovars", "attivazione");//получ ключ активации


                string url = "http://ecad.giulianovars.ru/php/license/load_page_for_dll.php?authotiz_root=" + authotiz_root;
                var response = client.DownloadData(url);
                string str = System.Text.Encoding.UTF8.GetString(response);
                string[] parse = str.Split('=');

                id_clienta_root = parse[0];
                email_root = parse[1];
                abilitato_root = parse[2];
                moduli_root = parse[3];

                license f3 = new license(id_clienta_root, email_root, authotiz_root, moduli_root, abilitato_root, client);
                f3.ShowDialog(); //блокируется основная форма
            }
            catch (WebException ex)
            {
                MessageBox.Show("Не могу подключится к интернету, измените настройки прокси");
                Proxy f_Update = new Proxy();
                f_Update.ShowDialog();
            }

        }













        void zagruz()
        {
            //try
            //{

            Window1 f1 = new Window1(this);
            f1.ShowDialog(); //блокируется основная форма

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Ошибка: " + ex.Message);
            //}
        }

        int prov_vh_str_na_prigodnostb(string str1)
        {
            int otv = 0;
            try
            {
                string index1, index2, index3, index4, index5, index6, index7, index8, index9, index10;


                string[] words = str1.Split(';');

                if (words.Length == 10)
                {
                    otv++;
                }

                index1 = words[0];//Индекс Внешней пласти
                index2 = words[1];//индекс внутренней пласти
                index3 = words[2];//Направление текстуры 1- вертик, 2-горизонтально
                index4 = words[3];//Направление текстуры Внутренней пласти
                index5 = words[4];//Флаг, 1-одинаковая отделака обеих пластей, при этом назначать разную отделку нельзя
                index6 = words[5];//Индекс Эффекта
                index7 = words[6];//Галка один отд
                index8 = words[7];//нестанд цвет
                index9 = words[8];//не исп
                index10 = words[9];// спис разреш групп для отделки

                if (Convert.ToInt32(index1) >= 0)
                {
                    otv++;
                }
                if (Convert.ToInt32(index2) >= 0)
                {
                    otv++;
                }
                if (index3 == "0" || index3 == "1")
                {
                    otv++;
                }
                if (index4 == "0" || index4 == "1")
                {
                    otv++;
                }
                if (index5 == "0" || index5 == "1")
                {
                    otv++;
                }
                if (Convert.ToInt32(index6) >= 0)
                {
                    otv++;
                }
                if (index7 == "0" || index7 == "1")
                {
                    otv++;
                }




                string[] words_index10 = index10.Split('^');
                int asda1 = words_index10.Length;
                int s = 0;
                for (int x = 0; x <= words_index10.Length - 1; x++)
                {

                    s += Convert.ToInt32(words_index10[x]);//Каждая гр отделки

                }
                if (Convert.ToInt32(s) >= 0)
                {
                    otv++;

                }

            }
            catch { }
            // MessageBox.Show(otv.ToString());
            return otv;
        }




    }



    public class log
    {
        //использование
        //new log().Add("");
        //log().Add("");
        public void Add(string str = "", bool del = false)
        {
            if (del) Clear();
            string save_log = Environment.NewLine + DateTime.Now + " - " + str;
            File.AppendAllText(Environment.CurrentDirectory + "\\TreeCadN.log", save_log, System.Text.Encoding.UTF8);
        }
        private void Clear()
        {
            File.Delete(Environment.CurrentDirectory + "\\TreeCadN.log");

        }
    }

}
