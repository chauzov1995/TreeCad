using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace Obnovl_N
{
    public class Obnovl
    {
         public void Obnovl_func(Process proc){

            //  proc.WaitForInputIdle();
            //Process asdasd = proc;
            Process aass = Process.GetCurrentProcess();
          //  Thread.Sleep(5000);
            MessageBox.Show(aass.ProcessName); 



            }


    }
}
