using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessDomain.Contracts;
using BusinessDomain.Data.EntityModel;

namespace BusinessDomain.Data
{
    public class FlightPlanRepository : IFlightPlanRepository
    {
        private readonly string _connectionString;
        private FlyAwayAirlinesWriteOnly _flyAwayAirlinesWriteOnlyModel;

        public FlightPlanRepository(string connectionString)
        {
            _connectionString = connectionString;
            _flyAwayAirlinesWriteOnlyModel = new FlyAwayAirlinesWriteOnly(_connectionString);
        }

        public void CreateFlightBooking(IFlightPlanViewModel flightPlan)
        {
            var entity = new FlightPlanDatabaseEntity
            {
                FlightPlanId = flightPlan.FlightPlanId,
                AircraftName = flightPlan.AircraftName,
                OriginAirport = flightPlan.OriginAirport,
                DestinationAirport = flightPlan.DestinationAirport,
                DepartureTime = flightPlan.DepartureTime,
                ArrivalTime = flightPlan.ArrivalTime,
                SeatPrice = flightPlan.SeatPrice
            };

            _flyAwayAirlinesWriteOnlyModel.FlightPlanDatabaseEntities.Add(entity);
            _flyAwayAirlinesWriteOnlyModel.SaveChanges();
        }
    }
}
