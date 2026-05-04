using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")] // Validation
        public required string Name { get; set; }

        [EmailAddress(ErrorMessage = "Email invalide")] // Validation
        public required string Email { get; set; }
    }
}
