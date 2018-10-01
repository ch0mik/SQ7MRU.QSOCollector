using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQ7MRU.QSOCollector.Services.EQSL
{
    public class Client
    {
        private readonly Config _config;
        private ILogger<Client> _logger;
        private readonly CookieContainer _container;
        private readonly string patternAlphaNumeric = "[^a-zA-ZæÆøØåÅéÉöÖäÄüÜ-ñÑõÕéÉáÁóÓôÔzżźćńółęąśŻŹĆĄŚĘŁÓŃ _]";
        private readonly Uri baseAddress = new Uri("http://eqsl.cc/qslcard/");

        public Client(Config config)
        {
            _config = config;
            _container = new CookieContainer();
            //Task.Run(async () =>
            //{
            //    await LogonAsync();
            //});
        }

        private async Task LogonAsync()
        {
            string action = "LoginFinish.cfm";
            using (var handler = new HttpClientHandler() { CookieContainer = _container })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Callsign", _config.Callsign),
                    new KeyValuePair<string, string>("EnteredPassword", _config.Password),
                });
                var result = client.PostAsync(action, content).Result;
                result.EnsureSuccessStatusCode();
                string response = await result.Content.ReadAsStringAsync();

                if (response.Contains("Callsign or Password Error!"))
                {
                    throw new Exception("Callsign or Password Error!");
                }

                if (response.Contains(@">Select one<"))
                {
                    string[] QTHNicknamesArray = Regex.Split(response, @"NAME=""HamID"" VALUE=""(.*)""").Where(S => S.Length < 50).ToArray();

                    foreach (var hamId in QTHNicknamesArray)
                    {
                        content = new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("HamID", hamId),
                        new KeyValuePair<string, string>("Callsign", _config.Callsign),
                        new KeyValuePair<string, string>("EnteredPassword", _config.Password),
                        new KeyValuePair<string, string>("SelectCallsign","Log+In")
                        });

                        result = client.PostAsync(action, content).Result;
                        result.EnsureSuccessStatusCode();
                        response = await result.Content.ReadAsStringAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Logon to eQSL.cc
        /// </summary>
        /// <param name="callSign"></param>
        /// <param name="hamID"></param>
        public void Logon(string callSign, string hamID = null)
        {
            string action = "LoginFinish.cfm";
            using (var handler = new HttpClientHandler() { CookieContainer = this._container })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                FormUrlEncodedContent content;

                if (!string.IsNullOrEmpty(hamID))
                {
                    content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("HamID", hamID),
                        new KeyValuePair<string, string>("Callsign", _config.Callsign),
                        new KeyValuePair<string, string>("EnteredPassword", _config.Password),
                        new KeyValuePair<string, string>("SelectCallsign","Log+In")
                    });
                }
                else
                {
                    content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("Callsign", _config.Callsign),
                        new KeyValuePair<string, string>("EnteredPassword", _config.Password),
                        new KeyValuePair<string, string>("Login", "Go")
                    });
                }
                var result = client.PostAsync(action, content).Result;
                result.EnsureSuccessStatusCode();
            }
        }

        public bool UploadAdif(string adif)
        {
            bool result = false;
            string action = "UploadFile.cfm";
            using (var handler = new HttpClientHandler() { CookieContainer = _container })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {
                    var values = new[]
                       {
                            new KeyValuePair<string, string>("AsyncMode", "TRUE")
                       };

                    foreach (var keyValuePair in values)
                    {
                        content.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                    }

                    var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(adif));
                    var streamContent = new StreamContent(fileStream);
                    streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                    streamContent.Headers.ContentDisposition.Name = "\"Filename\"";
                    streamContent.Headers.ContentDisposition.FileName = "\"" + "UPLOAD.ADIF" + "\"";
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    string boundary = new Guid().ToString();
                    var fContent = new MultipartFormDataContent(boundary);
                    fContent.Headers.Remove("Content-Type");
                    fContent.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);
                    fContent.Add(streamContent);
                    var response = client.PostAsync(action, fContent).Result;
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        result = true;
                    }
                    return result;
                }
            }
        }
    }
}