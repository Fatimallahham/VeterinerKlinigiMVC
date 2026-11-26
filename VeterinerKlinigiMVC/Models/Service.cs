using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinerKlinigiMVC.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [StringLength(100)]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Hizmet süresi zorunludur.")]
        [Range(1, 480, ErrorMessage = "Hizmet süresi 1-480 dakika arasında olmalıdır.")]
        [Display(Name = "Süre (Dakika)")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Hizmet ücreti zorunludur.")]
        [Range(0, 100000, ErrorMessage = "Geçerli bir ücret giriniz.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Ücret (TL)")]
        public decimal Price { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } = true;
    }
}