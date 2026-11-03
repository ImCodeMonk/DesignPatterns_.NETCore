namespace StrategyDesignPattern.Behaviour.Interface
{
    public interface IPacked
    {
        void Pack();
    }

    public class Packed : IPacked
    {
        public void Pack()
        {
            Console.WriteLine("P : Packing the order...");
        }
    }

    public class NonPackable : IPacked
    {
        public void Pack()
        {
            Console.WriteLine("P : Nonpackable as its 3rd-Party Order...");
        }
    }


}
