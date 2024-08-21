using Domain.Entites;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions.Interfaces.IServises
{
    public interface ICategoryService
    {
        Task<CategoryDTO> UploadCategory(UploadCategoryDTO model);
        Task<CategoryDTO> UpdateCategory(int id, UploadCategoryDTO model);
        Task<Category?> ToggleDelete(int id);
        Task<CategoryDTO?> GetById(int id);
        Task<IEnumerable<CategoryDTO>> GetAll();
    }
}
