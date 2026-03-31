# ?? Propuesta de Modernización - AppWeb EPSEPAR

## ?? Resumen Ejecutivo

Este documento presenta una propuesta detallada para la modernización completa de la aplicación **AppWeb EPSEPAR**, migrando desde la arquitectura actual (ASP.NET Web Forms + Angular 8) hacia una arquitectura moderna basada en **.NET 8 Web API** y **React + TypeScript**.

---

## ?? Comparativa: Estado Actual vs. Propuesta

| Aspecto                       | Estado Actual                | Propuesta                            |
| ----------------------------- | ---------------------------- | ------------------------------------ |
| **Backend Framework**         | ASP.NET Web Forms (.NET 4.0) | .NET 8 Web API                       |
| **Frontend Framework**        | Angular 8.2.14               | React 18 + TypeScript                |
| **ORM**                       | ADO.NET manual con reflexión | Entity Framework Core 8              |
| **Base de Datos**             | MySQL (Stored Procedures)    | MySQL (Code First + Migrations)      |
| **Autenticación**             | Token personalizado AES      | JWT con Identity                     |
| **Inyección de Dependencias** | Manual (new Repository())    | Built-in DI Container                |
| **Arquitectura**              | N-Capas básica               | Clean Architecture                   |
| **Estado Frontend**           | Servicios Angular            | Zustand / Redux Toolkit              |
| **Estilos**                   | Bootstrap 4 + CSS            | Tailwind CSS / Material UI           |
| **Testing**                   | Sin tests                    | xUnit + Jest + React Testing Library |

---

## ??? Arquitectura Propuesta

### Diagrama de Arquitectura de Alto Nivel

```
???????????????????????????????????????????????????????????????????????????????
?                              FRONTEND (React + TypeScript)                   ?
???????????????????????????????????????????????????????????????????????????????
?  ???????????????  ???????????????  ???????????????  ??????????????????????? ?
?  ?   Pages     ?  ? Components  ?  ?   Hooks     ?  ?   State (Zustand)   ? ?
?  ?  (Routes)   ?  ?(Reusables)  ?  ?  (Custom)   ?  ?   Global Store      ? ?
?  ???????????????  ???????????????  ???????????????  ??????????????????????? ?
?                                    ?                                         ?
?                          ?????????????????????                              ?
?                          ?   API Services    ?                              ?
?                          ?   (Axios/Fetch)   ?                              ?
?                          ?????????????????????                              ?
???????????????????????????????????????????????????????????????????????????????
                                     ? HTTP/HTTPS (REST API)
                                     ?
???????????????????????????????????????????????????????????????????????????????
?                           BACKEND (.NET 8 Web API)                           ?
???????????????????????????????????????????????????????????????????????????????
?                                                                              ?
?  ?????????????????????????????????????????????????????????????????????????? ?
?  ?                        PRESENTATION LAYER                              ? ?
?  ?  ????????????????  ????????????????  ????????????????                 ? ?
?  ?  ?  Controllers ?  ?  Middlewares ?  ?   Filters    ?                 ? ?
?  ?  ?  (API)       ?  ?  (Auth,CORS) ?  ?  (Validation)?                 ? ?
?  ?  ????????????????  ????????????????  ????????????????                 ? ?
?  ?????????????????????????????????????????????????????????????????????????? ?
?                                     ?                                        ?
?  ?????????????????????????????????????????????????????????????????????????? ?
?  ?                        APPLICATION LAYER                               ? ?
?  ?  ????????????????  ????????????????  ????????????????                 ? ?
?  ?  ?   Services   ?  ?    DTOs      ?  ?  Validators  ?                 ? ?
?  ?  ?  (Use Cases) ?  ?  (Mappings)  ?  ?(FluentValid) ?                 ? ?
?  ?  ????????????????  ????????????????  ????????????????                 ? ?
?  ?  ????????????????  ????????????????                                   ? ?
?  ?  ?  Interfaces  ?  ?   Mappers    ?                                   ? ?
?  ?  ? (Contracts)  ?  ? (AutoMapper) ?                                   ? ?
?  ?  ????????????????  ????????????????                                   ? ?
?  ?????????????????????????????????????????????????????????????????????????? ?
?                                     ?                                        ?
?  ?????????????????????????????????????????????????????????????????????????? ?
?  ?                          DOMAIN LAYER                                  ? ?
?  ?  ????????????????  ????????????????  ????????????????                 ? ?
?  ?  ?   Entities   ?  ?    Enums     ?  ?  Exceptions  ?                 ? ?
?  ?  ?  (Models)    ?  ?              ?  ?   (Custom)   ?                 ? ?
?  ?  ????????????????  ????????????????  ????????????????                 ? ?
?  ?  ????????????????  ????????????????  ????????????????                 ? ?
?  ?  ? Value Objects?  ?Specifications?  ?  Queries     ?                 ? ?
?  ?  ?              ?  ?              ?  ?              ?                 ? ?
?  ?  ????????????????  ????????????????  ????????????????                 ? ?
?  ?????????????????????????????????????????????????????????????????????????? ?
?                                     ?                                        ?
?  ?????????????????????????????????????????????????????????????????????????? ?
?  ?                      INFRASTRUCTURE LAYER                              ? ?
?  ?  ????????????????  ????????????????  ????????????????                 ? ?
?  ?  ? Repositories ?  ?   DbContext  ?  ?    Email     ?                 ? ?
?  ?  ?(EF Core Impl)?  ?   (MySQL)    ?  ?   Service    ?                 ? ?
?  ?  ????????????????  ????????????????  ????????????????                 ? ?
?  ?  ????????????????  ????????????????  ????????????????                 ? ?
?  ?  ?   Identity   ?  ?    Cache     ?  ?   Logging    ?                 ? ?
?  ?  ?  (JWT Auth)  ?  ?   (Redis)    ?  ?  (Serilog)   ?                 ? ?
?  ?  ????????????????  ????????????????  ????????????????                 ? ?
?  ?????????????????????????????????????????????????????????????????????????? ?
?                                     ?                                        ?
???????????????????????????????????????????????????????????????????????????????
                                      ?
                                      ?
???????????????????????????????????????????????????????????????????????????????
?                              DATABASE LAYER                                  ?
???????????????????????????????????????????????????????????????????????????????
?                              MySQL Server 8.0                                ?
?                         Entity Framework Core 8                              ?
?                    Code First + Migrations + Seeding                         ?
???????????????????????????????????????????????????????????????????????????????
```

---

## ?? Principios SOLID Aplicados

### 1. **S - Single Responsibility Principle (SRP)**

Cada clase tiene una única responsabilidad.

```csharp
// ? ANTES: Una clase hacía todo
public class ClienteRepository
{
    public void AddCliente() { /* lógica de BD */ }
    public void SendEmail() { /* lógica de email */ }
    public void GenerateReport() { /* lógica de reportes */ }
}

// ? DESPUÉS: Responsabilidades separadas
public class ClienteRepository : IClienteRepository
{
    public Task<Cliente> AddAsync(Cliente cliente) { /* solo BD */ }
}

public class EmailService : IEmailService
{
    public Task SendAsync(EmailMessage message) { /* solo email */ }
}

public class ReportService : IReportService
{
    public Task<byte[]> GenerateAsync(ReportRequest request) { /* solo reportes */ }
}
```

### 2. **O - Open/Closed Principle (OCP)**

Abierto para extensión, cerrado para modificación.

```csharp
// Interfaz base
public interface INotificationService
{
    Task SendAsync(string message, string recipient);
}

// Implementaciones extensibles sin modificar código existente
public class EmailNotificationService : INotificationService { }
public class SmsNotificationService : INotificationService { }
public class PushNotificationService : INotificationService { }

// Registro en DI
services.AddScoped<INotificationService, EmailNotificationService>();
```

### 3. **L - Liskov Substitution Principle (LSP)**

Las clases derivadas deben ser sustituibles por sus clases base.

```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class Cliente : BaseEntity
{
    public string Nit { get; set; }
    public string Nombre { get; set; }
    // Puede usarse donde se espere BaseEntity
}
```

### 4. **I - Interface Segregation Principle (ISP)**

Interfaces pequeñas y específicas.

```csharp
// ? ANTES: Interfaz grande
public interface IRepository<T>
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<byte[]> ExportToPdfAsync(); // No todos los repos necesitan esto
    Task SendNotificationAsync();     // No pertenece aquí
}

// ? DESPUÉS: Interfaces segregadas
public interface IReadRepository<T>
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}

public interface IWriteRepository<T>
{
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
    where T : BaseEntity
{
}
```

### 5. **D - Dependency Inversion Principle (DIP)**

Depender de abstracciones, no de implementaciones concretas.

```csharp
// ? El servicio depende de abstracciones (interfaces)
public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ClienteService> _logger;

    // Inyección por constructor
    public ClienteService(
        IClienteRepository repository,
        IMapper mapper,
        ILogger<ClienteService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }
}
```

---

## ?? Principio DRY (Don't Repeat Yourself)

### Repositorio Genérico Base

```csharp
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public virtual async Task<T> AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
    }
}
```

### Servicio Base con Operaciones CRUD

```csharp
public abstract class BaseService<TEntity, TDto, TCreateDto, TUpdateDto>
    : IBaseService<TEntity, TDto, TCreateDto, TUpdateDto>
    where TEntity : BaseEntity
{
    protected readonly IRepository<TEntity> _repository;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IMapper _mapper;

    protected BaseService(
        IRepository<TEntity> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public virtual async Task<TDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? default : _mapper.Map<TDto>(entity);
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TDto>>(entities);
    }

    public virtual async Task<TDto> CreateAsync(TCreateDto createDto)
    {
        var entity = _mapper.Map<TEntity>(createDto);
        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<TDto> UpdateAsync(int id, TUpdateDto updateDto)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");

        _mapper.Map(updateDto, entity);
        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");

        await _repository.DeleteAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}
```

---

## ??? Rich Domain Model (Entities Ricas)

### Filosofía de Diseño

En lugar de usar **Anemic Domain Model** (entidades planas sin lógica), implementamos **Rich Domain Model** donde las entidades:

- ? Encapsulan su estado con setters privados
- ? Contienen validaciones de invariantes
- ? Exponen métodos de dominio que modifican el estado
- ? Garantizan que el objeto siempre esté en un estado válido

### Capas de Validación

