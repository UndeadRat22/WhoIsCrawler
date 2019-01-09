using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Crawler
{
    public class Crawler
    {
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
            using (StreamWriter file = File.CreateText(Program.Settings.OutputFileName))
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
                reader = new StreamReader(Program.Settings.InputFileName);
            }
            catch
            {
                Console.WriteLine($"Failed to open file:{Program.Settings.InputFileName}");
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
