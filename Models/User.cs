using System;
using System.ComponentModel.DataAnnotations;

namespace Week5Lab.Models
{
    public class User
    {
        [Key]
         public int Id { get; set; }  // <-- BU ÇOK ÖNEMLİ
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
