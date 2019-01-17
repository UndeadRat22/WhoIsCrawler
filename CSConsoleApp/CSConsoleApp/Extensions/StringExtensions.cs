using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WhoIsCrawler.Models;

namespace WhoIsCrawler.Extensions
{
    public static class StringExtensions
    {
        public static string ApplyKeyFilters(this string value, string key){
            try
            {
                var filters = FilterConfig.Current.Filters[key];
                foreach (var filter in filters)
                {
                    value = Regex.Replace(value, filter, "");
                }
            }
            catch { }
            return value;
        }
    }
}