```
???????????????????????????????????????????????????????????????????
?                    CAPAS DE VALIDACIÓN                          ?
???????????????????????????????????????????????????????????????????
?                                                                  ?
?  1?? API Layer (FluentValidation)                                ?
?     ??> Validación de DTOs de entrada                           ?
?         • Campos requeridos                                      ?
?         • Formatos (email, teléfono)                            ?
?         • Longitudes máximas                                     ?
?                                                                  ?
?  2?? Application Layer (Services)                                ?
?     ??> Validaciones de negocio                                 ?
?         • Reglas que requieren acceso a BD                      ?
?         • Unicidad (NIT duplicado)                              ?
?         • Permisos de usuario                                   ?
?                                                                  ?
?  3?? Domain Layer (Entities)                                     ?
?     ??> Invariantes del dominio                                 ?
?         • Estado consistente del objeto                         ?
?         • Reglas de negocio puras                               ?
?         • Transiciones de estado válidas                        ?
?                                                                  ?
?  4?? Infrastructure Layer (EF Core)                              ?
?     ??> Restricciones de BD                                     ?
?         • Constraints (unique, foreign keys)                    ?
?         • Tipos de datos                                        ?
?                                                                  ?
???????????????????????????????????????????????????????????????????
```

---

### Entidades Base

```csharp
// Domain/Entities/BaseEntity.cs
public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Domain/Entities/AuditableEntity.cs
public abstract class AuditableEntity : BaseEntity
{
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
    public string? CreatedByIp { get; set; }
    public string? UpdatedByIp { get; set; }
}
```

---

### Excepciones de Dominio

```csharp
// Domain/Exceptions/DomainException.cs
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }

    public DomainException(string message, Exception innerException)
        : base(message, innerException) { }
}

// Domain/Exceptions/BusinessException.cs
public class BusinessException : Exception
{
    public string Code { get; }

    public BusinessException(string message) : base(message)
    {
        Code = "BUSINESS_ERROR";
    }

    public BusinessException(string code, string message) : base(message)
    {
        Code = code;
    }
}

// Domain/Exceptions/NotFoundException.cs
public class NotFoundException : Exception
{
    public string EntityName { get; }
    public object Key { get; }

    public NotFoundException(string entityName, object key)
        : base($"Entidad '{entityName}' con ID ({key}) no fue encontrada.")
    {
        EntityName = entityName;
        Key = key;
    }
}

// Domain/Exceptions/ValidationException.cs
public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException() : base("Se han producido uno o más errores de validación.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IDictionary<string, string[]> errors) : this()
    {
        Errors = errors;
    }

    public ValidationException(string propertyName, string error) : this()
    {
        Errors = new Dictionary<string, string[]>
        {
            { propertyName, new[] { error } }
        };
    }
}
```

---

### Entity Rica: Cliente

```csharp
// Domain/Entities/Cliente.cs
public class Cliente : AuditableEntity
{
    // ???????????????????????????????????????????????????????
    // CONSTRUCTORES
    // ???????????????????????????????????????????????????????

    // Constructor privado para EF Core
    private Cliente()
    {
        _empleados = new List<Empleado>();
        _pagos = new List<PagoCliente>();
    }

    // Constructor público con validaciones
    public Cliente(string nit, string nombre, TipoCliente tipoCliente, int sedeId) : this()
    {
        SetNit(nit);
        SetNombre(nombre);
        TipoCliente = tipoCliente;
        SedeId = sedeId;
        Estado = EstadoCliente.Activo;
    }

    // ???????????????????????????????????????????????????????
    // PROPIEDADES (Setters privados para encapsulación)
    // ???????????????????????????????????????????????????????

    public string Nit { get; private set; } = null!;
    public string Nombre { get; private set; } = null!;
    public TipoCliente TipoCliente { get; private set; }
    public string? Referente { get; private set; }
    public string? Direccion { get; private set; }
    public string? Telefono { get; private set; }
    public string? Email { get; private set; }
    public int SedeId { get; private set; }
    public EstadoCliente Estado { get; private set; }

    // Relaciones con empresas prestadoras
    public int? ArlId { get; private set; }
    public int? EpsId { get; private set; }
    public int? AfpId { get; private set; }
    public int? CajaId { get; private set; }

    // ???????????????????????????????????????????????????????
    // NAVEGACIÓN (Solo lectura)
    // ???????????????????????????????????????????????????????

    public Sede Sede { get; private set; } = null!;
    public Arl? Arl { get; private set; }
    public EmpresaPrestadora? Eps { get; private set; }
    public EmpresaPrestadora? Afp { get; private set; }
    public EmpresaPrestadora? Caja { get; private set; }

    private readonly List<Empleado> _empleados;
    public IReadOnlyCollection<Empleado> Empleados => _empleados.AsReadOnly();

    private readonly List<PagoCliente> _pagos;
    public IReadOnlyCollection<PagoCliente> Pagos => _pagos.AsReadOnly();

    // ???????????????????????????????????????????????????????
    // MÉTODOS DE DOMINIO CON VALIDACIONES
    // ???????????????????????????????????????????????????????

    public void SetNit(string nit)
    {
        if (string.IsNullOrWhiteSpace(nit))
            throw new DomainException("El NIT es requerido");

        nit = nit.Trim();

        if (nit.Length < 6)
            throw new DomainException("El NIT debe tener al menos 6 caracteres");

        if (nit.Length > 20)
            throw new DomainException("El NIT no puede exceder 20 caracteres");

        if (!System.Text.RegularExpressions.Regex.IsMatch(nit, @"^[\d\-]+$"))
            throw new DomainException("El NIT solo puede contener números y guiones");

        Nit = nit;
    }

    public void SetNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre es requerido");

        nombre = nombre.Trim();

        if (nombre.Length < 3)
            throw new DomainException("El nombre debe tener al menos 3 caracteres");

        if (nombre.Length > 200)
            throw new DomainException("El nombre no puede exceder 200 caracteres");

        Nombre = nombre;
    }

    public void SetReferente(string? referente)
    {
        if (referente?.Length > 100)
            throw new DomainException("El referente no puede exceder 100 caracteres");

        Referente = referente?.Trim();
    }

    public void SetDireccion(string? direccion)
    {
        if (direccion?.Length > 300)
            throw new DomainException("La dirección no puede exceder 300 caracteres");

        Direccion = direccion?.Trim();
    }

    public void SetTelefono(string? telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono))
        {
            Telefono = null;
            return;
        }

        telefono = telefono.Trim();

        if (!IsValidPhone(telefono))
            throw new DomainException("El formato del teléfono es inválido. Use solo números, espacios, guiones y paréntesis");

        Telefono = telefono;
    }

    public void SetEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            Email = null;
            return;
        }

        email = email.Trim().ToLowerInvariant();

        if (!IsValidEmail(email))
            throw new DomainException("El formato del email es inválido");

        if (email.Length > 100)
            throw new DomainException("El email no puede exceder 100 caracteres");

        Email = email;
    }

    public void CambiarSede(int sedeId)
    {
        if (sedeId <= 0)
            throw new DomainException("Debe seleccionar una sede válida");

        SedeId = sedeId;
    }

    public void CambiarTipoCliente(TipoCliente tipoCliente)
    {
        if (!Enum.IsDefined(typeof(TipoCliente), tipoCliente))
            throw new DomainException("Tipo de cliente inválido");

        TipoCliente = tipoCliente;
    }

    // ???????????????????????????????????????????????????????
    // GESTIÓN DE EMPRESAS PRESTADORAS
    // ???????????????????????????????????????????????????????

    public void AsignarArl(int? arlId)
    {
        ArlId = arlId;
    }

    public void AsignarEps(int? epsId)
    {
        EpsId = epsId;
    }

    public void AsignarAfp(int? afpId)
    {
        AfpId = afpId;
    }

    public void AsignarCaja(int? cajaId)
    {
        CajaId = cajaId;
    }

    // ???????????????????????????????????????????????????????
    // GESTIÓN DE ESTADO
    // ???????????????????????????????????????????????????????

    public void Activar()
    {
        if (Estado == EstadoCliente.Activo)
            throw new DomainException("El cliente ya se encuentra activo");

        Estado = EstadoCliente.Activo;
    }

    public void Suspender(string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo))
            throw new DomainException("Debe indicar el motivo de suspensión");

        if (Estado == EstadoCliente.Suspendido)
            throw new DomainException("El cliente ya se encuentra suspendido");

        Estado = EstadoCliente.Suspendido;
    }

    public void Inactivar()
    {
        if (Estado == EstadoCliente.Inactivo)
            throw new DomainException("El cliente ya se encuentra inactivo");

        var empleadosActivos = _empleados.Count(e => e.Activo);
        if (empleadosActivos > 0)
            throw new DomainException($"No se puede inactivar el cliente. Tiene {empleadosActivos} empleado(s) activo(s)");

        Estado = EstadoCliente.Inactivo;
    }

    // ???????????????????????????????????????????????????????
    // GESTIÓN DE EMPLEADOS
    // ???????????????????????????????????????????????????????

    public Empleado AgregarEmpleado(
        string documento,
        TipoDocumento tipoDocumento,
        string nombres,
        string apellidos,
        int? arlId = null)
    {
        if (Estado != EstadoCliente.Activo)
            throw new DomainException("No se pueden agregar empleados a un cliente que no está activo");

        if (_empleados.Any(e => e.Documento == documento && e.Activo))
            throw new DomainException($"Ya existe un empleado activo con documento {documento}");

        var empleado = new Empleado(
            clienteId: this.Id,
            documento: documento,
            tipoDocumento: tipoDocumento,
            nombres: nombres,
            apellidos: apellidos,
            arlId: arlId ?? this.ArlId
        );

        _empleados.Add(empleado);

        return empleado;
    }

    public void RemoverEmpleado(int empleadoId)
    {
        var empleado = _empleados.FirstOrDefault(e => e.Id == empleadoId);

        if (empleado == null)
            throw new DomainException($"No se encontró el empleado con ID {empleadoId}");

        if (empleado.TieneBeneficiariosActivos())
            throw new DomainException("No se puede remover un empleado con beneficiarios activos");

        empleado.Desactivar();
    }

    // ???????????????????????????????????????????????????????
    // CONSULTAS DE DOMINIO
    // ???????????????????????????????????????????????????????

    public int CantidadEmpleadosActivos() => _empleados.Count(e => e.Activo);

    public bool EstaActivo() => Estado == EstadoCliente.Activo;

    public bool TieneEmpleados() => _empleados.Any();

    public bool EstaEnMora() => _pagos.Any(p => p.EstaPendiente() && p.EstaVencido());

    // ???????????????????????????????????????????????????????
    // VALIDACIONES PRIVADAS
    // ???????????????????????????????????????????????????????

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        // Permite: números, espacios, guiones, paréntesis, signo +
        return System.Text.RegularExpressions.Regex.IsMatch(
            phone, @"^[\d\s\-\+\(\)]{7,20}$");
    }
}
```

---

### Entity Rica: Empleado

