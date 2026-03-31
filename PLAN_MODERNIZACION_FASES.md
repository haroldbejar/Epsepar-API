# Plan de Modernización por Fases – AppWeb EPSEPAR

Modernización progresiva de AppWeb EPSEPAR, migrando de ASP.NET Web Forms + Angular 8 a .NET 8 Web API y React + TypeScript, siguiendo Clean Architecture y prácticas modernas. El plan se estructura en fases cortas, priorizando entregables funcionales y reducción de riesgos.

## Fases y pasos

### Fase 1: Preparación y Base Técnica

1. Crear nuevo repositorio y solución base (.NET 8, estructura Clean Architecture)
2. Configurar CI/CD básico y entorno de desarrollo
3. Definir entidades principales y migraciones iniciales en EF Core
4. Configurar base de datos MySQL y conexión
5. Documentar dependencias técnicas y riesgos identificados

### Fase 2: Backend Core y Servicios

1. Implementar capa de dominio (entidades, enums, excepciones, interfaces)
2. Crear repositorios genéricos y específicos, Unit of Work
3. Implementar servicios de aplicación y validaciones (FluentValidation)
4. Configurar autenticación JWT e Identity
5. Crear controladores API REST para módulos principales (Clientes, Empleados, Beneficiarios, Planillas, Sedes, ARL, Empresas Prestadoras, Usuarios)
6. Configurar AutoMapper y logging estructurado

### Fase 3: Frontend Base y Autenticación

1. Inicializar proyecto React + TypeScript + Vite
2. Configurar Tailwind CSS y estructura de carpetas
3. Implementar componentes UI base reutilizables
4. Configurar Zustand para estado global
5. Implementar servicios API y sistema de rutas
6. Implementar autenticación y manejo de sesión JWT

### Fase 4: Migración Modular por Funcionalidad

1. Migrar módulo de Clientes (backend + frontend)
2. Migrar módulo de Empleados
3. Migrar módulo de Beneficiarios
4. Migrar módulo de Planillas
5. Migrar módulo de Configuración y Dashboard
6. Migrar reportes y funcionalidades adicionales

### Fase 5: Testing y Calidad

1. Escribir tests unitarios backend (xUnit)
2. Escribir tests de integración backend
3. Escribir tests frontend (Jest + React Testing Library)
4. Code review, refactor y documentación API (Swagger)

### Fase 6: Migración de Datos

1. Crear scripts de migración y validación de datos
2. Ejecutar migración en entorno staging
3. Validar integridad y pruebas de aceptación

### Fase 7: Despliegue y Operaciones

1. Configurar entorno de producción, DNS y SSL
2. Deploy a producción y monitoreo post-deploy
3. Documentar operaciones y soporte

## Archivos y módulos críticos

- Backend: Controladores API, entidades dominio, repositorios, servicios, configuración EF Core, autenticación JWT
- Frontend: Componentes React, stores Zustand, servicios API, rutas, formularios, tablas, autenticación
- Migración: Scripts de datos, validaciones, pruebas de integridad

## Verificación

1. Validar cada fase con entregables funcionales y pruebas automatizadas
2. Checklist de calidad: cobertura de tests, documentación, manejo de errores, seguridad, performance
3. Pruebas de aceptación con usuarios clave tras cada módulo migrado

## Decisiones y consideraciones

- Migración modular para minimizar riesgos y permitir entregas incrementales
- Priorización de módulos críticos: Clientes, Empleados, Beneficiarios, Planillas
- Mantener interoperabilidad temporal si es necesario (APIs legacy)
- Documentar dependencias técnicas y riesgos antes de cada fase
- Excluir microservicios y migración de base de datos a otro motor (solo MySQL)

## Siguientes pasos recomendados

1. Validar alcance y prioridades con stakeholders
2. Ajustar fases según recursos y dependencias identificadas
3. Iniciar Fase 1 con setup técnico y documentación de riesgos
