using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Obnov_dll_N
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                log.Add("Входная строка "+ args[0]);
               string[]  split_vhparam = args[0].Split('=');
           
                Process proc2 = Process.GetProcessById(Convert.ToInt32(split_vhparam[0]));

                proc2.WaitForExit();

                string path = split_vhparam[2];//new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.ToString() + @"\TreeCadN.dll";


                WebClient client = new WebClient();
                var url = "http://ecad.giulianovars.ru/TreeCadN/TreeCadN.dll";


                string tmppath = Path.GetTempPath() + @"\TreeCadN.dll";


                client.DownloadFile(url, tmppath);//скачаем новую


                RIPEMD160 myRIPEMD160 = RIPEMD160Managed.Create();
                FileStream tmppathStream = File.OpenRead(tmppath);
                byte[] hashValue = myRIPEMD160.ComputeHash(tmppathStream);
                tmppathStream.Close();
                string hash = BitConverter.ToString(hashValue).Replace("-", String.Empty);


                if (hash == split_vhparam[1])
                {
                    log.Add("заменим на новую - успех" );
                    File.Copy(tmppath, path, true);//заменим на новую
                 //   MessageBox.Show("Процесс завершён обновимся сумма=" + hash);

                }


                File.Delete(tmppath);                                                                                            // File.Delete("TreeCadN_pre.dll");
            
            }
            catch(Exception err) {
                log.Add("catch " + err.Message);
            }
        }
    }


    public static class log
    {
        public static void Add(string str = "")
        {

            string localpath = (new FileInfo(Assembly.GetExecutingAssembly().Location).Directory).ToString();
            string path = localpath + @"\Obnov_dll_N.log";

            if (File.Exists(path))
            {
                string save_log = Environment.NewLine + DateTime.Now + " - " + str;
                File.AppendAllText(path, save_log, System.Text.Encoding.UTF8);
            }
        }

    }
}
