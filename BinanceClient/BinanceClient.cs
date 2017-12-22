using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BinanceAPI;
using BinanceClient;


namespace BinanceClient
{

    public class BinanceClient : IBinanceClient
    {
        private readonly HttpClient _httpClient;
        private string url = "https://www.binance.com/api/";
        private string key;
        private string secret;

        public BinanceClient(string apiKey, string apiSecret)
        {
            key = apiKey;
            secret = apiSecret;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(url)

            };
            _httpClient.DefaultRequestHeaders
                .Add("X-MBX-APIKEY", key);

            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET
        public async Task<T> GetAsync<T>(string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?{args}");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString());

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        //SIGNED GET
        public async Task<T> GetSignedAsync<T>(string endpoint, string args = null)
        {
            string headers = _httpClient.DefaultRequestHeaders.ToString();
            string timestamp = GetTimestamp();
            args += "&timestamp=" + timestamp;

            var signature = args.CreateSignature(secret);
            var response = await _httpClient.GetAsync($"{endpoint}?{args}&signature={signature}");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString());

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        //SIGNED POST
        public async Task<T> PostSignedAsync<T>(string endpoint, string args = null)
        {
            string headers = _httpClient.DefaultRequestHeaders.ToString();
            string timestamp = GetTimestamp();
            args += "&timestamp=" + timestamp;


            var signature = args.CreateSignature(secret);
            var response = await _httpClient.PostAsync($"{endpoint}?{args}&signature={signature}", null);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString());

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        //SIGNED DELETE
        public async Task<T> DeleteSignedAsync<T>(string endpoint, string args = null)
        {
            string headers = _httpClient.DefaultRequestHeaders.ToString();
            string timestamp = GetTimestamp();
            args += "&timestamp=" + timestamp;

            var signature = args.CreateSignature(secret);
            var response = await _httpClient.DeleteAsync($"{endpoint}?{args}&signature={signature}");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString());

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        //Timestamp for signature
        private static string GetTimestamp()
        {
            long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return milliseconds.ToString();
        }

        //Not in use
        public static string QueryString(IDictionary<string, object> dict)
        {
            var list = new List<string>();
            foreach (var item in dict)
            {
                list.Add(item.Key + "=" + item.Value);
            }
            return string.Join("&", list);
        }


       


    }
}

