/*
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
    }
}
