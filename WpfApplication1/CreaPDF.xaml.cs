using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;

namespace TreeCadN
{
    /// <summary>
    /// Логика взаимодействия для CreaPDF.xaml
    /// </summary>
    public partial class CreaPDF : Window
    {
        string table_name;
        public CreaPDF(string table_name)
        {
            InitializeComponent();

            this.table_name = table_name;


       

                WebClient client = new WebClient();
                string url = "http://ecad.giulianovars.ru/autoprice/sh_get_spis_tabl.php";
                var response = client.DownloadData(url);
                string html = Encoding.UTF8.GetString(response).Trim();
                string[] splithtmnl = html.Split(';');

                List<spis_table> spis_tabl = new List<spis_table>();
                foreach (string elem in splithtmnl)
                {
                    spis_tabl.Add(new spis_table() { ssilk = elem });

                }
                lb1.ItemsSource = spis_tabl;
       
        }

        static void skleitb2f(string table_name)
        {
            //склеить 2 файла
            string table_ = table_name;
            table_ = table_.Replace("AP_TC", "");



            WebClient client = new WebClient();
            string url = "http://ecad.giulianovars.ru/autoprice/pdf_visual.php?command=1&table=" + table_;
            var response = client.DownloadData(url);
            File.WriteAllBytes("header.pdf", response);

            FileInfo file = new FileInfo(table_name+".pdf");
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(file));
            PdfMerger merger = new PdfMerger(pdfDoc);
            FileInfo header = new FileInfo("header.pdf");
            PdfDocument header_doc = new PdfDocument(new PdfReader(header));
            FileInfo body = new FileInfo("body.pdf");
            PdfDocument body_doc = new PdfDocument(new PdfReader(body));
            merger.Merge(header_doc, 1, header_doc.GetNumberOfPages());
            merger.Merge(body_doc, 1, body_doc.GetNumberOfPages());
            header_doc.Close();
            body_doc.Close();
            pdfDoc.Close();
          //  GC.Collect();
        }

        static void generPDF(string table_name)
        {
          
            FileStream SourceStream = File.Create("body.pdf");
            //  FileStream SourceStream = File.Open("input.html", FileMode.Open);


            WebClient client = new WebClient();
            string url = "http://ecad.giulianovars.ru/autoprice/crea_shablon_csharp.php?table=" + table_name;
            var response = client.DownloadData(url);
            string html = Encoding.UTF8.GetString(response);
         
            // pdfHTML specific code
            ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(html, SourceStream);
         //   GC.Collect();
        }

     
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            table_name = (lb1.SelectedItem as spis_table).ssilk;
            generPDF(table_name);
            skleitb2f(table_name);
            Process.Start(table_name + ".pdf");
           
            GC.Collect();
        }
    public static  void generprice(string tablename)
        {
           
            generPDF(tablename);
            skleitb2f(tablename);
            Process.Start(tablename + ".pdf");

            GC.Collect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          //  GC.Collect();
        }
    }
    class spis_table{
        public string id { get; set; }
        public string name { get; set; }
        public string ssilk { get; set; }
  
    }
}
