using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
using AmonicAirLines.Classes;

namespace AmonicAirLines.page
{
    
    /// <summary>
    /// Логика взаимодействия для AdminMainPage.xaml
    /// </summary>
    public partial class AdminMainPage : Page
    {
        public List<Office> OfficeList = new List<Office>();
        public List<AnotherUser> users1 = new List<AnotherUser>();

        public AdminMainPage()
        {
            InitializeComponent();
            foo();

        }
        public void foo()
        {
            
            LoadOfficesAsync();
            fillComboBox();
        }

        private int LoadOfficesAsync()
        {
            Task.Run(async () =>
            {
                OfficeList = await Office.GetOffices();
            }).Wait();

            OfficeComboBox.ItemsSource = OfficeList;
            OfficeComboBox.DisplayMemberPath = "Title";

            OfficeComboBox.SelectedValuePath = "Id";
            return 1;
        }

        public int fillComboBox()
        {

            int officeId = -1;
            if (OfficeComboBox.SelectedItem != null)
            {
                if (OfficeComboBox.SelectedItem is Office selectedOffice)
                {
                    officeId = selectedOffice.Id;
                }
            }
            Console.WriteLine(officeId);
            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/UsersFromOffice?officeId={officeId}";
            string responseBody = "";
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                Task.Run(async () =>
                {
                    response = await HttpClientSingleton.Client.GetAsync(apiUrl);
                }).Wait();
                if (response.IsSuccessStatusCode)
                {
                    Task.Run(async () =>
                    {
                        responseBody = await response.Content.ReadAsStringAsync();
                    }).Wait();
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(responseBody);
                    users1.Clear();
                    foreach (var item in users)
                    {
                        users1.Add(new AnotherUser(item, OfficeList.Where(x => x.Id == item.OfficeId).SingleOrDefault().Title));
                        //Console.WriteLine(users1[users1.Count - 1].ToString());
                    }
                    //Console.WriteLine(users.Count);
                    dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = users1;
                    dataGrid.SelectedValuePath = "Id";
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
            }
            return 1;
        }

        public void OfficeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _ = fillComboBox();
        }


        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUser window = new AddUser();
            window.ShowDialog();
            _ = fillComboBox();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnClearOffice_Click(object sender, RoutedEventArgs e)
        {
            if (OfficeComboBox.SelectedItem == null)
            {
                _ = fillComboBox();
            }
            OfficeComboBox.SelectedItem = null;
        }

        private void btnChangeRole_Click(object sender, RoutedEventArgs e)
        {
            AnotherUser user;
            if (dataGrid.SelectedItem != null)
            {
                user = dataGrid.SelectedItem as AnotherUser;
            }
            else
            {
                MessageBox.Show("Choose");
                return;
            }

            EditRole editRole = new EditRole();
            editRole.anotherUser = user;

            //Hide();
            editRole.ShowDialog();
            //Show();
            _ = fillComboBox();
        }

        private void btnEnDisLogin_Click(object sender, RoutedEventArgs e)
        {
            int userId = -1;
            if (dataGrid.SelectedItem != null)
            {
                if (dataGrid.SelectedItem is AnotherUser user)
                {
                    userId = user.Id;
                }
            }
            else
            {
                MessageBox.Show("Choose");
                return;
            }
            if (dataGrid.SelectedItems.Count > 1)
            {
                MessageBox.Show("выберите одного юзера");
                return;
            }
            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/UserSwitch?userId={userId}";
            Task.Run(async () =>
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.PutAsync(apiUrl, null);
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Успешно выполнен запрос.");
                        }
                        else
                        {
                            Console.WriteLine("Запрос завершился с ошибкой: " + response.StatusCode);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при выполнении запроса: " + ex.Message);
                        return;
                    }
                }
            }).Wait();
            _ = fillComboBox();
        }

        private void ManageFligght_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageFlightSchedulesPage());
        }

        private void SearchForFlights_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SearchForFlights());
        }
    }
}
