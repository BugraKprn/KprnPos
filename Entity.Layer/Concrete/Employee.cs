using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Layer.Concrete
{
    public class Employee : IdentityUser<int>
    {
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public string? Password { get; set; }
        public bool? Status { get; set; }
        public string? ImageUrl { get; set; }

    }
}
