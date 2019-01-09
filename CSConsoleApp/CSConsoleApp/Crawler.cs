using System.Diagnostics;

using System;
using System.Collections.Generic;
using System.IO;

namespace Crawler
{
    public class Crawler
    {
        private static string _inputFileName = @"c:\domains.txt";
        private static string _outputFileName = @"c:\output.txt";

        private WhoIsDataParser _parser;
        private WhoIsClient _client;

        public Crawler(WhoIsDataParser parser, WhoIsClient client)
        {
            _parser = parser;
            _client = client;
        }

        public void Run()
        {
            Stopwatch sw = new Stopwatch();
            foreach (var name in GetDomainNames())
            {
                sw.Restart();
                var queryResult = DoQuery(name);
                var info = _parser.ParseHtml(queryResult);
                //TODO: write info to some stream.
                sw.Stop();
                Console.WriteLine($"Time elapsed to get info for {name}: {sw.Elapsed}");
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
