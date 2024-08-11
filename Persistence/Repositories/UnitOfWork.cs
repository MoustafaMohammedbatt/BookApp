using Domain.Entites;
using Persistence.Data;
using Service.Abstractions.Interfaces.IBaseRepository;
using Service.Abstractions.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IBaseRepository<AppUser> ApplicationUsers => new BaseRepository<AppUser>(_context);
        public IBaseRepository<Book> Books => new BaseRepository<Book>(_context);

        public IBaseRepository<Category> Categories => new BaseRepository<Category>(_context);

        public IBaseRepository<Sold> Solds => new BaseRepository<Sold>(_context);

        public IBaseRepository<Rented> Renteds => new BaseRepository<Rented>(_context);

        public IBaseRepository<Author> Authors => new BaseRepository<Author>(_context);

        public IBaseRepository<Cart> Carts => new BaseRepository<Cart>(_context);

        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}