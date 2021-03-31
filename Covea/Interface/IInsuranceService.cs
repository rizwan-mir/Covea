using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Covea.Models;

namespace Covea.Interface
{
    public interface IInsuranceService
    {
        Details GetDetails(Details details);
    }
}
