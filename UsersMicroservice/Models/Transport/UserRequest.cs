using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UsersMicroservice.Models
{
    public class UserRequest
    {
        [Required]
        public string Username { get; set; }
    }
}
