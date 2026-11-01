using FacadePattern;
using FacadePattern.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-IN");

CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-IN");

var serviceCollection = new ServiceCollection()
    .AddSingleton<IAccountService, AccountService>()
    .AddSingleton<INotificationService, NotificationService>()
    .AddSingleton<IKycVerification, KycVerificationService>()
    .AddSingleton<ICheckCreditScore, CheckCreditScoreService>()
    .AddSingleton<ITransactionService, TransactionService>()
    .AddSingleton<BankFacade>()
    .BuildServiceProvider();

var bankFacade = serviceCollection.GetService<BankFacade>();

bankFacade.OpenAccount("John Doe");
bankFacade.Deposit("John Doe", 10000);
bankFacade.Withdraw("John Doe", 500);