```csharp
// Domain/Entities/Empleado.cs
public class Empleado : AuditableEntity
{
    // ???????????????????????????????????????????????????????
    // CONSTRUCTORES
    // ???????????????????????????????????????????????????????

    private Empleado()
    {
        _beneficiarios = new List<Beneficiario>();
        _planillas = new List<Planilla>();
    }

    public Empleado(
        int clienteId,
        string documento,
        TipoDocumento tipoDocumento,
        string nombres,
        string apellidos,
        int? arlId = null) : this()
    {
        if (clienteId <= 0)
            throw new DomainException("El cliente es requerido");

        ClienteId = clienteId;
        SetDocumento(documento, tipoDocumento);
        SetNombres(nombres);
        SetApellidos(apellidos);
        ArlId = arlId;
        FechaAfiliacion = DateTime.UtcNow;
        Activo = true;
        NivelRiesgo = 1; // Nivel de riesgo por defecto
    }

    // ???????????????????????????????????????????????????????
    // PROPIEDADES
    // ???????????????????????????????????????????????????????

    public int ClienteId { get; private set; }
    public string Documento { get; private set; } = null!;
    public TipoDocumento TipoDocumento { get; private set; }
    public string Nombres { get; private set; } = null!;
    public string Apellidos { get; private set; } = null!;
    public string? Telefono { get; private set; }
    public DateTime FechaAfiliacion { get; private set; }
    public int NivelRiesgo { get; private set; }
    public bool Activo { get; private set; }

    // Empresas prestadoras
    public int? ArlId { get; private set; }
    public int? EpsId { get; private set; }
    public int? AfpId { get; private set; }
    public int? CajaId { get; private set; }

    // ???????????????????????????????????????????????????????
    // NAVEGACIÓN
    // ???????????????????????????????????????????????????????

    public Cliente Cliente { get; private set; } = null!;
    public Arl? Arl { get; private set; }

    private readonly List<Beneficiario> _beneficiarios;
    public IReadOnlyCollection<Beneficiario> Beneficiarios => _beneficiarios.AsReadOnly();

    private readonly List<Planilla> _planillas;
    public IReadOnlyCollection<Planilla> Planillas => _planillas.AsReadOnly();

    // ???????????????????????????????????????????????????????
    // PROPIEDADES CALCULADAS
    // ???????????????????????????????????????????????????????

    public string NombreCompleto => $"{Nombres} {Apellidos}";

    public int AntiguedadEnMeses =>
        ((DateTime.UtcNow.Year - FechaAfiliacion.Year) * 12) +
        (DateTime.UtcNow.Month - FechaAfiliacion.Month);

    // ???????????????????????????????????????????????????????
    // MÉTODOS DE DOMINIO
    // ???????????????????????????????????????????????????????

    public void SetDocumento(string documento, TipoDocumento tipoDocumento)
    {
        if (string.IsNullOrWhiteSpace(documento))
            throw new DomainException("El documento es requerido");

        documento = documento.Trim();

        if (!Enum.IsDefined(typeof(TipoDocumento), tipoDocumento))
            throw new DomainException("Tipo de documento inválido");

        // Validar según tipo de documento
        switch (tipoDocumento)
        {
            case TipoDocumento.CedulaCiudadania:
                if (!System.Text.RegularExpressions.Regex.IsMatch(documento, @"^\d{6,12}$"))
                    throw new DomainException("La cédula debe contener entre 6 y 12 dígitos");
                break;

            case TipoDocumento.TarjetaIdentidad:
                if (!System.Text.RegularExpressions.Regex.IsMatch(documento, @"^\d{10,11}$"))
                    throw new DomainException("La tarjeta de identidad debe contener 10 u 11 dígitos");
                break;

            case TipoDocumento.Pasaporte:
                if (documento.Length < 5 || documento.Length > 20)
                    throw new DomainException("El pasaporte debe tener entre 5 y 20 caracteres");
                break;
        }

        Documento = documento;
        TipoDocumento = tipoDocumento;
    }

    public void SetNombres(string nombres)
    {
        if (string.IsNullOrWhiteSpace(nombres))
            throw new DomainException("Los nombres son requeridos");

        nombres = nombres.Trim();

        if (nombres.Length < 2)
            throw new DomainException("Los nombres deben tener al menos 2 caracteres");

        if (nombres.Length > 100)
            throw new DomainException("Los nombres no pueden exceder 100 caracteres");

        Nombres = nombres;
    }

    public void SetApellidos(string apellidos)
    {
        if (string.IsNullOrWhiteSpace(apellidos))
            throw new DomainException("Los apellidos son requeridos");

        apellidos = apellidos.Trim();

        if (apellidos.Length < 2)
            throw new DomainException("Los apellidos deben tener al menos 2 caracteres");

        if (apellidos.Length > 100)
            throw new DomainException("Los apellidos no pueden exceder 100 caracteres");

        Apellidos = apellidos;
    }

    public void SetTelefono(string? telefono)
    {
        if (!string.IsNullOrEmpty(telefono) && telefono.Length > 20)
            throw new DomainException("El teléfono no puede exceder 20 caracteres");

        Telefono = telefono?.Trim();
    }

    public void SetNivelRiesgo(int nivel)
    {
        if (nivel < 1 || nivel > 5)
            throw new DomainException("El nivel de riesgo debe estar entre 1 y 5");

        NivelRiesgo = nivel;
    }

    public void AsignarArl(int? arlId)
    {
        ArlId = arlId;
    }

    public void AsignarEps(int? epsId)
    {
        EpsId = epsId;
    }

    public void AsignarAfp(int? afpId)
    {
        AfpId = afpId;
    }

    public void AsignarCaja(int? cajaId)
    {
        CajaId = cajaId;
    }

    // ???????????????????????????????????????????????????????
    // GESTIÓN DE ESTADO
    // ???????????????????????????????????????????????????????

    public void Activar()
    {
        if (Activo)
            throw new DomainException("El empleado ya está activo");

        Activo = true;
    }

    public void Desactivar()
    {
        if (!Activo)
            throw new DomainException("El empleado ya está inactivo");

        if (TieneBeneficiariosActivos())
            throw new DomainException("No se puede desactivar un empleado con beneficiarios activos");

        Activo = false;
    }

    // ???????????????????????????????????????????????????????
    // GESTIÓN DE BENEFICIARIOS
    // ???????????????????????????????????????????????????????

    public Beneficiario AgregarBeneficiario(
        string documento,
        TipoDocumento tipoDocumento,
        string nombres,
        string apellidos,
        DateTime fechaNacimiento,
        Parentesco parentesco)
    {
        if (!Activo)
            throw new DomainException("No se pueden agregar beneficiarios a un empleado inactivo");

        // Validar parentescos únicos (solo puede haber un cónyuge, por ejemplo)
        if (parentesco == Parentesco.Conyuge &&
            _beneficiarios.Any(b => b.Parentesco == Parentesco.Conyuge))
        {
            throw new DomainException("El empleado ya tiene un cónyuge registrado");
        }

        // Validar documento único entre beneficiarios
        if (_beneficiarios.Any(b => b.Documento == documento))
            throw new DomainException($"Ya existe un beneficiario con documento {documento}");

        var beneficiario = new Beneficiario(
            empleadoId: this.Id,
            documento: documento,
            tipoDocumento: tipoDocumento,
            nombres: nombres,
            apellidos: apellidos,
            fechaNacimiento: fechaNacimiento,
            parentesco: parentesco
        );

        _beneficiarios.Add(beneficiario);

        return beneficiario;
    }

    // ???????????????????????????????????????????????????????
    // CONSULTAS
    // ???????????????????????????????????????????????????????

    public bool TieneBeneficiariosActivos() => _beneficiarios.Any();

    public int CantidadBeneficiarios() => _beneficiarios.Count;
}
```

---

### Entity Rica: Beneficiario

```csharp
// Domain/Entities/Beneficiario.cs
public class Beneficiario : AuditableEntity
{
    private Beneficiario() { }

    public Beneficiario(
        int empleadoId,
        string documento,
        TipoDocumento tipoDocumento,
        string nombres,
        string apellidos,
        DateTime fechaNacimiento,
        Parentesco parentesco)
    {
        if (empleadoId <= 0)
            throw new DomainException("El empleado es requerido");

        EmpleadoId = empleadoId;
        SetDocumento(documento, tipoDocumento);
        SetNombres(nombres);
        SetApellidos(apellidos);
        SetFechaNacimiento(fechaNacimiento);
        SetParentesco(parentesco);
    }

    // ???????????????????????????????????????????????????????
    // PROPIEDADES
    // ???????????????????????????????????????????????????????

    public int EmpleadoId { get; private set; }
    public string Documento { get; private set; } = null!;
    public TipoDocumento TipoDocumento { get; private set; }
    public string Nombres { get; private set; } = null!;
    public string Apellidos { get; private set; } = null!;
    public DateTime FechaNacimiento { get; private set; }
    public Parentesco Parentesco { get; private set; }
    public string? CertificadoEstudio { get; private set; }

    // Navegación
    public Empleado Empleado { get; private set; } = null!;

    // ???????????????????????????????????????????????????????
    // PROPIEDADES CALCULADAS
    // ???????????????????????????????????????????????????????

    public string NombreCompleto => $"{Nombres} {Apellidos}";

    public int Edad => CalcularEdad(FechaNacimiento);

    public bool EsMenorDeEdad => Edad < 18;

    public bool RequiereCertificadoEstudio =>
        Parentesco == Parentesco.Hijo && Edad >= 18 && Edad <= 25;

    // ???????????????????????????????????????????????????????
    // MÉTODOS DE DOMINIO
    // ???????????????????????????????????????????????????????

    public void SetDocumento(string documento, TipoDocumento tipoDocumento)
    {
        if (string.IsNullOrWhiteSpace(documento))
            throw new DomainException("El documento es requerido");

        Documento = documento.Trim();
        TipoDocumento = tipoDocumento;
    }

    public void SetNombres(string nombres)
    {
        if (string.IsNullOrWhiteSpace(nombres))
            throw new DomainException("Los nombres son requeridos");

        if (nombres.Length > 100)
            throw new DomainException("Los nombres no pueden exceder 100 caracteres");

        Nombres = nombres.Trim();
    }

    public void SetApellidos(string apellidos)
    {
        if (string.IsNullOrWhiteSpace(apellidos))
            throw new DomainException("Los apellidos son requeridos");

        if (apellidos.Length > 100)
            throw new DomainException("Los apellidos no pueden exceder 100 caracteres");

        Apellidos = apellidos.Trim();
    }

    public void SetFechaNacimiento(DateTime fechaNacimiento)
    {
        if (fechaNacimiento > DateTime.UtcNow)
            throw new DomainException("La fecha de nacimiento no puede ser futura");

        if (fechaNacimiento < DateTime.UtcNow.AddYears(-120))
            throw new DomainException("La fecha de nacimiento no es válida");

        FechaNacimiento = fechaNacimiento;
    }

    public void SetParentesco(Parentesco parentesco)
    {
        if (!Enum.IsDefined(typeof(Parentesco), parentesco))
            throw new DomainException("Parentesco inválido");

        Parentesco = parentesco;
    }

    public void AdjuntarCertificadoEstudio(string rutaCertificado)
    {
        if (!RequiereCertificadoEstudio)
            throw new DomainException("Este beneficiario no requiere certificado de estudio");

        if (string.IsNullOrWhiteSpace(rutaCertificado))
            throw new DomainException("La ruta del certificado es requerida");

        CertificadoEstudio = rutaCertificado;
    }

    // ???????????????????????????????????????????????????????
    // MÉTODOS PRIVADOS
    // ???????????????????????????????????????????????????????

    private static int CalcularEdad(DateTime fechaNacimiento)
    {
        var today = DateTime.UtcNow;
        var age = today.Year - fechaNacimiento.Year;

        if (fechaNacimiento.Date > today.AddYears(-age))
            age--;

        return age;
    }
}
```

