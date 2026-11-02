namespace DecoratorDesignPattern
{
    interface IBeverage
    {
        string Description();
        double Cost();

    }

    public class Coffee : IBeverage
    {
        public string Description()
        {
            return "Plain Coffee";
        }
        public double Cost()
        {
            return 2.0;
        }
    }

    public class Tea : IBeverage
    {
        public string Description()
        {
            return "Plain Tea";
        }
        public double Cost()
        {
            return 1.5;
        }
    }
}
