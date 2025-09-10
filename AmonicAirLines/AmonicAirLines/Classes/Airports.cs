using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirLines.Classes
{
    public class Airports
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Iatacode { get; set; }
        public string Name { get; set; }

        public static async Task<List<Airports>> GetAirports()
        {

            string url = $"{App.PROTOCOL}://localhost:{App.PORT}/Airports";
            string responseBody = "asd";
            
            HttpResponseMessage response = await HttpClientSingleton.Client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                responseBody = response.Content.ReadAsStringAsync().Result;
                List<Airports> airports = JsonConvert.DeserializeObject<List<Airports>>(responseBody);
                Console.WriteLine(responseBody);
                
                return airports;
            }
            else
            {
            }
            return new List<Airports>();
        }
    }
}
