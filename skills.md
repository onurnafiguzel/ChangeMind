# Skills.md

Bu dosya ChangeMind projesinde kullanılacak temel mimarı prensipleri, tasarım desenleri ve C# özelliklerini dokümante eder. Projede solo çalışıldığından, bu kurallar tutarlılık ve kod kalitesi için önemlidir.

## Primary Constructor (C# 12+)

Primary constructor, constructor parametrelerini sınıf tanımında direkt olarak bildirmek için kullanılan C# 12 özelliğidir. Boilerplate kodu azaltır.

### Kullanım

```csharp
// ❌ Geleneksel yaklaşım (verbose)
public class CreateUserCommand
{
    private readonly string _email;
    private readonly string _name;

    public CreateUserCommand(string email, string name)
    {
        _email = email;
        _name = name;
    }
}

// ✅ Primary Constructor (tercih edilen)
public class CreateUserCommand(string email, string name)
{
    // email ve name otomatik olarak private field olarak erişilebilir
}

// ✅ Property'lerle birlikte
public record CreateUserCommand(string Email, string Name);

// ✅ Domain Entity'sinde
public class User(string email, string name)
{
    public string Email { get; } = email;
    public string Name { get; } = name;
}
```

### Kurallar

- Primary constructor'ı **Application layer** (command/query) ve **Domain entities** için kullan
- **API/Infrastructure** katmanlarda gerekli değilse geleneksel constructor tercih et
- Parametreler otomatik olarak scope'lu (private) olur

---

## Domain-Driven Design (DDD)

DDD, iş mantığını kod merkezine alarak, teknik detaylardan ayıran bir yaklaşımdır.

### Temel Konseptler

#### 1. **Entity** (Varlık)

Kimliği olan, durumu değişebilen domain objeleri. Veritabanı identitisinden bağımsız bir domain identity'sine sahip olmalıdır.

```csharp
// Domain/Entities/User.cs
public class User(string email, string name)
{
    public UserId Id { get; private set; } = UserId.CreateNew(); // Domain identity
    public string Email { get; private set; } = email;
    public string Name { get; private set; } = name;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Davranış (behavior) - veri taşıyıcısı değil
    public void UpdateEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
            throw new InvalidOperationException("Email boş olamaz");
        Email = newEmail;
    }

    public void Deactivate()
    {
        // Business logic validations
    }
}
```

#### 2. **Value Object** (Değer Nesnesi)

Kimliği olmayan, immutable nesneler. Equality'si değerlerine göre belirlenir.

```csharp
// Domain/ValueObjects/UserId.cs
public record UserId
{
    public Guid Value { get; init; }

    public static UserId CreateNew() => new() { Value = Guid.NewGuid() };
    public static UserId From(Guid id) => new() { Value = id };
}

// Domain/ValueObjects/Email.cs
public record Email
{
    public string Value { get; init; }

    public Email(string value)
    {
        if (!value.Contains("@"))
            throw new InvalidOperationException("Geçersiz email formatı");
        Value = value;
    }
}
```

#### 3. **Aggregate** (Toplam)

İlişkili Entity'ler ve Value Object'lerin gruplanması. Bir tanesi root'tur (Aggregate Root).

```csharp
// Domain/Aggregates/UserAggregate.cs
public class User(string email, string name) // Aggregate Root
{
    public UserId Id { get; private set; } = UserId.CreateNew();
    public Email Email { get; private set; } = new(email);
    public UserProfile Profile { get; private set; } = new();

    // Aggregate'in dışından sadece root aracılığıyla işlem yapılır
    public void AddPhoneNumber(string phone)
    {
        Profile = Profile.WithPhoneNumber(phone);
    }
}

public record UserProfile // Entity ama aggregate'in parçası
{
    public string? PhoneNumber { get; init; }
    public string? Bio { get; init; }

    public UserProfile WithPhoneNumber(string phone)
        => this with { PhoneNumber = phone };
}
```

#### 4. **Repository**

Aggregate'leri persist etmekten sorumlu. Repository pattern, domain'i infrastructure'dan izole eder.

