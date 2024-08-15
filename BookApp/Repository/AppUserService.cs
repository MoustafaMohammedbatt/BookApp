using Domain.Entites;
using Service.Abstractions.Interfaces.IRepositories;
using Service.Abstractions.Interfaces.IServises;

namespace BookApp.Repository
{
    public class AppUserService :IAppUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AppUserService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AppUser?> ToggleAdminAccepted(string id)
        {
            var user = await _unitOfWork.ApplicationUsers.GetUserById(id);

            if (user is null) return null;

            user.AdminAccepted = !user.AdminAccepted;

            _unitOfWork.Complete();

            return user;
        }

        public async Task<AppUser?> ToggleDelete(string id)
        {
            var user = await _unitOfWork.ApplicationUsers.GetUserById(id);

            if (user is null) return null;

            user.IsDeleted = !user.IsDeleted;

            _unitOfWork.Complete();

            return user;
        }

        
    }
}
