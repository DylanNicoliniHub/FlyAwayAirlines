using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessDomain.Contracts;
using FlyAwayAIrlines.WebApi.Models;

namespace FlyAwayAIrlines.WebApi.Controllers
{
    public class FlightPlansController : ApiController
    {
        private readonly IFlightPlanService _flightPlanService;

        public FlightPlansController(IFlightPlanService flightPlanService)
        {
            _flightPlanService = flightPlanService;
        }

        public HttpResponseMessage Post(FlightPlanViewModel flightPlan)
        {
            HttpResponseMessage response;

            if (!ModelState.IsValid) 
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            
            try
            {
                _flightPlanService.BookFlight(flightPlan);
                response = Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return response;
        }
    }
}
