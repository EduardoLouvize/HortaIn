using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HortaIn.BLL.Models
{
    public class UserDetails
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

        //public virtual string ApplicationUserId { get; set; }
    }
}
