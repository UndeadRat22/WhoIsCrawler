using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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
        private ILogger _successLogger;

        public Crawler(WhoIsDataParser parser, WhoIsClient client, ILogger failLogger, ILogger successLogger)
        {
            _parser = parser;
            _client = client;
            _failLogger = failLogger;
            _successLogger = successLogger;
        }

        public void Run()
        {
            var domainData = new List<DomainInformation>();
            var registrantData = new List<RegistrantInformation>();
            Stopwatch sw = new Stopwatch();
            foreach (var name in GetDomainNames())
            {
                sw.Restart();
                var queryResult = DoQuery(name);
                if (queryResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DomainInformation domainInfo = null;
                    RegistrantInformation registrantInfo = null;
                    try
                    {
                        registrantInfo = _parser.ParseRegistrantInfo(queryResult.Content);
                        domainInfo = _parser.ParseDomainInfo(queryResult.Content);
                    }
                    catch
                    {
                        _failLogger.Log($"{name}");
                        continue;
                    }
                    if (domainInfo != null)
                        domainData.Add(domainInfo);
                    if (registrantInfo != null)
                        registrantData.Add(registrantInfo);
                    sw.Stop();
                    _successLogger.Log($"Got info for: {name} Elapsed: {sw.Elapsed}");
                }
                else
                {
                    _failLogger.Log($"{name}");
                }
            }
            WriteToFile(domainData, Configuration.Current.OutputFileName);
            WriteToFile(registrantData, @"C:\Users\Public\registrants.json");
        }

        private void WriteToFile<T>(List<T> data, string filename)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }
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
