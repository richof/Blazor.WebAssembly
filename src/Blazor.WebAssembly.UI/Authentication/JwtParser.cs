﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.Json;

namespace Blazor.WebAssembly.UI.Authentication
{
    public class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            string payload = (jwt.Split('.')[1]).ToString();
            ///payload = payload.Replace("'", "\"").Replace("'", "\"");
            var jsonBytes=ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            ExtractRolesFromJWT(claims, keyValuePairs);
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }
        private static void ExtractRolesFromJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
        {
            const string roleClaimType = "role";
           
            keyValuePairs.TryGetValue(roleClaimType, out object roles);
            if (roles is not null)
            {
                var parsedRoles = roles.ToString().Trim().TrimStart('[').TrimEnd(']').Split(',');
                if (parsedRoles.Length > 1)
                {
                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));
                    }
                }
                else
                {
                    claims.Add(new Claim(roleClaimType, parsedRoles[0]));
                }
                keyValuePairs.Remove(roleClaimType);
            }
        }
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
                default:
                    break;
            }
            return Convert.FromBase64String(base64); //'rGEpOtO8uk1oeh3vNMrO37YlpaDak-lMkkt3ZRfFleA='
        }
    }
}
