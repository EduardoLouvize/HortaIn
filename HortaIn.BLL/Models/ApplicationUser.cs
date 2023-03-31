using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HortaIn.BLL.Models
{
    public class ApplicationUser : IdentityUser
    {        
        public ICollection<Post> Posts { get; set; }
        public ICollection<Image> Images { get; set; }
    }
}
