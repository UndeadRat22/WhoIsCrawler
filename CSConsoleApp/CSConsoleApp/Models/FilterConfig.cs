using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsCrawler.Models
{
    public class FilterConfig
    {
        public static FilterConfig Current;
        public Dictionary<string, string[]> Filters { get; set; }
        public List<string> Ignore { get; set; }
    }
}
