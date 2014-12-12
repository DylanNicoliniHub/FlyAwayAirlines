using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessDomain.Contracts;
using BusinessDomain.Implementation.Commands;
using Infrastructure.Messaging;

namespace BusinessDomain.Implementation.Handlers
{
    public class FlightBookingHandler : 
        ICommandHandler<BookFlight>
    {
        private readonly IFlightPlanRepository _flightPlanRepository;

        public FlightBookingHandler(IFlightPlanRepository flightPlanRepository)
        {
            _flightPlanRepository = flightPlanRepository;
        }

        public void Handle(BookFlight command)
        {
            _flightPlanRepository.CreateFlightBooking(MapCommandToViewModelDto(command));

            // Raise the event back to the service bus!
            // FlightBooked!
        }

        private FlightPlanViewModel MapCommandToViewModelDto(BookFlight command)
        {
            var viewModel = new FlightPlanViewModel
            {
                FlightPlanId = command.FlightPlanId,
                AircraftName = command.AircraftName,
                OriginAirport = command.OriginAirport,
                DestinationAirport = command.DestinationAirport,
                DepartureTime = command.DepartureTime,
                ArrivalTime = command.ArrivalTime,
                SeatPrice = command.SeatPrice,
            };

            return viewModel;
        }
    }
}
