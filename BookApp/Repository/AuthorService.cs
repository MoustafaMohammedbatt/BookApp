using Domain.Entites;
using Service.Abstractions.Interfaces.IRepositories;
using Service.Abstractions.Interfaces.IServises;
using Shared.DTOs;

namespace BookApp.Repository
{
    public class AuthorService : IAuthorService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string[] _allowedFileExtensions = { ".jpg", ".jpeg", ".png" };
        private const long _maxAllowedSizeFile = 5 * 1024 * 1024; // 5 MB

        public AuthorService(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }
        public async Task<Author?> ToggleDelete(int id)
        {
            var author = await _unitOfWork.Authors.GetById(id);

            if (author is null) return null;

            author.IsDeleted = !author.IsDeleted;

            _unitOfWork.Complete();

            return author;
        }

        public async Task<AuthorDTO> UpdateAuthor(int id, UploadAuthorDTO model)
        {
            var author = await _unitOfWork.Authors.GetById(id);

            if (author == null)
                return null;

            if (model.CoverImage != null)
            {
                var extension = Path.GetExtension(model.CoverImage.FileName);

                if (!_allowedFileExtensions.Contains(extension))
                    return new AuthorDTO { Notes = "Only .jpg, .jpeg, .png files are allowed!" };

                if (model.CoverImage.Length > _maxAllowedSizeFile)
                    return new AuthorDTO { Notes = "File cannot be more than 5 MB!" };

                var fileName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/author", fileName);
                using var stream = File.Create(path);
                model.CoverImage.CopyTo(stream);

                author.CoverImage = fileName;
            }

            author.FullName = model.FullName;
            author.Bio = model.Bio;

            _unitOfWork.Authors.Update(author);
            _unitOfWork.Complete();

            return new AuthorDTO { Id = author.Id, FullName = author.FullName, Bio = author.Bio, CoverImage = author.CoverImage };

        }

        public async Task<AuthorDTO> UploadAuthor(UploadAuthorDTO model)
        {
            var extension = Path.GetExtension(model.CoverImage.FileName);

            if (!_allowedFileExtensions.Contains(extension))
                return new AuthorDTO { Notes = "Only .jpg, .jpeg, .png files are allowed!" };

            if (model.CoverImage.Length > _maxAllowedSizeFile)
                return new AuthorDTO { Notes = "File cannot be more than 5 MB!" };

            var fileName = $"{Guid.NewGuid()}{extension}";

            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/author", fileName);
            using var stream = File.Create(path);
            model.CoverImage.CopyTo(stream);

            var author = new Author
            {
                
                FullName = model.FullName,
                Bio = model.Bio,
                CoverImage = fileName
            };

            await _unitOfWork.Authors.Add(author);
            _unitOfWork.Complete();

            return new AuthorDTO { Id = author.Id, FullName = author.FullName, Bio = author.Bio, CoverImage = author.CoverImage };

        }
    }
}
