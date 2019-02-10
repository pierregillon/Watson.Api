using System;
using System.Net;
using System.Threading.Tasks;
using Watson.Domain.ReportSuspiciousFact;

namespace Watson.Infrastructure
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
                var request = WebRequest.Create(uri);
                var response = await request.GetResponseAsync();
                if(response is HttpWebResponse) {
                    return ((HttpWebResponse)response).StatusCode == HttpStatusCode.OK;
                }
                else if(response is FileWebResponse) {
                    return true;
                }
                throw new Exception("Unknown web response");
            }
            catch (WebException)
            {
                return false;
            }
        }
    }
}