using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DataLayer.DTO
{
    public class DTORegisterUser
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "invalid email")]
        [Remote(action: "IsEmailIsAlreadyRegistered", controller: "UserController", ErrorMessage = "Email is already registered before")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name Should be less than 100 char")]
        public string? PersonName { get; set; }

        [DataType(DataType.Password, ErrorMessage = "invalid password")]
        [Required(ErrorMessage = "password is required")]
        public string? Password { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "invalid phone number")]
        public string? PhoneNumber { get; set; }
    }
    
    public class DTOUserLogin
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "invalid email")]
        public string? Email { get; set; }
        [DataType(DataType.Password, ErrorMessage = "invalid password")]
        [Required(ErrorMessage = "password is required")]
        public string? Password { get; set; }
    }
}