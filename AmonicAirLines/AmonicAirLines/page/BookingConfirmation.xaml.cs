using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AmonicAirLines.page
{
    /// <summary>
    /// Логика взаимодействия для BookingConfirmation.xaml
    /// </summary>
    public partial class BookingConfirmation : Page
    {
        public bool ret;
        public FlightForBooking outFlight;
        public FlightForBooking retFlight;
        public int passengerCount;

        List<Country> CountryList = new List<Country>();
        //List<Passenger> passengers = new List<Passenger>();
        private ObservableCollection<Passenger> passengers = new ObservableCollection<Passenger>();
        internal string outIatacode;
        internal string retIatacode;
        internal string cabinType;
        internal int cabinTypeID;
        internal string outDate;
        internal string retDate;

        public bool completed = false;

        public BookingConfirmation()
        {
            InitializeComponent();
            foo();
            passengersDataGrid.ItemsSource = passengers;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Key.ToString());
            if (e.Key == Key.D && Keyboard.Modifiers == ModifierKeys.Control)
            {
                tb_Firstname.Text = "max";
                tb_Lastname.Text = "portnov";
                tb_Birthdate.Text = "21/01/2005";
                tb_PassportNumber.Text = "123321";
                cb_PassportCountry.SelectedIndex = 4;
                tb_Phone.Text = "(913) 465-9662";
                Button_Click_1(sender, e);
                tb_Firstname.Text = "petya";
                tb_Lastname.Text = "kornov";
                tb_Birthdate.Text = "17/04/2004";
                tb_PassportNumber.Text = "321123";
                cb_PassportCountry.SelectedIndex = 3;
                tb_Phone.Text = "(952) 876-9361";
                Button_Click_1(sender, e);
            }
        }

        public void init()
        {
            lb_outFrom.Content += " " + outIatacode;
            lb_outTo.Content += " " + retIatacode;
            lb_outCabinType.Content += " " + cabinType;
            lb_outDate.Content += " " + outDate;
            lb_outFlightNumber.Content += " " + outFlight.FlightNumbers.Replace(", ", " - ");

            if (!ret)
            {
                heightReturnGB.Height = new GridLength(0);
                ReturnFlightDetails.Visibility = Visibility.Collapsed;
            } else
            {
                lb_retFrom.Content += " " + retIatacode;
                lb_retTo.Content += " " + outIatacode;
                lb_retCabinType.Content += " " + cabinType;
                lb_retDate.Content += " " + retDate;
                lb_retFlightNumber.Content += " " + retFlight.FlightNumbers.Replace(", ", " - ");

            }
        }

        async public void foo()
        {
            await LoadAirportsAsync();

        }
        private async Task<int> LoadAirportsAsync()
        {
            CountryList = await Country.GetCountry();

            cb_PassportCountry.ItemsSource = CountryList;
            cb_PassportCountry.DisplayMemberPath = "Name";

            cb_PassportCountry.SelectedValuePath = "Id";

            return 1;
        }

        private void fillDataGrid()
        {
            
        }

        private void OutboundTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = tb_Birthdate.Text;

            // Если вводимый символ не цифра и не '/', и длина текста уже максимальна, то игнорируем ввод
            if ((!char.IsDigit(e.Text[0]) && e.Text[0] != '/') || text.Length >= 10)
            {
                e.Handled = true;
                return;
            }

            // Если вводимый символ '/' и в тексте уже есть 2 '/' или длина текста больше 9, то игнорируем ввод
            if (e.Text[0] == '/' && (text.Count(c => c == '/') >= 2 || text.Length >= 10))
            {
                e.Handled = true;
                return;
            }

            // Если вводим '/' и находимся на 2 или 5 позиции, то игнорируем ввод
            if (e.Text[0] == '/' && (text.Length == 2 || text.Length == 5))
            {
                e.Handled = true;
                return;
            }
        }

        private void OutboundTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = tb_Birthdate.Text;

            // Если текст короче 2 символов, то ничего не делаем
            if (text.Length < 2)
                return;

            // Если вставлен символ после 2 или 5 символа и он не '/', то добавляем '/'
            if (text.Length == 3 && text[2] != '/')
            {
                tb_Birthdate.Text = text.Insert(2, "/");
                tb_Birthdate.SelectionStart = 4;
            }
            else if (text.Length == 6 && text[5] != '/')
            {
                tb_Birthdate.Text = text.Insert(5, "/");
                tb_Birthdate.SelectionStart = 7;
            }

            // Если длина текста больше 2 и предыдущий введенный символ был '/', а текущий не '/', то перемещаем курсор в конец
            if (text.Length > 2 && text[text.Length - 2] == '/' && text[text.Length - 1] != '/')
            {
                tb_Birthdate.CaretIndex = text.Length;
            }
        }
        private void PhoneNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = (sender as TextBox).Text;

            // Проверка, что вводится только цифра
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true; // Отмена ввода, если символ не цифра
                return;
            }

            // Ограничение длины вводимого номера (допускается максимум 14 символов, включая скобки и дефис)
            if (text.Length >= 14)
            {
                e.Handled = true;
                return;
            }
        }

        private void PhoneNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            string text = t.Text;
            int selectionStart = t.SelectionStart;
            Console.WriteLine(selectionStart);
            // Автоматическое добавление скобок и дефиса
            if (text.Length == 4 && !text.StartsWith("("))
            {
                t.Text = t.Text.Insert(0, "(");
                t.Text = t.Text.Insert(4, ") ");
                t.SelectionStart = selectionStart + 6; // Смещение курсора за скобки и пробел
            }
            else if (text.Length == 10 && !text.Contains("-"))
            {
                //t.Text = $"{text}-";
                t.Text = t.Text.Insert(9, "-");
                t.SelectionStart = selectionStart + 1; // Смещение курсора за дефис
            }

            // Восстановление позиции курсора при удалении символов
            //if (selectionStart < t.Text.Length)
                //t.SelectionStart = t.SelectionStart;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private bool TryConvertPhoneToLong(string phoneText, out long phoneNumber)
        {
            string digitsOnly = Regex.Replace(phoneText, "[^0-9]", "");
            return long.TryParse(digitsOnly, out phoneNumber);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string firstName = tb_Firstname.Text;
            string lastName = tb_Lastname.Text;
            string passportNumber = tb_PassportNumber.Text;
            string phone = tb_Phone.Text;

            if (passengers.Count >= passengerCount)
            {
                return;
            }

            if (!DateTime.TryParseExact(tb_Birthdate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthdate))
            {
                MessageBox.Show("Некорректная дата рождения. Формат должен быть dd/MM/yyyy.");
                return ;
            }

            // Проверка имени и фамилии
            if (string.IsNullOrWhiteSpace(tb_Firstname.Text) || !tb_Firstname.Text.All(char.IsLetter))
            {
                MessageBox.Show("Некорректное имя.");
                return ;
            }

            if (string.IsNullOrWhiteSpace(tb_Lastname.Text) || !tb_Lastname.Text.All(char.IsLetter))
            {
                MessageBox.Show("Некорректная фамилия.");
                return ;
            }

            // Проверка номера паспорта
            if (string.IsNullOrWhiteSpace(tb_PassportNumber.Text) || !tb_PassportNumber.Text.All(char.IsLetterOrDigit))
            {
                MessageBox.Show("Некорректный номер паспорта.");
                return ;
            }

            // Проверка номера телефона
            long phoneAsLong;
            string pattern = @"^\(\d{3}\) \d{3}-\d{4}$";
            if (!Regex.IsMatch(tb_Phone.Text, pattern))
            {
                MessageBox.Show("Некорректный номер телефона. Формат должен быть (XXX) XXX-XXXX.");
                return ;
            }
            else if (!TryConvertPhoneToLong(tb_Phone.Text, out phoneAsLong))
            {
                MessageBox.Show("Не удалось преобразовать номер телефона в числовой формат.");
                return ;
            }
            string phoneNumber = phoneAsLong.ToString().Insert(3, "-").Insert(7, "-");

            // Проверка ID страны паспорта
            if (!(cb_PassportCountry.SelectedValue is int passportCountryID) || passportCountryID <= 0)
            {
                MessageBox.Show("Некорректный ID страны паспорта.");
                return ;
            }


            passengers.Add(new Passenger()
            {
                birthdate = birthdate,
                firstName = firstName,
                lastName = lastName,
                passportNumber = passportNumber,
                phone = phoneNumber,
                passportCountryID = passportCountryID
            });
            fillDataGrid();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (passengersDataGrid.SelectedItems.Count != 1)
            {
                MessageBox.Show("выбирите одного пасажира");
                return;
            }
            if (passengersDataGrid.SelectedIndex >= passengers.Count)
            {
                return;
            }
            passengers.RemoveAt(passengersDataGrid.SelectedIndex);
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // {
            //  "userID":userID,
            //  "scheduleID":[
            //      1,2,3,4
            //  ],
            //  "cabinType":cabinTypeID,
            //  "passengers":[
            //      {
            //          "birthdate":"tt",
            //          "firstName":"tt",
            //          "lastName":"tt",
            //          "passportNumber":"tt",
            //          "phone":"tt",
            //          "passportCountryaID":"tt",
            //      }
            //  ]   
            // }
            int userID = StartPage.userID;
            if (passengers.Count != passengerCount)
            {
                MessageBox.Show("добавьте всех пасажиров");
                return;
            }

            var win = new BillingConfirmation();
            win.totalAmount = outFlight.CabinPrice + (ret ? retFlight.CabinPrice : 0);
            win.init();
            win.ShowDialog();
            completed = win.paymentCompleted;
            if (!completed)
            {
                return;
            }

            List<int> outScheduleID = new List<int>(outFlight.Ids);
            List<Passenger> pas = new List<Passenger>(passengers);
            var t = new Ticket()
            {
                UserID = userID,
                ScheduleID = outScheduleID,
                CabinTypeID = cabinTypeID + 1,
                Passengers = pas,
            };
            string json = JsonConvert.SerializeObject(t);
            string apiUrl = $"{App.PROTOCOL}://localhost:{App.PORT}/CreateTickets";
            post(apiUrl, json);
            if (ret)
            {
                List<int> retScheduleID = new List<int>(retFlight.Ids);
                var t2 = new Ticket()
                {
                    UserID = userID,
                    ScheduleID = retScheduleID,
                    CabinTypeID = cabinTypeID + 1,
                    Passengers = pas,
                };
                json = JsonConvert.SerializeObject(t2);
                post(apiUrl, json);
            }
            NavigationService.GoBack();
            NavigationService.GoBack();
        }
        private async void post(string apiUrl, string content)
        {
            string responseBody = "";
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, apiUrl))
                {
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await HttpClientSingleton.Client.SendAsync(request).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
            }
        }
    }
}
