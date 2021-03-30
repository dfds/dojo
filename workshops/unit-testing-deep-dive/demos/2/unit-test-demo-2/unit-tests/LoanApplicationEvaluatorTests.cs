using System;
using Xunit;
using unit_test_demo_2;
using Moq;

namespace unit_tests
{
    public class LoanApplicationEvaluatorTests
    {
        [Fact]
        public void CanAcceptHighIncomeApplications()
        {
            // Arrange
            // define a mock for the PostalCodeValidator
            Mock<IPostalCodeValidator> mockPostalCodeValidator = new Mock<IPostalCodeValidator>();

            // define the system under test
            var sut = new LoanApplicationEvaluator(mockPostalCodeValidator.Object);

            // create the loan application we will use to test the evaluator
            // the income is set high so that the Evaluate method should automatically approve the application
            var application = new LoanApplication { GrossAnnualIncome = 100_000 };

            // Act
            // Invoke the Evaluate method of the system under test
            LoanApplicationDecision decision = sut.Evaluate(application);

            // Assert
            // Use the equal assertion to confirm that the Evaluate method returned the result AutoAccepted
            Assert.Equal(LoanApplicationDecision.AutoAccepted, decision);
        }

        [Fact]
        public void CanReferYoungApplications()
        {
            // Arrange
            // define a mock for the PostalCodeValidator
            Mock<IPostalCodeValidator> mockPostalCodeValidator = new Mock<IPostalCodeValidator>();

            // to ensure we reach the age comparison step in the mock we need to make sure the mockPostalCodeValidator
            // always returns true when a string is passed in; this is done to reach the section of the method code
            // we actually want to test

            // this line will cause mocks to automaticaly be created where possible
            // its a scattergun approach though
            mockPostalCodeValidator.DefaultValue = DefaultValue.Mock;

            // return true if any string is passed into the IsValid method
            mockPostalCodeValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            // define the system under test
            var sut = new LoanApplicationEvaluator(mockPostalCodeValidator.Object);

            // note: when we introduce the PostalCodeValidator concept it breaks the code; we could include a reference
            // to the concrete class but doing so would break the concept of a Unit Test
            //var sut = new LoanApplicationEvaluator(new PostalCodeValidator());

            // create the application we will use to test the evaluator
            // the age is deliberately set to 19 which is below the auto-approval threshold
            var application = new LoanApplication { Age = 19 };

            // Act
            // Invoke the Evaluate method of the system under test
            LoanApplicationDecision decision = sut.Evaluate(application);

            // Assert
            // Use the equal assertion to confirm that the Evaluate method returned the result ReferredToHuman
            Assert.Equal(LoanApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void CanDeclineLowIncomeApplications()
        {
            // Arrange
            // define the mock PostalCodeValidator (this will default to loose behaviour)
            Mock<IPostalCodeValidator> mockPostalCodeValidator = new Mock<IPostalCodeValidator>();

            // configure the LicenseKey property of the License so that it always returned 'OK'
            mockPostalCodeValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            // this version uses the IsAny method of the It type to allow any string to be specified
            mockPostalCodeValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            // define the system under test; passing in our mockPostalCodeValidator
            var sut = new LoanApplicationEvaluator(mockPostalCodeValidator.Object);

            // define a new loan application
            var application = new LoanApplication
            {
                GrossAnnualIncome = 5_000,
                Age = 45,
                PostalCode = "dn"
            };

            // Act
            // use the evaluate method on the application and store the decision
            LoanApplicationDecision decision = sut.Evaluate(application);

            // Assert
            // use an asserion to check if the decision matches our expected result of AutoDeclined
            Assert.Equal(LoanApplicationDecision.AutoDeclined, decision);
        }

        [Fact]
        public void ReferInvalidPostalCode()
        {
            // Arrange
            // example of mock definition using strict behaviour; this will cause a test failure if a .Setup is not included
            Mock<IPostalCodeValidator> mockPostalCodeValidator = new Mock<IPostalCodeValidator>();

            // we configure the validator so that it always returns false when any string is passed in; this is so that
            // the appropriate behaviour in the method is tested (having it set true would cause an early exit with the wrong result)
            mockPostalCodeValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            mockPostalCodeValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

            // define the system under test
            var sut = new LoanApplicationEvaluator(mockPostalCodeValidator.Object);

            // create a new loan application instance
            var application = new LoanApplication();

            // Act
            // Check to see what decision we get from the Evaluate method
            LoanApplicationDecision decision = sut.Evaluate(application);

            // Assert
            // Compare the result with the enum value we expect
            Assert.Equal(LoanApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void ReferWhenLicenseKeyExpired()
        {
            // Arrange
            // define a mock for the IPostalCodeValidator interface
            var mockValidator = new Mock<IPostalCodeValidator>();

            // configure the mock so that the LicenseKey property always returns EXPIRED
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("EXPIRED");

            // configure the mock so that providing any string as input will cause it to return true
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            // define the new system under test
            var sut = new LoanApplicationEvaluator(mockValidator.Object);
            
            // create a loan application
            var application = new LoanApplication { Age = 42 };

            // Act
            // evaluate the loan application
            LoanApplicationDecision decision = sut.Evaluate(application);
            
            // Assert
            // use an assertion to make sure that the decision was to refer the application to a human
            Assert.Equal(LoanApplicationDecision.ReferredToHuman, decision);
        }
        string GetLicenseKeyExpiryString()
        {
            // simply return the string literal 'EXPIRED' for testing purposes
            return "EXPIRED";
        }

        [Fact]
        public void UserDetailedLookupForOlderApplications()
        {
            // Arrange
            // define a mock object for the IPostalCodeValidator interface
            var mockValidator = new Mock<IPostalCodeValidator>();

            // use SetupAllProperties method to enable tracking on all properties
            // this should be called before any specific setup calls
            mockValidator.SetupAllProperties();

            // individual setup calls can then be used if specific properties need a particular value
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            // define the system under test
            var sut = new LoanApplicationEvaluator(mockValidator.Object);

            // create a new loan application
            var application = new LoanApplication { Age = 30 };

            // Act
            // invoke the evaluation method on the application
            sut.Evaluate(application);

            // Assert
            // use an assert to ensure that the mock object had it's validationmode set appropriately
            // based on the application parameters
            Assert.Equal(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
        }
    }
}
