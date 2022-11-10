using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTouch.Models.DbModels
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        // ФИО
        [Required]
        public string? FullName { get; set; }

        // E-mail
        [Required]
        public string? Email { get; set; }

        // Password
        [Required]
        public string? Password { get; set; }

        // Avatar image
        public byte[]? Avatar { get; set; }
    }
}