---

### Enums del Dominio

```csharp
// Domain/Enums/EstadoCliente.cs
public enum EstadoCliente
{
    Activo = 1,
    Inactivo = 2,
    Suspendido = 3
}

// Domain/Enums/TipoCliente.cs
public enum TipoCliente
{
    Natural = 1,
    Juridico = 2
}

// Domain/Enums/TipoDocumento.cs
public enum TipoDocumento
{
    TarjetaIdentidad = 1,
    CedulaCiudadania = 2,
    Pasaporte = 3,
    CedulaExtranjeria = 4
}

// Domain/Enums/Parentesco.cs
public enum Parentesco
{
    Conyuge = 1,
    Hijo = 2,
    Padre = 3,
    Madre = 4,
    Hermano = 5
}

// Domain/Enums/TipoEmpresaPrestadora.cs
public enum TipoEmpresaPrestadora
{
    EPS = 1,
    AFP = 2,
    Caja = 3
}
```

---

### Flujo Completo: DTO ? Service ? Entity

#### 1. DTOs con FluentValidation (API Layer)

```csharp
// Application/DTOs/Clientes/CreateClienteDto.cs
public record CreateClienteDto(
    string Nit,
    string Nombre,
    TipoCliente TipoCliente,
    string? Referente,
    string? Direccion,
    string? Telefono,
    string? Email,
    int SedeId,
    int? ArlId,
    int? EpsId,
    int? AfpId,
    int? CajaId
);

// Application/Validators/CreateClienteValidator.cs
public class CreateClienteValidator : AbstractValidator<CreateClienteDto>
{
    public CreateClienteValidator()
    {
        RuleFor(x => x.Nit)
            .NotEmpty().WithMessage("El NIT es requerido")
            .MinimumLength(6).WithMessage("El NIT debe tener al menos 6 caracteres")
            .MaximumLength(20).WithMessage("El NIT no puede exceder 20 caracteres")
            .Matches(@"^[\d\-]+$").WithMessage("El NIT solo puede contener números y guiones");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres");

        RuleFor(x => x.TipoCliente)
            .IsInEnum().WithMessage("Tipo de cliente inválido");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("El formato del email es inválido")
            .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres");

        RuleFor(x => x.Telefono)
            .Matches(@"^[\d\s\-\+\(\)]{7,20}$").When(x => !string.IsNullOrEmpty(x.Telefono))
            .WithMessage("El formato del teléfono es inválido");

        RuleFor(x => x.SedeId)
            .GreaterThan(0).WithMessage("Debe seleccionar una sede válida");
    }
}
```

#### 2. Service con Validaciones de Negocio (Application Layer)

```csharp
// Application/Services/ClienteService.cs
public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly ISedeRepository _sedeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ClienteService> _logger;

    public ClienteService(
        IClienteRepository clienteRepository,
        ISedeRepository sedeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<ClienteService> logger)
    {
        _clienteRepository = clienteRepository;
        _sedeRepository = sedeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ClienteDto> CreateAsync(CreateClienteDto dto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creando cliente con NIT: {Nit}", dto.Nit);

        // ??????????????????????????????????????????????????
        // VALIDACIONES DE NEGOCIO (requieren acceso a BD)
        // ??????????????????????????????????????????????????

        // Verificar NIT único
        var nitExists = await _clienteRepository.ExistsAsync(c => c.Nit == dto.Nit, cancellationToken);
        if (nitExists)
        {
            _logger.LogWarning("Intento de crear cliente con NIT duplicado: {Nit}", dto.Nit);
            throw new BusinessException("CLIENTE_NIT_DUPLICADO", $"Ya existe un cliente con NIT {dto.Nit}");
        }

        // Verificar que la sede existe
        var sedeExists = await _sedeRepository.ExistsAsync(dto.SedeId, cancellationToken);
        if (!sedeExists)
        {
            throw new NotFoundException("Sede", dto.SedeId);
        }

        // ??????????????????????????????????????????????????
        // CREAR ENTIDAD (validaciones de dominio en Entity)
        // ??????????????????????????????????????????????????

        // El constructor y los métodos Set* validan internamente
        var cliente = new Cliente(
            nit: dto.Nit,
            nombre: dto.Nombre,
            tipoCliente: dto.TipoCliente,
            sedeId: dto.SedeId
        );

        // Setear propiedades opcionales (cada método valida internamente)
        cliente.SetReferente(dto.Referente);
        cliente.SetDireccion(dto.Direccion);
        cliente.SetTelefono(dto.Telefono);
        cliente.SetEmail(dto.Email);

        // Asignar empresas prestadoras
        cliente.AsignarArl(dto.ArlId);
        cliente.AsignarEps(dto.EpsId);
        cliente.AsignarAfp(dto.AfpId);
        cliente.AsignarCaja(dto.CajaId);

        // Persistir
        await _clienteRepository.AddAsync(cliente, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cliente creado exitosamente con ID: {Id}", cliente.Id);

        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<ClienteDto> UpdateAsync(int id, UpdateClienteDto dto, CancellationToken cancellationToken = default)
    {
        var cliente = await _clienteRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Cliente", id);

        // Verificar NIT único si cambió
        if (dto.Nit != null && dto.Nit != cliente.Nit)
        {
            var nitExists = await _clienteRepository.ExistsAsync(
                c => c.Nit == dto.Nit && c.Id != id, cancellationToken);

            if (nitExists)
                throw new BusinessException("CLIENTE_NIT_DUPLICADO", $"Ya existe otro cliente con NIT {dto.Nit}");

            cliente.SetNit(dto.Nit);
        }

        // Actualizar propiedades (cada método valida internamente)
        if (dto.Nombre != null) cliente.SetNombre(dto.Nombre);
        if (dto.TipoCliente.HasValue) cliente.CambiarTipoCliente(dto.TipoCliente.Value);
        if (dto.Referente != null) cliente.SetReferente(dto.Referente);
        if (dto.Direccion != null) cliente.SetDireccion(dto.Direccion);
        if (dto.Telefono != null) cliente.SetTelefono(dto.Telefono);
        if (dto.Email != null) cliente.SetEmail(dto.Email);

        if (dto.SedeId.HasValue && dto.SedeId != cliente.SedeId)
        {
            var sedeExists = await _sedeRepository.ExistsAsync(dto.SedeId.Value, cancellationToken);
            if (!sedeExists)
                throw new NotFoundException("Sede", dto.SedeId.Value);

            cliente.CambiarSede(dto.SedeId.Value);
        }

        await _clienteRepository.UpdateAsync(cliente, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task SuspenderClienteAsync(int id, string motivo, CancellationToken cancellationToken = default)
    {
        var cliente = await _clienteRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Cliente", id);

        // La validación está en el método de dominio
        cliente.Suspender(motivo);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cliente {Id} suspendido. Motivo: {Motivo}", id, motivo);
    }

    public async Task InactivarClienteAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await _clienteRepository.GetByIdWithEmpleadosAsync(id, cancellationToken)
            ?? throw new NotFoundException("Cliente", id);

        // La validación de empleados activos está en el método de dominio
        cliente.Inactivar();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cliente {Id} inactivado", id);
    }
}
```

#### 3. Middleware de Manejo de Excepciones

```csharp
// API/Middlewares/ExceptionHandlingMiddleware.cs
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Error no manejado: {Message}", exception.Message);

        var response = exception switch
        {
            DomainException domainEx => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de validación de dominio",
                Detail = domainEx.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            },

            BusinessException businessEx => new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Error de regla de negocio",
                Detail = businessEx.Message,
                Type = "https://tools.ietf.org/html/rfc4918#section-11.2",
                Extensions = { ["code"] = businessEx.Code }
            },

            NotFoundException notFoundEx => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Recurso no encontrado",
                Detail = notFoundEx.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            },

            ValidationException validationEx => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de validación",
                Detail = "Uno o más errores de validación ocurrieron",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Extensions = { ["errors"] = validationEx.Errors }
            },

            UnauthorizedAccessException => new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "No autorizado",
                Detail = "No tiene autorización para realizar esta acción",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
            },

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error interno del servidor",
                Detail = "Ha ocurrido un error inesperado. Por favor, intente más tarde.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            }
        };

        context.Response.StatusCode = response.Status ?? 500;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(response);
    }
}
```

---

### Tests de Entidades de Dominio

```csharp
// Tests/Epsepar.UnitTests/Domain/ClienteTests.cs
public class ClienteTests
{
    [Fact]
    public void Constructor_ConDatosValidos_CreaClienteActivo()
    {
        // Arrange & Act
        var cliente = new Cliente("123456789", "Test Cliente S.A.", TipoCliente.Juridico, 1);

        // Assert
        Assert.Equal("123456789", cliente.Nit);
        Assert.Equal("Test Cliente S.A.", cliente.Nombre);
        Assert.Equal(TipoCliente.Juridico, cliente.TipoCliente);
        Assert.Equal(1, cliente.SedeId);
        Assert.Equal(EstadoCliente.Activo, cliente.Estado);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetNit_ConValorVacio_LanzaDomainException(string? nit)
    {
        // Arrange
        var cliente = new Cliente("123456789", "Test", TipoCliente.Natural, 1);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => cliente.SetNit(nit!));
        Assert.Equal("El NIT es requerido", exception.Message);
    }

    [Fact]
    public void SetNit_ConMenosDe6Caracteres_LanzaDomainException()
    {
        // Arrange
        var cliente = new Cliente("123456789", "Test", TipoCliente.Natural, 1);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => cliente.SetNit("12345"));
        Assert.Equal("El NIT debe tener al menos 6 caracteres", exception.Message);
    }

    [Fact]
    public void SetEmail_ConFormatoInvalido_LanzaDomainException()
    {
        // Arrange
        var cliente = new Cliente("123456789", "Test", TipoCliente.Natural, 1);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => cliente.SetEmail("email-invalido"));
        Assert.Equal("El formato del email es inválido", exception.Message);
    }

    [Fact]
    public void SetEmail_ConFormatoValido_AsignaEmail()
    {
        // Arrange
        var cliente = new Cliente("123456789", "Test", TipoCliente.Natural, 1);

        // Act
        cliente.SetEmail("test@example.com");

        // Assert
        Assert.Equal("test@example.com", cliente.Email);
    }

    [Fact]
    public void Inactivar_ConEmpleadosActivos_LanzaDomainException()
    {
        // Arrange
        var cliente = new Cliente("123456789", "Test", TipoCliente.Natural, 1);
        // Simular que tiene empleados activos (necesitaría reflection o hacer el campo interno)

        // Este test requiere setup adicional para agregar empleados
    }

    [Fact]
    public void Suspender_SinMotivo_LanzaDomainException()
    {
        // Arrange
        var cliente = new Cliente("123456789", "Test", TipoCliente.Natural, 1);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => cliente.Suspender("");
        Assert.Equal("Debe indicar el motivo de suspensión", exception.Message);
    }

    [Fact]
    public void Suspender_ConMotivo_CambiaEstado()
    {
        // Arrange
        var cliente = new Cliente("123456789", "Test", TipoCliente.Natural, 1);

        // Act
        cliente.Suspender("Falta de pago");

        // Assert
        Assert.Equal(EstadoCliente.Suspendido, cliente.Estado);
    }

    [Fact]
    public void Activar_ClienteYaActivo_LanzaDomainException()
    {
        // Arrange
        var cliente = new Cliente("123456789", "Test", TipoCliente.Natural, 1);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => cliente.Activar());
        Assert.Equal("El cliente ya se encuentra activo", exception.Message);
    }
}
```

