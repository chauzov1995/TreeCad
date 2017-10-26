using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

            Grid.SetColumnSpan(browser, 2);
            browser.Source = new System.Uri("https://oauth.yandex.ru/authorize?response_type=code&client_id=bfec704e15514115b366a5d3512cd66f");
        }

        private void browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Grid.SetColumnSpan(browser, 2);
            string code = HttpUtility.ParseQueryString(browser.Source.Query).Get("code");
            if (browser.Source.ToString() == "https://www.yandex.ru/")
            {
                browser.Source = new System.Uri("https://oauth.yandex.ru/authorize?response_type=code&client_id=bfec704e15514115b366a5d3512cd66f");

            }
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


                //webClient = new WebClient();
                //url = "https://webdav.yandex.ru/?userinfo";
                //Thread.Sleep(1000);
                //webClient.Headers.Add("Authorization", "OAuth " + account.access_token);
                //string response1 = webClient.DownloadString(url);
                //string save_html = System.Text.Encoding.UTF8.GetString(response);
                //MessageBox.Show(response1);
                //Login login = JsonConvert.DeserializeObject<Login>(response);

                //   MessageBox.Show(login);

                //сохраняем в настройках
                YA ps = YA.Default;
                ps.OAuth = account.access_token;
                ps.Save();

                Grid.SetColumnSpan(browser, 1);
                // browser.SetValue(Grid.ColumnSpan, 1); .Grid.ColumnSpan
                //   Close();
            }
            else
            {

            }

        }
        string getname(string OAuthp)
        {
            WebClient webClient = new WebClient();
            string url = "https://webdav.yandex.ru/?userinfo";
            webClient.Headers.Add("Authorization", "OAuth " + OAuthp);
            string response = webClient.DownloadString(url);
            //   string save_html = System.Text.Encoding.UTF8.GetString(response);
            MessageBox.Show(response);
            Login login = JsonConvert.DeserializeObject<Login>(response);
            return login.login;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
    public class OAuth
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
    public class Login
    {
        public string login { get; set; }

    }
}

