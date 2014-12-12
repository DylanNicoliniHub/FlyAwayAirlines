using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BusinessDomain.Contracts;
using BusinessDomain.Implementation.Commands;
using Infrastructure;
using Infrastructure.Messaging;

namespace BusinessDomain.Implementation
{
    public class FlightPlanService : IFlightPlanService
    {
        private readonly IFlightPlanRepository _flightPlanRepository;
        private readonly ICommandBus _commandBus;

        public FlightPlanService(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public void BookFlight(FlightPlanViewModel flightPlan)
        {

            // Translate and Send to Bus 
            _commandBus.Send(MapViewModelToCommand(flightPlan));
        }

        public BookFlight MapViewModelToCommand(FlightPlanViewModel flightPlan)
        {
            if (flightPlan == null)
                throw new Exception("Flight Plan Cannot Be Null!!!");

            var bookFlightRequest = new BookFlight
            {
                FlightPlanId = flightPlan.FlightPlanId,
                AircraftName = flightPlan.AircraftName,
                OriginAirport = flightPlan.OriginAirport,
                DestinationAirport = flightPlan.DestinationAirport,
                DepartureTime = flightPlan.DepartureTime,
                ArrivalTime = flightPlan.ArrivalTime,
                SeatPrice = flightPlan.SeatPrice
            };

            return bookFlightRequest;
        }

        public List<FlightPlanViewModel> GetFlightPlans()
        {
            return new List<FlightPlanViewModel>();
        }
    }
}
