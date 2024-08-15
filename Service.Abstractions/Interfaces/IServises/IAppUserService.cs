using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions.Interfaces.IServises
{
    public interface IAppUserService
    {
        Task<AppUser?> ToggleDelete(string id);
        Task<AppUser?> ToggleAdminAccepted(string id);
    }
}
