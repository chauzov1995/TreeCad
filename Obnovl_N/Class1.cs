using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Obnovl_N
{
    public partial class Obnovl
    {
        public Obnovl(Process proc){

            proc.WaitForInputIdle();
            MessageBox.Show("обнови"); 



            }


    }
}
