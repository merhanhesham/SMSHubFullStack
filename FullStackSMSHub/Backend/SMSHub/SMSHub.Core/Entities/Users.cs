using SMSHub.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSHub.Core.Entities
{
    public class Users 
    {
            public string Id { get; set; }
            
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string DisplayName { get; set; }
            [Required]
            [Phone]
            public string PhoneNumber { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The password must be at least 8 characters long.", MinimumLength = 8)]
            [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@#$%^&+=]).{8,}$", ErrorMessage = "The password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
            //public string Password { get; set; }

            public string Role { get; set; }
            public ICollection<SenderId> SenderIds { get; set; }
    }
}
