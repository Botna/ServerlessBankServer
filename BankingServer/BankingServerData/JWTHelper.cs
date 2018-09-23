using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BankingServerData
{
    public class JWTHelper : IJWTHelper
    {
        //Possibly very borken re-implementation of JWT nuget pacakge.  It doesnt like being deployed on AWS as i suspect its not .netcore compatable, and the System.IdentityModel.Tokens stuff
        //was being very difficult.  This gets the poitn across that "we need authentication of some kind" and will probably not break.
        string notSoSecretKey = "PleaseGiveMeAJob";
        public string buildToken(string userName)
        {
            var header = "{\"typ\":\"JWT\",\"alg\":\"HS256\"}";
            var claims = new Dictionary<string, string>();
            claims.Add("userName", userName);

            var b64header = Convert.ToBase64String(Encoding.UTF8.GetBytes(header))
            .Replace('+', '-')
            .Replace('/', '_');
            var b64claims = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(claims)))
            .Replace('+', '-')
            .Replace('/', '_');

            var payload = b64header + "." + b64claims;
            Console.WriteLine("JWT without sig:    " + payload);

            byte[] key = Convert.FromBase64String(notSoSecretKey);
            byte[] message = Encoding.UTF8.GetBytes(payload);

            string sig = Convert.ToBase64String(HashHMAC(key, message))
            .Replace('+', '-')
            .Replace('/', '_');

            Console.WriteLine("JWT with signature: " + payload + "." + sig);
            //IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            //IJsonSerializer serializer = new JsonNetSerializer();
            //IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            //IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            //var token = encoder.Encode(myMap, notSoSecretSecret);
            //return token;
            return payload + '.' + sig;
        }

        public string decodeToken(string myToken)
        {
            string[] split = myToken.Split('.');

            var payload = split[0] + "." + split[1];
            var revertedHeader = split[0].Replace('-', '+')
            .Replace('_', '/');

            var revertedClaims = split[1].Replace('-', '+')
            .Replace('_', '/');

            byte[] message = Encoding.UTF8.GetBytes(payload);
            byte[] key = Convert.FromBase64String(notSoSecretKey);
            string sig = Convert.ToBase64String(HashHMAC(key, message))
            .Replace('+', '-')
            .Replace('/', '_');

            if (sig != split[2])
            {
                throw new Exception("Token signature wasnt valid");
            }

            byte[] data = Convert.FromBase64String(revertedClaims);
            string decodedString = Encoding.UTF8.GetString(data);

            Dictionary<string, string> claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedString);
            //IJsonSerializer serializer = new JsonNetSerializer();
            //IDateTimeProvider provider = new UtcDateTimeProvider();
            //IJwtValidator validator = new JwtValidator(serializer, provider);
            //IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            //IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
            //var json = decoder.Decode(myToken, notSoSecretSecret, verify: true);
            //return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return claims["userName"];

        }
        private static byte[] HashHMAC(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }
    }
}
