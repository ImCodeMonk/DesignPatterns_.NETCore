namespace StrategyDesignPattern.Behaviour.Interface
{
    public interface IPreProduction
    {
        void PreProduct();
    }

    public class PreProduction : IPreProduction
    {
        public void PreProduct()
        {
            Console.WriteLine("PP : Order is in pre-production...");
        }
    }

    public class NonPreProducible : IPreProduction
    {
        public void PreProduct()
        {
            Console.WriteLine("NPP : Can't be in PreProdution as its 3rd-Party Order ...");
        }
    }
}
