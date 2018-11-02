using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SQ7MRU.FLLog.Requests;
using SQ7MRU.Utils;
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
            {
                BaseAddress = new Uri(this.config.BaseUrl),
                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", GenerateToken()) }
            })
            {
                string action = $"restricted/stations/{config.StationId}/check_dup";
                try
                {
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
                }
                catch { }
                return false;
            }
        }

        /// <summary>
        /// Get ADIF Record
        /// POST:stations/{stationId}/get_record
        /// </summary>
        /// <param name="callSign"></param>
        /// <returns></returns>
        public string GetRecord(string callSign)
        {
            string response = "NO_RECORD";

            using (HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new Uri(this.config.BaseUrl),
                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", GenerateToken()) }
            })
            {
                string action = $"restricted/stations/{config.StationId}/get_record";

                try
                {
                    Task.Run(async () =>
                    {
                        HttpResponseMessage responseMessage = await httpClient.PostAsync(action, new StringContent($"\"{callSign}\"", Encoding.UTF8, "application/json"));

                        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            AdifRow record = JsonConvert.DeserializeObject<AdifRow>(responseMessage.Content.ReadAsStringAsync().Result);
                            response = AdifHelper.ConvertToString(record);
                        }
                        else
                        {
                            throw new Exception($"{Path.Combine(httpClient.BaseAddress.AbsoluteUri, action)} returned {responseMessage.StatusCode}");
                        }
                    }).GetAwaiter().GetResult();
                }
                catch { }
            }
            return response;
        }

        /// <summary>
        /// Insert QSO
        /// /restricted/stations/{stationId}/insert/adif/{minutesAccept}
        /// </summary>
        /// <param name="record"></param>
        public void AddRecord(string record)
        {
            using (HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new Uri(this.config.BaseUrl),
                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", GenerateToken()) }
            })
            {
                string action = $"restricted/stations/{config.StationId}/insert/adif/10";

                Task.Run(async () =>
                {
                    var response = await httpClient.PostAsync(action, new StringContent(record, Encoding.UTF8, "application/json"));
                }).GetAwaiter().GetResult();
            }
        }

        private string GenerateToken()
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken = new JwtSecurityToken(
                issuer: config.BaseUrl,
                claims: new[] {
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) },
                expires: DateTime.Now.AddHours(1),
                audience: config.BaseUrl,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.JwtSecretKey)), SecurityAlgorithms.HmacSha256Signature));

            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}