using Chat.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Helper
{
    public class HTTP
    {
        public string HttpResultByURl(string url)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/json";
                string responseData = string.Empty;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseData = reader.ReadToEnd();
                        reader.Close();
                    }
                    response.Close();
                }
                return responseData;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CreateProductAsync(string url, object obj)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ChatConstants.HTTPURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.PostAsJsonAsync(
                url, obj);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.StatusCode.ToString();
        }
    }
}