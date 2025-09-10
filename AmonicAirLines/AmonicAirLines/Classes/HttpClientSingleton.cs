using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirLines.Classes
{
    internal class HttpClientSingleton
    {
        public static HttpClient Client { get; } = new HttpClient();
    }
}
