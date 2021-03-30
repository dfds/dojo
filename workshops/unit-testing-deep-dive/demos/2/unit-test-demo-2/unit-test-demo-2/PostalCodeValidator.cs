using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unit_test_demo_2
{
    /// <summary>
    /// Contacts a web-based API to validate the provided postal code and retrieve address information
    /// 
    /// We may choose to mock this service because of:
    ///  - Cost.  The web-based API might charge per use so mocking would allow us to test without incurring costs
    ///  - Effort.  The effort (and/or complexity) to use the real service makes test painful to write
    ///  - Unreliable.  If the web-based API fails regularly then this could impact on our Unit Tests
    ///  - Speed.  The call could take time to complete which could result in slower unit testing
    /// </summary>
    public class PostalCodeValidatorService : IPostalCodeValidator
    {
        public bool IsValid(string postalCode) { throw new NotImplementedException("Simulate that this dependency is hard to use."); }
        public bool IsValid(string postalCode, out bool isValid) { throw new NotImplementedException("Simulate that this dependency is hard to use."); }
        public IServiceInformation ServiceInformation => throw new NotImplementedException();

        public ValidationMode ValidationMode
        {
            get => throw new NotImplementedException("For demo purposes");
            set => throw new NotImplementedException("For demo purposes");
        }
    }
}
