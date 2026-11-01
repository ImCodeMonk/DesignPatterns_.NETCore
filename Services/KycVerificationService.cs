namespace FacadePattern.Services
{
    interface IKycVerification
    {
        bool VerifyKyc();
    }
    internal class KycVerificationService : IKycVerification
    {
        public bool VerifyKyc()
        {
           Console.WriteLine($"KYC for account has been verified.");
            return true;
        }
    }
}
