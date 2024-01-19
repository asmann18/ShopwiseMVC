﻿using System.ComponentModel.DataAnnotations;

namespace Shopwise.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
