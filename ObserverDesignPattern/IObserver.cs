using System;
using System.Collections.Generic;
using System.Text;

namespace ObserverDesignPattern
{
    internal interface IObserver
    {
        void Update(string news);
    }
}
