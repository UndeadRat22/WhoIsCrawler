using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RestSharp;
using WhoIsCrawler.Infrastructure.Abstract;

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
            List<DomainInformation> data = new List<DomainInformation>();
            Stopwatch sw = new Stopwatch();
            foreach (var name in GetDomainNames())
            {
                sw.Restart();
                var queryResult = DoQuery(name);
                if (queryResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DomainInformation info = null;
                    try
                    {
                        info = _parser.ParseHtml(queryResult.Content);
                    }
                    catch
                    {
                        _failLogger.Log($"{name}");
                        continue;
                    }
                    if (info == null)
                        continue;
                    data.Add(info);
                    sw.Stop();
                    _successLogger.Log($"Got info for: {name} Elapsed: {sw.Elapsed}");
                }
                else
                {
                    _failLogger.Log($"{name}");
                }
            }
            WriteToFile(data);
        }

        private void WriteToFile(List<DomainInformation> data)
        {
            using (StreamWriter file = File.CreateText(Configuration.Current.OutputFileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }
        }

        public IRestResponse<string> DoQuery(string domain)
        {
            var resp = _client.QueryWithProxyAsync(domain,
                Configuration.Current.ProxyAddress,
                Configuration.Current.ProxyUsername,
                Configuration.Current.ProxyPassword);
                
            return resp.Result;
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
