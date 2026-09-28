# Fundation.Core

کتابخانه‌ی زیرساختی مشترک برای برنامه‌های .NET که الگوهای رایج Domain-Driven Design، CQRS، پیام‌رسانی، Event Store، کش، serialization، ثبت سرویس‌ها و ابزارهای عمومی را در یک لایه‌ی قابل استفاده‌ی مجدد جمع می‌کند.

این پروژه بخشی از خانواده‌ی `Fundation` است و قراردادهای اصلی خود را از پروژه‌ی `Fundation.Abstractions` دریافت می‌کند. `Fundation.Core` عمدتاً پیاده‌سازی‌های پیش‌فرض و registrationهای Microsoft DI را فراهم می‌کند؛ بنابراین برای استفاده‌ی کامل، قراردادهای abstraction و پیاده‌سازی‌های اختصاصی برنامه باید در کنار آن قرار بگیرند.

## فهرست مطالب

- [ویژگی‌ها](#ویژگیها)
- [الزامات](#الزامات)
- [نصب و راه‌اندازی](#نصب-و-راهاندازی)
- [معماری و ساختار پروژه](#معماری-و-ساختار-پروژه)
- [ثبت سرویس‌ها](#ثبت-سرویسها)
- [مدل دامنه](#مدل-دامنه)
- [CQRS](#cqrs)
- [کش درخواست‌ها](#کش-درخواستها)
- [پیام‌رسانی](#پیامرسانی)
- [Event Store و Event Sourcing](#event-store-و-event-sourcing)
- [Persistence با EF Core](#persistence-با-ef-core)
- [Serialization و Configuration](#serialization-و-configuration)
- [ابزارها و extensionها](#ابزارها-و-extensionها)
- [چرخه‌ی توسعه](#چرخهی-توسعه)
- [نکات طراحی و خطاهای رایج](#نکات-طراحی-و-خطاهای-رایج)

## ویژگی‌ها

- پشتیبانی از `net10.0` و ثبت وابستگی‌ها با `Microsoft.Extensions.DependencyInjection`.
- مدل‌های پایه‌ی `Entity`، `Aggregate`، `ValueObject`، `Enumeration` و موجودیت‌های قابل audit.
- نگهداری Domain Eventهای منتشرنشده در aggregate و انتشار آن‌ها پس از تغییرات دامنه.
- پردازش command، query و event با MediatR و اسکن assemblyها.
- pipeline behaviorهای کش و invalidation کش.
- broker و persistence in-memory برای توسعه، تست و نمونه‌سازی.
- abstractionهای event store به همراه پیاده‌سازی `InMemoryEventStore`.
- repository، unit of work، transaction behavior و query extensionهای EF Core.
- serializer پیش‌فرض برای پیام‌ها و داده‌های عمومی.
- helperهای configuration، reflection، type mapping، HTTP، header، claims، تاریخ و collectionها.
- generatorهای شناسه برای `Guid` و شناسه‌ی عددی مبتنی بر Snowflake.
- schedulerهای null و in-memory برای جایگزینی آسان با پیاده‌سازی واقعی.

## الزامات

- .NET SDK 10.
- یک برنامه‌ی host مانند ASP.NET Core یا Worker Service.
- پروژه‌ی `Fundation.Abstractions` با reference معتبر.
- برای قابلیت‌های اختیاری، provider مناسب مانند EF Core، broker واقعی، serializer یا persistence اختصاصی.

Target framework و نسخه‌ی packageها در خود `Fundation.Core.csproj` مشخص شده‌اند. مدیریت package به‌صورت مرکزی برای این پروژه استفاده نمی‌شود؛ هنگام اضافه کردن package جدید، نسخه را در همان `PackageReference` ثبت کنید.

## نصب و راه‌اندازی

### Project reference در solution محلی

```xml
<ItemGroup>
  <ProjectReference Include="..\Fundation.Core\Fundation.Core.csproj" />
</ItemGroup>
```

### نصب package

در صورتی که کتابخانه به‌صورت NuGet منتشر شده باشد، package متناظر نسخه‌ی پروژه را نصب کنید:

```bash
dotnet add package Fundation.Core --version <version>
```

برای توسعه‌ی این repository:

```bash
dotnet restore Fundation.Core/Fundation.Core.csproj
dotnet build Fundation.Core/Fundation.Core.csproj
```

### حداقل registration

`AddCore` سرویس‌های پایه مانند serializer، machine information، lock، domain-event store، HTTP context accessor و generator شناسه را ثبت می‌کند. همچنین هسته‌ی messaging را فعال می‌کند.

```csharp
using Fundation.Core.Registrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCore(
	builder.Configuration,
	assembliesToScan: typeof(Program).Assembly);

var app = builder.Build();
app.Run();
```

پارامتر `rootSectionName` برای قرار دادن تنظیمات زیر یک section ریشه استفاده می‌شود:

```csharp
builder.Services.AddCore(
	builder.Configuration,
	rootSectionName: "Infrastructure",
	assembliesToScan: typeof(Program).Assembly);
```

## معماری و ساختار پروژه

| مسیر | مسئولیت |
| --- | --- |
| `Domain` | entity، aggregate، value object، enumeration، event sourcing و domain exception |
| `CQRS` | command، query، event processor، publisher و MediatR extensionها |
| `Caching` | cache key، cache manager و pipeline policyها |
| `Messaging` | message، header، broker، handler، background service و message persistence |
| `Persistence/EfCore` | repository و unit of work برای EF Core |
| `Persistence/EventStore` | aggregate store، stream event، projection و in-memory event store |
| `Registrations` | extensionهای ثبت سرویس در `IServiceCollection` |
| `Serialization` | serializer عمومی و serializer پیام |
| `Extensions` | extensionهای عمومی برای framework و typeها |
| `IdsGenerator` | generatorهای `Guid` و شناسه‌ی عددی |
| `Exception` | guard extensionها و exceptionهای عمومی برنامه |
| `Types` و `Utils` | type mapping، اطلاعات instance، reflection و IP helper |

مرز قراردادها در `Fundation.Abstractions` است. مصرف‌کننده باید برای interfaceها به abstraction project reference داشته باشد و برای implementationهای این README به `Fundation.Core` reference بدهد.

## ثبت سرویس‌ها

### Core

```csharp
services.AddCore(
	configuration,
	rootSectionName: "Infrastructure",
	typeof(OrderCommandHandler).Assembly,
	typeof(OrderQueryHandler).Assembly);
```

در `AddCore`، مقدار `IdGenerator:Type` بررسی می‌شود:

```json
{
  "IdGenerator": {
	"Type": "Guid"
  }
}
```

اگر مقدار `Guid` باشد، `IIdGenerator<Guid>` ثبت می‌شود؛ در غیر این صورت generator عددی `IIdGenerator<long>` ثبت خواهد شد.

### CQRS

```csharp
services.AddCqrs(
	serviceLifetime: ServiceLifetime.Scoped,
	assemblies: typeof(OrderCommandHandler).Assembly);
```

`AddCqrs` موارد زیر را ثبت و discovery می‌کند:

- MediatR handlerها در assemblyهای مشخص‌شده.
- `ICommandProcessor`، `IQueryProcessor` و `IEventProcessor`.
- publisherهای domain event و notification event.
- schedulerهای پیش‌فرض null.
- `IEventMapper`، `IIntegrationEventMapper` و `IIDomainNotificationEventMapper`.
- `IDomainEventsAccessor` پیش‌فرض.

اگر assembly ارسال نشود، assembly فراخواننده برای MediatR استفاده می‌شود. برای جلوگیری از discovery ناخواسته، assemblyها را صریح ارسال کنید.

### Caching

```csharp
services.AddCachingRequestPolicies(
	typeof(GetOrderQuery).Assembly);
```

کلاس‌هایی که `ICachePolicy<,>` یا `IInvalidateCachePolicy<,>` را پیاده‌سازی کنند به‌صورت transient اسکن و ثبت می‌شوند.

### Event Store

برای توسعه و تست:

```csharp
services.AddInMemoryEventStore();
services.AddReadProjections(typeof(OrderProjection).Assembly);
```

برای event store واقعی:

```csharp
services.AddEventStore<SqlEventStore>(ServiceLifetime.Scoped);
```

`SqlEventStore` باید `IEventStore` را پیاده‌سازی کند. `AddEventStore<T>` علاوه بر implementation، `IAggregateStore` و mapping مربوط به `IEventStore` را ثبت می‌کند.

### Messaging in-memory

```csharp
services.AddCore(configuration, assembliesToScan: typeof(Program).Assembly);
services.AddInMemoryMessagePersistence();
services.AddInMemoryCommandScheduler();
services.AddInMemoryBroker(configuration);
```

این registrationها implementationهای null را با نسخه‌های in-memory جایگزین می‌کنند. برای محیط production باید broker، scheduler و message persistence واقعی جایگزین شوند.

## مدل دامنه

### Entity و Aggregate

`Entity<TId>` شناسه و زمان ایجاد را فراهم می‌کند. `Aggregate<TId>` علاوه بر آن، صف Domain Eventهای منتشرنشده و کنترل business rule را دارد.

```csharp
using Fundation.Core.Domain;

public sealed class Order : Aggregate<Guid>
{
	private readonly List<OrderLine> _lines = [];

	private Order() { }

	public Order(Guid id) : base()
	{
		Id = id;
	}

	public IReadOnlyCollection<OrderLine> Lines => _lines;

	public void AddLine(Guid productId, int quantity)
	{
		CheckRule(new PositiveQuantityRule(quantity));

		_lines.Add(new OrderLine(productId, quantity));
		AddDomainEvents(new OrderLineAddedDomainEvent(Id, productId, quantity));
	}
}
```

متد `AddDomainEvents` از بیرون aggregate قابل دسترسی نیست و eventهای تکراری را بر اساس `EventId` وارد صف نمی‌کند. برای خواندن و پاک‌سازی صف از این APIها استفاده می‌شود:

```csharp
if (order.HasUncommittedDomainEvents())
{
	var events = order.GetUncommittedDomainEvents();
	await publisher.PublishAsync(events, cancellationToken);
	order.MarkUncommittedDomainEventAsCommitted();
}
```

روش واقعی انتشار باید با lifecycle واحد کاری برنامه هماهنگ باشد؛ event را قبل از موفقیت transaction نهایی به broker خارجی ارسال نکنید مگر اینکه outbox یا سازوکار تضمین تحویل داشته باشید.

### Value Objectها

Value objectهای موجود شامل `Address`، `Amount`، `BirthDate`، `Currency`، `Email`، `Money` و `PhoneNumber` هستند. این typeها را برای اعتبارسنجی و مدل‌سازی مقدارهای بدون identity استفاده کنید، نه برای entityهایی که lifecycle مستقل دارند.

```csharp
var email = Email.Create("developer@example.com");
var amount = Amount.Create(125.50m);
var money = new Money(amount, Currency.USD);
```

قبل از استفاده در کد production، signature دقیق factoryها و policy اعتبارسنجی value object موردنظر را با نسخه‌ی abstraction پروژه تطبیق دهید.

### Business rule و exception

`Aggregate.CheckRule` در صورت شکستن `IBusinessRule`، `BusinessRuleValidationException` پرتاب می‌کند. برای خطاهای فنی و HTTP نیز exceptionهای اختصاصی مانند `NotFoundException`، `ConflictException`، `BadRequestException` و `ApiException` وجود دارد.

```csharp
public sealed class PositiveQuantityRule(int quantity) : IBusinessRule
{
	public bool IsBroken() => quantity <= 0;
	public string Message => "Quantity must be greater than zero.";
}
```

برای guardهای متداول می‌توان از `GuardExtensions` مانند `NullOrEmpty`، `NullOrWhiteSpace`، `Negative` و `NegativeOrZero` استفاده کرد.

## CQRS

### Command

برای command بدون response:

```csharp
public sealed record CreateOrderCommand(Guid OrderId) : ICommand;

public sealed class CreateOrderHandler : CommandHandler<CreateOrderCommand>
{
	protected override async Task<Unit> HandleCommandAsync(
		CreateOrderCommand command,
		CancellationToken cancellationToken)
	{
		// ایجاد aggregate و ذخیره‌ی آن در اینجا انجام می‌شود.
		await Task.CompletedTask;
		return Unit.Value;
	}
}
```

برای command دارای response:

```csharp
public sealed record CreateOrderCommand(Guid CustomerId) : ICommand<Guid>;

public sealed class CreateOrderHandler
	: CommandHandler<CreateOrderCommand, Guid>
{
	protected override Task<Guid> HandleCommandAsync(
		CreateOrderCommand command,
		CancellationToken cancellationToken = default)
	{
		var orderId = Guid.NewGuid();
		return Task.FromResult(orderId);
	}
}
```

کلاس‌های handler فقط متد `HandleCommandAsync` را پیاده‌سازی می‌کنند؛ adapter عمومی MediatR در `CommandHandler` قرار دارد.

### Query و paging

```csharp
public sealed record GetOrdersQuery(int Page, int PageSize)
	: ListQuery<OrderSummary>;

public sealed class GetOrdersHandler
	: QueryHandler<GetOrdersQuery, ListResultModel<OrderSummary>>
{
	protected override Task<ListResultModel<OrderSummary>> HandleQueryAsync(
		GetOrdersQuery query,
		CancellationToken cancellationToken = default)
	{
		// query روی repository اجرا می‌شود.
		throw new NotImplementedException();
	}
}
```

`PageRequest`، `ListQuery<TResponse>` و `ListResultModel<T>` برای قراردادهای معمول paging فراهم شده‌اند. در queryهای EF Core از `ApplyPagingAsync` در `EfCoreQueryableExtensions` استفاده کنید تا تعداد کل و صفحه‌ی جاری هم‌زمان محاسبه شود.

### Event

eventهای domain، notification و integration مسیرهای جداگانه دارند. handlerها و mapperهای خود را در assemblyهای موردنظر قرار دهید و آن assemblyها را هنگام `AddCqrs` ارسال کنید تا discovery قابل پیش‌بینی باشد.

## کش درخواست‌ها

`CachingBehavior<TRequest, TResponse>` برای خواندن مقدار cache و `InvalidateCachingBehavior<TRequest, TResponse>` برای invalidation در pipeline قرار دارند. سیاست‌های پروژه باید قراردادهای abstraction یعنی `ICachePolicy<,>` و `IInvalidateCachePolicy<,>` را پیاده‌سازی کنند.

```csharp
public sealed class GetOrderCachePolicy
	: ICachePolicy<GetOrderQuery, OrderDto>
{
	public string GetCacheKey(GetOrderQuery request)
		=> $"orders:{request.OrderId}";

	public TimeSpan? GetExpiration(GetOrderQuery request)
		=> TimeSpan.FromMinutes(5);
}
```

جزئیات متدهای policy به نسخه‌ی `Fundation.Abstractions` وابسته است؛ نام cache key باید deterministic باشد و تمام پارامترهای مؤثر در نتیجه را پوشش دهد. برای mutationها invalidation را بر اساس aggregate یا resource انجام دهید، نه صرفاً بر اساس نام command.

`CacheOptions.DefaultCacheTime` زمان پیش‌فرض cache را بر حسب ثانیه نگه می‌دارد و مقدار اولیه‌ی آن `60` است.

## پیام‌رسانی

### Message و header

`Message` و `IntegrationEvent` typeهای پایه‌ی پیام هستند. extensionهای header برای correlation id، message name و message type فراهم شده‌اند:

```csharp
var headers = new Dictionary<string, object?>();
headers.AddCorrelationId(correlationId.ToString());
headers.AddMessageName("OrderCreated");

var currentCorrelationId = headers.GetCorrelationId();
```

`MessageEnvelopeExtensions` نیز برای خواندن شناسه، نام و metadata از envelope استفاده می‌کند.

### Handler و broker

handlerهایی که `IMessageHandler<TMessage>` را پیاده‌سازی کنند توسط `AddCore` در assemblyهای اسکن‌شده پیدا می‌شوند. broker پیش‌فرض `NullBus` است؛ این implementation عمداً عملیات واقعی ارسال را انجام نمی‌دهد. برای اجرای محلی از `AddInMemoryBroker` استفاده کنید و در production آن را با adapter broker سازمان جایگزین کنید.

### Persistence پیام

`MessagePersistenceOptions` از configuration bind می‌شود. وقتی `rootSectionName` خالی باشد، section پیش‌فرض برابر نام type یعنی `MessagePersistenceOptions` است:

```json
{
  "MessagePersistenceOptions": {
	"BatchSize": 100,
	"IntervalInSeconds": 5
  }
}
```

نام propertyها را با نسخه‌ی abstraction خود بررسی کنید. background serviceهای persistence و bus با host برنامه شروع و متوقف می‌شوند؛ در integration testها از implementationهای in-memory یا null استفاده کنید.

## Event Store و Event Sourcing

`IEventStore` با stream id کار می‌کند و APIهای خواندن stream، append با expected revision و aggregate کردن eventها را ارائه می‌دهد. `InMemoryEventStore` برای test و توسعه است و storage آن با process lifecycle از بین می‌رود.

```csharp
var result = await eventStore.AppendEventsAsync(
	streamId: $"order-{orderId}",
	events: events,
	expectedRevision: ExpectedStreamVersion.NoStream,
	cancellationToken);

var history = await eventStore.GetStreamEventsAsync(
	$"order-{orderId}",
	cancellationToken: cancellationToken);
```

برای جلوگیری از overwrite در aggregateهای هم‌زمان، `ExpectedStreamVersion` را مطابق version خوانده‌شده ارسال کنید. خطاهای concurrency را به خطای قابل فهم برای application تبدیل کنید و append را بدون کنترل revision انجام ندهید.

`AddReadProjections` projectionهای implement‌کننده‌ی `IHaveReadProjection` را در assemblyهای مشخص‌شده ثبت می‌کند. projectionها را idempotent طراحی کنید، چون retry پیام یا replay stream ممکن است یک event را بیش از یک بار به مسیر projection برساند.

## Persistence با EF Core

مسیر `Persistence/EfCore` شامل `EfDbContextBase`، `EfRepositoryBase`، repository PostgreSQL، `EfUnitOfWork`، domain-event accessor و transaction behavior است.

الگوی پیشنهادی registration:

```csharp
services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(configuration.GetConnectionString("Default")));

services.AddScoped<EfUnitOfWork<AppDbContext>>();
```

کلاس application باید `EfDbContextBase` و قراردادهای repository مورد استفاده‌ی solution را مطابق abstraction پیاده‌سازی کند. `EfTxBehavior` را فقط برای commandهایی فعال کنید که واقعاً باید در transaction اجرا شوند؛ transaction را روی queryهای read-only تحمیل نکنید.

نکته‌ی سازگاری: پروژه در حال حاضر EF Core `10.0.0` دارد، اما `EFCore.NamingConventions` موجود در project file نسخه‌ی `8.0.3` است و ممکن است warning مربوط به constraint نسخه ایجاد کند. برای production، نسخه‌ی compatible با EF Core 10 را انتخاب و restore/build را دوباره اجرا کنید.

## Serialization و Configuration

### Serializer

`DefaultSerializer`، `DefaultMessageSerializer` و interfaceهای `ISerializer` و `IMessageSerializer` مسیر استاندارد serialization را فراهم می‌کنند. برای payloadهای integration event قرارداد نسخه‌پذیر تعریف کنید و type name داخلی یا namespace را بدون دلیل به قرارداد عمومی تبدیل نکنید.

### Configuration

`ConfigurationExtensions.GetOptions<TModel>` یک model جدید می‌سازد و section را روی آن bind می‌کند:

```csharp
public sealed class ApiOptions
{
	public string BaseUrl { get; set; } = "https://localhost";
	public int TimeoutSeconds { get; set; } = 30;
}

var options = configuration.GetOptions<ApiOptions>("Api");
```

overload بدون نام section از نام type استفاده می‌کند؛ برای `ApiOptions`، section پیش‌فرض `ApiOptions` خواهد بود. اگر validation لازم است، در DI از `AddOptions<T>().Bind(...).ValidateDataAnnotations()` استفاده کنید.

## ابزارها و extensionها

- `ServiceCollectionExtensions`: ثبت generic serviceها با lifetimeهای مختلف، replace کردن registration و scan سرویس‌ها.
- `ServiceProviderExtensions`: resolve سرویس و scopeهای موردنیاز.
- `TypeExtensions` و `ReflectionExtensions`: invoke متدهای generic، پیدا کردن implementationها و بررسی type hierarchy.
- `TypeMapper`: تبدیل type به نام کامل یا نام کوتاه برای metadata و serialization.
- `DateTimeExtensions`: تبدیل و مقایسه‌ی مقدارهای date/time.
- `ClaimsPrincipalExtensions`: خواندن claimهای متداول.
- `HeaderDictionaryExtensions` و `QueryCollectionExtensions`: خواندن امن داده‌های HTTP.
- `HttpClientExtensions`: helperهای ارسال request و خواندن response.
- `PredicateBuilder`: ساخت predicateهای قابل ترکیب برای LINQ.
- `ReflectionUtilities`: ساخت typeهای generic و اسکن assemblyها.
- `IpUtilities`: عملیات مرتبط با IP address.
- `ExclusiveLock`: هماهنگ‌سازی بخش‌هایی که باید به‌صورت انحصاری اجرا شوند.
- `ServiceActivator`: resolve کردن سرویس در موارد legacy؛ در کد جدید ترجیحاً constructor injection استفاده کنید.

## چرخه‌ی توسعه

از ریشه‌ی repository:

```bash
dotnet restore Fundation.Core/Fundation.Core.csproj
dotnet build Fundation.Core/Fundation.Core.csproj --no-restore
```

برای بررسی سریع package referenceها:

```bash
dotnet list Fundation.Core/Fundation.Core.csproj package
```

پیش از merge:

1. کد جدید را در namespace و assembly مناسب قرار دهید.
2. اگر discovery لازم است، assembly آن را در registration صریح کنید.
3. برای integrationهای واقعی، implementation پیش‌فرض null یا in-memory را جایگزین کنید.
4. restore و build را روی SDK 10 اجرا کنید.
5. تست‌های concurrency، cancellation، retry، duplicate message و replay event را اضافه کنید.

## نکات طراحی و خطاهای رایج

### استفاده از implementation پیش‌فرض در production

`NullBus`، `NullScheduler`، `NullCommandScheduler` و `NullMessagePersistenceService` برای نبودن integration واقعی طراحی شده‌اند. فعال بودن آن‌ها به‌معنای انجام شدن عملیات خارجی نیست. registration production باید آن‌ها را replace کند.

### اسکن بیش از حد assembly

وقتی assembly ارسال نشود، برخی registrationها کل assemblyهای موجود در `AppDomain` را scan می‌کنند. این کار در برنامه‌های بزرگ می‌تواند startup را کند و registration ناخواسته ایجاد کند؛ تا حد امکان assemblyهای feature را صریح ارسال کنید.

### Domain Event و transaction

وجود event در صف aggregate به‌معنای commit شدن آن نیست. eventها را همراه unit of work ذخیره و پس از commit منتشر کنید یا از outbox استفاده کنید. `MarkUncommittedDomainEventAsCommitted` را قبل از موفقیت persistence صدا نزنید.

### In-memory فقط برای test

پیاده‌سازی‌های in-memory داده را پایدار نمی‌کنند، concurrency واقعی provider production را شبیه‌سازی نمی‌کنند و جایگزین integration test با broker یا database واقعی نیستند.

### package compatibility

نسخه‌ی مستقیم package باید با dependency graph کل solution هماهنگ باشد. در صورت مشاهده‌ی `NU1605` یا `NU1608`، ابتدا نسخه‌ی مستقیم و نسخه‌ی موردنیاز پروژه‌های مرجع را هم‌تراز کنید؛ suppress کردن warning راه‌حل اول نیست.

## وضعیت پشتیبانی

این README رفتار و APIهای موجود در همین نسخه‌ی source را توضیح می‌دهد. قراردادهای `Fundation.Abstractions` و implementationهای providerها ممکن است مستقل version شوند؛ هنگام ارتقا، ابتدا release note و سپس restore/build و تست‌های integration را بررسی کنید.
