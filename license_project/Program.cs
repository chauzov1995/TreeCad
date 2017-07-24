using System;
using System.Reflection;

namespace license_project
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Assembly s = Assembly.LoadFile(Environment.CurrentDirectory + @"\TreeCadN.dll");
            Type ourClass = s.GetType("TreeCadN.neqweqe", true, true);
            Object instane = Activator.CreateInstance(ourClass);
            MethodInfo meth = ourClass.GetMethod("GNLICENSE"); //нужен тот Show, который не принимает параметров
            object result = meth.Invoke(instane, new object[] {
			
				
			});
        

           
        }
    }
}
