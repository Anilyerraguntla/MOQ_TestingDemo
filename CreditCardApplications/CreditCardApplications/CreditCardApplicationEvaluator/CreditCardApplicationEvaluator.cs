using CreditCardApplications.BLL.CreditCardApplicationEvaluator;

namespace CreditCardApplications
{
    public class CreditCardApplicationEvaluator : ICreditCardApplicationEvaluator
    {
        private readonly IFrequentFlyerNumberValidator validator;
        private const int AutoReferralMaxAge = 20;
        private const int HighIncomeThreshold = 100_000;
        private const int LowIncomeThreshold = 20_000;

        // comment before adding depndency
        public CreditCardApplicationEvaluator(IFrequentFlyerNumberValidator validator)
        {
            this.validator = validator ?? throw new System.ArgumentNullException(nameof(validator));
        }

        public CreditCardApplicationDecision Evaluate(CreditCardApplication application)
        {
            if (application.GrossAnnualIncome >= HighIncomeThreshold)
            {
                return CreditCardApplicationDecision.AutoAccepted;
            }

            // comment before adding depndency
            var isValidNumber = validator.IsValid(application.FrequentFlyerNumber);

            if (!isValidNumber)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }


            if (application.Age <= AutoReferralMaxAge)
            {
                return CreditCardApplicationDecision.ReferredToHuman;
            }

            if (application.GrossAnnualIncome < LowIncomeThreshold)
            {
                return CreditCardApplicationDecision.AutoDeclined;
            }

            return CreditCardApplicationDecision.ReferredToHuman;
        }       
    }
}
