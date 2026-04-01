using AutoMapper;
using NewEpsepar.Domain;
using NewEpsepar.Application.DTOs.ARLs;
using NewEpsepar.Application.DTOs.Beneficiarios;
using NewEpsepar.Application.DTOs.Clientes;
using NewEpsepar.Application.DTOs.Empleados;
using NewEpsepar.Application.DTOs.EmpresasPrestadoras;
using NewEpsepar.Application.DTOs.Planillas;
using NewEpsepar.Application.DTOs.Sedes;
using NewEpsepar.Application.DTOs.Usuarios;

namespace NewEpsepar.WebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ARL
            CreateMap<ARL, ARLDto>().ReverseMap();
            // Beneficiario
            CreateMap<Beneficiario, BeneficiarioDto>().ReverseMap();
            // Cliente
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            // Empleado
            CreateMap<Empleado, EmpleadoDto>().ReverseMap();
            // Empresa Prestadora
            CreateMap<EmpresaPrestadora, EmpresaPrestadoraDto>().ReverseMap();
            // Planilla
            CreateMap<Planilla, PlanillaDto>().ReverseMap();
            // Sede
            CreateMap<Sede, SedeDto>().ReverseMap();
            // Usuario
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
        }
    }
}
