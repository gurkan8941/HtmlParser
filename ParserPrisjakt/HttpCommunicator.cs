using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParserPrisjakt
{
    public class HttpCommunicator
    {
        private int _timeoutMilliseconds = 1000;

        public HttpCommunicator(int timeoutMilliseconds)
        {
            _timeoutMilliseconds = timeoutMilliseconds;
        }

        private void SetHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:52.0) Gecko/20100101 Firefox/52.0");
            request.Headers.Add("Host", "feetfirst.se");
        }

        public string GetWebPage(string url, Encoding encoding)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = new TimeSpan(0,0,0,0,_timeoutMilliseconds);

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                SetHeaders(request);

                var response = httpClient.SendAsync(request).Result;
                //var response = httpClient.GetAsync(url).Result;
                var bytes = response.Content.ReadAsByteArrayAsync().Result;

                var htmlResponse = encoding.GetString(bytes);

                return htmlResponse;
            }
        }
    }
}
