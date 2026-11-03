using System;
using System.Collections.Generic;
using System.Text;

namespace ObserverDesignPattern
{
    internal class NewsAgency : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }
        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }
        public void NotifyObservers(string news)
        {
            foreach (var observer in _observers)
            {
                observer.Update(news);
            }
        }
    }
}
