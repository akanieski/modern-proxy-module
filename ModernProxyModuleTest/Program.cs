using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ModernProxyModule
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var request = WebRequest.Create("https://api.ipify.org");

            request.Proxy = new CustomProxy();

            var response = request.GetResponse();
            

            Console.WriteLine(new StreamReader(response.GetResponseStream()).ReadToEnd());

            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
