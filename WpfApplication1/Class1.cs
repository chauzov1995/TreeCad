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
using TreeCadN.evesync;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace TreeCadN
{
    [ComVisible(true)]
    public class neqweqe
    {
        //  public string path1 = "";
        //   public string str1 = "", str2 = "";

        public string papka_s_foto = Environment.CurrentDirectory + @"\";
        public string index1idotd, index2idotd;
        public string Bazis;
        public object Ambiente;
        public object xamb, aamain, engine;
        public string pathordini;

        public string attiva(String s, String Valori, String DES, String PX, String PY, String param)
        {
            string returnValue = "";
            try
            {
                string katalog = getParamI(Ambiente, "xPercorso").ToString();
                if (s == null) s = "";
                switch (param)
                {
                    case "TAccess":
                        returnValue = TAccess(GetPathMDB(katalog), 1, s);
                        break;
                    case "Kommunikacii":
                        returnValue = Kommunikacii(GetPathMDB(katalog));
                        break;
                    case "evesync":
                        evesync(GetPathMDB(katalog));
                        break;
                    case "GNviewer":
                        GNviewer(Environment.CurrentDirectory + @"\GIULIANOVARS\procedure");
                        break;
                    case "uploadPROGR":
                        var ini = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
                        var path_ecadpro = ini.GetPrivateString("Infogen", "percorsoordini");//версия клиента
                        uploadPROGR(Environment.CurrentDirectory + @"\" + path_ecadpro);
                        break;
                    case "zenakorpvspom":
                        returnValue = zenakorp(Environment.CurrentDirectory + @"\" + katalog + @"\PROCEDURE\3CadBase.sqlite", s);
                        break;
                    case "findprice":



                        this.xamb = getParam(Ambiente, "GetObject", "XAMB");
                        object info = getParamG(xamb, "info");
                        string modello = getParam(info, "modello").ToString();

                       
                        string b = GNfindprice(Environment.CurrentDirectory + @"\" + katalog + @"\PROCEDURE\3CadBase.sqlite", modello);
                       
                        setParamP(Ambiente, "Codice", "");//очистить название артикула
                        getParam(Ambiente, "CaricaRiga2", "s", b);//заменить артикул или создать новый
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return returnValue;
        }
        public void SetAmbiente(ref object x)
        {

            //перед аттиваой устанавливается амбиенти
            Ambiente = x;
        }
        public void Termina()
        {

        }
        #region GetParam

        public void Param(object obj, string param, string value)
        {
            obj.GetType().InvokeMember(param, BindingFlags.InvokeMethod, null, obj, new object[] { value });
        }

        public void Param(object obj, string param)
        {
            obj.GetType().InvokeMember(param, BindingFlags.InvokeMethod, null, obj, new object[0]);
        }

        public object getParamI(object obj, string param)
        {
            var obj1 = obj.GetType().InvokeMember(param, BindingFlags.InvokeMethod, null, obj, new object[0]);

            if (obj1 != null)
            {
                return obj1;
            }
            else
            {
                return "";
            }

        }

        public object getParam(object obj, string param)
        {
            var obj1 = obj.GetType().InvokeMember(param, BindingFlags.GetProperty, null, obj, new object[0]);

            if (obj1 != null)
            {
                return obj1;
            }
            else
            {
                return "";
            }

        }

        public object getParam(object obj, string param, string value)
        {
            var obj1 = obj.GetType().InvokeMember(param, BindingFlags.InvokeMethod, null, obj, new object[] { value.ToString() });

            if (obj1 != null)
            {
                return obj1;
            }
            else
            {
                return "";
            }

        }

        public object getParam(object obj, string param, string value, string value1)
        {
            var obj1 = obj.GetType().InvokeMember(param, BindingFlags.InvokeMethod, null, obj, new object[] { value.ToString(), value1.ToString() });

            if (obj1 != null)
            {
                return obj1;
            }
            else
            {
                return "";
            }

        }

        public object getParam(object obj, string param, string value, string value1, string value2)
        {
            var obj1 = obj.GetType().InvokeMember(param, BindingFlags.InvokeMethod, null, obj, new object[] { value.ToString(), value1.ToString(), value2.ToString() });

            if (obj1 != null)
            {
                return obj1;
            }
            else
            {
                return "";
            }

        }

        public object getParam(object obj, string param, string value, string value1, string value2, string value3)
        {
            var obj1 = obj.GetType().InvokeMember(param, BindingFlags.InvokeMethod, null, obj, new object[] { value.ToString(), value1.ToString(), value2.ToString(), value3.ToString() });

            if (obj1 != null)
            {
                return obj1;
            }
            else
            {
                return "";
            }

        }

        public object getParamG(object obj, string param)
        {
            var obj1 = obj.GetType().InvokeMember(param, BindingFlags.GetProperty, null, obj, new object[0]);

            if (obj1 != null)
            {
                return obj1;
            }
            else
            {
                return "";
            }

        }

        public object getParamG(object obj, string param, string value)
        {
            var obj1 = obj.GetType().InvokeMember(param, BindingFlags.GetProperty, null, obj, new object[] { value.ToString() });

            if (obj1 != null)
            {
                return obj1;
            }
            else
            {
                return "";
            }

        }

        public void setParam(object obj, string param, string param1, string value)
        {
            //setParam(xamb, "curbox", node.GetDisplayText(column).ToString());
            obj.GetType().InvokeMember(param, BindingFlags.InvokeMethod, null, obj, new object[] { param1, value.ToString() });

        }

        public void setParam(object obj, string param, string value)
        {
            //setParam(xamb, "curbox", node.GetDisplayText(column).ToString());
            obj.GetType().InvokeMember(param, BindingFlags.SetField, null, obj, new object[] { value.ToString() });

        }

        public void setParamP(object obj, string param, string value)
        {
            //setParam(xamb, "curbox", node.GetDisplayText(column).ToString());
            obj.GetType().InvokeMember(param, BindingFlags.SetProperty, null, obj, new object[] { value.ToString() });

        }


        #endregion

        public string GNOTD(string path, string str)//для старых версий тех у кого ещё стоит старый размерный ряд
        {


            string str1 = str;
            string str2 = str;
            string path1 = path;
            bool uslovvipol = false;


            if (prov_vh_str_na_prigodnostb(str1) == 9)
            {
                uslovvipol = true;
            }
            else
            {
                if (str1 == "")
                {
                    str1 = "0;0;0;0;0;0;0;;;1^2^3^4^5^6^7^22^33^37^40^46^50^51";
                    uslovvipol = true;
                }
                else
                {
                    if (MessageBox.Show(
              "Входная строка имеет недопустимые значения, использовать значения по умолчанию?",
             "Ошибка",
              MessageBoxButton.YesNo,
              MessageBoxImage.Error) == MessageBoxResult.Yes)
                    {
                        str1 = "0;0;0;0;0;0;0;;;1^2^3^4^5^6^7^22^33^37^40^46^50^51";
                        uslovvipol = true;
                    }
                    else
                    {
                        MessageBox.Show("Извините, программа не может работать по таким значениям, Исправьте их вручную и попробуйте заново \r\nФормат по умолчанию 0;0;0;0;0;0;0;;;0");
                    }
                }
            }
            if (uslovvipol)
            {
                Window1 f1 = new Window1(path1, str1);
                f1.ShowDialog(); //блокируется основная форма
                return f1.str2;
            }
            else
            {
                return str2;
            }


        }



        public string GNOTD1(ref object xAmbiente, string str)//для новых версий
        {
            this.Ambiente = xAmbiente;
            string katalog = getParamI(Ambiente, "xPercorso").ToString();
            string path = GetPathMDB(katalog);
            return GNOTD(path, str);
        }


        public string BASIS()
        {
            Basis f_Basis = new Basis(this);
            f_Basis.ShowDialog();


            return Bazis;
        }
        public string GNPrimN1(ref object xAmbiente, int filtr, ref string text)
        {
            this.Ambiente = xAmbiente;
            string katalog = getParamI(Ambiente, "xPercorso").ToString();

            return GNPrimN(GetPathMDB(katalog), filtr, ref text);

        }
        public string GNPrimN(string path, int filtr, ref string text)
        {

            if (path.Split('.').Last() != "mdb") path += @"\system.mdb";


            Prim f_prim = new Prim(path, text);
            f_prim.ShowDialog();
            return f_prim.text_otvet;
        }




        public string GNfastbuild()
        {


            fastbuild.fast_build f_prim = new fastbuild.fast_build();
            f_prim.Show();

            return "";
        }

        public string GNfastbuild1(ref object xAmbiente)
        {


            fastbuild.fast_build f_prim = new fastbuild.fast_build();
            f_prim.Show();

            return "";
        }


        public string GNfindprice(string path, string modello)
        {

            findprice.findprice f_prim = new findprice.findprice(path, modello);
            f_prim.ShowDialog();

            return f_prim.text_otvet;
        }

        public string GNfindprice1(ref object xAmbiente)
        {
            this.Ambiente = xAmbiente;
            this.xamb = getParam(Ambiente, "GetObject", "XAMB");
            string katalog = getParamI(Ambiente, "xPercorso").ToString();

            if (katalog.ToUpper() == "GIULIANOVARSA")
            {
                string path = Environment.CurrentDirectory + @"\" + katalog + @"\PROCEDURE\3CadBase.sqlite";

                object info = getParamG(xamb, "info");
                string modello = getParam(info, "modello").ToString();
                
                findprice.findprice f_prim = new findprice.findprice(path, modello);
                f_prim.ShowDialog();



                setParamP(Ambiente, "Codice", "");//очистить



                getParam(Ambiente, "CaricaRiga2", "s", f_prim.text_otvet);

                return f_prim.text_otvet;
            }
            else
            {
                MessageBox.Show("Поиск работает только в новой базе");
                return "";
            }
        }


        public string GNPrimNAUTO(ref object xAmbiente, int filtr, ref string text)
        {
            this.Ambiente = xAmbiente;
            string katalog = getParamI(Ambiente, "xPercorso").ToString();

            Prim2 f_prim = new Prim2(GetPathMDB(katalog), text);
            f_prim.ShowDialog();
            return f_prim.text_otvet;
        }

        public string TAccess(string path, int filtr, string text)
        {
            //  MessageBox.Show(path);
            TAccessories f_TAccess = new TAccessories(path, text);
            f_TAccess.ShowDialog();
            return f_TAccess.text_otvet;
        }
        public string zenakorp(string path, string param)
        {
            //  MessageBox.Show(param);
            zenakorp.zenakorp f_TAccess = new zenakorp.zenakorp(path, param);
            f_TAccess.ShowDialog();
            return "asd";
        }


        public string Kommunikacii(string path)
        {

            kommunikacii.kommunikacii_main f_kommunikacii = new kommunikacii.kommunikacii_main(path);
            f_kommunikacii.ShowDialog();
            log.Add(f_kommunikacii.selectedModel);
            return f_kommunikacii.selectedModel;
        }

        public void evesync(string path)
        {
            evesync.evesync f_evesync = new evesync.evesync(path, path);
            f_evesync.ShowDialog();


        }
        public void GNviewer(string path)
        {


            string otvet = "";
            string patholdtv = "";



            // string path = @"C:\evolution\giulianovars\GIULIANOVARS\procedure";
            string[] dirs = Directory.GetFiles(path, "TeamViewerQS*.exe");


            foreach (string dir in dirs)
            {
                // if (dir.Split('\\').Last().Substring(0, 12) == "TeamViewerQS")
                // {
                if (dir.Split('\\').Last() == "TeamViewerQS.exe")
                {


                }
                else
                {

                    // File.Move(dir, path + "\\TeamViewerQS.exe", true);//востановили
                }
                patholdtv = dir;
                //   otvet = path;
                //  break;

                //  }

            }

            WebClient webClient = new WebClient();
            string url;
            url = "https://webapi.teamviewer.com/api/v1/sessions";
            webClient.Headers.Add("Authorization", "Bearer 2875381-8jCmpdsLcQm5FelCc9rv");
            webClient.Headers.Add("Content-Type", "application/json");


            string JSON = webClient.UploadString(url, "POST", @"{""groupid"" : ""g18888010""}");
            session_TV login = JsonConvert.DeserializeObject<session_TV>(JSON);

            string code = login.code.Replace("-", "");
            string newpathtv = path + @"\TeamViewerQS-id" + code + ".exe";

            File.Copy(patholdtv, newpathtv, true);

            Process.Start(newpathtv);


            //GNviewer.teamv timview = new GNviewer.teamv("");
            //  timview.ShowDialog();


        }
        public void evesync_save(ref object xAmbiente)
        {
            YA ps = YA.Default;
            if (ps.isSave)
            {
                INIManager client_man;
                string path_sysdba;
                client_man = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
                path_sysdba = client_man.GetPrivateString("Infogen", "percorsoordini");//версия клиента

                this.Ambiente = xAmbiente;
                this.xamb = getParam(Ambiente, "GetObject", "XAMB");


                object info = getParamG(xamb, "info");

                string nomzakaza = getParam(info, "Numero").ToString();
                int skolko0 = 6 - nomzakaza.Length;
                string evepath = Environment.CurrentDirectory + @"\" + path_sysdba + @"\";
                for (int i = 0; i < skolko0; i++)
                {
                    evepath += "0";
                }
                evepath += nomzakaza + ".eve";



                string katalog = getParamI(Ambiente, "xPercorso").ToString();



                evesync.saveeve saveeve = new evesync.saveeve(GetPathMDB(katalog), evepath);
                saveeve.Show();
            }
        }


        public void uploadPROGR(string path)
        {
            //получим номер заказа  
            this.xamb = getParam(Ambiente, "GetObject", "XAMB");
            object info = getParamG(xamb, "info");
            string nomzakaza = getParam(info, "Numero").ToString();
            int skolko0 = 6 - nomzakaza.Length;
            string evename = "";
            for (int i = 0; i < skolko0; i++)
            {
                evename += "0";
            }
            evename += nomzakaza + ".eve";
            string evepath = path + @"\" + evename;


            uploadPROGR.uploadPROGR sss = new uploadPROGR.uploadPROGR(evepath, this);
            sss.ShowDialog();



        }

        public List<UpdateUPD> UPdate = new List<UpdateUPD>();

        public void UPDATE1(ref object xAmbiente)//для новой
        {
            this.Ambiente = xAmbiente;
            CATALOGGN = getParamI(Ambiente, "xPercorso").ToString();
            UPDATE();

        }
        string CATALOGGN = "Giulianovars";
        public void UPDATE()
        {
            // this.Ambiente = xAmbiente;
            //  string catalog = "Giulianovars";// getParamI(Ambiente, "xPercorso").ToString();


            log.Add("Старт обновления!");

            try
            {

                string catalog = CATALOGGN;
                log.Add("обновление dll treecadN ред");
                Obnov_dll_N.Create(catalog);//обновление dll treecadN ред

                WebClient client = new WebClient();
                WebProxy myProxy = new proxy_LPS().init();
                client.Proxy = myProxy;
                string path = Environment.CurrentDirectory + @"\" + catalog + @"\procedure\updN.ini";
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

                        evesync.YA ps = YA.Default;


                        url = "http://ecad.giulianovars.ru/php/upd/dll_prov_ver.php?upd_time=" + client_ver + "&attivazione=" + authotiz_root + "&yadisk=" + ps.OAuth;
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




                string client_ver = "";
                string path = Environment.CurrentDirectory + @"\giulianovars\procedure\updN.ini";
                if (File.Exists(path))
                {
                    INIManager client_man = new INIManager(path);
                    client_ver = client_man.GetPrivateString("GN_UPD", "last_upd");//версия клиента
                }


                string url = "http://ecad.giulianovars.ru/php/license/load_page_for_dll.php?authotiz_root=" + authotiz_root + "&upd_time=" + client_ver;
                var response = client.DownloadData(url);
                string str = System.Text.Encoding.UTF8.GetString(response);
                string[] parse = str.Split('=');

                id_clienta_root = parse[0];
                email_root = parse[1];
                abilitato_root = parse[2];
                moduli_root = parse[3];

                string actual_ver = "Актуальная ver " + parse[4];
                string tekver = "Текущяя ver " + parse[5];

                license f3 = new license(id_clienta_root, email_root, authotiz_root, moduli_root, abilitato_root, client, actual_ver, tekver);
                f3.ShowDialog(); //блокируется основная форма
            }
            catch (WebException ex)
            {
                MessageBox.Show("Не могу подключится к интернету, измените настройки прокси");
                Proxy f_Update = new Proxy();
                f_Update.ShowDialog();
            }

        }





        string GetPathMDB(string catalog)
        {

            var client_man = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
            var path_sysdba = client_man.GetPrivateString("Infogen", "percorsoordini");//версия клиента




            switch (catalog.ToUpper())
            {

                case "GIULIANOVARSA":
                    return Environment.CurrentDirectory + @"\" + path_sysdba + @"\systema.mdb";
                default:
                    return Environment.CurrentDirectory + @"\" + path_sysdba + @"\system.mdb";
            }
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



        private static void GetVersionFromRegistry()
        {
            // Opens the registry key for the .NET Framework entry.
            using (RegistryKey ndpKey =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
                OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                // As an alternative, if you know the computers you will query are running .NET Framework 4.5 
                // or later, you can use:
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, 
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {

                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        string name = (string)versionKey.GetValue("Version", "");
                        string sp = versionKey.GetValue("SP", "").ToString();
                        string install = versionKey.GetValue("Install", "").ToString();
                        if (install == "") //no install info, must be later.
                            MessageBox.Show(versionKeyName + "  " + name);
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                MessageBox.Show(versionKeyName + "  " + name + "  SP" + sp);
                            }

                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");
                            if (name != "")
                                sp = subKey.GetValue("SP", "").ToString();
                            install = subKey.GetValue("Install", "").ToString();
                            if (install == "") //no install info, must be later.
                                MessageBox.Show(versionKeyName + "  " + name);
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    MessageBox.Show("  " + subKeyName + "  " + name + "  SP" + sp);
                                }
                                else if (install == "1")
                                {
                                    MessageBox.Show("  " + subKeyName + "  " + name);
                                }
                            }
                        }
                    }
                }
            }
        }
    }



    public static class log
    {
        public static void Add(string str = "")
        {

            string localpath = (new FileInfo(Assembly.GetExecutingAssembly().Location).Directory).ToString();
            string path = localpath + @"\TreeCadN.log";

            if (File.Exists(path))
            {
                string save_log = Environment.NewLine + DateTime.Now + " - " + str;
                File.AppendAllText(path, save_log, System.Text.Encoding.UTF8);
            }
        }

    }
    class session_TV
    {

        public string code { get; set; }
        public string end_customer_link { get; set; }
        public string supporter_link { get; set; }
    }
    public static class ProcessExtensions
    {
        private static string FindIndexedProcessName(int pid)
        {
            var processName = Process.GetProcessById(pid).ProcessName;
            var processesByName = Process.GetProcessesByName(processName);
            string processIndexdName = null;

            for (var index = 0; index < processesByName.Length; index++)
            {
                processIndexdName = index == 0 ? processName : processName + "#" + index;
                var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
                if ((int)processId.NextValue() == pid)
                {
                    return processIndexdName;
                }
            }

            return processIndexdName;
        }

        private static Process FindPidFromIndexedProcessName(string indexedProcessName)
        {
            var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
            return Process.GetProcessById((int)parentId.NextValue());
        }

        public static Process Parent(this Process process)
        {
            return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
        }
    }
}
