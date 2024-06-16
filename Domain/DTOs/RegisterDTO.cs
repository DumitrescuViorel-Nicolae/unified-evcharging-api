using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class RegisterDTO
    {
        public UserDTO UserDetails { get; set; }

        [Required(AllowEmptyStrings = true)]
        public CompanyDTO? CompanyDetails { get; set; }
    }
}
