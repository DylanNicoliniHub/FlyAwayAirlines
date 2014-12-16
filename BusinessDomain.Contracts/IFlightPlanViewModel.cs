using System;

namespace BusinessDomain.Contracts
{
    public interface IFlightPlanViewModel
    {
        int FlightPlanId { get; set; }
        string AircraftName { get; set; }
        string OriginAirport { get; set; }
        string DestinationAirport { get; set; }
        DateTime DepartureTime { get; set; }
        DateTime ArrivalTime { get; set; }
        decimal SeatPrice { get; set; }
    }
}