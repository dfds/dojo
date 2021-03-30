using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_test_demo_2
{
    public class LoanApplicationEvaluator
    {
        // define class-level constants and instances
        private readonly IPostalCodeValidator _validator;
        private const int AutoReferralMaxAge = 20;
        private const int HighIncomeThreshold = 100_000;
        private const int LowIncomeThreshold = 20_000;

        public LoanApplicationEvaluator(IPostalCodeValidator validator) { _validator = validator ?? throw new System.ArgumentNullException(nameof(validator)); }

        public LoanApplicationDecision Evaluate(LoanApplication application)
        {
            // A method which will perform logic to identify what action should be performede
            // based on the application being passed into the method

            // auto accept if gross annual income is above the high income threshold
            if (application.GrossAnnualIncome >= HighIncomeThreshold) { return LoanApplicationDecision.AutoAccepted; }

            // adjusted the setup to use a property hierarchy
            if (_validator.ServiceInformation.License.LicenseKey == "EXPIRED") { return LoanApplicationDecision.ReferredToHuman; }

            // set the ValidationMode based on the age of the applicant
            _validator.ValidationMode = application.Age >= 30 ? ValidationMode.Detailed : ValidationMode.Quick;

            // the line below includes a dependency which has to be satisfied using mocking
            var isValidPostalCode = _validator.IsValid(application.PostalCode);

            // if the PostalCode is not valid then return the ReferredToHuman option
            if (!isValidPostalCode) { return LoanApplicationDecision.ReferredToHuman; }

            // refer to a human if applicant age is below the auto referral max age
            if (application.Age <= AutoReferralMaxAge) { return LoanApplicationDecision.ReferredToHuman; }

            // auto decline if the gross annual income is below the low income threshold
            if (application.GrossAnnualIncome <= LowIncomeThreshold) { return LoanApplicationDecision.AutoDeclined; }

            // otherwise refer to a human
            return LoanApplicationDecision.ReferredToHuman;
        }
    }
}
