using AmonicAirLines.Classes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AmonicAirLines.page
{
    public class Statement
    {
        public int DepartureAirportID { get; set; }
        public int Age {get; set;}
        public string Gender {get; set;}
        public int CabinTypeID {get; set;}
        public string Answer {get; set;}
        public static async Task<List<Statement>> GetStatement(int month, int year)
        {

            string url = $"{App.PROTOCOL}://localhost:{App.PORT}/GetReport?month={month}&year={year}";
            string responseBody;

            HttpResponseMessage response = await HttpClientSingleton.Client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                responseBody = response.Content.ReadAsStringAsync().Result;
                List<Statement> res= JsonConvert.DeserializeObject<List<Statement>>(responseBody);
                return res;
            }
            return new List<Statement>();
        }
    }
}