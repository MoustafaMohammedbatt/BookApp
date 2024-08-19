using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using Shared.DTOs;

namespace Service.Abstractions.Interfaces.IServises
{

    public interface ISoldService
    {
        Task<IEnumerable<SoldCreateDTO>> GetAllSoldsAsync();
        Task<SoldCreateViewModel> PrepareSoldCreateViewModelAsync(int cartId, string userId);
        Task<bool> CreateSoldAsync(SoldCreateViewModel viewModel);
    }

}
