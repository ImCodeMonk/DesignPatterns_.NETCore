namespace DecoratorDesignPattern
{
    abstract class Decorator(IBeverage beverage) : IBeverage
    {
        public virtual string Description()
        {
            return beverage.Description();
        }
        public virtual double Cost()
        {
            return beverage.Cost();
        }
    }

    class MilkDecorator(IBeverage beverage) : Decorator(beverage)
    {
        public override string Description()
        {
            return base.Description() + " + Milk";
        }
        public override double Cost()
        {
            return base.Cost() + 0.5;
        }
    } 

    class SugarDecorator(IBeverage beverage) : Decorator(beverage)
    {
        public override string Description()
        {
            return base.Description() + " + Sugar";
        }
        public override double Cost()
        {
            return base.Cost() + 0.2;
        }
    }

    class LemonDecorator(IBeverage beverage) : Decorator(beverage)
    {
        public override string Description()
        {
            return base.Description() + " + Lemon";
        }
        public override double Cost()
        {
            return base.Cost() + 0.3;
        }
    }
}
