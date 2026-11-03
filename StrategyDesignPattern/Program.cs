
using StrategyDesignPattern;
using StrategyDesignPattern.Behaviour.Interface;

OrderStatus shippableOrders = new ShippableOrders();
shippableOrders.Booked();
shippableOrders.PreProduction();
shippableOrders.InProduction();
shippableOrders.Packed();
shippableOrders.Shipped();
Console.WriteLine();

Console.WriteLine("============== 3rd-Party Order ==================");
//In you remove the below line, the order will be treated as a shippable order.
shippableOrders.SetPreProduction(new NonPreProducible());
//If you remove the below line, the order will not print the non-producible message.
shippableOrders.PreProduction();

Console.WriteLine();
Console.WriteLine("--------------------- Non-Shippable Orders -----------------------------");
Console.WriteLine();

OrderStatus nonshippableOrders = new NonShippableOrders();
nonshippableOrders.Booked();
nonshippableOrders.PreProduction();
nonshippableOrders.InProduction();
nonshippableOrders.Packed();
nonshippableOrders.Shipped();