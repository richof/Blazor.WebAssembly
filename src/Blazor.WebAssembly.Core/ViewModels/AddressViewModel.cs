using System.ComponentModel.DataAnnotations;

namespace Blazor.WebAssembly.Core.ViewModels
{
    public class AddressViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "{0} must have between {1} and {2} characters", MinimumLength = 3)]
        public string StreetName { get; set; }
        [Required]
        [StringLength(5, ErrorMessage = "{0} must have between {1} and {2} characters", MinimumLength = 1)]
        public string Number { get; set; }
        public Guid CompanyId { get; set; }
    }
}
