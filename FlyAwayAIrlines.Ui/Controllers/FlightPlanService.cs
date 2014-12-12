using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FlyAwayAIrlines.Ui.Models;

namespace FlyAwayAIrlines.Ui.Controllers
{
    public class FlightPlanService : IFlightPlanService
    {
        public async Task<bool> BookFlight(FlightPlan flightPlan)
        {
            // Make Service Call
            var returnValue = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://flyawayairlinewebapi.azurewebsites.net/");
                //client.BaseAddress = new Uri(" http://localhost:41623/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsJsonAsync("api/flightplans", flightPlan);

                if (response.IsSuccessStatusCode)
                    returnValue = true;

            }

            return returnValue;
        }
    }
}