# Autenticación JWT e Identity

El proyecto implementa autenticación basada en JWT y ASP.NET Core Identity:

1. **Identity**: Se utiliza `EpseparIdentityDbContext` y `IdentityUser` para la gestión de usuarios y roles.
2. **JWT**: La configuración se encuentra en `appsettings.json` bajo la sección `Jwt`.
3. **Endpoints de autenticación**: Existen endpoints `/api/auth/register` y `/api/auth/login` para registro y login de usuarios.
4. **Configuración de servicios**: En `Program.cs` se registran Identity, JWT y los middlewares de autenticación/autorización.

Para pruebas rápidas, puedes registrar un usuario y luego loguearte para obtener un token JWT, el cual debe ser enviado en el header `Authorization: Bearer {token}` para acceder a endpoints protegidos.

# NewEpsepar

Prueba de activación de workflow CI/CD con GitHub Actions (31/03/2026).

Solución base creada bajo Clean Architecture (.NET 8).

## Estructura

- **Domain**: Entidades y lógica de dominio
- **Application**: Casos de uso y lógica de aplicación
- **Infrastructure**: Implementaciones técnicas (repositorios, servicios externos)
- **WebApi**: API REST principal

## Próximos pasos

- Configuración de MySQL y EF Core
- Integración continua y despliegue
- Documentación técnica y dependencias

---

> Documentación generada automáticamente. Actualizar según avance el proyecto.
