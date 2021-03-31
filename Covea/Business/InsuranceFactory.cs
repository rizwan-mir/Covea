using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Covea.Interface;

namespace Covea.Business
{
    public class InsuranceFactory
    {
        public static IInsuranceService GetInsuranceObj()
        {
            return new InsuranceService();
        }
    }
}