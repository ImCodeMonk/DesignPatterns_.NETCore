namespace FacadePattern.Services
{
    interface INotificationService
    {
        void SendNotification(string accountName, string message);
        void SendEmailNotification(string accountName, string message);
    }
    internal class NotificationService : INotificationService
    {
        public void SendEmailNotification(string accountName, string message)
        {
            Console.WriteLine($"Email notification sent to account {accountName}: {message}");
        }

        public void SendNotification(string accountName, string message)
        {
            Console.WriteLine($"Notification sent to account {accountName}: {message}");
        }
    }
}
