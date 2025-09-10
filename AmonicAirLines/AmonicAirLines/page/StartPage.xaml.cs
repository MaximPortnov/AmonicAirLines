using AmonicAirLines.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AmonicAirLines.page
{
    public class Usr
    {
        public int userID {  get; set; }
        public string status {  get; set; }
        public Usr(int userID, string status) 
        { 
            this.userID = userID;
            this.status = status;
        }
        public Usr()
        {
            userID = -1;
            status = "";
        }
    }
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        private static Dictionary<string, int> loginAttempts = new Dictionary<string, int>();
        private static Dictionary<string, DateTime> lockedUsers = new Dictionary<string, DateTime>();
        public static int userID;

        public StartPage()
        {
            InitializeComponent();
        }
        static public async Task<Usr> checkUser(string username, string password)
        {
            string responseBody = "False";

            string hashedPassword = "";
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = md5.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                hashedPassword = sb.ToString();
            }
            Console.WriteLine(hashedPassword);

            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/CheckUsers?login={username}&password={hashedPassword}";

            //string status = "";
            Usr usr = new Usr();
            try
            {
                HttpResponseMessage response = await HttpClientSingleton.Client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                    UserCheckResult result = JsonConvert.DeserializeObject<UserCheckResult>(responseBody);

                    // Получение переменных userID и status из объекта result
                    usr = new Usr(result.UserId, result.Status);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
            }
            return usr;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Text;

            if (lockedUsers.ContainsKey(username) && lockedUsers[username] > DateTime.Now)
            {
                // Пользователь заблокирован, показать сообщение о времени ожидания
                TimeSpan remainingTime = lockedUsers[username] - DateTime.Now;
                MessageBox.Show($"Пользователь заблокирован. Повторите попытку через {remainingTime.TotalSeconds} секунд.");
                return;
            }

            Usr usr = new Usr();
            Task.Run(async () =>
            {
                usr = await checkUser(username, password);
            }).Wait();
            userID = usr.userID;
            if (usr.status == "user")
            {
                loginAttempts[username] = 0;
                if (lockedUsers.ContainsKey(username))
                    lockedUsers.Remove(username);
                NavigationService.Navigate(new UserMainPage());
            }
            if (usr.status == "admin")
            {
                loginAttempts[username] = 0;
                if (lockedUsers.ContainsKey(username))
                    lockedUsers.Remove(username);
                NavigationService.Navigate(new AdminMainPage());

            }
            else if (usr.status == "block")
            {
                MessageBox.Show("Вы заблокированы!");
            }
            else if (usr.status == "false")
            {
                if (!loginAttempts.ContainsKey(username))
                {
                    loginAttempts[username] = 0;
                }
                loginAttempts[username]++;
                if (loginAttempts[username] >= 3)
                {
                    lockedUsers[username] = DateTime.Now.AddSeconds(10);
                    MessageBox.Show("Превышен лимит попыток входа. Пользователь заблокирован на 10 секунд.");
                    return;
                }
                MessageBox.Show("Неправильное имя пользователя или пароль.");
            }
            else if (usr.status == "error")
            {
                MessageBox.Show("Ошибка входа");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
