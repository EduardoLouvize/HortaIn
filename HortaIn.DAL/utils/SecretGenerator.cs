using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HortaIn.DAL.utils
{
    public class SecretGenerator
    {
  static public string Generate(){
    string allowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    Random random = new Random();
    char[] senha = new char[200];
    for (int i = 0; i < 200; i++)
    {
        senha[i] = allowedCharacters[random.Next(allowedCharacters.Length)];
    }

        return new string(senha);

        }
    }
}