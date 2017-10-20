using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TreeCadN.evesync
{
    /// <summary>
    /// Логика взаимодействия для auth.xaml
    /// </summary>
    public partial class auth : Window
    {

        public auth()
        {
            InitializeComponent();

            browser.Source = new System.Uri("https://oauth.yandex.ru/authorize?response_type=code&client_id=bfec704e15514115b366a5d3512cd66f");
        }

        private void browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            string code = HttpUtility.ParseQueryString(browser.Source.Query).Get("code");
            if (code != "")
            {
                var url = "https://oauth.yandex.ru/token";
                var webClient = new WebClient();
                // Создаём коллекцию параметров
                var pars = new NameValueCollection();

                webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                webClient.Headers.Add("Authorization", "Basic YmZlYzcwNGUxNTUxNDExNWIzNjZhNWQzNTEyY2Q2NmY6M2Y2MzZlMjE3NWExNDQwZWFmZDdkODYzMGJhNTJiMjA=");


                // Добавляем необходимые параметры в виде пар ключ, значение
                pars.Add("grant_type", "authorization_code");
                pars.Add("code", code);
                
                // Посылаем параметры на сервер
                // Может быть ответ в виде массива байт
                var response = webClient.UploadValues(url, pars);
                string save_html = System.Text.Encoding.UTF8.GetString(response);
                OAuth account = JsonConvert.DeserializeObject<OAuth>(save_html);
                // MessageBox.Show(account.access_token.ToString());

                //сохраняем в настройках
                YA ps = YA.Default;
                ps.OAuth = account.access_token;
                ps.Save();

                Close();
            }
        }
    }
    public class OAuth
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}

