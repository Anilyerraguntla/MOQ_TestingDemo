using CreditCardApplications.BLL.CreditCardApplicationEvaluator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CreditCardApplications.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreditCardApplicationController : ControllerBase
    {
        private readonly ICreditCardApplicationEvaluator creditCardApplicationEvaluator;
        private readonly IFrequentFlyerNumberValidator frequentFlyerNumberValidator;
        public CreditCardApplicationController(ICreditCardApplicationEvaluator creditCardApplicationEvaluator, IFrequentFlyerNumberValidator frequentFlyerNumberValidator)
        {
            this.creditCardApplicationEvaluator = creditCardApplicationEvaluator;
            this.frequentFlyerNumberValidator = frequentFlyerNumberValidator;
        }
        [HttpGet]
        public IActionResult SubmitCreditCardApplication([FromBody] CreditCardApplication application)
        {
            // Database or a service call
            //if(application.FrequentFlyerNumber != string.Empty)
            //{
            //    frequentFlyerNumberValidator.IsValid(application.FrequentFlyerNumber);
            //}
            CreditCardApplicationDecision decision = creditCardApplicationEvaluator.Evaluate(application);
            return new JsonResult(decision.ToString());
        }
        

    }
}
