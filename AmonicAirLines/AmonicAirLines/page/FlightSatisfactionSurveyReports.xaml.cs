using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using AmonicAirLines.Classes;
using AmonicAirLines.forXaml;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace AmonicAirLines.page
{
    public class Question
    {
        public string title { get; set; }
        public List<Statement> statements = new List<Statement>();
        public Grid perent { get; set; }
        public int questionNum {  get; set; }
        public int countColumns { get; set; } // 16
        
        public List<Cabins> cabinsList = new List<Cabins>();
        public List<Airports> airportsList = new List<Airports>();

        private int month {  get; set; }
        private int year { get; set; }
        private SolidColorBrush colorLightBlue = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dde5f2"));
        private List<int> q = new List<int>(8);
        private List<List<int>> table;

        public Question(int month, int year,int cc, ref List<Cabins> cabinsL,ref List<Airports> airportsL) 
        {
            cabinsList = cabinsL;
            airportsList = airportsL;
            countColumns = cc;
            Task.Run(async () =>
            {
                statements = Statement.GetStatement(month, year).Result;
            }).Wait();

            table = new List<List<int>>();
            for(int i = 0;i < countColumns; i++)
            {
                table.Add(new List<int>());
                for(int j = 0;j < answer.Count; j++)
                {
                    table[i].Add(0);
                }
            }

            for(int i = 0; i < 8; i++)
                q.Add(0);
            foreach (Statement statement in statements)
            {
                var t =int.Parse( statement.Answer.Split('|')[questionNum]);
                if (t != 0)
                {
                    int t1 = t - 1;
                    if(statement.Gender == "M")
                        table[0][t1]++;
                    else
                        table[1][t1]++;

                    if(18 <= statement.Age && statement.Age <= 24)
                        table[2][t1]++;
                    else if (25 <= statement.Age && statement.Age <= 39)
                        table[3][t1]++;
                    else if (48 <= statement.Age && statement.Age <= 59)
                        table[4][t1]++;
                    else
                        table[5][t1]++;

                    List<int> temp = new List<int>();
                    var t11 = cabinsList.Find(s => s.Id == statement.CabinTypeID);
                    var t12 = cabinsList.IndexOf(t11);
                    var t21 = airportsList.Find(s => s.Id == statement.DepartureAirportID);
                    var t22 = airportsList.IndexOf(t21);
                    table[
                        6
                        +t12
                        ][t1]++;
                    table[
                        6
                        + cabinsList.Count
                        + t22
                        ][t1]++;
                }
                q[t] += 1;
            }
        }

        private List<string> answer = new List<string>() 
        {
            "Outstanding",
            "VeryGood",
            "Good",
            "Adequate",
            "NeedsImprovement",
            "Poor",
            "DontKnow"
        };
        private List<SolidColorBrush> colorBrushes = new List<SolidColorBrush>()
        {
            //new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dad7db")),
            //new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e26a0c")),
            //new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e6b8b4")),
            //new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dbe1bc")),
            //new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c5d798")),
            //new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c5d49e")),
            //new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0000ff")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00ff00")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff0000")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0000ff")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00ff00")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff0000")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0000ff")),
        };


        public void Create()
        {
            //    < TextBlock ns: GridHelpers.GridConfig = "0,3,1,1" Text = "test" />
            //    < Grid ns: Rack.RackColumns = "65* 34* 123* 42* 12* 65* 34*" ns: GridHelpers.GridConfig = "1,3,16,1" Height = "9" >
            //        < Rectangle Grid.Column = "0" x: Name = "OutstandingBar" Fill = "Green" />
            //        < Rectangle Grid.Column = "1" x: Name = "VeryGoodBar" Fill = "Blue" />
            //        < Rectangle Grid.Column = "2" x: Name = "GoodBar" Fill = "Yellow" />
            //        < Rectangle Grid.Column = "3" x: Name = "AdequateBar" Fill = "Orange" />
            //        < Rectangle Grid.Column = "4" x: Name = "NeedsImprovementBar" Fill = "Red" />
            //        < Rectangle Grid.Column = "5" x: Name = "PoorBar" Fill = "DarkRed" />
            //        < Rectangle Grid.Column = "6" x: Name = "DontKnowBar" Fill = "Gray" />
            //    </ Grid >
            //    < !--2 row q-->
            //    < TextBlock
            //        ns: GridHelpers.GridConfig = "0,4,1,1"
            //        Text = "test"
            //        Background = "#dde5f2"
            //        HorizontalAlignment = "Stretch"
            //        TextAlignment = "Right" />
            //    < TextBlock
            //        ns: GridHelpers.GridConfig = "1,4,1,1"
            //        Text = "123"
            //        HorizontalAlignment = "Stretch"
            //        TextAlignment = "Center" />
            //    < TextBlock
            //        ns: GridHelpers.GridConfig = "2,4,1,1"
            //        Text = "123"
            //        Background = "#dde5f2"
            //        HorizontalAlignment = "Stretch"
            //        TextAlignment = "Center" />

            Func<string, int, string> getSett = (string src, int count) => {
                if (count <= 0)
                    return "";
                string res = "";
                for (int i = 0; i < count - 1; i++)
                    res += $"{src} ";
                res += $"{src}";
                return res;
            };

            TextBlock textBlock = new TextBlock();
            textBlock.Text = title;
            GridHelpers.SetGridConfig(textBlock, $"0,{questionNum*2+2},1,1");
            perent.Children.Add( textBlock );

            Grid bar = new Grid();
            GridHelpers.SetGridConfig(bar, $"1,{questionNum*2+2},{countColumns},1");
            string s1 = "";
            for (int i = 1; i < q.Count - 1; i++)
                s1 += q[i].ToString() + "* ";
            s1 += q[q.Count-1].ToString() + "*";
            Rack.SetRackColumns(bar, s1);
            for (int i = 0; i < colorBrushes.Count; i++)
            {
                Rectangle r = new Rectangle();
                GridHelpers.SetGridConfig(r, $"{i},0,1,1");
                r.Fill = colorBrushes[i];
                bar.Children.Add ( r );
            }
            bar.Height = 9;
            perent.Children.Add(bar);


            Grid answ = new Grid();
            GridHelpers.SetGridConfig(answ, $"0,{questionNum * 2 + 2 + 1},1,1");
            Rack.SetRackRows(answ, getSett("*", answer.Count));
            for (int i = 0; i < answer.Count; i++)
            {
                TextBlock t = new TextBlock();
                GridHelpers.SetGridConfig(t, $"0,{i},1,1");
                if (i % 2 == 1)
                {
                    t.Background = colorLightBlue;
                }
                t.Text = answer[i]; // заполнить
                t.HorizontalAlignment = HorizontalAlignment.Stretch;
                t.TextAlignment = TextAlignment.Right;
                answ.Children.Add(t);
            }
            perent.Children.Add(answ);


            Grid totalGrid = new Grid();
            GridHelpers.SetGridConfig(totalGrid, $"1,{questionNum*2+2 + 1},1,1");
            Rack.SetRackRows(totalGrid, getSett("*", answer.Count));
            for (int i = 0; i < answer.Count; i++)
            {
                TextBlock t = new TextBlock();
                GridHelpers.SetGridConfig(t, $"0,{i},1,1");
                t.Text = q[i+1].ToString(); // заполнить
                t.HorizontalAlignment = HorizontalAlignment.Stretch;
                t.TextAlignment = TextAlignment.Center;
                totalGrid.Children.Add( t );
            }
            perent.Children.Add( totalGrid);

            // тут два for
            for (int i = 0; i < table.Count; i++)
            {
                Grid column = new Grid();
                GridHelpers.SetGridConfig(column, $"{2+i},{questionNum * 2 + 2 + 1},1,1");
                Rack.SetRackRows(column, getSett("*", table[i].Count));
                for (int j = 0; j < table[i].Count; j++)
                {
                    TextBlock t = new TextBlock();
                    GridHelpers.SetGridConfig(t, $"0,{j},1,1");
                    t.Text = table[i][j].ToString();
                    t.HorizontalAlignment = HorizontalAlignment.Stretch;
                    t.TextAlignment = TextAlignment.Center;
                    column.Children.Add(t);
                }
                perent.Children.Add(column);
            }
        }
    }

    public class pd
    {
        public string content;
        public string gridConfig;
        public pd(string content, string gridConfig)
        {
            this.content = content;
            this.gridConfig = gridConfig;
        }
    }
    /// <summary>
    /// Логика взаимодействия для FlightSatisfactionSurveyReports.xaml
    /// </summary>
    public partial class FlightSatisfactionSurveyReports : Page
    {
        private List<Cabins> cabins = new List<Cabins>();
        private List<Airports> airports = new List<Airports>();
        private DataTable dataTable = new DataTable();
        List<Report> reports = new List<Report>();
        List<ReportDate> reportDates = new List<ReportDate>();
        //private string dataReport = string.Empty;
        Dictionary<int, string> months = new Dictionary<int, string>();
        public FlightSatisfactionSurveyReports()
        {
            InitializeComponent();
            months = new Dictionary<int, string>()
            {
                {1, "Январь"},
                {2, "Февраль"},
                {3, "Март"},
                {4, "Апрель"},
                {5, "Май"},
                {6, "Июнь"},
                {7, "Июль"},
                {8, "Август"},
                {9, "Сентябрь"},
                {10, "Октябрь"},
                {11, "Ноябрь"},
                {12, "Декабрь"}
            };
            PopulateInitComboBox();
            init();
            init1();

        }
        public void init1()
        {
            // https://localhost:7139/CabinTypes
            // https://localhost:7139/Airports
            Task.Run(async () =>
            {
                cabins = Cabins.GetCabinTypes().Result;
                airports = Airports.GetAirports().Result;
                dataTable = GetSumReport().Result;
                reportDates = ReportDate.GetReportDate().Result;
            }).Wait();
            string t = months[reportDates[0].Month] + " " + reportDates[0].Year + " - " + months[reportDates[reportDates.Count - 1].Month] + " " + reportDates[reportDates.Count - 1].Year;
            tb_fieldwork.Text = t;
            PrintDataTable(dataTable);
            List<pd> list = new List<pd>()
            {
                new pd("", "1,0,1,1"),
                new pd("Gender", "2,0,2,1"),
                new pd("Age", "4,0,4,1"),
                new pd("Cabin Type", $"8,0,{cabins.Count},1"),
                new pd("Destination Airport", $"{8+cabins.Count},0,{airports.Count},1"),
                new pd("total", "1,1,1,1"),
            };
            tb_sampleSize.Text = dataTable.Rows[0]["count"].ToString();
            int k = 0;
            foreach (DataColumn column in dataTable.Columns)
            {
                if (k != 0)
                    list.Add(new pd(column.ColumnName, $"{k+1},1,1,1"));
                k++;
            }

            var gr = g_table1;
            Rack.SetRackColumns(gr, "Auto " + getSettings(7, "*") + " " + getSettings(cabins.Count + airports.Count, "Auto"));
            Rack.SetRackRows(gr, "Auto Auto Auto Auto");
            k = 0;
            foreach (pd pd in list)
            {
                var border = new Border();
                var textBlock = new TextBlock();

                textBlock.Text = pd.content;
                textBlock.Style = (Style)Application.Current.FindResource("CenteredTextBlockStyle");

                border.Style = (Style)Application.Current.FindResource("DefaultBorderStyle");
                if (k >= 6)
                {
                    textBlock.Margin = new Thickness(5, 0, 5, 0);
                }
                k++;
                GridHelpers.SetGridConfig(border, pd.gridConfig);

                border.Child = textBlock;

                gr.Children.Add(border);
            }
            Question question = new Question(1, 2021, 6 + cabins.Count + airports.Count,ref cabins,ref airports);
            question.title = "test1";
            question.perent = gr;
            question.questionNum = 0;
            question.countColumns = 8 + cabins.Count + airports.Count;
            question.Create();

        }
        private void PopulateInitComboBox()
        {

            monthComboBox.ItemsSource = months;
            monthComboBox.SelectedIndex = -1;
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear - 10; year <= currentYear + 10; year++)
            {
                yearComboBox.Items.Add(year);
            }
            yearComboBox.SelectedItem = currentYear;
            yearComboBox.SelectedIndex = -1;
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.Key.ToString());
            if (e.Key == Key.D && Keyboard.Modifiers == ModifierKeys.Control)
            {
                tc_main.SelectedIndex = 2;
            }
        }

        public string getSettings(int count, string src)
        {
            if (count <= 0)
                return "";
            
            string res = "";
            
            for (int i = 0; i < count -1; i++)
                res += $"{src} ";
            res += $"{src}";
            
            return res;
        }
        public async Task<DataTable> GetSumReport()
        {
            string url = $"{App.PROTOCOL}://localhost:{App.PORT}/GetSumReport";
            string responseBody = "asd";
            DataTable dataTable = new DataTable();

            HttpResponseMessage response = await HttpClientSingleton.Client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                responseBody = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(responseBody);
                dataTable = JsonConvert.DeserializeObject<DataTable>(responseBody);
                return dataTable;
            }
            else
            {
            }
            return dataTable;
        }
        public void PrintDataTable(DataTable dataTable)
        {
            // Проверяем, не пуста ли таблица
            if (dataTable == null || dataTable.Columns.Count == 0)
            {
                Console.WriteLine("DataTable is empty or null.");
                return;
            }

            // Вывод названий столбцов
            foreach (DataColumn column in dataTable.Columns)
            {
                Console.Write(column.ColumnName + "\t");
            }
            Console.WriteLine();

            // Вывод значений каждой строки
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    Console.Write(row[column] + "\t");
                }
                Console.WriteLine();
            }
        }
        public void init()
        {
            // https://localhost:7139/CabinTypes
            // https://localhost:7139/Airports
            Task.Run(async () =>
            {
                cabins = Cabins.GetCabinTypes().Result;
                airports = Airports.GetAirports().Result;
                dataTable = GetSumReport().Result;
                reportDates = ReportDate.GetReportDate().Result;
            }).Wait();
            //foreach (var i in reportDates)
            //{
            //    Console.WriteLine(i.Month.ToString() + " " + i.Year.ToString());
            //}
            string t = months[reportDates[0].Month] + " " + reportDates[0].Year + " - " + months[reportDates[reportDates.Count-1].Month] + " " + reportDates[reportDates.Count - 1].Year;
            tb_fieldwork.Text = t;
            PrintDataTable(dataTable);
            List<pd> list = new List<pd>() 
            { 
                new pd("Gender", "0,0,2,1"),
                new pd("Age", "2,0,4,1"),
                new pd("Cabin Type", $"6,0,{cabins.Count},1"),
                new pd("Destination Airport", $"{6+cabins.Count},0,{airports.Count},1"),

                //new podsos("Male", "0,1,1,1"),
                //new podsos("Female", "1,1,1,1"),
                //new podsos("18-24","2,1,1,1"),
                //new podsos("25-39", "3,1,1,1"),
                //new podsos("40-59", "4,1,1,1"),
                //new podsos("60+", "5,1,1,1"),
                //new podsos("Economy", "6,1,1,1"),
                //new podsos("Busines", "7,1,1,1"),
                //new podsos("First", "8,1,1,1"),
                //new podsos("AUH", "9,1,1,1"),
                //new podsos("BAH", "10,1,1,1"),
                //new podsos("DOH", "11,1,1,1"),
                //new podsos("RYU", "12,1,1,1"),
                //new podsos("CAI", "13,1,1,1"),
            };
            tb_sampleSize.Text = dataTable.Rows[0]["count"].ToString();
            int k = 0;
            foreach (DataColumn column in dataTable.Columns)
            {
                if (k != 0)
                {
                    list.Add(new pd(column.ColumnName, $"{k-1},1,1,1"));
                    //Console.Write(column.ColumnName + "\t");
                }
                k++;
            }
            //Console.WriteLine();
            // Вывод значений каждой строки
            foreach (DataRow row in dataTable.Rows)
            {
                k = 0;
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (k != 0)
                    {
                        list.Add(new pd(row[column].ToString(), $"{k - 1},2,1,1"));
                    //Console.Write(row[column] + "\t");
                    }
                    k++;
                }
                //Console.WriteLine();
            }

            //foreach (var cabin in cabins)
            //{
            //    list.Add(new podsos(cabin.Name, $"{k},1,1,1"));
            //    k++;
            //}

            //foreach (var airport in airports)
            //{
            //    list.Add(new podsos(airport.Iatacode, $"{k},1,1,1"));
            //    k++;
            //}

            var gr = g_table;
            Rack.SetRackColumns(gr, getSettings(6, "*") + " " + getSettings(cabins.Count + airports.Count, "Auto"));
            Rack.SetRackRows(gr, "* * *");
            k = 0;
            foreach (pd pd in list)
            {
                var border = new Border();
                var textBlock = new TextBlock();
                    
                textBlock.Text = pd.content;
                textBlock.Style = (Style)Application.Current.FindResource("CenteredTextBlockStyle");

                border.Style = (Style)Application.Current.FindResource("DefaultBorderStyle");
                if (k >= 6)
                {
                    textBlock.Margin = new Thickness(5, 0, 5, 0);
                }
                k++;
                GridHelpers.SetGridConfig(border, pd.gridConfig);

                border.Child = textBlock;

                gr.Children.Add(border);
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as MenuItem).Header)
            {
                case ("_View Result Summary"):
                    tc_main.SelectedIndex = 0;
                    break;
                case ("_View Datailed Result"):
                    tc_main.SelectedIndex = 1;
                    break;
                case ("_Exit"):
                    NavigationService.GoBack();
                    break;

            }
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog();
                fileDialog.Filter = "CSV file (.csv)|*.csv";
                bool? resilt = fileDialog.ShowDialog();
                if (resilt == true)
                {
                    string filePath = fileDialog.FileName;
                    var t = filePath.Split('\\');
                    tb_fileName.Text = t[t.Length-1];
                    //Console.WriteLine(filePath);
                    var r = File.ReadAllText(filePath).Replace("\r", "");
                    var lines = r.Split('\n');
                    for (int i = 1; i < lines.Length; i++)
                    {
                        var args = lines[i].Split(',');
                        bool containsEmpty = args.Any(s => string.IsNullOrEmpty(s));
                        //Console.Write(lines[i] + " ");
                        if (containsEmpty)
                        {
                            //Console.WriteLine("~");
                            continue;
                        }
                        //Console.WriteLine("!!!!!");
                        int DepartureAirportID = airports.Where(s => s.Iatacode == args[0]).First().Id;
                        int ArrivalAirportID = airports.Where(s => s.Iatacode == args[1]).First().Id;
                        int Age = int.Parse(args[2]);
                        string Gender = args[3];
                        int CabinTypeID = cabins.Where(s => s.Name.Contains(args[4])).First().Id;
                        string Answer = "";
                        for (int j = 5; j < args.Length; j++)
                        {
                            Answer += args[j] + "|";
                        }
                        Answer = Answer.Trim('|');
                        reports.Add(
                            new Report(
                                    DepartureAirportID,
                                    ArrivalAirportID,
                                    Age,
                                    Gender,
                                    CabinTypeID,
                                    Answer
                                )
                            );
                    }
                }
                else
                {
                    MessageBox.Show("Файл не выбран");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка\n" + ex.Message);
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string API = $"{App.PROTOCOL}://localhost:{App.PORT}/AddReports";
            if (reports.Count <= 0)
            {
                MessageBox.Show("данные не загруженны");
                return;
            }
            if (monthComboBox.SelectedIndex == -1 || yearComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("выбирите дату");
                return;
            }
            for (int i = 0; i < reports.Count; i++)
            {
                reports[i].Month = ((KeyValuePair<int, string>)monthComboBox.SelectedItem).Key;
                reports[i].Year = (int)yearComboBox.SelectedValue;
            }
            //Console.WriteLine(JsonConvert.SerializeObject(reports));
            post(API, JsonConvert.SerializeObject(reports));
            reports.Clear();
            monthComboBox.SelectedIndex = -1;
            yearComboBox.SelectedIndex = -1;
            tb_fileName.Text = "";
        }
    }
}
