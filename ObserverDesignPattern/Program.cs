using ObserverDesignPattern;

User user1 = new User("Alice");
User user2 = new User("Bob");

School sc1 = new School("Greenwood High");
School sc2 = new School("Sunnydale Elementary");

NewsAgency newsAgency = new NewsAgency();

newsAgency.RegisterObserver(user1);
newsAgency.RegisterObserver(sc1);
newsAgency.NotifyObservers("Breaking News: New policy changes in education sector!");

Console.WriteLine("============================================================================");

newsAgency.RegisterObserver(user2);
newsAgency.RegisterObserver(sc2);
newsAgency.NotifyObservers("Breaking News: New policy changes in education sector!");

Console.WriteLine("============================================================================");

newsAgency.RemoveObserver(user1);
newsAgency.RemoveObserver(sc1);
newsAgency.NotifyObservers("Update: New curriculum introduced in schools!");



