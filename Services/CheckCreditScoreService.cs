namespace FacadePattern.Services
{
    interface ICheckCreditScore
    {
        bool CheckScore();
    }

    internal class CheckCreditScoreService : ICheckCreditScore
    {
        public bool CheckScore()
        {
            Console.WriteLine($"Credit score for account has been checked.");
            return true;
        }
    }
}
