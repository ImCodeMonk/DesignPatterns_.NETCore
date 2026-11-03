using System;
using System.Collections.Generic;
using System.Text;

namespace ObserverDesignPattern
{
    internal class User(string name) : IObserver
    {
        private string _name = name;
        
        public void Update(string news)
        {
            Console.WriteLine($"User {_name} received news: {news}");
        }
    }
}
