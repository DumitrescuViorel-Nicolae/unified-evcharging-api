using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserUpdateResponse
    {
        public UserDTO OldUserDetails { get; set; }
        public UserDTO NewUserDetails { get; set;}
    }
}
