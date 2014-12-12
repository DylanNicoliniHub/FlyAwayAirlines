using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Messaging;

namespace BusinessDomain.Implementation.Commands
{    
    [Serializable]
    public class BookFlight : ICommand
    {
        public Guid Id { get; private set; }
        public int FlightPlanId { get; set; }
        public string AircraftName { get; set; }
        public string OriginAirport { get; set; }
        public string DestinationAirport { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal SeatPrice { get; set; }
    }
}
