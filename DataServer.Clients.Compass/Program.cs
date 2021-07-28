using DataServer.ClientLibrary;
using System;
using System.Threading.Tasks;

namespace DataServer.Clients.Navigation
{
    class Program
    {
        static void Main(string[] args)
        {
            var compassApp = new CompassApp();
            compassApp.Run();

            Console.ReadLine();
        }


    }
}
