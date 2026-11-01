using FacadePattern.Services;
using System.Globalization;

namespace FacadePattern
{
    internal class BankFacade(IKycVerification kycVerificationService,
                              ICheckCreditScore checkCreditScoreService,
                              IAccountService accountService,
                              INotificationService notificationService,
                              ITransactionService transactionService)
    {
        // Store account balances in this class.
        private readonly Dictionary<string, decimal> accountBalances = new();


        public void OpenAccount(string accountName)
        {
            if (!kycVerificationService.VerifyKyc())
            {
                Console.WriteLine("KYC verification failed.");
                return;
            }

            if (!checkCreditScoreService.CheckScore())
            {
                Console.WriteLine("Credit score check failed.");
                return;
            }

            accountService.CreateAccount(accountName);

            // Initialize the balance when an account is created.
            accountBalances[accountName] = 0m;

            notificationService.SendNotification(accountName, "Your account has been created successfully.");

            notificationService.SendEmailNotification(accountName, "Welcome to our Email service!");
        }

        public void CloseAccount(string accountName)
        {
            accountService.CloseAccount(accountName);

            notificationService.SendNotification(accountName, "Your account has been closed.");

            notificationService.SendEmailNotification(accountName, "Your account has been closed.");
        }

        public void Deposit(string accountName, decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Deposit amount must be greater than zero.");
                return;
            }

            if (!accountBalances.ContainsKey(accountName))
            {
                Console.WriteLine("Account does not exist.");
                return;
            }

            // Add the deposited amount to the existing balance.
            accountBalances[accountName] += amount;

            transactionService.DepositTransaction(accountName, amount);

            decimal balance = accountBalances[accountName];
            string amountText = amount.ToString("C", CultureInfo.GetCultureInfo("en-IN"));
            string balanceText = balance.ToString("C", CultureInfo.GetCultureInfo("en-IN"));

            Console.WriteLine($"Deposited: {amount:C}");
            Console.WriteLine($"Current balance: {balance:C}");

            notificationService.SendNotification(accountName, $"You deposited {amount:C}. Balance: {balance:C}");

            notificationService.SendEmailNotification(accountName, $"You deposited {amount:C}. Balance: {balance:C}");
        }

        public void Withdraw(string accountName, decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Withdrawal amount must be greater than zero.");
                return;
            }

            if (!accountBalances.ContainsKey(accountName))
            {
                Console.WriteLine("Account does not exist.");
                return;
            }

            // Check the balance before withdrawing.
            if (accountBalances[accountName] < amount)
            {
                Console.WriteLine("Insufficient balance. Withdrawal failed.");
                return;
            }

            // Deduct the withdrawn amount.
            accountBalances[accountName] -= amount;

            transactionService.WithDrawlTransaction(accountName, amount);

            decimal balance = accountBalances[accountName];
            string amountText = amount.ToString("C", CultureInfo.GetCultureInfo("en-IN"));
            string balanceText = balance.ToString("C", CultureInfo.GetCultureInfo("en-IN"));

            Console.WriteLine($"Withdrawn: {amount:C}");
            Console.WriteLine($"Remaining balance: {balance:C}");

            notificationService.SendNotification(accountName, $"You withdrew {amount:C}. Balance: {balance:C}");
            notificationService.SendEmailNotification(accountName, $"You withdrew {amount:C}. Balance: {balance:C}");
        }

        public void TotalBalance(string accountName)
        {
            if (!accountBalances.ContainsKey(accountName))
            {
                Console.WriteLine("Account does not exist.");
                return;
            }

            decimal balance = accountBalances[accountName];
            string balanceText = balance.ToString("C", CultureInfo.GetCultureInfo("en-IN"));

            Console.WriteLine($"Total balance: {balance:C}");

            notificationService.SendNotification(accountName, $"Your total balance is {balance:C}.");
            notificationService.SendEmailNotification(accountName, $"Your total balance is {balance:C}.");
        }
    }
}
