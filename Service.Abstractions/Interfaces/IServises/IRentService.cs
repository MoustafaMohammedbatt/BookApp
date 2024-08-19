using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions.Interfaces.IServises
{
    public interface IRentService
    {
        Task<Rented?> ToggleReturned(int id);
    }
}
