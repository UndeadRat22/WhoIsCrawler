using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Crawler
{
    public class Crawler
    {
        private static string _inputFileName = @"c:\domains.txt";
        private static string _outputFileName = @"d:\output.json";

        private WhoIsDataParser _parser;
        private WhoIsClient _client;

        public Crawler(WhoIsDataParser parser, WhoIsClient client)
        {
            _parser = parser;
            _client = client;
        }

        public void Run()
        {
            List<DomainInformation> data = new List<DomainInformation>();
            Stopwatch sw = new Stopwatch();
            foreach (var name in GetDomainNames())
            {
                sw.Restart();
                var queryResult = DoQuery(name);
                var info = _parser.ParseHtml(queryResult);
                data.Add(info);
                sw.Stop();
                Console.WriteLine($"Time elapsed to get info for {name}: {sw.Elapsed}");
            }
            WriteToFile(data);
        }

        private void WriteToFile(List<DomainInformation> data)
        {
            using (StreamWriter file = File.CreateText(_outputFileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }
        }

        public string DoQuery(string domain)
        {
            var resp = _client.QueryAsync(domain);
            var result = resp.Result;
            return result.Content;
        }

        private static IEnumerable<string> GetDomainNames()
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(_inputFileName);
            }
            catch
            {
                Console.WriteLine($"Failed to open file:{_inputFileName}");
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
