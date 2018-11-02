using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SQ7MRU.FLLog.Requests;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SQ7MRU.FLLog
{
    public class QsoCollectorClient
    {
        private readonly Config config;

        public QsoCollectorClient(IConfiguration configuration)
        {
            config = configuration?.GetSection("QsoCollectorClient")?.Get<Config>();
        }

        /// <summary>
        /// Check QSO Duplicates
        /// POST:stations/{stationId}/check_dup
        /// </summary>
        /// <param name="checkDupRequest"></param>
        /// <returns></returns>
        public bool CheckDup(CheckDupRequest checkDupRequest)
        {
            using (HttpClient httpClient = new HttpClient()
            {   BaseAddress = new Uri(this.config.BaseUrl),
                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", GenerateToken()) } })
            {
                string action = $"restricted/stations/{config.StationId}/check_dup";
                Task.Run(async () =>
                {
                    HttpResponseMessage responseMessage = await httpClient.PostAsync(action, new StringContent(JsonConvert.SerializeObject(checkDupRequest), Encoding.UTF8, "application/json"));

                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<bool>(responseMessage.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        throw new Exception($"{Path.Combine(httpClient.BaseAddress.AbsoluteUri, action)} returned {responseMessage.StatusCode}");
                    }
                }).GetAwaiter().GetResult();
                return false;
            }
        }

        private string GenerateToken()
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken = new JwtSecurityToken(
                issuer: config.BaseUrl,
                claims: new[] {
                  new Claim(JwtRegisteredClaimNames.Website, config.BaseUrl),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) },
                expires: DateTime.Now.AddHours(1),
                audience: config.BaseUrl,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.JwtSecretKey)), SecurityAlgorithms.HmacSha256Signature));

            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}