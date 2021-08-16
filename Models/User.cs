using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogRegJune
{
    public class User
    {
        [Key]
        public int UserId {get; set;}
        [Required]
        public string Name {get; set;}
        [Required]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "PW must b 8 characters!")]
        public string Password {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPW {get; set;}

    }
}