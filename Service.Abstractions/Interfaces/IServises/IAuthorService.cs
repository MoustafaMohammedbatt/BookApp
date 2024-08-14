using Domain.Entites;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions.Interfaces.IServises
{
    public interface IAuthorService
    {
        Task<AuthorDTO> UploadAuthor(UploadAuthorDTO model);
        Task<AuthorDTO> UpdateAuthor(int id, UploadAuthorDTO model);
        Task<Author?> ToggleDelete(int id);
    }
}
