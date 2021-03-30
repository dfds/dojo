using System;

namespace unit_test_demo_2
{
    public interface ILicenseData { string LicenseKey { get; } }
    public interface IServiceInformation { ILicenseData License { get; } }
    public interface IPostalCodeValidator
    {
        bool IsValid(string postalCode);
        bool IsValid(string postalCode, out bool isValid);
        //string LicenseKey { get; }
        IServiceInformation ServiceInformation { get; }
        ValidationMode ValidationMode { get; set; }
    }
}
