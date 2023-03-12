using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HortaIn.BLL.Models
{
    public class PasswordRecovery
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public bool Used { get; set; }
        public string ? Secret { get; set; }

    }
}