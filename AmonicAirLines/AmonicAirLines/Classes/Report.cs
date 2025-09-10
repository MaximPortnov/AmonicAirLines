using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirLines.Classes
{
    public class ReportDate
    {
        public int Month { get; set; }
        public int Year { get; set; }
        static public async Task<List<ReportDate>> GetReportDate()
        {
            string url = $"{App.PROTOCOL}://localhost:{App.PORT}/GetSumReportDate";
            string responseBody = "asd";
            List<ReportDate> reportDates = new List<ReportDate>();

            HttpResponseMessage response = await HttpClientSingleton.Client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                responseBody = response.Content.ReadAsStringAsync().Result;
                reportDates = JsonConvert.DeserializeObject<List<ReportDate>>(responseBody);
                return reportDates;
            }
            else
            {
            }
            return reportDates;
        }
    }

    public class Report
    {
        public int DepartureAirportID;
        public int ArrivalAirportID;
        public int Age;
        public string Gender;
        public int CabinTypeID;
        public string Answer;
        public int Month;
        public int Year;

        public Report(int departureAirportID, int arrivalAirportID, int age, string gender, int cabinTypeID, string answer)
        {
            DepartureAirportID = departureAirportID;
            ArrivalAirportID = arrivalAirportID;
            Age = age;
            Gender = gender;
            CabinTypeID = cabinTypeID;
            Answer = answer;
        }
    }
}
