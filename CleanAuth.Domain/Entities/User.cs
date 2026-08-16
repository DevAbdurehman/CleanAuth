using CleanAuth.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAuth.Domain.Entities
{
    public class User: BaseEntity
    {
        [Required]
        public string FirstName { get; set; }= string.Empty;
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; }= string.Empty;
        public bool IsEmailVerified { get; set; } = false;
        public bool  IsActive { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    = new List<RefreshToken>();
    }
}
