using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Covea.Models;
using Covea.Business;
using Covea.Interface;

namespace Covea.Controllers
{
    public class InsuranceController : ApiController
    {
        // GET: Insurance
        [HttpGet]
        public IHttpActionResult GetInsurance([FromBody] Details details)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
                        
            try
            {
                IInsuranceService insuranceService = InsuranceFactory.GetInsuranceObj();
                details = insuranceService.GetDetails(details);
                return Ok(details);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}