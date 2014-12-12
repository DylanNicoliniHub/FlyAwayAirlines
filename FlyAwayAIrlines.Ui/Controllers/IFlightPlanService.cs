using System.Threading.Tasks;
using FlyAwayAIrlines.Ui.Models;

namespace FlyAwayAIrlines.Ui.Controllers
{
    public interface IFlightPlanService
    {
        Task<bool> BookFlight(FlightPlan flightPlan);
    }
}