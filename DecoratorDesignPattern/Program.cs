using DecoratorDesignPattern;

IBeverage beverage = new Tea();
Console.WriteLine("========================= TEA ==============================");

Console.WriteLine($"Description: {beverage.Description()} \nCost: {beverage.Cost()}");

Console.WriteLine("=============================================================");

beverage = new MilkDecorator(beverage);
Console.WriteLine($"Description: {beverage.Description()} \nCost: {beverage.Cost()}");

beverage = new SugarDecorator(beverage);
Console.WriteLine($"Description: {beverage.Description()} \nCost: {beverage.Cost()}");

beverage = new LemonDecorator(beverage);
Console.WriteLine($"Description: {beverage.Description()} \nCost: {beverage.Cost()}");