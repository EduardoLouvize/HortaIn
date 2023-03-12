using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HortaIn.DAL.Mailer.templates
{
    public class MakePasswordChangeTemplate
    {
        public static string make(string secret) {
            return $"<h1>Trocar Senha</h1> <p>Clique no link abaixo para redefinir sua senha.<br/> <a href='http://localhost:8080/Home/Change-Password/{secret}'>http://localhost:8080/Home/Change-Password/{secret}</a></p>";
        }
    }
}