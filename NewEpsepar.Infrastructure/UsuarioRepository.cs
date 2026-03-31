using NewEpsepar.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace NewEpsepar.Infrastructure;

public interface IUsuarioRepository
{
    Usuario Add(Usuario usuario);
    IEnumerable<Usuario> GetAll();
}

public class UsuarioRepository : IUsuarioRepository
{
    private readonly EpseparDbContext _context;

    public UsuarioRepository(EpseparDbContext context)
    {
        _context = context;
    }

    public Usuario Add(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        return usuario;
    }

    public IEnumerable<Usuario> GetAll()
    {
        return _context.Usuarios.AsNoTracking().ToList();
    }
}
