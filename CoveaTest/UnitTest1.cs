using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Covea.Business;
using Covea.Interface;
using Covea.Models;

namespace CoveaTest
{
    [TestClass]
    public class UnitTest1
    {
        //Test normal scenario
        [TestMethod]
        public void GetDetails_Test()
        {
            //Arrange
            IInsuranceService service = InsuranceFactory.GetInsuranceObj();
            int age = 25;
            double sum = 50000;
            Details expected = new Details();
            expected.Age = age;
            expected.Sum = sum;
            expected.RiskPremium = 0.82;
            expected.RenewalCommision = 0.02;
            expected.NetPremium = 0.84;
            expected.InitialCommission = 1.72;
            expected.GrossPremium = 2.56;
            expected.Notes = "Successfully return a premium";
            Details actual = new Details();
            actual.Age = age;
            actual.Sum = sum;

            //Act
            actual = service.GetDetails(actual);

            //Assert
            Assert.AreEqual(expected.RiskPremium, actual.RiskPremium);
            Assert.AreEqual(expected.GrossPremium, actual.GrossPremium);
        }

        //Test for when age range is invalid
        [TestMethod]
        public void GetDetails_InvalidAge()
        {
            //Arrange
            IInsuranceService service = InsuranceFactory.GetInsuranceObj();
            string expected = "Invalid Age. Age Range is 18 - 65";
            Details actual = new Details();
            actual.Age = 16;
            actual.Sum = 25000;

            //Act
            actual = service.GetDetails(actual);

            //Assert
            Assert.AreEqual(expected, actual.Notes);
        }

        //Test for when the risk value does not exist for the age and sum amount range
        [TestMethod]
        public void GetDetails_InvalidSum()
        {
            //Arrange
            IInsuranceService service = InsuranceFactory.GetInsuranceObj();
            string expected = "Unable to provide premium";
            Details actual = new Details();
            actual.Age = 62;
            actual.Sum = 350000;

            //Act
            actual = service.GetDetails(actual);

            //Assert
            Assert.AreEqual(expected, actual.Notes);
        }
    }
}
