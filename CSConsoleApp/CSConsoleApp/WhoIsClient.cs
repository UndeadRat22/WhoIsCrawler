using System.Threading.Tasks;
using RestSharp;

namespace Crawler
{
    public class WhoIsClient
    {
        public async Task<IRestResponse<string>> QueryAsync(string domain)
        {
            var client = new RestClient("https://www.whois.com/");
            var request = new RestRequest($"whois/{domain}", Method.GET);
            var resp = client.ExecuteTaskAsync<string>(request);
            return await resp;
        }
    }
}
