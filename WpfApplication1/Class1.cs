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
using System.Data.SQLite;
using TreeCadN.open_ordini;
using System.Data.Common;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using TreeCadN.smarktkitchen;

namespace TreeCadN
{
    public class alert
    {
        public alert(String msg)
        {
            MessageBox.Show(msg);
        }
    }


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
            //   MessageBox.Show("");
            string returnValue = "";
            object info;
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

                        /*
                          MessageBox.Show("test");
                          this.xamb = getParam(Ambiente, "GetObject", "XAMB");
                          this.aamain = getParam(Ambiente, "GetObject", "aaMain");
                         this.engine = getParam(Ambiente, "GetObject", "ENGINE");
                          getParam(engine, "wrCompress3D", @"C:\33\29680550RU__CVA6805_CLST.3DS", @"C:\1.DR3D", "0");
                   */
                        returnValue = Kommunikacii(GetPathMDB(katalog));


                        break;
                    case "evesync":
                        var ini2 = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
                        var path_ordini = ini2.GetPrivateString("Infogen", "percorsoordini");//версия клиента

                        evesync(GetPathMDB(katalog), path_ordini);
                        break;
                    case "GNviewer":
                        GNviewer(Environment.CurrentDirectory + @"\GIULIANOVARSA\procedure");
                        break;
                    case "smartkitchen":
                        returnValue=smartkitchen(s);
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
                        this.aamain = getParam(Ambiente, "GetObject", "aaMain");
                        info = getParamG(xamb, "info");
                        string modello = getParam(info, "modello").ToString();


