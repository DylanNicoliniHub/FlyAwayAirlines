namespace BusinessDomain.Data.EntityModel
{
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FlyAwayAirlinesWriteOnly : DbContext
    {
        public FlyAwayAirlinesWriteOnly(string connectionString)
            : base(connectionString)
        {
        }

        public FlyAwayAirlinesWriteOnly()
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlightPlanDatabaseEntity>()
            .Property(e => e.SeatPrice)
            .HasPrecision(18, 4);
        }

        public virtual DbSet<FlightPlanDatabaseEntity> FlightPlanDatabaseEntities { get; set; }
    }
}
