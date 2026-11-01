namespace FacadePattern.Services
{
    interface IAccountService
    {
        void CreateAccount(string accountName);
        void CloseAccount(string accountName);
        void UpdateAccount(string accountName);
    }

    internal class AccountService : IAccountService
    {
        public void CloseAccount(string accountName)
        {
            Console.WriteLine($"Account {accountName} has been closed.");
        }

        public void CreateAccount(string accountName)
        {
            Console.WriteLine($"Account {accountName} has been created.");
        }

        public void UpdateAccount(string accountName)
        {
            Console.WriteLine($"Account {accountName} has been updated.");
        }
    }
}
