using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace copy_treecad
{
    [ComVisible(true)]
    public class Class1
    {
        public void GNLICENSE1(ref object xAmbiente)
        {
            File.Copy(Environment.CurrentDirectory + @"\giulianovarsa\procedure\TreeCadN.dll", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TreeCadN.dll", true);
            // Console.WriteLine("Hello, World!" + Environment.CurrentDirectory + "  " + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            log.Add("Hello, World!" + Environment.CurrentDirectory + "  " + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            Console.ReadLine();
        }
    }

        public static class log
        {
            public static void Add(string str = "")
            {

                string localpath = (new FileInfo(Assembly.GetExecutingAssembly().Location).Directory).ToString();
                string path = localpath + @"\TreeCadN1.log";

                // if (File.Exists(path))
                //{
                string save_log = Environment.NewLine + DateTime.Now + " - " + str;
                File.AppendAllText(path, save_log, System.Text.Encoding.UTF8);
                // }

            }

        }

    }
