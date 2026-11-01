namespace FacadePattern.Services
{
    interface ITransactionService
    {
        void ProcessTransaction(string accountName, decimal amount);
        void RefundTransaction(string accountName, decimal amount);
        void DepositTransaction(string accountName, decimal amount);
        void WithDrawlTransaction(string accountName, decimal amount);
        void TotalBalance(string accountName, decimal amount);
    }
    internal class TransactionService : ITransactionService
    {
        public void DepositTransaction(string accountName, decimal amount)
        {
            Console.WriteLine($"Deposit of amount {amount} for account {accountName} has been processed.");
        }

        public void WithDrawlTransaction(string accountName, decimal amount)
        {
            Console.WriteLine($"WithDrawl of amount {amount} for account {accountName} has been processed.");
        }

        public void ProcessTransaction(string accountName, decimal amount)
        {
            Console.WriteLine($"Transaction of amount {amount} for account {accountName} has been processed.");
        }

        public void RefundTransaction(string accountName, decimal amount)
        {
            Console.WriteLine($"Refund of amount {amount} for account {accountName} has been processed.");
        }

        public void TotalBalance(string accountName, decimal amount)
        {
           Console.WriteLine($"Total balance for account {accountName} is {amount}.");
        }
    }
}
