using Covea.Models;
using Covea.Interface;
using Covea.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Covea.Business
{
    public class InsuranceService : IInsuranceService
    {
        IDataHelper _dataHelper;
        public InsuranceService()
        {
            _dataHelper = DataFactory.GetDataHelperObj();
        }

        //Check age range
        private static bool CheckAgeRange(int age)
        {
            if (age > 17 && age < 66)
                return true;
            else
                return false;
        }

        //Get Premium details
        public Details GetDetails(Details details)
        {
            bool blnAgeRange = CheckAgeRange(details.Age);
            double dblRiskRate;
            
            //Check if age is within specified range
            if (!blnAgeRange)
            {
                details.Notes = "Invalid Age. Age Range is 18 - 65";
                return details;
            }

            dblRiskRate = GetRiskRate(details.Age, details.Sum);
            //if risk rate is zeo this means there is no risk value available for this age and sum amount
            if (dblRiskRate == 0)
                details.Notes = "Unable to provide premium";
            else 
            {
                details.RiskPremium = Math.Round(dblRiskRate * (details.Sum / 1000), 2);
                details.RenewalCommision = Math.Round(details.RiskPremium * 0.03, 2);
                details.NetPremium = Math.Round(details.RiskPremium + details.RenewalCommision, 2);
                details.InitialCommission = Math.Round(details.NetPremium * 2.05, 2);
                details.GrossPremium = Math.Round(details.NetPremium + details.InitialCommission, 2);

                //check id gross premium is greater than 2 if not continue to add £5000
                //to the sum amount until greater than 2
                if (details.GrossPremium < 2)
                {
                    do
                    {
                        details.Sum = details.Sum + 5000;
                        dblRiskRate = GetRiskRate(details.Age, details.Sum);
                        details.RiskPremium = Math.Round(dblRiskRate * (details.Sum / 1000), 2);
                        details.RenewalCommision = Math.Round(details.RiskPremium * 0.03, 2);
                        details.NetPremium = Math.Round(details.RiskPremium + details.RenewalCommision, 2);
                        details.InitialCommission = Math.Round(details.NetPremium * 2.05, 2);
                        details.GrossPremium = Math.Round(details.NetPremium + details.InitialCommission, 2);

                    }
                    while (details.GrossPremium < 2);
                }
                details.Notes = "Successfully return a premium";
            }
            return details;

        }

        //Calculate the risk rate
        private double GetRiskRate(int age, double sum)
        {
            double dblRisk = 0.00;
            XElement myxml = _dataHelper.GetRiskRules();

            var riskRates = myxml.Elements("Age").Where(a => int.Parse(a.Attribute("min").Value) <= age && int.Parse(a.Attribute("max").Value) >= age).Descendants().ToList();
            var riskRate = from a in riskRates where a.Attribute("Sum").Value == sum.ToString() select a.Value.ToString();
            
            
            if (riskRate.Count() != 0)
                dblRisk = double.Parse(riskRate.First());
            else
            {
                //if risk rate is between bands calculate min and max values
                var minVals = GetMinRisk(riskRates, sum);
                var maxVals = GetMaxRisk(riskRates, sum);

                if (minVals != null && maxVals != null)
                {
                    var minSum = double.Parse(minVals.Select(a => a.Attribute("Sum").Value).First());
                    var minRiskRate = double.Parse(minVals.First().Value);

                    var maxSum = double.Parse(maxVals.Select(a => a.Attribute("Sum").Value).First());
                    var maxRiskRate = double.Parse(maxVals.First().Value);

                    dblRisk = Math.Round(((sum - minSum) / (maxSum - minSum) * maxRiskRate) + ((maxSum - sum) / (maxSum - minSum) * minRiskRate), 4);
                }

            }

            return dblRisk;
        }

        //Get lower band risk rate
        private static IEnumerable<XElement> GetMinRisk(List<XElement> riskRates, double sum)
        {
            var shorterEnumerable = riskRates.Where(x => double.Parse(x.Attribute("Sum").Value) <= sum);
            var shorterList = shorterEnumerable as IEnumerable<XElement> ?? shorterEnumerable.ToArray();
            var minSum = shorterList.Max(x => double.Parse(x.Attribute("Sum").Value));
            if (shorterList.Count() > 0)
            {
                IEnumerable<XElement> minVals = shorterList.Where(a => a.Attribute("Sum").Value == minSum.ToString());
                if (minVals.Count() != 0)
                    return minVals;
            }

            return null;
        }

        //Get higher band risk rate
        private static IEnumerable<XElement> GetMaxRisk(List<XElement> riskRates, double sum)
        {
            var longerEnumerable = riskRates.Where(x => double.Parse(x.Attribute("Sum").Value) >= sum);
            var longerList = longerEnumerable as IEnumerable<XElement> ?? longerEnumerable.ToArray();
            if (longerList.Count() > 0)
            {
                var minSum = longerList.Min(x => double.Parse(x.Attribute("Sum").Value));
                IEnumerable<XElement> maxVals = longerList.Where(a => a.Attribute("Sum").Value == minSum.ToString());
                if (maxVals.Count() != 0)
                    return maxVals;
            }
            return null;
        }
    }
}