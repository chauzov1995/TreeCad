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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Management;
using System.IO.Compression;

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
                textBox2.Text,
                false

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

         

            try
            {
                foreach (Process proc in Process.GetProcessesByName("giulianovars"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
          //  MessageBox.Show("Готово");
            File.Copy(Directory.GetCurrentDirectory() + @"\TreeCadN.dll", @"C:\evolution\giulianovars\Giulianovarsa\PROCEDURE\TreeCadN.dll", true);
 

            Process.Start(@"C:\evolution\giulianovars\giulianovars.exe");///O 45 /TAB2 
            Close();




        }

        private void button5_Click(object sender, EventArgs e)
        {
         


            try
            {
                foreach (Process proc in Process.GetProcessesByName("ecadpro"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
          //  MessageBox.Show("Готово");
            File.Copy(Directory.GetCurrentDirectory() + @"\TreeCadN.dll", @"W:\eCadPro\Giulianovarsa\PROCEDURE\TreeCadN.dll", true);
            
          //  Process.Start(@"C:\evolution\eCadPro\eCadPro.exe", " /O 13218 /TAB2 ");
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\evolution\eCadPro\eCadPro.exe");
            startInfo.WorkingDirectory = @"W:\eCadPro";
           // startInfo.Arguments = "1";
            Process.Start(startInfo);
            Close();
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

            textBox5.Text = result.ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("evesync"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
                textBox4.Text
                });
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("GNviewer"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
            @"C:\!qwerty\TreeCadN\WpfApplication1\bin\Debug\GIULIANOVARS\procedure"
                });
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("uploadPROGR"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
            @"C:\evolution\giulianovars\_ecadpro\ordini"
                });
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("zenakorp"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
                textBox4.Text,"KSS; 150; 360; 560"



            });
            textBox3.Text = result.ToString();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("GNfindprice"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
           @"C:\!qwerty\TreeCadN\WpfApplication1\bin\Debug\GIULIANOVARS\procedure\3CadBase.sqlite", "1BR"



            });
            // textBox3.Text = result.ToString();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("GNfastbuild"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
      


            });
        }

 

        private void button12_Click_2(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("test"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {

            });
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("creaPDF"); 
            object result = meth.Invoke(instane, new object[] {
              "AC_TP12"
            });
        }

      //  string[] detealirovkakomps = { "Markova-ov", "CHAUZOVA-KA", "CHERKASOVA-M", "KALININA-NS", "JEREBCOV-AV", "TERENTIEVA-YV" };
        string[] detealirovkakomps = { "Markova-ov" };
        private void button17_Click(object sender, EventArgs e)
        {


            foreach (var elem in detealirovkakomps) { 

            string pathmdb = @"W:\eCadPro\Giulianovarsa\PROCEDURE\systemA.mdb";
            string pathdll = @"W:\eCadPro\Giulianovarsa\PROCEDURE\TreeCadN.dll";

            string copypath = @"\\" + elem + @"\c$\evolution\eCadPro\";
            //    File.Delete(copypath + "TreeCadN1.dll");
            //    File.Move(copypath+ "TreeCadN.dll", copypath + "TreeCadN1.dll");
                File.Copy(pathdll, copypath + "TreeCadN.dll", true);
            File.Copy(pathmdb, copypath + "systemA.mdb", true);
        }
            MessageBox.Show("Готово");

        }

        private void button18_Click(object sender, EventArgs e)
        {

            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("smartkitchen"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
                textBox4.Text,
              


            });
            textBox5.Text = result.ToString();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            foreach (Process proc in Process.GetProcessesByName("giulianovars"))
            {
                proc.Kill();
            }
            if(Directory.Exists(@"C:\evolution\giulianovars\Giulianovarsa"))
            Directory.Delete(@"C:\evolution\giulianovars\Giulianovarsa", true);
            if (Directory.Exists(@"C:\evolution\giulianovars\Giulianovarsa_arh"))
                Directory.Delete(@"C:\evolution\giulianovars\Giulianovarsa_arh", true);
         //   File.Delete(@"C:\evolution\giulianovars\GIULIANOVARSA.zip");
         //   File.Copy(@"\\KUZNECOV-AA\C$\out\GIULIANOVARSA.zip", @"C:\evolution\giulianovars\GIULIANOVARSA.zip");

            ZipFile.ExtractToDirectory(@"\\KUZNECOV-AA\C$\out\GIULIANOVARSA.zip", @"C:\evolution\giulianovars\Giulianovarsa_arh");
            Directory.Move(@"C:\evolution\giulianovars\Giulianovarsa_arh\Giulianovarsa", @"C:\evolution\giulianovars\Giulianovarsa");

            MessageBox.Show("Готово");
            Process.Start(@"C:\evolution\giulianovars\giulianovars.exe", @"/C giulianovarsa  ");///O 45 /TAB2 

        }
    }

}
