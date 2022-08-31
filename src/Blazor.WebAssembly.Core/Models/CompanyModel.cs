using System.ComponentModel.DataAnnotations;

namespace Blazor.WebAssembly.Core.Models
{
    public class CompanyModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "{0} must have between {1} and {2} characters", MinimumLength = 3)]
        public string TradeName { get; set; }
        public byte[]? Logo { get; set; }
        public virtual List<AddressModel>? Addresses { get; set; }
    }
}