```csharp
// Domain/Repositories/IUserRepository.cs
public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id);
    Task<User?> GetByEmailAsync(Email email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}

// Infrastructure/Repositories/UserRepository.cs
public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(UserId id)
    {
        // EF Core implementation
    }
}
```

#### 5. **Application Service** (Use Case)

Aggregate'leri orchestrate eden, business flow'u yöneten servisler.

```csharp
// Application/CreateUserCommand.cs
public record CreateUserCommand(string Email, string Name) : ICommand<UserId>;

// Application/CreateUserCommandHandler.cs
public class CreateUserCommandHandler(IUserRepository userRepository)
    : ICommandHandler<CreateUserCommand, UserId>
{
    public async Task<UserId> Handle(CreateUserCommand command)
    {
        var existingUser = await userRepository.GetByEmailAsync(new(command.Email));
        if (existingUser != null)
            throw new UserAlreadyExistsException();

        var user = new User(command.Email, command.Name);
        await userRepository.AddAsync(user);

        return user.Id;
    }
}
```

---

## Rich Model (Zengin Model)

Entity'ler sadece veri taşıyıcısı değil, domain davranışını içermelidir. Anemic (zayıf) model'den kaçın.

### ❌ Anemic Model (Kötü)

```csharp
public class User
{
    public string Email { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}

// Service'te logic (domain'den uzak)
public class UserService
{
    public void DeactivateUser(User user)
    {
        user.IsActive = false;
        // Validations, rules?
    }
}
```

### ✅ Rich Model (İyi)

```csharp
public class User(string email, string name)
{
    public UserId Id { get; private set; } = UserId.CreateNew();
    public Email Email { get; private set; } = new(email);
    public string Name { get; private set; } = name;
    public UserStatus Status { get; private set; } = UserStatus.Active;

    // Domain logic encapsulation
    public void Deactivate(string reason)
    {
        if (Status == UserStatus.Deleted)
            throw new InvalidOperationException("Silinmiş user deactivate edilemez");

        Status = UserStatus.Inactive;
        // Domain events fire edilebilir
        // AddDomainEvent(new UserDeactivatedEvent(Id, reason));
    }

    public void Delete()
    {
        if (Status == UserStatus.Deleted)
            throw new InvalidOperationException("User zaten silinmiş");

        Status = UserStatus.Deleted;
    }
}

public enum UserStatus
{
    Active,
    Inactive,
    Deleted
}
```

---

## SOLID Prensipleri

### 1. Single Responsibility Principle (SRP)

Bir sınıf sadece bir nedenden dolayı değişmeli.

```csharp
// ❌ Kötü: İki sorumluluk
public class UserService
{
    public void CreateUser(string email, string name)
    {
        // Validation
        // Database save
        // Email gönderme
        // Logging
    }
}

// ✅ İyi: Ayrı sorumluluğu olan sınıflar
public class CreateUserCommandHandler(IUserRepository repo)
{
    public async Task Handle(CreateUserCommand cmd)
    {
        var user = new User(cmd.Email, cmd.Name);
        await repo.AddAsync(user);
    }
}

public class SendWelcomeEmailHandler(IEmailService emailService)
{
    public async Task Handle(UserCreatedEvent @event)
    {
        await emailService.SendWelcomeEmailAsync(@event.Email);
    }
}
```

### 2. Open/Closed Principle (OCP)

Extension'a açık, modification'a kapalı.

```csharp
// ✅ Interface abstraksiyonu ile
public interface INotificationService
{
    Task SendAsync(string message);
}

public class EmailNotificationService : INotificationService
{
    public async Task SendAsync(string message)
    {
        // Email gönder
    }
}

public class SMSNotificationService : INotificationService
{
    public async Task SendAsync(string message)
    {
        // SMS gönder
    }
}

// Yeni notification tipi eklerken existing code değişmez
```

### 3. Liskov Substitution Principle (LSP)

Derived class, base class yerine kullanılabilir olmalı.

```csharp
// ✅ Doğru
public interface IRepository<T> where T : IAggregateRoot
{
    Task<T?> GetByIdAsync(object id);
    Task AddAsync(T aggregate);
}

// Tüm implementations aynı contract'ı doğru şekilde implement eder
```

### 4. Interface Segregation Principle (ISP)

Geniş interface'ler yerine spesifik interface'ler tercih et.

