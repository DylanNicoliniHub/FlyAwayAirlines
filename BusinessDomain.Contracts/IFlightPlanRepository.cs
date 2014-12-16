using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Contracts
{
    public interface IFlightPlanRepository
    {
        void CreateFlightBooking(IFlightPlanViewModel flightPlan);
    }
}
