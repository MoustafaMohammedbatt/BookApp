using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entites;
using Shared.DTOs;

namespace Service.Abstractions.Interfaces.IServises
{

    public interface ISoldService
    {
        Task<IEnumerable<SoldDTO>> GetAllSoldsAsync();
        Task<SoldCreateViewModel> PrepareSoldCreateViewModelAsync(int cartId, string userId);
        Task<bool> CreateSoldAsync(SoldCreateViewModel viewModel);
        Task<Sold?> IncreaseQuantity(int id);
        Task<Sold?> DecreaseQuantity(int id);
    }
}
