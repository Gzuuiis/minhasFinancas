using System;
using BC = BCrypt.Net.BCrypt;

namespace crud.services
{
    internal class hashingService
    {

        public string setHashPassword(string str_hash)
        {
            return BC.HashPassword(str_hash);  
        }

        public bool verificarHashPassword(string str_senha, string str_hash)
        {
            return BC.Verify(str_senha, str_hash);
        }

    }
}
