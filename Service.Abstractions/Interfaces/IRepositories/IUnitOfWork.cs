using Domain.Entites;
using Service.Abstractions.Interfaces.IBaseRepository;

namespace Service.Abstractions.Interfaces.IRepositories
{
    public interface IUnitOfWork
    {
        IBaseRepository<AppUser> ApplicationUsers { get; }
        IBaseRepository<Category> Categories { get; }
        IBaseRepository<Book> Books { get; }
        IBaseRepository<Sold> Solds { get; }
        IBaseRepository<Rented> Renteds { get; }
        IBaseRepository<Author> Authors { get; }
        IBaseRepository<Cart> Carts { get; }

        int Complete();
    }
}
