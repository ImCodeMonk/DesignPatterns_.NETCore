using System;
using System.Collections.Generic;
using System.Text;

namespace ObserverDesignPattern
{
    internal interface ISubject
    {
        void RegisterObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers(string news);
    }
}
