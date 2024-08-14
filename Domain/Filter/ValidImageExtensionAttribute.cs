using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Filter
{

    public class ValidImageExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _validExtensions;

        public ValidImageExtensionAttribute()
        {
            // Define all possible image extensions here
            _validExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".tiff", ".webp" };
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var fileName = value.ToString()!;
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            if (_validExtensions.Contains(fileExtension))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"Invalid file extension. Allowed extensions are: {string.Join(", ", _validExtensions)}");
        }
    }
}