                        string b = GNfindprice(Environment.CurrentDirectory + @"\" + katalog + @"\PROCEDURE\3CadBase.sqlite", modello);
                        b = b.Replace(Environment.NewLine, "");
                        log.Add("строка постоения : " + b);

                        //   setParamP(Ambiente, "Codice", "");//очистить название артикула

                        // setParamP(Ambiente, "gDisSetFocus", "");//заменить артикул или создать новый


                        getParam(Ambiente, "CaricaRiga2", "s", b);//заменить артикул или создать новый
                                                                  // getParamI(Ambiente, "gDisSetFocus");//заменить артикул или создать новый
                        returnValue = b;
                        break;

                    case "status_zakaza":


                        returnValue = status_zakaza(s);

                        break;

                    case "GNOTD_list_det":


                        returnValue = GNOTD_list_det(s);

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


        //Param
        //getParamI  visov metods  bez parametrov
        //getParam  visov metods  s parametrom
        //getParamG  poluchitb znach svoystva
        //setParam   
        //setParamP   ustanovitb znach svoystva

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






        public void copy_to_share()
        {
            //  SQLiteConnection m_dbConn = new SQLiteConnection();
            //   SQLiteCommand m_sqlCmd = new SQLiteCommand();

            //      var ini = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
            //      var path_ecadpro = ini.GetPrivateString("Infogen", "percorsoordini");//версия клиента
            //     string dbFileName = Environment.CurrentDirectory + @"\" + path_ecadpro + @"\sample.sqlite";

            //   m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
            //   m_dbConn.Open();
            //   m_sqlCmd.Connection = m_dbConn;





            string percorsoordini = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\_ecadpro\ordini";
            string[] files = Directory.GetFiles(percorsoordini, "*.eve", SearchOption.TopDirectoryOnly);
            if (files.Length == 0 || percorsoordini == (Environment.CurrentDirectory + @"\_ecadpro\ordini"))
            {
                MessageBox.Show("Нечего копировать");
            }
            else
            {

                this.xamb = getParam(Ambiente, "GetObject", "XAMB");
                object info = getParamG(xamb, "INFO");
                object info2 = getParamG(info, "INFO");


                if (!Directory.Exists(percorsoordini + @"\_old")) Directory.CreateDirectory(percorsoordini + @"\_old");
                foreach (string file in files)
                {
                    string nomfile = file.Split('\\').Last().Split('.').First();
                    getParam(xamb, "carica", file);
                    string newnum = getParamI(info, "NuovoNumeroOrdine").ToString();
                    setParamP(info, "Numero", newnum);
                    getParam(info2, "Add", "_NOMEFILEPARETI", nomfile);
                    getParamI(xamb, "salva");//сохраним
                    if (File.Exists(percorsoordini + @"\_old\" + nomfile + ".eve")) File.Delete(percorsoordini + @"\_old\" + nomfile + ".eve");
                    File.Move(file, percorsoordini + @"\_old\" + nomfile + ".eve");

                    //      string FIO = getParam(info2, "Var", "CLI_1").ToString();
                    //      string Manager = getParam(info2, "Var", "Manager").ToString();
                    //     string orderprice = getParam(info2, "Var", "orderprice").ToString();
                    //     string _RIFFABRICA = getParam(info2, "Var", "_RIFFABRICA").ToString();
                    //     string _RIFSALON = getParam(info2, "Var", "_RIFSALON").ToString();
                    //     string SROK = getParam(info2, "Var", "SROK").ToString();

                    //     string pattern = "000000";
                    //     string nom_form = pattern.Remove(0, newnum.Length) + newnum;
                    //      m_sqlCmd.CommandText = "INSERT OR IGNORE INTO ordini (file_path, nomer_zakaza, FIO, manager, orderprice, _RIFFABRICA, _RIFSALON, SROK) " +
                    //        "VALUES ('" + file + "', '" + nomfile + "','" + FIO + "','" + Manager + "', '" + orderprice + "', '" + _RIFFABRICA + "', '" + _RIFSALON + "', '" + SROK + "')";
                    //     m_sqlCmd.ExecuteNonQuery();
                }
            }
            //   m_dbConn.Close();
            //   GC.Collect();
        }

        public string GNOTD(string path, string str, bool listdet = false)//для старых версий тех у кого ещё стоит старый размерный ряд
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
                    str1 = "0;0;1;1;0;1;0;;;1^2^3^4^5^6^7^22^33^37^39^40^43^46^37^56^60^61^54";
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
                        str1 = "0;0;1;1;0;1;0;;;1^2^3^4^5^6^7^22^33^37^39^40^43^46^37^56^60^61^54";
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
                string katalog = getParamI(Ambiente, "xPercorso").ToString();

                Window1 f1 = new Window1(path1, str1, katalog, listdet);
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

        public string GNOTD_list_det(string str)//для листовых  
        {

            string katalog = getParamI(Ambiente, "xPercorso").ToString();
            string path = GetPathMDB(katalog);
            return GNOTD(path, str, true);
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
        public void GNspiszakaz(ref object xAmbiente)
        {
            this.Ambiente = xAmbiente;

            open sss = new open(this, GetPathOrdini());
            sss.ShowDialog();
        }
        public string GNPrimNAUTO(ref object xAmbiente, int filtr, ref string text)
        {
            this.Ambiente = xAmbiente;
            string katalog = getParamI(Ambiente, "xPercorso").ToString();

            object xamb = getParam(Ambiente, "GetObject", "XAMB");
            object info = getParamG(xamb, "INFO");
            object info2 = getParamG(info, "INFO");

            int intver = 1;
            try
            {
                string Versio = getParam(info2, "Var", "Versio").ToString();
                 intver = int.Parse(Versio.Split(',')[0]);
            }
            catch (Exception) {
            
            }

            //
            //MessageBox.Show(intver.ToString());
            if (intver >= 2)
            {
                Prim3 f_prim = new Prim3(GetPathMDB(katalog), text);          
                f_prim.ShowDialog();
                return f_prim.text_otvet;
            }
            else
            {
                Prim2 f_prim = new Prim2(GetPathMDB(katalog), text);
                f_prim.ShowDialog();
                return f_prim.text_otvet;
            }

          
        }
        public string TAccess(string path, int filtr, string text)
        {
            object xamb = getParam(Ambiente, "GetObject", "XAMB");
            object info = getParamG(xamb, "INFO");
            object info2 = getParamG(info, "INFO");
            string _RIFFABRICA = getParam(info2, "Var", "_RIFFABRICA").ToString();

            //   MessageBox.Show(path);
            TAccessories f_TAccess = new TAccessories(path, text, this, _RIFFABRICA);
            f_TAccess.ShowDialog();
            return f_TAccess.text_otvet;
        }
        public string smartkitchen(string path)
        {
            object xamb = getParam(Ambiente, "GetObject", "XAMB");           
            object info = getParamG(xamb, "INFO");
            object info2 = getParamG(info, "INFO");
            string _RIFFABRICA = getParam(info2, "Var", "_RIFFABRICA").ToString();

          //  //_RIFFABRICA = "32313";

                        backgrvibor f_TAccess = new smarktkitchen.backgrvibor(path, _RIFFABRICA);
            f_TAccess.ShowDialog();
            return f_TAccess.otvet;
            /*
       
            smartkitchen f_TAccess = new smarktkitchen.smartkitchen(path);
           f_TAccess.ShowDialog();
            return f_TAccess.otvet;*/
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

        public void evesync(string path, string pathordini)
        {
            evesync.evesync f_evesync = new evesync.evesync(path, pathordini);
            f_evesync.ShowDialog();


        }
        public void GNviewer(string path)
        {
            //path -путь к тимвиверу
            var sad = MessageBox.Show("Сейчас запустится Фабричный TeamViewer, текущий  будет закрыт. Продолжить?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (sad == MessageBoxResult.OK)
            {
                string otvet = "";
                string patholdtv = "";



                //на случай если как то переминована тв
                string[] dirs = Directory.GetFiles(path, "TeamViewerQS*.exe");
                foreach (string dir in dirs)
                {
                    patholdtv = dir;
                }

                WebClient webClient = new WebClient();
                string url;
                url = "https://webapi.teamviewer.com/api/v1/sessions";
                log.Add(url);
                webClient.Headers.Add("Authorization", "Bearer 2875381-8jCmpdsLcQm5FelCc9rv");
                webClient.Headers.Add("Content-Type", "application/json");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string JSON = webClient.UploadString(url, "POST", @"{""groupid"" : ""g18888010""}");
                log.Add(JSON);
                session_TV login = JsonConvert.DeserializeObject<session_TV>(JSON);

                string code = login.code.Replace("-", "");


                string tmppath = Path.GetTempPath();


                string[] dirs2 = Directory.GetFiles(tmppath, "TeamViewerQS*.exe");
                foreach (string dir in dirs2)
                {
                    File.Delete(dir);//востановили
                }




                string newpathtv = tmppath + @"TeamViewerQS-id" + code + ".exe";

                File.Copy(patholdtv, newpathtv, true);

                //удалим текущий процесс тимвивера, (мало ли запущина боле новая версия)
                foreach (Process proc in Process.GetProcessesByName("TeamViewer"))
                {
                    proc.Kill();
                }

                //запустим тимвивер КУЭС
                Process.Start(newpathtv);


            }


        }



        public void open_save(ref object xAmbiente)
        {
            this.Ambiente = xAmbiente;
            string path_ordini = GetPathOrdini();

            open.createOpenBD(path_ordini);
            log.Add("createOpenBD готово");
            open.updateTekZakaz(this, path_ordini);
        }




        public void art_to_buf(ref object xAmbiente)
        {
            
            this.Ambiente = xAmbiente;
            this.xamb = getParam(Ambiente, "GetObject", "XAMB");
            string currbox = getParam(xamb, "curbox").ToString();
            object box = getParamG(xamb, "box", currbox);
            string cod = getParamG(getParamG(box, "gg"), "cod").ToString();

            //  cod = cod.Remove(3);

            Clipboard.SetText(cod);
            
            /*

            this.Ambiente = xAmbiente;
            this.xamb = getParam(Ambiente, "GetObject", "REMOTO");
           string currbox = getParamI(xamb, "RichiediPreventivo").ToString();

            MessageBox.Show(currbox);
           // ambiente.GetObject("REMOTO").RichiediPreventivo()*/
        }


        public void OutPrima(ref object xAmbiente)
        {
            var saveeve = new Window2();
            saveeve.Show();

        }



        public void treecadtobazis(object detalirovka)
        {
           // MessageBox.Show("asdasdasdas");
            var typedList = (object[])detalirovka;//.Cast<string>().ToArray();
            List<del> detali = new List<del>();


            using (SQLiteConnection conn = new SQLiteConnection("Data Source=toBazis.sqlite; Version=3;"))
            {


                string zapros = "";


                foreach (var elem in typedList)//перебор всех элементов
                {
                    string box = elem.GetType().InvokeMember("box", BindingFlags.GetProperty, null, elem, new object[0]).ToString();
                    string parrentid = elem.GetType().InvokeMember("parrentid", BindingFlags.GetProperty, null, elem, new object[0]).ToString();
                    string strdetal = elem.GetType().InvokeMember("strdetal", BindingFlags.GetProperty, null, elem, new object[0]).ToString();
                 //   log.Add(strdetal);




                    foreach (string strokidetalir in strdetal.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {//перебор строк деталировка
                         log.Add(strokidetalir);
                        detinpu boxterr = new detinpu();
                        boxterr.box = box;
                        boxterr.parrentid = parrentid;
                        boxterr.count = strokidetalir.Split(',')[1];
                        //     new alert(boxterr.box);

                        //  new alert(strokidetalir);

                        foreach (string primparams in strokidetalir.Split(',')[6].Split(';'))
                        {//здесь ищем левправверхниз

                            if (primparams.ToUpper().IndexOf("Art_Detal=".ToUpper()) == 0)
                            {
                                boxterr.Art_Detal = primparams.Substring("Art_Detal=".Length);
                            }
                            if (primparams.ToUpper().IndexOf("Name=".ToUpper()) == 0)
                            {
                                boxterr.Name = primparams.Substring("Name=".Length).ToUpper().Trim();
                            }
                            if (primparams.ToUpper().IndexOf("NNomenclature_id=".ToUpper()) == 0)
                            {
                                boxterr.NNomenclature_id = primparams.Substring("NNomenclature_id=".Length);
                            }
                            if (primparams.ToUpper().IndexOf("OTDA=".ToUpper()) == 0)
                            {
                                boxterr.OTDA = primparams.Substring("OTDA=".Length);
                            }
                            if (primparams.ToUpper().IndexOf("OTDF=".ToUpper()) == 0)
                            {
                                boxterr.OTDF = primparams.Substring("OTDF=".Length);
                            }
                            if (primparams.ToUpper().IndexOf("PanDirA=".ToUpper()) == 0)
                            {
                                boxterr.PanDirA = primparams.Substring("PanDirA=".Length);
                            }
                            if (primparams.ToUpper().IndexOf("PanDirF=".ToUpper()) == 0)
                            {
                                boxterr.PanDirF = primparams.Substring("PanDirF=".Length);
                            }
                            if (primparams.ToUpper().IndexOf("KR_DOP=".ToUpper()) == 0)
                            {
                                boxterr.KR_DOP = primparams.Substring("KR_DOP=".Length);
                            }
                        }

                        if (boxterr.Art_Detal != "")
                        {
                           var  materchert= zaprbazisbd(boxterr.Art_Detal);
                            boxterr.MaterialName = materchert.MaterialName;
                            boxterr.width = materchert.width;
                            boxterr.OTDAName = zaprbazisbd("TOTD_"+boxterr.OTDA).MaterialName;
                            boxterr.OTDFName = zaprbazisbd("TOTD_" + boxterr.OTDF).MaterialName;
                            boxterr.KR_DOPName = zaprbazisbd(boxterr.KR_DOP).MaterialName;
                        }

                        zapros += "('" + box + "','" + boxterr.Art_Detal + "','" + boxterr.Name + "','"
                            + boxterr.NNomenclature_id + "','" + boxterr.OTDA + "','" + boxterr.OTDF + "','"
                             + boxterr.PanDirA + "','" + boxterr.PanDirF + "','"
                          + boxterr.KR_DOP + "','" + boxterr.KR_DOPName + "','" + boxterr.OTDAName + "','" + boxterr.OTDFName + "','"
                            + boxterr.count + "','" + boxterr.MaterialName + "','" + boxterr.parrentid + "','" + boxterr.width + "'),";





                    }

                }

                zapros = zapros.Trim(',');
                //   log.Add(zapros);


                try
                {
                    conn.Open();
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    SQLiteCommand cmd = conn.CreateCommand();
                    string sql_command = "DROP TABLE IF EXISTS person";
                    cmd.CommandText = sql_command;
                    cmd.ExecuteNonQuery();


                    sql_command = "CREATE TABLE person("
                     + "id INTEGER PRIMARY KEY AUTOINCREMENT, "
                     + "box TEXT, "
                     + "Art_Detal TEXT, "
                     + "Name TEXT, "
                     + "NNomenclature_id TEXT, "
                     + "OTDA TEXT, "
                                    + "OTDF TEXT, "
                                    + "PanDirA TEXT, "
                                    + "PanDirF TEXT, "
                                    + "KR_DOP TEXT, "
                                    + "KR_DOPName TEXT, "
                           + "OTDAName TEXT, "
                     + "OTDFName TEXT, "
                     + "count TEXT, "
                     + "parrentid TEXT, "
                     + "width TEXT, "
                     + "MaterialName TEXT)";
                //    log.Add(sql_command);
                    cmd.CommandText = sql_command;
                    cmd.ExecuteNonQuery();


                    sql_command = "INSERT INTO person(box, Art_Detal, Name, NNomenclature_id, OTDA, OTDF, PanDirA, PanDirF, KR_DOP, KR_DOPName, OTDAName, OTDFName, count, MaterialName, parrentid, width) "
                     + "VALUES " + zapros;
               //     log.Add(sql_command);
                    cmd.CommandText = sql_command;
                    cmd.ExecuteNonQuery();




                }
                catch (SQLiteException ex)
                {
                    new alert(ex.Message);
                    //  new alert(sql_command);
                }






            }
            MessageBox.Show("Готово");

        }

        detinpu zaprbazisbd(string idmaterial)
        {
            string connectionString_bazis =
     "User=SYSDBA;" +
     "Password=masterkey;" +
     "Database=C:\\BAZIS\\MTDBR10.FDB;" +
     "DataSource=172.16.4.239;" +
     "Port=14357;" +
     "Dialect=3;" +
     "Charset=WIN1251;" +
     "Role=;" +
     "Connection lifetime=15;" +
     "Pooling=true;" +
     "MinPoolSize=0;" +
     "MaxPoolSize=50;" +
     "Packet Size=8192;" +
     "ServerType=0";

            detinpu sasasssdds=new detinpu();
        //    string material = "";
            try
            {

                FbConnection conn = new FbConnection(connectionString_bazis);
                conn.Open();
                string sql = "select  m.NAME_MAT, ma.THICKNESS from material m LEFT JOIN MATERIAL_ADVANCE  ma on m.ID_M = ma.ID_M  where  m.ARTICLE='" + idmaterial + "'";

                FbCommand com = new FbCommand(sql, conn);
                FbDataReader dr = com.ExecuteReader();


                while (dr.Read())
                {
                    sasasssdds.MaterialName = dr["NAME_MAT"].ToString();
                    sasasssdds.width = dr["THICKNESS"].ToString();



                }
                dr.Close();
                conn.Close();
            }
            catch (FbException ex)
            {
                new alert(ex.Message);
            }

            //  new alert(material);
            return sasasssdds;


        }

        public string bazisgetdrombd()
        {

            string json = "";
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=C:\evolution\giulianovars\toBazis.sqlite; Version=3;"))
            {
                try
                {
                    conn.Open();
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                try
                {
                    SQLiteCommand cmd = conn.CreateCommand();
                    string sql_command = "Select * from person where Name !=''";
                    cmd.CommandText = sql_command;
                    SQLiteDataReader r = cmd.ExecuteReader();
                    List<detinpu> nneww = new List<detinpu>();
                    while (r.Read())
                    {
                        detinpu ddelem = new detinpu();
                        ddelem.box = r["box"].ToString();
                        ddelem.Art_Detal = r["Art_Detal"].ToString();
                        ddelem.Name = r["Name"].ToString();
                        ddelem.NNomenclature_id = r["NNomenclature_id"].ToString();
                        ddelem.OTDA = r["OTDA"].ToString();
                        ddelem.OTDF = r["OTDF"].ToString();
                        ddelem.PanDirA = r["PanDirA"].ToString();
                        ddelem.PanDirF = r["PanDirF"].ToString();
                        ddelem.KR_DOP = r["KR_DOP"].ToString();
                        ddelem.KR_DOPName = r["KR_DOPName"].ToString();
                        ddelem.OTDAName = r["OTDAName"].ToString();
                        ddelem.OTDFName = r["OTDFName"].ToString();
                        ddelem.count = r["count"].ToString();
                        ddelem.MaterialName = r["MaterialName"].ToString();
                        ddelem.parrentid = r["parrentid"].ToString();
                        ddelem.width = r["width"].ToString();
                        //asdad
                        nneww.Add(ddelem);



                    }
                    r.Close();






                    json = JsonConvert.SerializeObject(nneww, Formatting.Indented);










                }
                catch (SQLiteException ex)
                {
                    new alert(ex.Message);
                    //  new alert(sql_command);
                }





            }

            //    new alert(json);
            return json;
            //   MessageBox.Show("Работает");
        }




        public void evesync_save(ref object xAmbiente)
        {
            //  MessageBox.Show("asda");


            //  Set x = Ambiente.GetObject("GG," & a)
            //           var x = getParam(Ambiente, "GetObject", "GG","1");
            //   new alert(x.ToString());


            YA ps = YA.Default;
            if (ps.isSave)
            {
                //MessageBox.Show("asda");
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

            log.Add(path + @"\" + evename);
            // uploadPROGR.uploadPROGR sss = new uploadPROGR.uploadPROGR(evepath, this);
            uploadPROGR.dialprogr_new sss = new uploadPROGR.dialprogr_new(evepath, this);
            sss.ShowDialog();

            log.Add("ShowDialog");

        }

        public List<UpdateUPD> UPdate = new List<UpdateUPD>();

        public void UPDATE1(ref object xAmbiente)//для новой
        {
            this.Ambiente = xAmbiente;
            CATALOGGN = "Giulianovarsa";// getParamI(Ambiente, "xPercorso").ToString();
            UPDATE();

        }
        string CATALOGGN = "Giulianovars";


        void upd_last_eve()
        {

            INIManager client_man = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
            string numeroordine = client_man.GetPrivateString("Infogen", "numeroordine");//версия клиента
            string percorsoordini = client_man.GetPrivateString("Infogen", "percorsoordini");//версия клиента
            string[] files = Directory.GetFiles(Environment.CurrentDirectory + @"\" + percorsoordini, "*.eve", SearchOption.TopDirectoryOnly);
            Array.Sort(files);
            MessageBox.Show(files.Last());
        }


        public void UPDATE()
        {
            // this.Ambiente = xAmbiente;
            //  string catalog = "Giulianovars";// getParamI(Ambiente, "xPercorso").ToString();
            log.Add("вычислим последжний заказ");




            //upd_last_eve();




            log.Add("Старт обновления!");

            try
            {

                string catalog = CATALOGGN;
                log.Add("обновление dll treecadN ред");
                //MessageBox.Show(CATALOGGN);

                //получим код авторизации
                INIManager manager = new INIManager(Environment.CurrentDirectory + @"\ecadpro.ini");
                string authotiz_root = manager.GetPrivateString("giulianovars", "attivazione");//получ ключ активации

                //   Obnov_dll_N.Create(catalog, authotiz_root);//обновление dll treecadN ред

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

                        evesync.YA ps = YA.Default;


                        url = "http://ecad.giulianovars.ru/php/upd/dll_prov_ver.php?upd_time=" + client_ver + "&attivazione=" + authotiz_root + "&yadisk=" + ps.OAuth;

                        response = client.DownloadData(url);
                        last_upd = System.Text.Encoding.UTF8.GetString(response);
                        ////   MessageBox.Show(last_upd);
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


                string file1 = Environment.CurrentDirectory + @"\Giulianovarsa\PROCEDURE\TreeCadS.dll";
                string file2 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TreeCadS.dll";
                if (File.GetCreationTime(file1) > File.GetCreationTime(file2))
                {
                    File.Copy(file1, file2);
                }

            }
            catch (Exception e)
            {
                //  MessageBox.Show(e.Message);
            }

        }


        public string status_zakaza(string umol)
        {
            status status = new status(GetServ_path() + @"\spis_stat.txt", umol);
            status.ShowDialog();
            return status.otvet;
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
                INIManager manager = new INIManager(GetEcadProIni());
                authotiz_root = manager.GetPrivateString("giulianovars", "attivazione");//получ ключ активации




                string client_ver = "";
                string path = Environment.CurrentDirectory + @"\giulianovarsa\procedure\updN.ini";
                if (File.Exists(path))
                {
                    INIManager client_man = new INIManager(path);
                    client_ver = client_man.GetPrivateString("GN_UPD", "last_upd");//версия клиента
                }
                //   authotiz_root = "091993E2AED611F8052E";

                string url = "http://ecad.giulianovars.ru/php/license/load_page_for_dll.php?authotiz_root=" + authotiz_root + "&upd_time=" + client_ver;
                var response = client.DownloadData(url);
                string str = System.Text.Encoding.UTF8.GetString(response);
                string[] parse = str.Split('=');
                //   MessageBox.Show(str);
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

            //     var client_man = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
            //   var path_sysdba = client_man.GetPrivateString("Infogen", "percorsoordini");//версия клиента
            //

            //    MessageBox.Show(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\systema.mdb");
            switch (catalog.ToUpper())
            {

                case "GIULIANOVARSA":
                    return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\systema.mdb";
                default:
                    return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\system.mdb";
            }
        }

        string GetPathOrdini()
        {
            INIManager ini = new INIManager(Environment.CurrentDirectory + @"\_ecadpro\ecadpro.ini");
            string path_ecadpro = ini.GetPrivateString("Infogen", "percorsoordini");//версия клиента
            return Environment.CurrentDirectory + @"\" + path_ecadpro;
        }

        string GetEcadProIni()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\ecadpro.ini";
        }

        public static string GetServ_path()
        {
            return Environment.CurrentDirectory;
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


        public void test()
        {
            (new filtero()).ShowDialog();
        }

        public void creaPDF(string tablename = "")
        {

            if (tablename == "")
            {
                (new CreaPDF(tablename)).ShowDialog();

            }
            else
            {
                CreaPDF.generprice(tablename);
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

    class del
    {
        public String box;
        public String levaya;
        public String pravaya;
        public String top;
        public String bottom;

    }
    class detinpu
    {
        public String box;
        public String Art_Detal;
        public String Name;
        public String NNomenclature_id;
        public String OTDA;
        public String OTDF;
        public String PanDirA;
        public String PanDirF;
        public String KR_DOP;
        public String KR_DOPName;
        public String OTDAName;
        public String OTDFName;
        public String count;
        public String MaterialName;
        public String parrentid;
        public String width;

    }
}