---

## ?? Estructura del Proyecto Backend (.NET 8)

```
src/
??? ?? Epsepar.API/                          # Capa de Presentación
?   ??? ?? Controllers/
?   ?   ??? AuthController.cs
?   ?   ??? ClientesController.cs
?   ?   ??? EmpleadosController.cs
?   ?   ??? BeneficiariosController.cs
?   ?   ??? PlanillasController.cs
?   ?   ??? SedesController.cs
?   ?   ??? ArlsController.cs
?   ?   ??? EmpresasPrestadorasController.cs
?   ?   ??? EstadisticasController.cs
?   ??? ?? Middlewares/
?   ?   ??? ExceptionHandlingMiddleware.cs
?   ?   ??? RequestLoggingMiddleware.cs
?   ?   ??? PerformanceMiddleware.cs
?   ??? ?? Filters/
?   ?   ??? ValidationFilter.cs
?   ?   ??? ApiKeyAuthFilter.cs
?   ??? ?? Extensions/
?   ?   ??? ServiceCollectionExtensions.cs
?   ?   ??? ApplicationBuilderExtensions.cs
?   ??? Program.cs
?   ??? appsettings.json
?   ??? appsettings.Development.json
?
??? ?? Epsepar.Application/                  # Capa de Aplicación
?   ??? ?? Common/
?   ?   ??? ?? Behaviors/
?   ?   ?   ??? ValidationBehavior.cs
?   ?   ?   ??? LoggingBehavior.cs
?   ?   ??? ?? Exceptions/
?   ?   ?   ??? NotFoundException.cs
?   ?   ?   ??? ValidationException.cs
?   ?   ?   ??? BadRequestException.cs
?   ?   ??? ?? Interfaces/
?   ?   ?   ??? IApplicationDbContext.cs
?   ?   ?   ??? ICurrentUserService.cs
?   ?   ?   ??? IDateTime.cs
?   ?   ??? ?? Mappings/
?   ?       ??? MappingProfile.cs
?   ??? ?? DTOs/
?   ?   ??? ?? Auth/
?   ?   ?   ??? LoginRequest.cs
?   ?   ?   ??? LoginResponse.cs
?   ?   ?   ??? RegisterRequest.cs
?   ?   ?   ??? RefreshTokenRequest.cs
?   ?   ??? ?? Clientes/
?   ?   ?   ??? ClienteDto.cs
?   ?   ?   ??? CreateClienteDto.cs
?   ?   ?   ??? UpdateClienteDto.cs
?   ?   ??? ?? Empleados/
?   ?   ?   ??? EmpleadoDto.cs
?   ?   ?   ??? CreateEmpleadoDto.cs
?   ?   ?   ??? UpdateEmpleadoDto.cs
?   ?   ??? ?? Beneficiarios/
?   ?   ?   ??? BeneficiarioDto.cs
?   ?   ?   ??? CreateBeneficiarioDto.cs
?   ?   ?   ??? UpdateBeneficiarioDto.cs
?   ?   ??? ?? Planillas/
?   ?   ?   ??? PlanillaDto.cs
?   ?   ?   ??? CreatePlanillaDto.cs
?   ?   ?   ??? UpdatePlanillaDto.cs
?   ?   ??? ?? Common/
?   ?       ??? PaginatedList.cs
?   ?       ??? ApiResponse.cs
?   ??? ?? Services/
?   ?   ??? ?? Interfaces/
?   ?   ?   ??? IAuthService.cs
?   ?   ?   ??? IClienteService.cs
?   ?   ?   ??? IEmpleadoService.cs
?   ?   ?   ??? IBeneficiarioService.cs
?   ?   ?   ??? IPlanillaService.cs
?   ?   ?   ??? ISedeService.cs
?   ?   ?   ??? IArlService.cs
?   ?   ?   ??? IEmpresaPrestadoraService.cs
?   ?   ?   ??? IEstadisticasService.cs
?   ?   ??? AuthService.cs
?   ?   ??? ClienteService.cs
?   ?   ??? EmpleadoService.cs
?   ?   ??? BeneficiarioService.cs
?   ?   ??? PlanillaService.cs
?   ?   ??? SedeService.cs
?   ?   ??? ArlService.cs
?   ?   ??? EmpresaPrestadoraService.cs
?   ?   ??? EstadisticasService.cs
?   ??? ?? Validators/
?   ?   ??? CreateClienteValidator.cs
?   ?   ??? UpdateClienteValidator.cs
?   ?   ??? CreateEmpleadoValidator.cs
?   ?   ??? LoginRequestValidator.cs
?   ??? DependencyInjection.cs
?
??? ?? Epsepar.Domain/                       # Capa de Dominio
?   ??? ?? Entities/
?   ?   ??? BaseEntity.cs
?   ?   ??? AuditableEntity.cs
?   ?   ??? Cliente.cs
?   ?   ??? Empleado.cs
?   ?   ??? Beneficiario.cs
?   ?   ??? Planilla.cs
?   ?   ??? Sede.cs
?   ?   ??? Arl.cs
?   ?   ??? EmpresaPrestadora.cs
?   ?   ??? PagoCliente.cs
?   ?   ??? Usuario.cs
?   ??? ?? Enums/
?   ?   ??? TipoDocumento.cs
?   ?   ??? TipoCliente.cs
?   ?   ??? Parentesco.cs
?   ?   ??? TipoEmpresaPrestadora.cs
?   ?   ??? EstadoCliente.cs
?   ??? ?? ValueObjects/
?   ?   ??? Email.cs
?   ?   ??? Nit.cs
?   ?   ??? Documento.cs
?   ??? ?? Interfaces/
?   ?   ??? IRepository.cs
?   ?   ??? IUnitOfWork.cs
?   ?   ??? ISpecification.cs
?   ??? ?? Specifications/
?       ??? ClientesPorSedeSpec.cs
?       ??? EmpleadosActivosSpec.cs
?       ??? ClientesEnMoraSpec.cs
?
??? ?? Epsepar.Infrastructure/               # Capa de Infraestructura
?   ??? ?? Data/
?   ?   ??? ApplicationDbContext.cs
?   ?   ??? ?? Configurations/
?   ?   ?   ??? ClienteConfiguration.cs
?   ?   ?   ??? EmpleadoConfiguration.cs
?   ?   ?   ??? BeneficiarioConfiguration.cs
?   ?   ?   ??? PlanillaConfiguration.cs
?   ?   ?   ??? UsuarioConfiguration.cs
?   ?   ?   ??? SedeConfiguration.cs
?   ?   ??? ?? Migrations/
?   ?   ??? ?? Seeding/
?   ?       ??? DatabaseSeeder.cs
?   ?       ??? SedesSeeder.cs
?   ?       ??? RolesSeeder.cs
?   ??? ?? Repositories/
?   ?   ??? Repository.cs
?   ?   ??? ClienteRepository.cs
?   ?   ??? EmpleadoRepository.cs
?   ?   ??? BeneficiarioRepository.cs
?   ?   ??? PlanillaRepository.cs
?   ?   ??? UnitOfWork.cs
?   ??? ?? Identity/
?   ?   ??? JwtTokenService.cs
?   ?   ??? CurrentUserService.cs
?   ?   ??? IdentityService.cs
?   ??? ?? Services/
?   ?   ??? DateTimeService.cs
?   ?   ??? EmailService.cs
?   ?   ??? FileStorageService.cs
?   ??? DependencyInjection.cs
?
??? ?? Epsepar.Tests/                        # Tests
    ??? ?? Epsepar.UnitTests/
    ?   ??? ?? Services/
    ?   ?   ??? ClienteServiceTests.cs
    ?   ?   ??? AuthServiceTests.cs
    ?   ??? ?? Validators/
    ?       ??? CreateClienteValidatorTests.cs
    ??? ?? Epsepar.IntegrationTests/
        ??? ?? Controllers/
        ?   ??? ClientesControllerTests.cs
        ?   ??? AuthControllerTests.cs
        ??? CustomWebApplicationFactory.cs
```

---

## ?? Entity Framework Core - Configuración

### DbContext

```csharp
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService,
        IDateTime dateTime) : base(options)
    {
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    // DbSets
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Beneficiario> Beneficiarios => Set<Beneficiario>();
    public DbSet<Planilla> Planillas => Set<Planilla>();
    public DbSet<Sede> Sedes => Set<Sede>();
    public DbSet<Arl> Arls => Set<Arl>();
    public DbSet<EmpresaPrestadora> EmpresasPrestadoras => Set<EmpresaPrestadora>();
    public DbSet<PagoCliente> PagosClientes => Set<PagoCliente>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas las configuraciones del ensamblado
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Auditoría automática
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.CreatedAt = _dateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _currentUserService.UserId;
                    entry.Entity.UpdatedAt = _dateTime.Now;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
```

### Configuración de Entidad (Fluent API)

```csharp
public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("clientes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nit)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Email)
            .HasMaxLength(100);

        builder.Property(c => c.Telefono)
            .HasMaxLength(20);

        builder.Property(c => c.Direccion)
            .HasMaxLength(300);

        // Índices
        builder.HasIndex(c => c.Nit)
            .IsUnique();

        // Relaciones
        builder.HasOne(c => c.Sede)
            .WithMany(s => s.Clientes)
            .HasForeignKey(c => c.SedeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Empleados)
            .WithOne(e => e.Cliente)
            .HasForeignKey(e => e.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Arl)
            .WithMany()
            .HasForeignKey(c => c.ArlId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
```

### Configuración de MySQL

