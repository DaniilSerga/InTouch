using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTouch.Models.DbModels
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        // Full name
        [Required]
        public string? FullName { get; set; }

        // E-mail
        [Required]
        public string? Email { get; set; }

        // Position
        [Required]
        public string? Position { get; set; }
        // Password
        [Required]
        public string? Password { get; set; }
    }
}
