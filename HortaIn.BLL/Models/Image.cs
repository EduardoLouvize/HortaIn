using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HortaIn.BLL.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Uri { get; set; }

        public string? ApplicationUserId { get; set; }
        [NotMapped]
        public IFormFile? file { get; set; } = null;

    }
}