```csharp
// Program.cs o DependencyInjection.cs
services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString),
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null);

            mySqlOptions.MigrationsAssembly("Epsepar.Infrastructure");
        });

    // Solo en desarrollo
    if (environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});
```

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=epsepardb;User=root;Password=your_password;CharSet=utf8mb4;"
  },
  "JwtSettings": {
    "Secret": "your-super-secret-key-with-at-least-32-characters",
    "Issuer": "Epsepar.API",
    "Audience": "Epsepar.Client",
    "ExpirationInMinutes": 60,
    "RefreshTokenExpirationInDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "CorsOrigins": ["http://localhost:3000", "http://localhost:5173"]
}
```

---

## ?? Autenticación JWT

### Servicio de Tokens

```csharp
public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateAccessToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.Username),
            new(ClaimTypes.Email, usuario.Email ?? string.Empty),
            new("sede_id", usuario.SedeId.ToString()),
            new(ClaimTypes.Role, usuario.Admin ? "Admin" : "User")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            ValidateLifetime = false,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(
            token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
```

### AuthController

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result == null)
            return Unauthorized(new { message = "Credenciales inválidas" });

        return Ok(result);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return CreatedAtAction(nameof(Login), result);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request);

        if (result == null)
            return Unauthorized(new { message = "Token inválido" });

        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return NoContent();
    }
}
```

---

## ?? Inyección de Dependencias Desacoplada

### Extension Methods para Registro

```csharp
// Epsepar.Application/DependencyInjection.cs
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Services
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IEmpleadoService, EmpleadoService>();
        services.AddScoped<IBeneficiarioService, BeneficiarioService>();
        services.AddScoped<IPlanillaService, PlanillaService>();
        services.AddScoped<ISedeService, SedeService>();
        services.AddScoped<IArlService, ArlService>();
        services.AddScoped<IEmpresaPrestadoraService, EmpresaPrestadoraService>();
        services.AddScoped<IEstadisticasService, EstadisticasService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}

// Epsepar.Infrastructure/DependencyInjection.cs
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString)));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
        services.AddScoped<IBeneficiarioRepository, BeneficiarioRepository>();
        services.AddScoped<IPlanillaRepository, PlanillaRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Identity & JWT
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // External Services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IDateTime, DateTimeService>();

        // JWT Configuration
        services.Configure<JwtSettings>(
            configuration.GetSection("JwtSettings"));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}
```

### Program.cs Limpio

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services using extension methods
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// API Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Epsepar API",
        Version = "v1",
        Description = "API para el sistema de gestión EPSEPAR"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("CorsOrigins").Get<string[]>()!)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

// Custom Middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

// Database Migration & Seeding
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await DatabaseSeeder.SeedAsync(context);
}

app.Run();
```

---

## ?? Frontend - React + TypeScript

### Estructura del Proyecto Frontend

```
epsepar-client/
??? ?? public/
?   ??? favicon.ico
?   ??? index.html
??? ?? src/
?   ??? ?? api/                          # Servicios de API
?   ?   ??? axios.config.ts
?   ?   ??? auth.api.ts
?   ?   ??? clientes.api.ts
?   ?   ??? empleados.api.ts
?   ?   ??? beneficiarios.api.ts
?   ?   ??? planillas.api.ts
?   ?   ??? index.ts
?   ?
?   ??? ?? components/                   # Componentes Reutilizables
?   ?   ??? ?? ui/                       # Componentes UI base
?   ?   ?   ??? Button/
?   ?   ?   ?   ??? Button.tsx
?   ?   ?   ?   ??? Button.styles.ts
?   ?   ?   ?   ??? index.ts
?   ?   ?   ??? Input/
?   ?   ?   ??? Select/
?   ?   ?   ??? Modal/
?   ?   ?   ??? Table/
?   ?   ?   ??? Card/
?   ?   ?   ??? Spinner/
?   ?   ?   ??? Alert/
?   ?   ?   ??? index.ts
?   ?   ??? ?? forms/                    # Componentes de formularios
?   ?   ?   ??? ClienteForm/
?   ?   ?   ??? EmpleadoForm/
?   ?   ?   ??? BeneficiarioForm/
?   ?   ?   ??? LoginForm/
?   ?   ??? ?? layout/                   # Componentes de layout
?   ?   ?   ??? Header/
?   ?   ?   ??? Sidebar/
?   ?   ?   ??? Footer/
?   ?   ?   ??? MainLayout/
?   ?   ??? ?? shared/                   # Componentes compartidos
?   ?       ??? DataTable/
?   ?       ??? SearchBar/
?   ?       ??? Pagination/
?   ?       ??? ConfirmDialog/
?   ?       ??? ErrorBoundary/
?   ?
?   ??? ?? features/                     # Módulos por funcionalidad
?   ?   ??? ?? auth/
?   ?   ?   ??? components/
?   ?   ?   ??? hooks/
?   ?   ?   ??? pages/
?   ?   ??? ?? clientes/
?   ?   ?   ??? components/
?   ?   ?   ??? hooks/
?   ?   ?   ??? pages/
?   ?   ??? ?? empleados/
?   ?   ??? ?? beneficiarios/
?   ?   ??? ?? planillas/
?   ?   ??? ?? dashboard/
?   ?   ??? ?? configuracion/
?   ?
?   ??? ?? hooks/                        # Custom Hooks globales
?   ?   ??? useAuth.ts
?   ?   ??? useApi.ts
?   ?   ??? useDebounce.ts
?   ?   ??? usePagination.ts
?   ?   ??? useLocalStorage.ts
?   ?   ??? index.ts
?   ?
?   ??? ?? pages/                        # Páginas/Rutas
?   ?   ??? LoginPage.tsx
?   ?   ??? DashboardPage.tsx
?   ?   ??? ClientesPage.tsx
?   ?   ??? EmpleadosPage.tsx
?   ?   ??? BeneficiariosPage.tsx
?   ?   ??? PlanillasPage.tsx
?   ?   ??? ConfiguracionPage.tsx
?   ?   ??? NotFoundPage.tsx
?   ?
?   ??? ?? store/                        # Estado Global (Zustand)
?   ?   ??? authStore.ts
?   ?   ??? clientesStore.ts
?   ?   ??? empleadosStore.ts
?   ?   ??? uiStore.ts
?   ?   ??? index.ts
?   ?
?   ??? ?? types/                        # TypeScript Types/Interfaces
?   ?   ??? auth.types.ts
?   ?   ??? cliente.types.ts
?   ?   ??? empleado.types.ts
?   ?   ??? beneficiario.types.ts
?   ?   ??? planilla.types.ts
?   ?   ??? api.types.ts
?   ?   ??? index.ts
?   ?
?   ??? ?? utils/                        # Utilidades
?   ?   ??? formatters.ts
?   ?   ??? validators.ts
?   ?   ??? constants.ts
?   ?   ??? helpers.ts
?   ?   ??? index.ts
?   ?
?   ??? ?? routes/                       # Configuración de rutas
?   ?   ??? PrivateRoute.tsx
?   ?   ??? PublicRoute.tsx
?   ?   ??? AppRoutes.tsx
?   ?
?   ??? ?? styles/                       # Estilos globales
?   ?   ??? globals.css
?   ?   ??? variables.css
?   ?   ??? tailwind.css
?   ?
?   ??? App.tsx
?   ??? main.tsx
?   ??? vite-env.d.ts
?
??? .env
??? .env.development
??? .env.production
??? package.json
??? tsconfig.json
??? vite.config.ts
??? tailwind.config.js
??? postcss.config.js
```

### Gestión de Estado Global con Zustand

```typescript
// store/authStore.ts
import { create } from "zustand";
import { persist, createJSONStorage } from "zustand/middleware";
import { AuthState, LoginRequest, User } from "@/types";
import { authApi } from "@/api";

interface AuthStore extends AuthState {
  // Actions
  login: (credentials: LoginRequest) => Promise<void>;
  logout: () => void;
  refreshToken: () => Promise<void>;
  setUser: (user: User | null) => void;
}

export const useAuthStore = create<AuthStore>()(
  persist(
    (set, get) => ({
      // State
      user: null,
      accessToken: null,
      refreshToken: null,
      isAuthenticated: false,
      isLoading: false,
      error: null,

      // Actions
      login: async (credentials: LoginRequest) => {
        set({ isLoading: true, error: null });
        try {
          const response = await authApi.login(credentials);
          set({
            user: response.user,
            accessToken: response.accessToken,
            refreshToken: response.refreshToken,
            isAuthenticated: true,
            isLoading: false,
          });
        } catch (error: any) {
          set({
            error: error.response?.data?.message || "Error al iniciar sesión",
            isLoading: false,
          });
          throw error;
        }
      },

      logout: () => {
        set({
          user: null,
          accessToken: null,
          refreshToken: null,
          isAuthenticated: false,
          error: null,
        });
      },

      refreshToken: async () => {
        const { refreshToken } = get();
        if (!refreshToken) throw new Error("No refresh token");

        try {
          const response = await authApi.refreshToken({ refreshToken });
          set({
            accessToken: response.accessToken,
            refreshToken: response.refreshToken,
          });
        } catch (error) {
          get().logout();
          throw error;
        }
      },

      setUser: (user) => set({ user }),
    }),
    {
      name: "auth-storage",
      storage: createJSONStorage(() => localStorage),
      partialize: (state) => ({
        user: state.user,
        accessToken: state.accessToken,
        refreshToken: state.refreshToken,
        isAuthenticated: state.isAuthenticated,
      }),
    },
  ),
);
```

```typescript
// store/clientesStore.ts
import { create } from "zustand";
import {
  Cliente,
  CreateClienteDto,
  UpdateClienteDto,
  PaginatedResult,
} from "@/types";
import { clientesApi } from "@/api";

interface ClientesState {
  clientes: Cliente[];
  selectedCliente: Cliente | null;
  isLoading: boolean;
  error: string | null;
  pagination: {
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  };
  filters: {
    search: string;
    estado: string;
    sedeId: number | null;
  };
}

interface ClientesActions {
  fetchClientes: () => Promise<void>;
  fetchClienteById: (id: number) => Promise<void>;
  createCliente: (data: CreateClienteDto) => Promise<void>;
  updateCliente: (id: number, data: UpdateClienteDto) => Promise<void>;
  deleteCliente: (id: number) => Promise<void>;
  setPage: (page: number) => void;
  setFilters: (filters: Partial<ClientesState["filters"]>) => void;
  clearError: () => void;
}

