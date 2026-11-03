# Design Patterns in .NET Core (C#)

A hands-on collection of classic **Gang of Four (GoF) design patterns** implemented in **C# / .NET Core**. Each folder contains a runnable example, and this README explains the theory, the code, and where the pattern is used in real projects.

## Table of Contents

| # | Pattern | Category | One word | Folder |
|---|---------|----------|----------|--------|
| 1 | [Facade](#1-facade-pattern) | Structural | **Simplify** | `FacadePattern` |
| 2 | [Adapter](#2-adapter-pattern) | Structural | **Convert** | `AdapterPattern` |
| 3 | [Decorator](#3-decorator-pattern) | Structural | **Add** | `DecoratorDesignPattern` |
| 4 | [Command](#4-command-pattern) | Behavioral | **Encapsulate** | `CommandPattern` |
| 5 | [Observer](#5-observer-pattern) | Behavioral | **Notify** | `ObserverDesignPattern` |
| 6 | [Strategy](#6-strategy-pattern) | Behavioral | **Swap** | `StrategyDesignPattern` |

## What are Design Patterns?

Design patterns are **proven, reusable solutions to commonly occurring problems** in software design. They are not finished code you copy and paste. They are templates that describe how to structure classes and objects so the code stays flexible, readable, and easy to change.

**Three categories:**

- **Creational**: how objects are created (Singleton, Factory, Builder)
- **Structural**: how classes and objects are composed (Facade, Adapter, Decorator, Proxy)
- **Behavioral**: how objects communicate and share responsibility (Command, Observer, Strategy)

## Tech Stack and How to Run

- .NET Core / .NET SDK
- C#
- Console application per pattern

```bash
git clone https://github.com/ImCodeMonk/DesignPatterns_.NETCore.git
cd DesignPatterns_.NETCore/<PatternFolder>
dotnet run
```

---

# 1. Facade Pattern

## Theory

The **Facade Pattern** is a *structural* pattern that provides a **simplified, unified interface to a complex subsystem**. The client talks to one class (the facade) instead of coordinating many subsystem classes itself.

**Analogy:** A restaurant waiter. You don't talk to the chef, cashier, and delivery staff separately. The waiter is the facade.

**Use it when:**

- A subsystem has many classes and a complicated workflow
- You want to reduce coupling between client code and subsystem internals
- You want a clean entry point for a layer
- You want to hide third-party library complexity

**Structure:**

- **Subsystem classes** do the real work
- **Facade** knows which subsystem classes to call and in what order
- **Client** uses only the facade

## Example: Online Order Processing

Placing an order needs four steps: check inventory, charge payment, create shipment, send notification. Without a facade, the controller would have to coordinate all four.

```csharp
// Subsystem interfaces
public interface IInventoryService { bool IsInStock(int productId, int quantity); void Reserve(int productId, int quantity); }
public interface IPaymentService   { bool Charge(string customerId, decimal amount); }
public interface IShippingService  { string CreateShipment(string customerId, int productId); }
public interface INotificationService { void Send(string customerId, string message); }

// Facade
public record OrderResult(bool Success, string Message, string? TrackingNumber = null);

public interface IOrderFacade
{
    OrderResult PlaceOrder(string customerId, int productId, int quantity, decimal price);
}

public class OrderFacade : IOrderFacade
{
    private readonly IInventoryService _inventory;
    private readonly IPaymentService _payment;
    private readonly IShippingService _shipping;
    private readonly INotificationService _notification;

    public OrderFacade(IInventoryService inventory, IPaymentService payment,
                       IShippingService shipping, INotificationService notification)
    {
        _inventory = inventory; _payment = payment;
        _shipping = shipping;   _notification = notification;
    }

    public OrderResult PlaceOrder(string customerId, int productId, int quantity, decimal price)
    {
        if (!_inventory.IsInStock(productId, quantity))
            return new OrderResult(false, "Product out of stock.");

        _inventory.Reserve(productId, quantity);

        if (!_payment.Charge(customerId, price * quantity))
            return new OrderResult(false, "Payment failed.");

        var tracking = _shipping.CreateShipment(customerId, productId);
        _notification.Send(customerId, $"Order placed. Tracking: {tracking}");

        return new OrderResult(true, "Order placed successfully.", tracking);
    }
}
```

**Registering and using it (Program.cs and Controller):**

```csharp
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IShippingService, ShippingService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IOrderFacade, OrderFacade>();

[HttpPost]
public IActionResult Place(OrderRequest request)
{
    var result = _orderFacade.PlaceOrder(request.CustomerId, request.ProductId, request.Quantity, request.Price);
    return result.Success ? Ok(result) : BadRequest(result);
}
```

## Code Explanation

1. **Four subsystem services** each have a single job (inventory, payment, shipping, notification).
2. **`OrderFacade`** receives all four through constructor injection and exposes one method, `PlaceOrder`.
3. Inside `PlaceOrder`, the facade runs the workflow in the correct order and stops early if stock or payment fails.
4. The **controller** injects only `IOrderFacade`. It knows nothing about inventory, payment, or shipping, and it makes a single call.
5. If a service changes (for example, a new shipping provider), only the subsystem or the facade changes. The controller is untouched.

## Real-Time Examples

- **E-commerce checkout:** one `CheckoutFacade` over cart, tax, payment, and order services
- **Third-party integrations:** one facade over several vendor APIs (KYC, SMS, email) so controllers call one method
- **Banking app:** `TransferFacade` coordinating account validation, fraud check, debit, credit, and audit logging
- **.NET itself:** `HttpClient`, `DbContext`, `ILogger<T>`, and `WebApplication.CreateBuilder()` all hide complex internals behind a simple API

## Benefits and Drawbacks

| Benefits | Drawbacks |
|----------|-----------|
| Simple API over complex subsystem | Can grow into a "God object" |
| Loose coupling | Extra layer of indirection |
| Easy to mock in tests | Doesn't stop clients from using subsystem classes directly |

## Interview One-Liner

> "Facade provides a simplified, higher-level interface to a subsystem, reducing coupling between clients and its internals."

---

# 2. Adapter Pattern

## Theory

The **Adapter Pattern** is a *structural* pattern that **converts the interface of an existing class into another interface the client expects**. It lets classes work together that otherwise couldn't because of incompatible interfaces.

**Analogy:** A travel plug adapter. Your charger has a two-pin plug, the foreign socket is three-pin. The adapter sits between them without changing either.

**Use it when:**

- You want to use an existing class (third-party or legacy) whose interface doesn't match what your code expects
- You can't or shouldn't modify that class
- You want to swap providers without changing client code
- You want to isolate your application from vendor SDKs

**Structure:**

- **Target**: the interface the client expects
- **Adaptee**: the existing class with an incompatible interface
- **Adapter**: implements Target, wraps the Adaptee, and translates calls
- **Client**: works only with the Target

## Example: Payment Gateway Integration

Your app expects `IPaymentProcessor`, but the Stripe and PayPal SDKs each have a different signature.

```csharp
// Target: what our app expects
public interface IPaymentProcessor
{
    PaymentResult Pay(string customerId, decimal amount, string currency);
}
public record PaymentResult(bool Success, string TransactionId, string? Error = null);

// Adaptee: third-party SDK we cannot change
public class StripeSdk
{
    public StripeResponse MakeCharge(string customerToken, long amountInCents, string currencyCode) { /* ... */ }
}

// Adapter
public class StripePaymentAdapter : IPaymentProcessor
{
    private readonly StripeSdk _stripe;
    public StripePaymentAdapter(StripeSdk stripe) => _stripe = stripe;

    public PaymentResult Pay(string customerId, decimal amount, string currency)
    {
        var cents = (long)Math.Round(amount * 100);              // decimal -> cents
        var response = _stripe.MakeCharge(customerId, cents, currency);

        return response.Status == "succeeded"                     // StripeResponse -> PaymentResult
            ? new PaymentResult(true, response.ChargeId)
            : new PaymentResult(false, string.Empty, response.FailureMessage);
    }
}

// A second adapter for a different provider
public class PayPalPaymentAdapter : IPaymentProcessor
{
    private readonly PayPalSdk _paypal;
    public PayPalPaymentAdapter(PayPalSdk paypal) => _paypal = paypal;

    public PaymentResult Pay(string customerId, decimal amount, string currency)
    {
        var ok = _paypal.SendPayment(customerId, (double)amount, currency, out var reference);
        return ok ? new PaymentResult(true, reference)
                  : new PaymentResult(false, string.Empty, "PayPal payment failed.");
    }
}
```

**Swapping providers with one line in DI:**

```csharp
builder.Services.AddScoped<StripeSdk>();
builder.Services.AddScoped<IPaymentProcessor, StripePaymentAdapter>();
// builder.Services.AddScoped<IPaymentProcessor, PayPalPaymentAdapter>();
```

## Code Explanation

1. `IPaymentProcessor` is **our own interface**. The application owns it, so the vendor adapts to us, not the other way around.
2. `StripeSdk` takes the amount in **cents** as a `long` and returns its own response type. Our app uses `decimal` and `PaymentResult`.
3. `StripePaymentAdapter` implements our interface, holds a `StripeSdk` instance (composition), converts the amount, calls Stripe, and **maps the response** back to `PaymentResult`.
4. `PayPalPaymentAdapter` does the same for a completely different SDK shape.
5. `CheckoutService` depends only on `IPaymentProcessor`. Changing the provider means changing one DI registration.
6. Note the use of `Math.Round` when converting to cents. A plain cast truncates and can lose a cent.

## Real-Time Examples

- **Payment gateways:** Stripe, PayPal, Razorpay behind one `IPaymentProcessor`
- **Messaging:** Twilio, AWS SNS, or any SMS provider behind `ISmsSender`
- **Legacy integration:** wrapping an old SOAP or XML service so new code sees a clean interface
- **Cloud storage:** AWS S3, Azure Blob, and local disk behind `IFileStorage`
- **.NET itself:** `StreamReader` adapts a byte stream to text, and `SqlDataAdapter` adapts a data source to a `DataSet`

## Benefits and Drawbacks

| Benefits | Drawbacks |
|----------|-----------|
| Reuse third-party or legacy code unchanged | Extra classes and indirection |
| Open/Closed Principle: add adapters freely | Too many adapters can clutter the code |
| Isolates vendor SDKs from business logic | Can get complex if interfaces differ a lot |

## Interview One-Liner

> "Adapter is a translator between two incompatible interfaces, so my code depends on my own abstraction instead of a vendor's API."

---

# 3. Decorator Pattern

## Theory

The **Decorator Pattern** is a *structural* pattern that **attaches new behavior to an object dynamically by wrapping it** in another object that implements the same interface. It is a flexible alternative to subclassing.

**Analogy:** Ordering coffee. Start with plain coffee, then add milk, then sugar, then whipped cream. Each addition wraps the previous one, and it is still a coffee.

**Use it when:**

- You want to add responsibilities at runtime without modifying the class
- Subclassing would cause a class explosion (`CachedLoggedRepository`, `LoggedRepository`, ...)
- You want cross-cutting concerns (logging, caching, retry, validation, auditing)
- You want to combine behaviors in different orders

**Structure:**

- **Component**: common interface
- **Concrete Component**: the original object with core behavior
- **Decorator**: same interface, holds a component and delegates to it
- **Concrete Decorators**: add behavior before or after delegating

## Example: Logging and Caching around a Repository

```csharp
// Component
public interface IProductRepository { Product? GetById(int id); }
public record Product(int Id, string Name);

// Concrete component
public class ProductRepository : IProductRepository
{
    public Product? GetById(int id)
    {
        Console.WriteLine($"Fetching product {id} from database...");
        return new Product(id, $"Product {id}");
    }
}

// Decorator 1: Logging
public class LoggingProductRepository : IProductRepository
{
    private readonly IProductRepository _inner;
    private readonly ILogger<LoggingProductRepository> _logger;

    public LoggingProductRepository(IProductRepository inner, ILogger<LoggingProductRepository> logger)
    { _inner = inner; _logger = logger; }

    public Product? GetById(int id)
    {
        _logger.LogInformation("GetById called with {Id}", id);
        var result = _inner.GetById(id);
        _logger.LogInformation("GetById returned {Result}", result);
        return result;
    }
}

// Decorator 2: Caching
public class CachingProductRepository : IProductRepository
{
    private readonly IProductRepository _inner;
    private readonly IMemoryCache _cache;

    public CachingProductRepository(IProductRepository inner, IMemoryCache cache)
    { _inner = inner; _cache = cache; }

    public Product? GetById(int id) =>
        _cache.GetOrCreate($"product:{id}", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return _inner.GetById(id);
        });
}
```

**Wiring the decorators in DI:**

```csharp
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ProductRepository>();

builder.Services.AddScoped<IProductRepository>(sp =>
{
    IProductRepository repo = sp.GetRequiredService<ProductRepository>();
    repo = new CachingProductRepository(repo, sp.GetRequiredService<IMemoryCache>());
    repo = new LoggingProductRepository(repo, sp.GetRequiredService<ILogger<LoggingProductRepository>>());
    return repo;   // Logging -> Caching -> Database
});

// With the Scrutor library this becomes:
// builder.Services.AddScoped<IProductRepository, ProductRepository>();
// builder.Services.Decorate<IProductRepository, CachingProductRepository>();
// builder.Services.Decorate<IProductRepository, LoggingProductRepository>();
```

## Code Explanation

1. All three classes implement `IProductRepository`, so the client cannot tell them apart.
2. `ProductRepository` holds the **core behavior** (talks to the database).
3. `CachingProductRepository` wraps it. On a cache hit it returns immediately, and on a miss it calls the inner repository and stores the result.
4. `LoggingProductRepository` wraps the caching one and logs before and after.
5. A call flows **Logging → Caching → Database**, then the result returns back through the same chain.
6. **Order matters.** With logging outermost, every call is logged, including cache hits. Reverse the order and cache hits would skip the logging.
7. Adding a retry or auditing decorator later requires no change to any existing class.

## Real-Time Examples

- **Repository cross-cutting concerns:** logging, caching, and retry wrapped around data access
- **.NET `Stream` classes:** `GZipStream`, `BufferedStream`, and `CryptoStream` each wrap another `Stream`
- **`HttpClient` with `DelegatingHandler`:** add auth headers, retry, or logging
- **ASP.NET Core middleware:** each middleware wraps the next one in the pipeline
- **MediatR pipeline behaviors:** validation and logging around handlers

## Benefits and Drawbacks

| Benefits | Drawbacks |
|----------|-----------|
| Add behavior without modifying existing code | Many small wrapper classes |
| Each decorator has a single responsibility | Order of decorators matters |
| Combine behaviors at runtime | Deeper stack traces |
| Avoids subclass explosion | Hard to remove one from the middle |

## Interview One-Liner

> "Decorator adds behavior to an object dynamically by wrapping it in another object with the same interface, as an alternative to subclassing."

---

# 4. Command Pattern

## Theory

The **Command Pattern** is a *behavioral* pattern that **encapsulates a request as an object**, letting you parameterize clients with different requests, queue or log them, and support undo/redo. The sender is decoupled from the object that performs the work.

**Analogy:** A restaurant order slip. The customer tells the waiter (invoker), who writes a slip (command) and hands it to the kitchen (receiver). The slip can be queued, logged, or cancelled.

**Use it when:**

- You want to decouple the invoker from the receiver
- You need undo/redo
- You want to queue, schedule, or log operations
- You want transactional behavior (execute a set, roll back on failure)

**Structure:**

- **Command**: interface with `Execute()` (and optionally `Undo()`)
- **Concrete Command**: binds a receiver to an action
- **Receiver**: does the real work
- **Invoker**: triggers the command without knowing what it does
- **Client**: creates commands and gives them to the invoker

## Example: Text Editor with Undo

```csharp
public interface ICommand
{
    void Execute();
    void Undo();
}

// Receiver
public class TextEditor
{
    public string Text { get; private set; } = "";
    public void Append(string value) => Text += value;
    public void RemoveLast(int count) => Text = Text[..^count];
}

// Concrete command
public class AppendTextCommand : ICommand
{
    private readonly TextEditor _editor;
    private readonly string _value;

    public AppendTextCommand(TextEditor editor, string value)
    { _editor = editor; _value = value; }

    public void Execute() => _editor.Append(_value);
    public void Undo()    => _editor.RemoveLast(_value.Length);
}

// Invoker: keeps history for undo
public class CommandInvoker
{
    private readonly Stack<ICommand> _history = new();

    public void Run(ICommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public void UndoLast()
    {
        if (_history.Count > 0) _history.Pop().Undo();
    }
}
```

**Client:**

```csharp
var editor  = new TextEditor();
var invoker = new CommandInvoker();

invoker.Run(new AppendTextCommand(editor, "Hello"));
invoker.Run(new AppendTextCommand(editor, " World"));
Console.WriteLine(editor.Text);   // Hello World

invoker.UndoLast();
Console.WriteLine(editor.Text);   // Hello
```

## Code Explanation

1. `TextEditor` is the **receiver**. It knows how to append and remove text but nothing about commands.
2. `AppendTextCommand` wraps *what to do* (append `_value`) and *how to reverse it* (remove `_value.Length` characters).
3. `CommandInvoker` runs a command and pushes it on a **stack**. It never knows what the command does.
4. `UndoLast` pops the most recent command and calls `Undo()`. Because it's a stack, undo happens in reverse order (last in, first out).
5. The client output shows `Hello World`, then `Hello` after one undo.
6. Adding a new action (delete, replace) means adding a new command class. The invoker stays unchanged.

## Real-Time Examples

- **CQRS with MediatR:** `CreateOrderCommand` is a request object handled by `CreateOrderHandler`

```csharp
public record CreateOrderCommand(string CustomerId, int ProductId, int Quantity) : IRequest<int>;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, int>
{
    public Task<int> Handle(CreateOrderCommand request, CancellationToken ct) => Task.FromResult(1);
}

// Controller: var id = await _mediator.Send(command);
```

- **Text editors and design tools:** undo/redo history
- **Background job queues:** Hangfire jobs and `Channel<T>` work items are commands to run later
- **WPF/MAUI:** `ICommand` and `RelayCommand` bind buttons to actions
- **Bank or workflow systems:** audit log of every executed command

## Benefits and Drawbacks

| Benefits | Drawbacks |
|----------|-----------|
| Decouples invoker from receiver | Many small classes (one per action) |
| Easy undo/redo and history | Overkill for simple operations |
| Commands can be queued, logged, retried | Undo can be tricky for complex state |

## Interview One-Liner

> "Command turns a request into a standalone object, decoupling the sender from the receiver and enabling undo, queuing, and logging."

---

# 5. Observer Pattern

## Theory

The **Observer Pattern** is a *behavioral* pattern that defines a **one-to-many dependency**: when one object (the subject) changes state, all its dependents (observers) are **notified automatically**.

**Analogy:** A YouTube channel. When the creator uploads a video (state change), every subscriber (observer) gets a notification. Subscribers can join or leave at any time, and the channel doesn't need to know who they are.

**Use it when:**

- A change in one object must trigger updates in others, and you don't know how many
- You want loose coupling between the event source and the reactions
- You need a publish/subscribe style notification

**Structure:**

- **Subject**: keeps a list of observers, with `Subscribe`, `Unsubscribe`, `Notify`
- **Observer**: interface with an `Update` method
- **Concrete Observers**: react to the notification in their own way

## Example: Order Status Notifications

When an order's status changes, the customer should get an email and an SMS, and the warehouse dashboard should update.

```csharp
public interface IOrderObserver
{
    void Update(int orderId, string status);
}

// Subject
public class OrderSubject
{
    private readonly List<IOrderObserver> _observers = new();

    public void Subscribe(IOrderObserver observer)   => _observers.Add(observer);
    public void Unsubscribe(IOrderObserver observer) => _observers.Remove(observer);

    public void ChangeStatus(int orderId, string status)
    {
        Console.WriteLine($"Order {orderId} is now {status}");
        foreach (var observer in _observers)
            observer.Update(orderId, status);
    }
}

// Concrete observers
public class EmailObserver : IOrderObserver
{
    public void Update(int orderId, string status)
        => Console.WriteLine($"[Email] Order {orderId}: {status}");
}

public class SmsObserver : IOrderObserver
{
    public void Update(int orderId, string status)
        => Console.WriteLine($"[SMS] Order {orderId}: {status}");
}

public class WarehouseDashboardObserver : IOrderObserver
{
    public void Update(int orderId, string status)
        => Console.WriteLine($"[Dashboard] Order {orderId} updated to {status}");
}
```

**Client:**

```csharp
var order = new OrderSubject();
var sms   = new SmsObserver();

order.Subscribe(new EmailObserver());
order.Subscribe(sms);
order.Subscribe(new WarehouseDashboardObserver());

order.ChangeStatus(101, "Shipped");   // all three react

order.Unsubscribe(sms);
order.ChangeStatus(101, "Delivered"); // only email and dashboard react
```

## Code Explanation

1. `IOrderObserver` defines the contract every observer must follow: one `Update` method.
2. `OrderSubject` stores observers in a list. `Subscribe` and `Unsubscribe` manage the list.
3. When `ChangeStatus` runs, the subject loops through all observers and calls `Update`. The subject never knows what each observer does.
4. `EmailObserver`, `SmsObserver`, and `WarehouseDashboardObserver` each react differently to the same event.
5. In the client, the first status change notifies all three. After `Unsubscribe(sms)`, the next change notifies only the other two.
6. Adding a new reaction (for example, a push notification) means adding a new observer class. `OrderSubject` is not modified.

> **Note:** .NET also has built-in support for this pattern through **events and delegates** and the **`IObservable<T>` / `IObserver<T>`** interfaces.

## Real-Time Examples

- **Order and notification systems:** order status triggers email, SMS, and push
- **Stock tickers:** price changes update many dashboards at once
- **UI data binding:** `INotifyPropertyChanged` in WPF/MAUI/Blazor notifies the UI when a property changes
- **C# `event` keyword:** `button.Click += handler` is the Observer pattern built into the language
- **SignalR:** broadcasting messages to all subscribed clients
- **Domain events:** `OrderPlaced` event handled by several independent handlers

## Benefits and Drawbacks

| Benefits | Drawbacks |
|----------|-----------|
| Loose coupling between subject and observers | Observers are notified in no guaranteed order |
| Add or remove observers at runtime | Memory leaks if observers never unsubscribe |
| Open/Closed Principle | Hard to debug long notification chains |

## Interview One-Liner

> "Observer defines a one-to-many dependency so that when one object changes state, all its subscribers are notified automatically."

---

# 6. Strategy Pattern

## Theory

The **Strategy Pattern** is a *behavioral* pattern that **defines a family of algorithms, puts each in its own class, and makes them interchangeable at runtime**. The client picks which algorithm to use without changing the code that uses it.

**Analogy:** Getting to the airport. You can go by car, bus, or taxi. The goal (reach the airport) is the same, and you pick the strategy based on time and cost.

**Use it when:**

- You have several ways of doing the same task and want to switch between them
- You want to remove large `if/else` or `switch` blocks that select an algorithm
- You want to add new algorithms without modifying existing code

**Structure:**

- **Strategy**: common interface for the algorithm
- **Concrete Strategies**: each implements a different version
- **Context**: holds a strategy and delegates the work to it

## Example: Discount Calculation

Different customers get different discounts. Without Strategy, this becomes an `if/else` chain that grows with every new customer type.

```csharp
// Strategy
public interface IDiscountStrategy
{
    string Name { get; }
    decimal Apply(decimal amount);
}

// Concrete strategies
public class NoDiscount : IDiscountStrategy
{
    public string Name => "None";
    public decimal Apply(decimal amount) => amount;
}

public class FestivalDiscount : IDiscountStrategy
{
    public string Name => "Festival";
    public decimal Apply(decimal amount) => amount * 0.90m;   // 10% off
}

public class PremiumMemberDiscount : IDiscountStrategy
{
    public string Name => "Premium";
    public decimal Apply(decimal amount) => amount * 0.80m;   // 20% off
}

// Context
public class PriceCalculator
{
    private readonly IEnumerable<IDiscountStrategy> _strategies;

    public PriceCalculator(IEnumerable<IDiscountStrategy> strategies)
        => _strategies = strategies;

    public decimal Calculate(decimal amount, string discountName)
    {
        var strategy = _strategies.FirstOrDefault(s => s.Name == discountName)
                       ?? new NoDiscount();
        return strategy.Apply(amount);
    }
}
```

**Registering all strategies in DI:**

```csharp
builder.Services.AddScoped<IDiscountStrategy, NoDiscount>();
builder.Services.AddScoped<IDiscountStrategy, FestivalDiscount>();
builder.Services.AddScoped<IDiscountStrategy, PremiumMemberDiscount>();
builder.Services.AddScoped<PriceCalculator>();
```

**Client:**

```csharp
var total = calculator.Calculate(1000m, "Premium");   // 800
var festival = calculator.Calculate(1000m, "Festival"); // 900
```

## Code Explanation

1. `IDiscountStrategy` defines what every discount must provide: a `Name` and an `Apply` method.
2. Each concrete class (`NoDiscount`, `FestivalDiscount`, `PremiumMemberDiscount`) contains **one algorithm only**.
3. `PriceCalculator` is the **context**. It receives *all* registered strategies through DI as `IEnumerable<IDiscountStrategy>`.
4. At runtime, `Calculate` finds the strategy whose `Name` matches and delegates to it. If none matches, it falls back to `NoDiscount`.
5. `1000` with `Premium` gives `800`, and with `Festival` gives `900`.
6. To add a `BlackFridayDiscount`, create one new class and register it. `PriceCalculator` is not touched, and no `if/else` grows.

## Real-Time Examples

- **Pricing and discounts:** festival, premium, coupon, and bulk-order rules
- **Payment methods:** credit card, UPI, wallet, and net banking, each as a strategy
- **Shipping cost:** standard, express, and international calculators
- **Sorting and filtering:** `IComparer<T>` passed to `List<T>.Sort` is Strategy in the BCL
- **Authentication:** JWT, cookie, and API-key schemes in ASP.NET Core
- **Tax calculation:** per-country tax rules

## Benefits and Drawbacks

| Benefits | Drawbacks |
|----------|-----------|
| Removes large `if/else` and `switch` blocks | More classes |
| Open/Closed Principle: add strategies freely | Client must know which strategy to choose |
| Algorithms are easy to unit test separately | Overkill if there are only two simple cases |
| Switch behavior at runtime | |

## Interview One-Liner

> "Strategy defines a family of interchangeable algorithms, encapsulates each one, and lets the client switch between them at runtime."

---

# Comparison Cheat Sheet

| Pattern | Category | Purpose | One word |
|---------|----------|---------|----------|
| **Facade** | Structural | Simpler interface over a complex subsystem | Simplify |
| **Adapter** | Structural | Make an incompatible interface compatible | Convert |
| **Decorator** | Structural | Same interface, add behavior by wrapping | Add |
| **Command** | Behavioral | Wrap a request as an object (undo, queue, log) | Encapsulate |
| **Observer** | Behavioral | Notify many subscribers when state changes | Notify |
| **Strategy** | Behavioral | Swap interchangeable algorithms at runtime | Swap |

## Commonly Confused Pairs

| Question | Answer |
|----------|--------|
| Adapter vs. Facade | Adapter converts one interface to another. Facade simplifies many classes behind one new interface. |
| Decorator vs. Proxy | Both keep the same interface. Decorator adds behavior, Proxy controls access. |
| Strategy vs. Command | Strategy chooses *how* to do one task. Command wraps *what* to do as an object (with undo, queue, log). |
| Observer vs. Command | Observer broadcasts a change to many listeners. Command packages one request for one receiver. |

# SOLID Principles Used

- **Single Responsibility:** each class has one reason to change (every pattern)
- **Open/Closed:** add new adapters, decorators, commands, observers, strategies without editing existing code
- **Dependency Inversion:** clients depend on interfaces, wired up with .NET Core DI

# Contributing

Suggestions and pull requests for additional patterns (Singleton, Factory, Builder, Repository, Proxy, Mediator) are welcome.

# Author

**ImCodeMonk : SHUBHAM NEHERE**
