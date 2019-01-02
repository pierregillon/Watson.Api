using System;
using System.Net;
using System.Threading.Tasks;

namespace fakenewsisor.server.Infrastructure
{
    public class HttpWebRequestChecker : IWebSiteChecker
    {
        public async Task<bool> IsOnline(string url)
        {
            var uri = new System.Uri(url);
            if (uri.IsAbsoluteUri == false)
                throw new Exception("The url must be absolute.");

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                var response = (HttpWebResponse)await request.GetResponseAsync();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (WebException)
            {
                return false;
            }
        }
    }
}