```csharp
// ❌ Kötü: Çok geniş interface
public interface IUserService
{
    Task CreateUserAsync(CreateUserCommand cmd);
    Task UpdateUserAsync(UpdateUserCommand cmd);
    Task DeleteUserAsync(Guid id);
    Task<UserDto> GetUserAsync(Guid id);
    Task SendEmailAsync(Email email);
    Task LogActionAsync(string action);
}

// ✅ İyi: Ayrı, focused interface'ler
public interface IUserCommandHandler
{
    Task CreateUserAsync(CreateUserCommand cmd);
    Task UpdateUserAsync(UpdateUserCommand cmd);
}

public interface IUserQueryHandler
{
    Task<UserDto> GetUserAsync(Guid id);
}
```

### 5. Dependency Inversion Principle (DIP)

High-level modules, low-level modules'a bağlı olmamalı. İkisi de abstraction'lara bağlı olmalı.

```csharp
// ❌ Kötü: Direct dependency
public class CreateUserCommandHandler
{
    private readonly SqlUserRepository _repo = new(); // Concrete class
}

// ✅ İyi: Interface dependency
public class CreateUserCommandHandler(IUserRepository repository)
{
    // repository abstraction'a bağlı
}

// Program.cs'de dependency injection
services.AddScoped<IUserRepository, UserRepository>();
```

---

## ChangeMind Projesinde Uygulama

### Dosya Yapısı

```
src/
├── ChangeMind.Domain/
│   ├── Entities/           # Rich models with behavior
│   ├── ValueObjects/       # Immutable value types
│   ├── Aggregates/         # Aggregate roots
│   ├── Repositories/       # Repository interfaces
│   └── Exceptions/         # Domain exceptions
├── ChangeMind.Application/
│   ├── Commands/           # Commands (CQS)
│   ├── Queries/            # Queries (CQS)
│   ├── Handlers/           # Command/Query handlers
│   └── Interfaces/         # Service interfaces
├── ChangeMind.Infrastructure/
│   ├── Repositories/       # Repository implementations
│   ├── Services/           # External service implementations
│   └── Persistence/        # DbContext, migrations
└── ChangeMind.Api/
    ├── Controllers/        # API endpoints
    └── Extensions/         # Service registration
```

### Örnek Implementation

```csharp
// Domain/Entities/Task.cs
public class Task(string title, string description, UserId assignedTo)
{
    public TaskId Id { get; private set; } = TaskId.CreateNew();
    public string Title { get; private set; } = title;
    public string Description { get; private set; } = description;
    public UserId AssignedTo { get; private set; } = assignedTo;
    public TaskStatus Status { get; private set; } = TaskStatus.New;

    public void Assign(UserId userId)
    {
        if (Status == TaskStatus.Completed)
            throw new InvalidOperationException("Tamamlanan task'ı başka kullanıcıya atayamazsınız");
        AssignedTo = userId;
    }

    public void Complete()
    {
        Status = TaskStatus.Completed;
    }
}

// Application/CreateTaskCommand.cs
public record CreateTaskCommand(string Title, string Description, Guid AssignedToId)
    : ICommand<TaskId>;

// Application/Handlers/CreateTaskCommandHandler.cs
public class CreateTaskCommandHandler(ITaskRepository taskRepository, IUserRepository userRepository)
    : ICommandHandler<CreateTaskCommand, TaskId>
{
    public async Task<TaskId> Handle(CreateTaskCommand command)
    {
        var user = await userRepository.GetByIdAsync(UserId.From(command.AssignedToId));
        if (user == null)
            throw new UserNotFoundException();

        var task = new Task(command.Title, command.Description, user.Id);
        await taskRepository.AddAsync(task);

        return task.Id;
    }
}
```

Yeni servisleri postman_collection.json dosyasına ekle.
---

## Kaynaklar & Referanslar

- **DDD**: Eric Evans - "Domain-Driven Design: Tackling Complexity in the Heart of Software"
- **Clean Architecture**: Robert C. Martin - "Clean Architecture"
- **SOLID**: Robert C. Martin - "Clean Code"
- **Primary Constructor**: [Microsoft C# 12 docs](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12)
