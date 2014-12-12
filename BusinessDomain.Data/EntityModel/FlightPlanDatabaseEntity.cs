using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessDomain.Contracts;

namespace BusinessDomain.Data.EntityModel
{
    [Table("FlightPlan")]
    public class FlightPlanDatabaseEntity : IFlightPlanViewModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public int FlightPlanId { get; set; }

        [Required]
        [StringLength(50)]
        public string AircraftName { get; set; }

        [Required]
        [StringLength(50)]
        public string OriginAirport { get; set; }

        [Required]
        [StringLength(50)]
        public string DestinationAirport { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal SeatPrice { get; set; }
    }
}