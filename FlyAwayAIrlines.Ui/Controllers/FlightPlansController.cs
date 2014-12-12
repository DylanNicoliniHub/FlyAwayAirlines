using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FlyAwayAIrlines.Ui.Models;

namespace FlyAwayAIrlines.Ui.Controllers
{
    public class FlightPlansController : ApiController
    {
        private IFlightPlanService _flightPlanService;

        public FlightPlansController(IFlightPlanService flightPlanService)
        {
            _flightPlanService = flightPlanService;
        }

        public async Task<HttpResponseMessage> Post(FlightPlan flightPlan)
        {
            HttpResponseMessage response;

            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, flightPlan);        

            try
            {
                var result = await _flightPlanService.BookFlight(flightPlan);

                if (!result)
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = "The Flight Is Full."
                    };
                else
                 response = Request.CreateResponse(HttpStatusCode.Created, flightPlan);
            }
            catch (Exception e)
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    ReasonPhrase = e.Message
                };

                throw new HttpResponseException(response);
            }

            return response;
        }
    }
}
