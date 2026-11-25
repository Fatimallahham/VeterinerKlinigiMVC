using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VeterinerKlinigiMVC.Models
{
    public class Pet : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hayvan adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Hayvan adı en fazla 50 karakter olabilir.")]
        [Display(Name = "Hayvan Adı")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hayvan türü zorunludur.")]
        [StringLength(50, ErrorMessage = "Hayvan türü en fazla 50 karakter olabilir.")]
        [Display(Name = "Tür")]
        public string Species { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hayvan ırkı zorunludur.")]
        [StringLength(100, ErrorMessage = "Hayvan ırkı en fazla 100 karakter olabilir.")]
        [Display(Name = "Irk")]
        public string Breed { get; set; } = string.Empty;

        [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime BirthDate { get; set; }

        [ValidateNever]
        public string OwnerId { get; set; } = string.Empty;

        [ValidateNever]
        public ApplicationUser Owner { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (BirthDate == default)
            {
                yield return new ValidationResult(
                    "Geçerli bir doğum tarihi giriniz.",
                    new[] { nameof(BirthDate) });
            }
            else if (BirthDate.Date > DateTime.Today)
            {
                yield return new ValidationResult(
                    "Doğum tarihi gelecekte olamaz.",
                    new[] { nameof(BirthDate) });
            }
        }
    }
}