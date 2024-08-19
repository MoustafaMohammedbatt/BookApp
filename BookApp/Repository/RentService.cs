using Domain.Entites;
using Service.Abstractions.Interfaces.IRepositories;
using Service.Abstractions.Interfaces.IServises;

namespace BookApp.Repository
{
    public class RentService: IRentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Rented?> ToggleReturned(int id)
        {
            var rent = await _unitOfWork.Renteds.GetById(id);

            if (rent is null) return null;

            rent.IsReturned = !rent.IsReturned;

            _unitOfWork.Complete();

            return rent;
        }
    }
}
