namespace ObserverDesignPattern
{
    internal class School(string name) : IObserver
    {
        private string _name;

        public void Update(string news)
        {
            Console.WriteLine($"School received news: {news}");
        }
    }
}
