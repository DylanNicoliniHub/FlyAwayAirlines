using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BusinessDomain.Contracts;

namespace FlightBooking.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {

            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://flyawayairlinewebapi.azurewebsites.net/");
                //client.BaseAddress = new Uri("http://localhost:11631/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                for (var i = 0; i < 5000; i++)
                {
                    var flightBooking = new FlightPlanViewModel
                    {
                        FlightPlanId = i,
                        AircraftName = "Wings " + i,
                        OriginAirport = "Bradley International Airport",
                        DestinationAirport = "Philadelphia International Airpot",
                        DepartureTime = DateTime.Now,
                        ArrivalTime = DateTime.Now.AddHours(5),
                        SeatPrice = 300.50m
                    };

                    await client.PostAsJsonAsync("api/FlightPlans", flightBooking);

                    Console.WriteLine(String.Format("Transmitted iteration: {0}", i));
                }
            }
        }
    }
}
