using System;
using System.Collections.Generic;
using System.IO;

namespace Crawler
{
    public class Crawler
    {
        private static string _inputFileName = @"c:\domains.txt";
        private static string _outputFileName = @"c:\output.txt";
        public void Run()
        {
            WhoIsDataParser parser = new WhoIsDataParser();
            foreach (var name in GetDomainNames())
            {
                var result = DoQuery(name);
                parser.ParseHtml(result);
            }
        }

        public string DoQuery(string domain)
        {
            var resp = new WhoIsClient().QueryAsync(domain);
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
