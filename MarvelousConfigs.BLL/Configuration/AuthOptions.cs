using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MarvelousConfigs.BLL.Configuration
{
    public class AuthOptions
    {
        public const string Issuer = "MarvelousConfigsBack"; // издатель токена
        public const string Audience = "MarvelousConfigsFront"; // потребитель токена
        private const string _key = "mysupersecret_secretkey!123";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
    }
}
