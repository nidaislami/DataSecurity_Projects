using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server_TCP
{
    class Jwt
    {
        Shfrytezuesi m = new Shfrytezuesi();
        Shpenzimet n = new Shpenzimet();
        private static RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        private static RSAParameters privateKey = rsa.ExportParameters(true);

        public String createToken(String userId, String email, String emriMbiemri, Decimal paga, String profesioni)
        {
            var payload = new Dictionary<string, object>
        {
            { "userId", userId },
            { "email", email },
            {"emriMbiemri", emriMbiemri },
            {"paga", paga },
            {"profesioni", profesioni },
        };
            const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, privateKey.ToString());
            return token;
        }
        public String createToken(String userId, String Llojifatures, Decimal viti, Decimal paga, Decimal muaji,Decimal vleraneeuro)
        {
            var payload = new Dictionary<string, object>
        {
            { "userId", userId },
            { "Llojifatures", Llojifatures },
            {"viti", viti },
            {"paga", paga },
            {"muaji", muaji },
            {"vleraneeuoro", vleraneeuro },
        };
            const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, privateKey.ToString());
            return token;
        }



    }
}
