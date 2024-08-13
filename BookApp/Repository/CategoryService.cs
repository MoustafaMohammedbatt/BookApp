using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Shared.DTOs;
using Service.Abstractions.Interfaces.IRepositories;
using Domain.Entites;
using Service.Abstractions.Interfaces.IServises;

namespace BookApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string[] _allowedFileExtensions = { ".jpg", ".jpeg", ".png" };
        private const long _maxAllowedSizeFile = 5 * 1024 * 1024; // 5 MB

        public CategoryService(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDTO> UploadCategory(UploadCategoryDTO model)
        {
            var extension = Path.GetExtension(model.CoverImage.FileName);

            if (!_allowedFileExtensions.Contains(extension))
                return new CategoryDTO { Notes = "Only .jpg, .jpeg, .png files are allowed!" };

            if (model.CoverImage.Length > _maxAllowedSizeFile)
                return new CategoryDTO { Notes = "File cannot be more than 5 MB!" };

            var fileName = $"{Guid.NewGuid()}{extension}";

            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/category", fileName);
            using var stream = File.Create(path);
            model.CoverImage.CopyTo(stream);

            var category = new Category
            {
                Name = model.Name,
                Description = model.Description,
                CoverImage = fileName
            };

            await _unitOfWork.Categories.Add(category);
            _unitOfWork.Complete();

            return new CategoryDTO { Id = category.Id, Name = category.Name, Description = category.Description, CoverImage = category.CoverImage };
        }

        public async Task<CategoryDTO> UpdateCategory(int id, UploadCategoryDTO model)
        {
            var category = await _unitOfWork.Categories.GetById(id);

            if (category == null)
                return null;

            if (model.CoverImage != null)
            {
                var extension = Path.GetExtension(model.CoverImage.FileName);

                if (!_allowedFileExtensions.Contains(extension))
                    return new CategoryDTO { Notes = "Only .jpg, .jpeg, .png files are allowed!" };

                if (model.CoverImage.Length > _maxAllowedSizeFile)
                    return new CategoryDTO { Notes = "File cannot be more than 5 MB!" };

                var fileName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/category", fileName);
                using var stream = File.Create(path);
                model.CoverImage.CopyTo(stream);

                category.CoverImage = fileName;
            }

            category.Name = model.Name;
            category.Description = model.Description;

            _unitOfWork.Categories.Update(category);
            _unitOfWork.Complete();

            return new CategoryDTO { Id = category.Id, Name = category.Name, Description = category.Description, CoverImage = category.CoverImage };
        }
    }
}
