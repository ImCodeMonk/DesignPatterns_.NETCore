using StrategyDesignPattern.Behaviour.Interface;

namespace StrategyDesignPattern
{
    public class NonShippableOrders : OrderStatus
    {
        public NonShippableOrders() : base(new NonPreProducible(), new NonProducible(), new NonPackable())
        {

        }
        public override void Shipped()
        {
            Console.WriteLine("Displaying non-shippable orders...");
        }
    }
}
