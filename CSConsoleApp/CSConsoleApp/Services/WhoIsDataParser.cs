using HtmlAgilityPack;

using WhoIsCrawler.Extensions;

using System.Linq;
using WhoIsCrawler.Models;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace WhoIsCrawler.Services
{
    public class WhoIsDataParser
    {
        private const string RawInfoModelPattern = @"([A-Z][a-zA-Z ]+):\s+([A-Za-z0-9-.:+@ ,]*)";
        public RawInfoJsonModel GetInfo(string domain, string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            try
            {
                var raw = doc.DocumentNode.SelectSingleNodeByClass("df-raw");
                if (raw == null)
                    return null;
                var info = new RawInfoJsonModel()
                {
                    Domain = domain
                };
                foreach (var line in raw.InnerText.Split("\n"))
                {
                    Match match = Regex.Match(line, RawInfoModelPattern);
                    info.AddText(match);
                }
                return info;
            }
            catch
            {
                return null;
            }
        }
    }
}
