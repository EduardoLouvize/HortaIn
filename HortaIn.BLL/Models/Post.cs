using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HortaIn.BLL.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Conteudo { get; set; }

        public string? ApplicationUserId { get; set; }

    }
}
