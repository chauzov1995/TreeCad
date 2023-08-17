// See https://aka.ms/new-console-template for more information

using System.Reflection;
using System.Runtime.InteropServices;

namespace copy_TreeCadN
{
    [ComVisible(true)]
    public  class GNLICENSE
    {
        public void GNLICENSE1()
        {
            Console.WriteLine("Hello, World!" + Environment.CurrentDirectory + "  " + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        //    log.Add("Hello, World!" + Environment.CurrentDirectory + "  " + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            Console.ReadLine();
        }
    }

    /*
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

    }*/
}