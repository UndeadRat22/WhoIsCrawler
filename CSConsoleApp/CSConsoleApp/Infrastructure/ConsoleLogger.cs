using System.Collections.Generic;
using System.Linq;
using WhoIsCrawler.Infrastructure.Abstract;

namespace WhoIsCrawler.Infrastructure
{
    public class ConsoleLogger : ILogger
    {
        public void Log(object o)
        {
            System.Console.WriteLine(o.ToString());
        }

        public void Log<T>(IEnumerable<T> enumerable, char separator = '\n')
        {
            foreach (var msg in enumerable)
                System.Console.Write($"{msg}{separator}");
        }
    }
}
