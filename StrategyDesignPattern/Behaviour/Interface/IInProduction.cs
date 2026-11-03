namespace StrategyDesignPattern.Behaviour.Interface
{
    public interface IInProduction
    {
        void InProduct();
    }

    public class InProduction : IInProduction
    {
        public void InProduct()
        {
            Console.WriteLine("IP : Order is InProduction...");
        }
    }

    public class NonProducible : IInProduction
    {
        public void InProduct()
        {
            Console.WriteLine("NIP : Can't be in InProduction as its 3rd-Party Order...");
        }
    }
}
