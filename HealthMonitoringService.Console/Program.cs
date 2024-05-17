using Common.Contracts;
using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HealthMonitoringService.ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input;
            do
            {
                Console.WriteLine("Lozinka: ");
                input = Console.ReadLine();
            } while (input != "111");

            string fullEndpoint = "net.tcp://127.255.0.0:10100/Mail";
            ChannelFactory<IMailService> factory = new ChannelFactory<IMailService>(new NetTcpBinding(), new EndpointAddress(fullEndpoint));
            var client = factory.CreateChannel();
            while (true)
            {
                Console.WriteLine("Komnade:");
                Console.WriteLine("1. Lista email-ova");
                Console.WriteLine("2. Dodavanje emaila");
                Console.WriteLine("3. Brisanje emaila");

                input = Console.ReadLine();
                if (int.TryParse(input, out int value))
                {
                    switch (value)
                    {
                        case 1:
                            foreach (var item in client.Get())
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        case 2:
                            input = Console.ReadLine();
                            client.Add(input);
                            break;
                        case 3:
                            input = Console.ReadLine();
                            client.Delete(input);
                            break;

                        default:
                            break;
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
