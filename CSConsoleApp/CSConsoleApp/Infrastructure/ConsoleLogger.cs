using WhoIsCrawler.Infrastructure.Abstract;

namespace WhoIsCrawler.Infrastructure
{
    public class ConsoleLogger : ILogger
    {
        public void Log(object o)
        {
            System.Console.WriteLine(o.ToString());
        }
    }
}
