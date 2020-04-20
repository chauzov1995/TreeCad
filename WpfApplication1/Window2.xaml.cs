
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();





            copytotot();

        }


        void copytotot()
        {
           var path = @"\\3CAD\Evolution\eCadPro\Giulianovarsa\FOTO";
          var files=  Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);
            foreach(var file in files)
            {
                MessageBox.Show(file);
/*
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddFile(@"C:\OUT\belaia.jpg", "");
                    zip.Save(@"C:\OUT\GIULIANOVARSA.zip");
                }
                */
                using (FileStream zipToOpen = new FileStream(@"C:\OUT\GIULIANOVARSA.zip", FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        ZipArchiveEntry readmeEntry = archive.CreateEntry("Readme.txt");
                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {
                            writer.WriteLine("Information about this package.");
                            writer.WriteLine("========================");
                        }
                    }
                }
            }

        }
    }
}
