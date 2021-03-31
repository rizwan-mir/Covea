using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Covea.Models
{
    public class Details
    {
        public int Age { get; set; }
        public double Sum { get; set; }
        public double RiskPremium { get; set; }
        public double RenewalCommision { get; set; }
        public double NetPremium { get; set; }
        public double InitialCommission { get; set; }
        public double GrossPremium { get; set; }
        public string Notes { get; set; }
    }
}