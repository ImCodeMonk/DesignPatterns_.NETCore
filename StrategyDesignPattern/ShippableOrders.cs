using StrategyDesignPattern.Behaviour.Interface;

namespace StrategyDesignPattern
{
    public class ShippableOrders : OrderStatus
    {
        public ShippableOrders(): base(new PreProduction(),new InProduction(),new Packed())
        {

        }
        public override void Shipped()
        {
            Console.WriteLine("Displaying shippable orders...");
        }
    }
}
