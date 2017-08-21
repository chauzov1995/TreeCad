﻿/*
 * Создано в SharpDevelop.
 * Пользователь: ncha003
 * Дата: 11.11.2016
 * Время: 8:02
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Windows;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace вызов
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            textBox1.Text = Environment.CurrentDirectory + @"\12\system.mdb";
            textBox4.Text = Environment.CurrentDirectory + @"\12";



            textBox6.Text = hash_value("TreeCadN.dll");
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }
        void Button1Click(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("GNOTD"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
                textBox1.Text,
                textBox2.Text

            });
            textBox2.Text = result.ToString();
            //	MessageBox.Show(result.ToString());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("GNPrimN"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
                textBox4.Text,
                1,
                textBox3.Text


            });
            textBox3.Text = result.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("TAccess"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
                textBox4.Text,
                1,
                textBox5.Text


            });
            textBox5.Text = result.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            File.Copy(Directory.GetCurrentDirectory() + @"\TreeCadN.dll", @"C:\evolution\giulianovars\TreeCadN.dll", true);
            MessageBox.Show("Готово");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            File.Copy(Directory.GetCurrentDirectory() + @"\TreeCadN.dll", @"C:\evolution\eCadPro\TreeCadN.dll", true);
            MessageBox.Show("Готово");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("GNLICENSE"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {


            });

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("UPDATE"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
                });
        }


        string hash_value(string local_path)
        {

            RIPEMD160 myRIPEMD160 = RIPEMD160Managed.Create();
            FileStream tmppathStream = File.OpenRead(local_path);
            byte[] hashValue = myRIPEMD160.ComputeHash(tmppathStream);
            tmppathStream.Close();
            string hash = BitConverter.ToString(hashValue).Replace("-", String.Empty);


            return hash;
        }


        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox7.Text == "1122123qQ")
                {

                    //1.,вычилсяем хээш сумму текущего файла 
                    string hash1 = hash_value("TreeCadN.dll");
                    //  MessageBox.Show(hash1);//asdsadas



                    //2. отправляем файл на сервер
                    FileInfo toUpload = new FileInfo("TreeCadN.dll");
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ecad.giulianovars.ru/public/TreeCadN//" + toUpload.Name);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential("ecad_ftp", "bFqeNo4Xp2");
                    Stream ftpStream = request.GetRequestStream();
                    FileStream fileStream = File.OpenRead("TreeCadN.dll");
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    do
                    {
                        bytesRead = fileStream.Read(buffer, 0, 1024);
                        ftpStream.Write(buffer, 0, bytesRead);
                    }
                    while (bytesRead != 0);
                    fileStream.Close();
                    ftpStream.Close();


                    //3. скачиваем файл 

                    WebClient client = new WebClient();
                    var url = "http://ecad.giulianovars.ru/TreeCadN/TreeCadN.dll";
                    string tmppath = Path.GetTempPath() + @"\TreeCadN.dll";
                    client.DownloadFile(url, tmppath);//скачаем новую




                    //    4. вычисляем хэшш

                    string hash2 = hash_value(tmppath);
                    // MessageBox.Show(hash2);//asdsadas

                    //   5.  проверяем сходится ли хэш

                    if (hash1 == hash2)
                    {


                        client = new WebClient();
                        url = "http://ecad.giulianovars.ru/TreeCadN/hash_prov.php?type=2&hash=" + hash1;
                        client.DownloadString(url);

                        MessageBox.Show("Сумма сходится = " + hash1);


                    }
                    else
                    {
                        MessageBox.Show("ERROR Сумма разная = " + hash1 + "\r\n" + hash2);

                    }

                }
                else
                {
                    MessageBox.Show("Неверный пароль");

                }

            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("Kommunikacii"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
                textBox4.Text
                });
        }
    }
}
