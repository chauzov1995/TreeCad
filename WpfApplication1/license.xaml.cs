﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class license : Window
    {
        // string URL_zapros = @"http://ecadrussia.alk.hr/ecadsv/RichiesteAdmin.aspx";
        List<Licenz> items = new List<Licenz>();
        string id_clienta_root, email_root, authotiz_root, moduli_root, abilitato_root;
        WebClient client = new WebClient();
        string ps = "0000000";
        //string[] moduli_pars;



        public license(string id_clienta_root, string email_root, string authotiz_root, string moduli_root, string abilitato_root, WebClient client, string actual_ver, string tekver)
        {

            InitializeComponent();
            this.client = client;
            this.id_clienta_root = id_clienta_root;
            this.email_root = email_root;
            this.authotiz_root = authotiz_root;
            this.moduli_root = moduli_root;
            this.abilitato_root = abilitato_root;


            tv_tek.Text = tekver;
            tv_ak.Text = actual_ver;



            otrisovka();
        }



        void otrisovka()
        {




            items.Add(new Licenz { id = "0", lic = "Основной модуль 3CAD", price = "13.44", modul_Act = abilitato_root });
            items.Add(new Licenz { id = "1", lic = "Продвинутый рендеринг", price = "3.36", modul_Act = moduli_root[3].ToString() });
            items.Add(new Licenz { id = "2", lic = "Экспорт в 3ds (3D Max)", price = "3.36", modul_Act = moduli_root[0].ToString() });
            items.Add(new Licenz { id = "3", lic = "Экспорт в dwg (AutoCAD)", price = "3.36", modul_Act = moduli_root[1].ToString() });
            items.Add(new Licenz { id = "4", lic = "Sketch Up", price = "2.24", modul_Act = moduli_root[8].ToString() });
            items.Add(new Licenz { id = "5", lic = "Предметы для обстановки помещений", price = "3.36", modul_Act = moduli_root[11].ToString() });
          //  items.Add(new Licenz { id = "6", lic = "Спецэффекты (плавное открывание дверей и пр.)", price = "3", modul_Act = moduli_root[2].ToString() });



            zapros_na_server();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].modul_Act == "1")
                {
                    items[i].sost = "Активна";
                    items[i].sost_iznach = "Активна";
                    items[i].color_sost = "#FFE42C3E";
                    items[i].button = "Отключить";
                }
                if (items[i].modul_Act == "0")
                {
                    if (ps[i] == '1')
                    {
                        items[i].sost = "Заявка подана";
                        items[i].sost_iznach = "Заявка подана";
                        items[i].button = "Отменить";
                        items[i].color_sost = "#FFE42C3E";
                    }
                    else
                    {
                        items[i].sost = "Не активна";
                        items[i].sost_iznach = "Не активна";
                        items[i].color_sost = "#FF6C6C6C";
                        items[i].button = "Подать заявку";
                    }
                }
            }
            idclienta.Text = "ID клиента: " + id_clienta_root;
            lvUsers.ItemsSource = items;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).Tag);
            string vspom = items[id].sost;


            if (vspom == "Заявка подана")
            {
                otrisovka("Не активна", id);
                items[id].modul_Act = "0";
            }


            if (vspom == "Не активна" && items[id].sost_iznach == "Активна")
            {
                otrisovka("Активна", id);
                items[id].modul_Act = "1";

            }
            if (vspom == "Не активна" && (items[id].sost_iznach == "Заявка подана" || items[id].sost_iznach == "Не активна"))
            {
                otrisovka("Заявка подана", id);
                items[id].modul_Act = "0";
            }



            if (vspom == "Активна")
            {
                otrisovka("Не активна", id);
                items[id].modul_Act = "0";
            }

            lvUsers.ItemsSource = null;
            lvUsers.ItemsSource = items;
        }


        void otrisovka(string otris, int id)
        {


            switch (otris)
            {
                case "Не активна":
                    items[id].sost = "Не активна";
                    items[id].button = "Подать заявку";
                    items[id].color_sost = "#FF6C6C6C";
                    break;
                case "Активна":
                    items[id].sost = "Активна";
                    items[id].button = "Отключить";
                    items[id].color_sost = "#FFE42C3E";
                    break;
                case "Заявка подана":
                    items[id].sost = "Заявка подана";
                    items[id].button = "Отменить";
                    items[id].color_sost = "#FFE42C3E";
                    break;


            }
        }
        string otvetnoe_pismo_1 = "\r\n\r\nПользователь отменил заявку на", otvetnoe_pismo_2 = "\r\n\r\nИнформирование об отключении", otvetnoe_pismo_3 = "\r\n\r\nЗаявка на подключение модуля";
        bool pismo_prov_1 = false, pismo_prov_2 = false, pismo_prov_3 = false;


   

        void otpravka_rez(int id)
        {
            if (items[id].sost != items[id].sost_iznach)
            {

                if (items[id].sost == "Не активна" && items[id].sost_iznach == "Заявка подана")
                {


                    //Пользователь отменил заявку на модуль
                    otvetnoe_pismo_1 += "\r\n-" + items[id].lic;
                    pismo_prov_1 = true;

                }

                if (items[id].sost == "Не активна" && items[id].sost_iznach == "Активна")
                {
                    //удалить лицензию код

                    //Информирование об отключении модуля
                    otvetnoe_pismo_2 += "\r\n-" + items[id].lic;
                    pismo_prov_2 = true;
                }

                if (items[id].sost == "Заявка подана")
                {
                    //Заявка на подключение модуля
                    otvetnoe_pismo_3 += "\r\n-" + items[id].lic;
                    pismo_prov_3 = true;
                }
            }
        }
        private void btnShowSelectedItem_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show(
"Вы уверены, что хотите изменить лицензию? Для подключения модуля нужно позвонить на фабрику в отдел программного обеспечения, чтобы подтвердить заявку.",
"Подтверждение",
MessageBoxButton.YesNo,
MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {



                int iasda = 0; foreach (TreeCadN.Licenz id in items)
                {


                    // otpravka_rez(Convert.ToInt32(id.id));
                    if (id.sost_iznach != id.sost)
                    {
                        otpr_na_server(id.id);
                    }

                }
                this.Close();
            }
        }
        void otpr_na_server(string mod)
        {

            string sost_type = "";
            string zel = "";
            if (items[Convert.ToInt32(mod)].sost == "Заявка подана" && items[Convert.ToInt32(mod)].sost_iznach == "Не активна")
            {

                sost_type = "1";
                zel = "Подключение";
            }
            if (items[Convert.ToInt32(mod)].sost == "Не активна" && items[Convert.ToInt32(mod)].sost_iznach == "Активна")
            {

                sost_type = "2";
                zel = "Отключение";
            }
            if (items[Convert.ToInt32(mod)].sost == "Не активна" && items[Convert.ToInt32(mod)].sost_iznach == "Заявка подана")
            {

                sost_type = "3";
                zel = "Отмена";
            }

            string url = "http://ecad.giulianovars.ru/php/vkl_mod_for_dll.php?id_clienta=" + id_clienta_root + "&modul=" + mod + "&sost_type=" + sost_type + "&zel=" + zel;
            var response = client.DownloadData(url);
            string str = System.Text.Encoding.UTF8.GetString(response);

        }
        void zhivoy()
        {
            string url = "http://ecad.giulianovars.ru/php/for_dll/zhivoy.php?id_clienta=" + id_clienta_root;
            var response = client.DownloadData(url);
            string str = System.Text.Encoding.UTF8.GetString(response);
            MessageBox.Show("Успех");

        }
        void zapros_na_server()
        {
            string url = "http://ecad.giulianovars.ru/php/zagr_for_dll.php?id_clienta=" + id_clienta_root;
            var response = client.DownloadData(url);
            string str = System.Text.Encoding.UTF8.GetString(response);
            ps = str;
        }
        private void btnSelectLast_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btn_zhivoy_Click(object sender, RoutedEventArgs e)
        {
            zhivoy();
        }

        private void proxy_setting_Click(object sender, RoutedEventArgs e)
        {


            licenziidrugie proxy = new licenziidrugie(id_clienta_root);
            proxy.ShowDialog();
            //Proxy proxy = new Proxy();
            // proxy.ShowDialog();
        }
    }



    public class Licenz
    {

        public string id { get; set; }
        public string lic { get; set; }
        public string price { get; set; }
        public string sost { get; set; }
        public string sost_iznach { get; set; }

        public string button { get; set; }
        public string izmenenie { get; set; }

        public string color_sost { get; set; }
        public string modul_Act { get; set; }
    }

    //Класс для чтения/записи INI-файлов
    public class INIManager
    {
        //Конструктор, принимающий путь к INI-файлу
        public INIManager(string aPath)
        {
            path = aPath;
        }

        //Конструктор без аргументов (путь к INI-файлу нужно будет задать отдельно)
        public INIManager() : this("") { }

        //Возвращает значение из INI-файла (по указанным секции и ключу) 
        public string GetPrivateString(string aSection, string aKey)
        {
            //Для получения значения
            StringBuilder buffer = new StringBuilder(SIZE);

            //Получить значение в buffer
            GetPrivateString(aSection, aKey, null, buffer, SIZE, path);

            //Вернуть полученное значение
            return buffer.ToString();
        }

        //Пишет значение в INI-файл (по указанным секции и ключу) 
        public void WritePrivateString(string aSection, string aKey, string aValue)
        {
            //Записать значение в INI-файл
            WritePrivateString(aSection, aKey, aValue, path);
        }

        //Возвращает или устанавливает путь к INI файлу
        public string Path { get { return path; } set { path = value; } }

        //Поля класса
        private const int SIZE = 1024; //Максимальный размер (для чтения значения из файла)
        private string path = null; //Для хранения пути к INI-файлу

        //Импорт функции GetPrivateProfileString (для чтения значений) из библиотеки kernel32.dll
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

        //Импорт функции WritePrivateProfileString (для записи значений) из библиотеки kernel32.dll
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);
    }



    public static class Obnov_dll_N
    {
        public static void Create(string catalog, string authotiz_root)
        {
            try
            {
                string localpath = (new FileInfo(Assembly.GetExecutingAssembly().Location).Directory).ToString();
                string TreeCadNpath = localpath + @"\" + catalog + @"\PROCEDURE\TreeCadN.dll";

                log.Add("обновление dll путь к длл " + TreeCadNpath);

                //  MessageBox.Show(catalog);
                WebClient client1 = new WebClient();
                var url1 = "http://ecad.giulianovars.ru/TreeCadN/hash_prov.php?type=1&catalog=" + catalog + "&authotiz_root=" + authotiz_root;
                string response1 = client1.DownloadString(url1);

                log.Add("обновление dll хэш с сервера " + response1);

                //dll нуждается в обновлении
              /*  if (hash_value(TreeCadNpath) != response1)
                {
                    log.Add("обновление dll нуждается в обновлении");




                    WebClient client = new WebClient();
                    var url = "http://ecad.giulianovars.ru/TreeCadN/TreeCadN.dll";


                    string tmppath = Path.GetTempPath() + @"\TreeCadN.dll";

                    //скачаем новую
                    client.DownloadFile(url, tmppath);


                    RIPEMD160 myRIPEMD160 = RIPEMD160Managed.Create();
                    FileStream tmppathStream = File.OpenRead(tmppath);
                    byte[] hashValue = myRIPEMD160.ComputeHash(tmppathStream);
                    tmppathStream.Close();
                    string hash = BitConverter.ToString(hashValue).Replace("-", String.Empty);


                    if (hash == response1)//заменим на новую
                    {
                        log.Add("заменим на новую - успех");
                        File.Copy(tmppath, TreeCadNpath, true);//заменим на новую
                                                               //   MessageBox.Show("Процесс завершён обновимся сумма=" + hash);

                    }
                    File.Delete(tmppath);




                }*/

            }
            catch (Exception err)
            {
                log.Add("Обновление DLL catch" + err.Message);

            }

        }
        /*
        static string hash_value(string local_path)
        {

            RIPEMD160 myRIPEMD160 = RIPEMD160Managed.Create();
            FileStream tmppathStream = File.OpenRead(local_path);
            byte[] hashValue = myRIPEMD160.ComputeHash(tmppathStream);
            tmppathStream.Close();
            string hash = BitConverter.ToString(hashValue).Replace("-", String.Empty);


            return hash;
        }*/
    }

}