export const useClientesStore = create<ClientesState & ClientesActions>(
  (set, get) => ({
    // State
    clientes: [],
    selectedCliente: null,
    isLoading: false,
    error: null,
    pagination: {
      page: 1,
      pageSize: 10,
      totalCount: 0,
      totalPages: 0,
    },
    filters: {
      search: "",
      estado: "",
      sedeId: null,
    },

    // Actions
    fetchClientes: async () => {
      const { pagination, filters } = get();
      set({ isLoading: true, error: null });

      try {
        const response = await clientesApi.getAll({
          page: pagination.page,
          pageSize: pagination.pageSize,
          ...filters,
        });

        set({
          clientes: response.items,
          pagination: {
            ...pagination,
            totalCount: response.totalCount,
            totalPages: response.totalPages,
          },
          isLoading: false,
        });
      } catch (error: any) {
        set({
          error: error.response?.data?.message || "Error al cargar clientes",
          isLoading: false,
        });
      }
    },

    fetchClienteById: async (id: number) => {
      set({ isLoading: true, error: null });
      try {
        const cliente = await clientesApi.getById(id);
        set({ selectedCliente: cliente, isLoading: false });
      } catch (error: any) {
        set({
          error: error.response?.data?.message || "Error al cargar cliente",
          isLoading: false,
        });
      }
    },

    createCliente: async (data: CreateClienteDto) => {
      set({ isLoading: true, error: null });
      try {
        await clientesApi.create(data);
        await get().fetchClientes();
      } catch (error: any) {
        set({
          error: error.response?.data?.message || "Error al crear cliente",
          isLoading: false,
        });
        throw error;
      }
    },

    updateCliente: async (id: number, data: UpdateClienteDto) => {
      set({ isLoading: true, error: null });
      try {
        await clientesApi.update(id, data);
        await get().fetchClientes();
      } catch (error: any) {
        set({
          error: error.response?.data?.message || "Error al actualizar cliente",
          isLoading: false,
        });
        throw error;
      }
    },

    deleteCliente: async (id: number) => {
      set({ isLoading: true, error: null });
      try {
        await clientesApi.delete(id);
        await get().fetchClientes();
      } catch (error: any) {
        set({
          error: error.response?.data?.message || "Error al eliminar cliente",
          isLoading: false,
        });
        throw error;
      }
    },

    setPage: (page: number) => {
      set((state) => ({
        pagination: { ...state.pagination, page },
      }));
      get().fetchClientes();
    },

    setFilters: (filters) => {
      set((state) => ({
        filters: { ...state.filters, ...filters },
        pagination: { ...state.pagination, page: 1 },
      }));
      get().fetchClientes();
    },

    clearError: () => set({ error: null }),
  }),
);
```

### Componentes Reutilizables

```typescript
// components/ui/Button/Button.tsx
import React from 'react';
import { Spinner } from '../Spinner';
import { cn } from '@/utils/helpers';

export interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary' | 'danger' | 'ghost' | 'outline';
  size?: 'sm' | 'md' | 'lg';
  isLoading?: boolean;
  leftIcon?: React.ReactNode;
  rightIcon?: React.ReactNode;
}

export const Button = React.forwardRef<HTMLButtonElement, ButtonProps>(
  (
    {
      className,
      variant = 'primary',
      size = 'md',
      isLoading = false,
      leftIcon,
      rightIcon,
      disabled,
      children,
      ...props
    },
    ref
  ) => {
    const baseStyles = 'inline-flex items-center justify-center font-medium rounded-lg transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed';

    const variants = {
      primary: 'bg-blue-600 text-white hover:bg-blue-700 focus:ring-blue-500',
      secondary: 'bg-gray-200 text-gray-900 hover:bg-gray-300 focus:ring-gray-500',
      danger: 'bg-red-600 text-white hover:bg-red-700 focus:ring-red-500',
      ghost: 'bg-transparent hover:bg-gray-100 focus:ring-gray-500',
      outline: 'border-2 border-blue-600 text-blue-600 hover:bg-blue-50 focus:ring-blue-500',
    };

    const sizes = {
      sm: 'px-3 py-1.5 text-sm',
      md: 'px-4 py-2 text-base',
      lg: 'px-6 py-3 text-lg',
    };

    return (
      <button
        ref={ref}
        className={cn(baseStyles, variants[variant], sizes[size], className)}
        disabled={disabled || isLoading}
        {...props}
      >
        {isLoading && <Spinner size="sm" className="mr-2" />}
        {!isLoading && leftIcon && <span className="mr-2">{leftIcon}</span>}
        {children}
        {!isLoading && rightIcon && <span className="ml-2">{rightIcon}</span>}
      </button>
    );
  }
);

Button.displayName = 'Button';
```

```typescript
// components/shared/DataTable/DataTable.tsx
import React from 'react';
import { Spinner } from '@/components/ui';
import { Pagination } from '../Pagination';

export interface Column<T> {
  key: keyof T | string;
  header: string;
  render?: (item: T) => React.ReactNode;
  sortable?: boolean;
  className?: string;
}

export interface DataTableProps<T> {
  data: T[];
  columns: Column<T>[];
  isLoading?: boolean;
  pagination?: {
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    onPageChange: (page: number) => void;
  };
  onRowClick?: (item: T) => void;
  emptyMessage?: string;
  rowKey: keyof T;
}

export function DataTable<T>({
  data,
  columns,
  isLoading = false,
  pagination,
  onRowClick,
  emptyMessage = 'No hay datos disponibles',
  rowKey,
}: DataTableProps<T>) {
  const getValue = (item: T, key: string): any => {
    const keys = key.split('.');
    return keys.reduce((obj: any, k) => obj?.[k], item);
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center py-12">
        <Spinner size="lg" />
      </div>
    );
  }

  return (
    <div className="overflow-x-auto">
      <table className="min-w-full divide-y divide-gray-200">
        <thead className="bg-gray-50">
          <tr>
            {columns.map((column) => (
              <th
                key={String(column.key)}
                className={`px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider ${column.className || ''}`}
              >
                {column.header}
              </th>
            ))}
          </tr>
        </thead>
        <tbody className="bg-white divide-y divide-gray-200">
          {data.length === 0 ? (
            <tr>
              <td
                colSpan={columns.length}
                className="px-6 py-12 text-center text-gray-500"
              >
                {emptyMessage}
              </td>
            </tr>
          ) : (
            data.map((item) => (
              <tr
                key={String(item[rowKey])}
                onClick={() => onRowClick?.(item)}
                className={onRowClick ? 'cursor-pointer hover:bg-gray-50' : ''}
              >
                {columns.map((column) => (
                  <td
                    key={String(column.key)}
                    className={`px-6 py-4 whitespace-nowrap text-sm text-gray-900 ${column.className || ''}`}
                  >
                    {column.render
                      ? column.render(item)
                      : getValue(item, String(column.key))}
                  </td>
                ))}
              </tr>
            ))
          )}
        </tbody>
      </table>

      {pagination && pagination.totalPages > 1 && (
        <div className="px-6 py-4 border-t">
          <Pagination
            currentPage={pagination.page}
            totalPages={pagination.totalPages}
            onPageChange={pagination.onPageChange}
          />
        </div>
      )}
    </div>
  );
}
```

### API Service con Axios

```typescript
// api/axios.config.ts
import axios, { AxiosError, InternalAxiosRequestConfig } from "axios";
import { useAuthStore } from "@/store";

const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5000/api";

export const apiClient = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Request interceptor - Añade token a las peticiones
apiClient.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const { accessToken } = useAuthStore.getState();

    if (accessToken) {
      config.headers.Authorization = `Bearer ${accessToken}`;
    }

    return config;
  },
  (error) => Promise.reject(error),
);

// Response interceptor - Maneja refresh token
apiClient.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const originalRequest = error.config as InternalAxiosRequestConfig & {
      _retry?: boolean;
    };

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        await useAuthStore.getState().refreshToken();
        const { accessToken } = useAuthStore.getState();
        originalRequest.headers.Authorization = `Bearer ${accessToken}`;
        return apiClient(originalRequest);
      } catch (refreshError) {
        useAuthStore.getState().logout();
        window.location.href = "/login";
        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  },
);
```

```typescript
// api/clientes.api.ts
import { apiClient } from "./axios.config";
import {
  Cliente,
  CreateClienteDto,
  UpdateClienteDto,
  PaginatedResult,
  ClienteFilters,
} from "@/types";

export const clientesApi = {
  getAll: async (
    params?: ClienteFilters,
  ): Promise<PaginatedResult<Cliente>> => {
    const response = await apiClient.get<PaginatedResult<Cliente>>(
      "/clientes",
      { params },
    );
    return response.data;
  },

  getById: async (id: number): Promise<Cliente> => {
    const response = await apiClient.get<Cliente>(`/clientes/${id}`);
    return response.data;
  },

  create: async (data: CreateClienteDto): Promise<Cliente> => {
    const response = await apiClient.post<Cliente>("/clientes", data);
    return response.data;
  },

  update: async (id: number, data: UpdateClienteDto): Promise<Cliente> => {
    const response = await apiClient.put<Cliente>(`/clientes/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/clientes/${id}`);
  },

  getEstadisticas: async (): Promise<ClienteEstadisticas> => {
    const response = await apiClient.get<ClienteEstadisticas>(
      "/clientes/estadisticas",
    );
    return response.data;
  },
};
```

### Custom Hooks

```typescript
// hooks/useApi.ts
import { useState, useCallback } from "react";

interface UseApiState<T> {
  data: T | null;
  isLoading: boolean;
  error: string | null;
}

interface UseApiReturn<T, P extends any[]> extends UseApiState<T> {
  execute: (...args: P) => Promise<T | null>;
  reset: () => void;
}

export function useApi<T, P extends any[]>(
  apiFunction: (...args: P) => Promise<T>,
): UseApiReturn<T, P> {
  const [state, setState] = useState<UseApiState<T>>({
    data: null,
    isLoading: false,
    error: null,
  });

  const execute = useCallback(
    async (...args: P): Promise<T | null> => {
      setState((prev) => ({ ...prev, isLoading: true, error: null }));

      try {
        const data = await apiFunction(...args);
        setState({ data, isLoading: false, error: null });
        return data;
      } catch (error: any) {
        const errorMessage =
          error.response?.data?.message || "Ha ocurrido un error";
        setState((prev) => ({
          ...prev,
          isLoading: false,
          error: errorMessage,
        }));
        return null;
      }
    },
    [apiFunction],
  );

  const reset = useCallback(() => {
    setState({ data: null, isLoading: false, error: null });
  }, []);

  return { ...state, execute, reset };
}
```

```typescript
// hooks/useAuth.ts
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuthStore } from "@/store";

export const useAuth = () => {
  const navigate = useNavigate();
  const { user, isAuthenticated, isLoading, error, login, logout, clearError } =
    useAuthStore();

  const handleLogin = async (credentials: LoginRequest) => {
    try {
      await login(credentials);
      navigate("/dashboard");
    } catch (error) {
      // Error handled in store
    }
  };

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return {
    user,
    isAuthenticated,
    isLoading,
    error,
    login: handleLogin,
    logout: handleLogout,
    clearError,
  };
};
```

### TypeScript Types

```typescript
// types/cliente.types.ts
export interface Cliente {
  id: number;
  nit: string;
  nombre: string;
  tipoCliente: TipoCliente;
  referente: string;
  direccion: string;
  telefono: string;
  email: string;
  sedeId: number;
  sedeName: string;
  estado: EstadoCliente;
  arlId: number | null;
  afpId: number | null;
  epsId: number | null;
  cajaId: number | null;
  createdAt: string;
  updatedAt: string | null;
}

