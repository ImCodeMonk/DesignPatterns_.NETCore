using StrategyDesignPattern.Behaviour.Interface;

namespace StrategyDesignPattern
{
    public abstract class OrderStatus
    {
        private IPreProduction _preProduction;
        private IInProduction _inProduction;
        private IPacked _packed;

        protected OrderStatus(IPreProduction preProduction,
                           IInProduction inProduction,
                           IPacked packed)
        {
            _preProduction = preProduction;
            _inProduction = inProduction;
            _packed = packed;
        }

        public void Booked()
        {
            Console.WriteLine("Order has been booked.");
        }

        public void PreProduction()
        {
            _preProduction?.PreProduct();
        }

        public void InProduction()
        {
            _inProduction?.InProduct();
        }

        public void Packed()
        {
            _packed?.Pack();
        }

        public void SetPreProduction(IPreProduction pp)
        {
            _preProduction = pp;
        }

        public void SetInProduction(IInProduction ip)
        {
            _inProduction = ip;
        }

        public void SetPacked(IPacked p)
        {
            _packed = p;
        }

        public abstract void Shipped();
        
    }
}
