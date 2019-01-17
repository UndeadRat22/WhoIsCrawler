using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.IO;
using RestSharp;
using WhoIsCrawler.Infrastructure.Abstract;
using WhoIsCrawler.Models;

namespace WhoIsCrawler.Services
{
    public class Crawler
    {
        private WhoIsDataParser _parser;
        private WhoIsClient _client;
        private ILogger _failLogger;
        private ILogger _outputLogger;
        private ILogger _successLogger;

        public Crawler(WhoIsDataParser parser, WhoIsClient client, ILogger failLogger, ILogger outputLogger, ILogger successLogger)
        {
            _parser = parser;
            _client = client;
            _failLogger = failLogger;
            _outputLogger = outputLogger;
            _successLogger = successLogger;
        }

        public void Run()
        {
            _outputLogger.Log("[");
            List<RawInfoJsonModel> infoList = new List<RawInfoJsonModel>();
            Stopwatch sw = new Stopwatch();
            foreach (var name in GetDomainNames())
            {
                sw.Restart();
                var queryResult = DoQuery(name);
                if (queryResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var info = _parser.GetInfo(name, queryResult.Content);
                    if (info == null)
                    {
                        _failLogger.Log($"{name}");
                        continue;
                    }
                    infoList.Add(info);
                    sw.Stop();
                    _successLogger.Log($"Got info for: {name} Elapsed: {sw.Elapsed}");
                }
                else
                {
                    _failLogger.Log($"{name}");
                }
            }
            _outputLogger.Log(infoList, ',');
            _outputLogger.Log("]");
        }

        public IRestResponse<string> DoQuery(string domain)
        {
            if (Configuration.Current.ProxyAddress == "")
                return _client.QueryAsync(domain).Result;
            
            return _client.QueryWithProxyAsync(domain,
                Configuration.Current.ProxyAddress,
                Configuration.Current.ProxyUsername,
                Configuration.Current.ProxyPassword)
                .Result;
        }

        private static IEnumerable<string> GetDomainNames()
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(Configuration.Current.InputFileName);
            }
            catch
            {
                Console.WriteLine($"Failed to open file:{Configuration.Current.InputFileName}");
            }
            if (reader != null)
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