export interface CreateClienteDto {
  nit: string;
  nombre: string;
  tipoCliente: TipoCliente;
  referente?: string;
  direccion: string;
  telefono: string;
  email?: string;
  sedeId: number;
  arlId?: number;
  afpId?: number;
  epsId?: number;
  cajaId?: number;
}

export interface UpdateClienteDto extends Partial<CreateClienteDto> {
  estado?: EstadoCliente;
}

export enum TipoCliente {
  Natural = 1,
  Juridico = 2,
}

export enum EstadoCliente {
  Activo = 1,
  Inactivo = 2,
  Suspendido = 3,
}

export interface ClienteFilters {
  page?: number;
  pageSize?: number;
  search?: string;
  estado?: EstadoCliente;
  sedeId?: number;
}
```

```typescript
// types/api.types.ts
export interface ApiResponse<T> {
  data: T;
  message?: string;
  success: boolean;
}

export interface ApiError {
  message: string;
  errors?: Record<string, string[]>;
  statusCode: number;
}

export interface PaginatedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
```

---

## ?? Testing

### Backend - xUnit

```csharp
// Tests/Epsepar.UnitTests/Services/ClienteServiceTests.cs
public class ClienteServiceTests
{
    private readonly Mock<IClienteRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ClienteService _sut;

    public ClienteServiceTests()
    {
        _repositoryMock = new Mock<IClienteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _sut = new ClienteService(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenClienteExists_ReturnsClienteDto()
    {
        // Arrange
        var cliente = new Cliente { Id = 1, Nombre = "Test Cliente" };
        var clienteDto = new ClienteDto { Id = 1, Nombre = "Test Cliente" };

        _repositoryMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(cliente);
        _mapperMock.Setup(m => m.Map<ClienteDto>(cliente))
            .Returns(clienteDto);

        // Act
        var result = await _sut.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Cliente", result.Nombre);
    }

    [Fact]
    public async Task GetByIdAsync_WhenClienteNotExists_ReturnsNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Cliente?)null);

        // Act
        var result = await _sut.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsCreatedCliente()
    {
        // Arrange
        var createDto = new CreateClienteDto { Nit = "123456", Nombre = "Nuevo Cliente" };
        var cliente = new Cliente { Id = 1, Nit = "123456", Nombre = "Nuevo Cliente" };
        var clienteDto = new ClienteDto { Id = 1, Nit = "123456", Nombre = "Nuevo Cliente" };

        _mapperMock.Setup(m => m.Map<Cliente>(createDto)).Returns(cliente);
        _mapperMock.Setup(m => m.Map<ClienteDto>(cliente)).Returns(clienteDto);
        _repositoryMock.Setup(r => r.AddAsync(cliente)).ReturnsAsync(cliente);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await _sut.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("123456", result.Nit);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    // Más tests para otras funcionalidades...
}
```

### Frontend - Jest + React Testing Library

```typescript
// features/clientes/components/__tests__/ClienteForm.test.tsx
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { ClienteForm } from '../ClienteForm';

const mockOnSubmit = jest.fn();
const mockOnCancel = jest.fn();

describe('ClienteForm', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('renders all form fields', () => {
    render(<ClienteForm onSubmit={mockOnSubmit} onCancel={mockOnCancel} />);

    expect(screen.getByLabelText(/nit/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/nombre/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/dirección/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/teléfono/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
  });

  it('shows validation errors for required fields', async () => {
    render(<ClienteForm onSubmit={mockOnSubmit} onCancel={mockOnCancel} />);

    fireEvent.click(screen.getByRole('button', { name: /guardar/i }));

    await waitFor(() => {
      expect(screen.getByText(/el nit es requerido/i)).toBeInTheDocument();
      expect(screen.getByText(/el nombre es requerido/i)).toBeInTheDocument();
    });

    expect(mockOnSubmit).not.toHaveBeenCalled();
  });

  it('calls onSubmit with form data when valid', async () => {
    const user = userEvent.setup();
    render(<ClienteForm onSubmit={mockOnSubmit} onCancel={mockOnCancel} />);

    await user.type(screen.getByLabelText(/nit/i), '123456789');
    await user.type(screen.getByLabelText(/nombre/i), 'Test Cliente');
    await user.type(screen.getByLabelText(/dirección/i), 'Calle 123');
    await user.type(screen.getByLabelText(/teléfono/i), '1234567890');

    fireEvent.click(screen.getByRole('button', { name: /guardar/i }));

    await waitFor(() => {
      expect(mockOnSubmit).toHaveBeenCalledWith({
        nit: '123456789',
        nombre: 'Test Cliente',
        direccion: 'Calle 123',
        telefono: '1234567890',
        email: '',
      });
    });
  });

  it('calls onCancel when cancel button is clicked', () => {
    render(<ClienteForm onSubmit={mockOnSubmit} onCancel={mockOnCancel} />);

    fireEvent.click(screen.getByRole('button', { name: /cancelar/i }));

    expect(mockOnCancel).toHaveBeenCalled();
  });
});
```

---

## ?? Plan de Migración

### Fase 1: Preparación (2 semanas)

- [ ] Configurar nuevo repositorio Git
- [ ] Crear solución .NET 8 con estructura Clean Architecture
- [ ] Configurar Entity Framework Core con MySQL
- [ ] Definir entidades y migraciones iniciales
- [ ] Configurar CI/CD básico

### Fase 2: Backend Core (4 semanas)

- [ ] Implementar capa de dominio (entidades, enums, interfaces)
- [ ] Implementar repositorios genéricos y específicos
- [ ] Crear Unit of Work
- [ ] Implementar servicios de aplicación
- [ ] Configurar AutoMapper
- [ ] Implementar validaciones con FluentValidation
- [ ] Configurar autenticación JWT
- [ ] Crear controladores API

### Fase 3: Frontend Base (3 semanas)

- [ ] Inicializar proyecto React + TypeScript + Vite
- [ ] Configurar Tailwind CSS
- [ ] Implementar componentes UI base
- [ ] Configurar Zustand stores
- [ ] Implementar servicios API
- [ ] Crear sistema de rutas
- [ ] Implementar autenticación

### Fase 4: Funcionalidades (4 semanas)

- [ ] Migrar módulo de Clientes
- [ ] Migrar módulo de Empleados
- [ ] Migrar módulo de Beneficiarios
- [ ] Migrar módulo de Planillas
- [ ] Migrar módulo de Configuración
- [ ] Implementar Dashboard

### Fase 5: Testing y Calidad (2 semanas)

- [ ] Escribir tests unitarios backend
- [ ] Escribir tests de integración
- [ ] Escribir tests frontend
- [ ] Code review y refactoring
- [ ] Documentación API (Swagger)

### Fase 6: Migración de Datos (1 semana)

- [ ] Crear scripts de migración de datos
- [ ] Validar integridad de datos
- [ ] Ejecutar migración en staging
- [ ] Pruebas de aceptación

### Fase 7: Despliegue (1 semana)

- [ ] Configurar entorno de producción
- [ ] Configurar DNS y certificados SSL
- [ ] Deploy a producción
- [ ] Monitoreo post-deploy
- [ ] Documentación de operaciones

---

## ?? Dependencias del Proyecto

### Backend (.NET 8)

```xml
<!-- Epsepar.API.csproj -->
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
  <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
  <PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
</ItemGroup>

<!-- Epsepar.Application.csproj -->
<ItemGroup>
  <PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
  <PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.9.0" />
  <PackageReference Include="MediatR" Version="12.2.0" />
</ItemGroup>

<!-- Epsepar.Infrastructure.csproj -->
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
  <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
</ItemGroup>
```

### Frontend (package.json)

```json
{
  "dependencies": {
    "react": "^18.2.0",
    "react-dom": "^18.2.0",
    "react-router-dom": "^6.21.0",
    "zustand": "^4.4.7",
    "axios": "^1.6.2",
    "react-hook-form": "^7.49.0",
    "@hookform/resolvers": "^3.3.2",
    "zod": "^3.22.4",
    "tailwindcss": "^3.4.0",
    "@headlessui/react": "^1.7.17",
    "@heroicons/react": "^2.1.1",
    "date-fns": "^3.0.6",
    "recharts": "^2.10.3"
  },
  "devDependencies": {
    "@types/react": "^18.2.45",
    "@types/react-dom": "^18.2.18",
    "@vitejs/plugin-react": "^4.2.1",
    "typescript": "^5.3.3",
    "vite": "^5.0.10",
    "vitest": "^1.1.0",
    "@testing-library/react": "^14.1.2",
    "@testing-library/jest-dom": "^6.1.6",
    "eslint": "^8.56.0",
    "prettier": "^3.1.1"
  }
}
```

---

## ?? Beneficios de la Modernización

| Aspecto                  | Beneficio                                            |
| ------------------------ | ---------------------------------------------------- |
| **Rendimiento**          | .NET 8 es hasta 40% más rápido que .NET Framework    |
| **Mantenibilidad**       | Clean Architecture facilita cambios y evolución      |
| **Testabilidad**         | DI y abstracciones permiten testing efectivo         |
| **Escalabilidad**        | Arquitectura preparada para microservicios           |
| **Developer Experience** | Mejor tooling, hot reload, debugging                 |
| **Seguridad**            | JWT estándar, Identity, HTTPS por defecto            |
| **Frontend**             | React más moderno, mejor ecosistema, TypeScript      |
| **Estado**               | Zustand más simple y eficiente que servicios Angular |
| **Componentes**          | Reutilizables, tipados, bien testeados               |

---

## ?? Checklist de Calidad

### Backend

- [ ] Todos los endpoints documentados en Swagger
- [ ] Validación de inputs con FluentValidation
- [ ] Manejo global de excepciones
- [ ] Logging estructurado con Serilog
- [ ] Tests unitarios > 80% cobertura
- [ ] Tests de integración para endpoints críticos
- [ ] Rate limiting implementado
- [ ] CORS configurado correctamente

### Frontend

- [ ] Todos los componentes tipados con TypeScript
- [ ] Componentes UI documentados
- [ ] Estado global bien estructurado
- [ ] Manejo de errores en API calls
- [ ] Loading states implementados
- [ ] Tests de componentes principales
- [ ] Responsive design
- [ ] Accesibilidad (a11y) básica

---

## ?? Contacto y Soporte

Para dudas sobre esta propuesta de modernización, contactar al equipo de desarrollo.

---

_Documento generado por GitHub Copilot - Propuesta de Modernización v1.0_
