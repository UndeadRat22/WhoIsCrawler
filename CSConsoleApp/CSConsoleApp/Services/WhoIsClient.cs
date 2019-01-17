using System.Net;
using System.Threading.Tasks;
using RestSharp;
using WhoIsCrawler.Models;

namespace WhoIsCrawler.Services
{
    public class WhoIsClient
    {
        public async Task<IRestResponse<string>> QueryAsync(string domain)
        {
            var client = new RestClient(Config.Current.WhoIsDomain);
            var request = new RestRequest($"whois/{domain}", Method.GET);
            var resp = client.ExecuteTaskAsync<string>(request);
            return await resp;
        }
        public async Task<IRestResponse<string>> QueryWithProxyAsync(string domain, string proxy, string user, string pass)
        {
            var webproxy = new WebProxy(proxy)
            {
                Credentials = new NetworkCredential(user, pass)
            };

            var client = new RestClient(Config.Current.WhoIsDomain)
            {
                Timeout = Config.Current.Timeout,
                Proxy = webproxy
            };
            var request = new RestRequest($"whois/{domain}", Method.GET);
            var resp = client.ExecuteTaskAsync<string>(request);
            return await resp;
        }
    }
}
