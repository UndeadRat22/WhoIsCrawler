using WhoIsCrawler.Infrastructure;
using WhoIsCrawler.Services;

namespace WhoIsCrawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                System.Console.WriteLine("Please enter the files to read and write from.");
                return;
            }
            InitConfig(args);
            Start();
        }

        private static void InitConfig(string[] args)
        {
            Configuration.Current = new Configuration
            {
                InputFileName = args[0],
                OutputFileName = args[1],
                ProxyAddress = args[2],
                ProxyUsername = args.Length > 3 ? args[3] : "",
                ProxyPassword = args.Length > 4 ? args[4] : "",
                WhoIsDomain = "https://www.whois.com/",
                Timeout = 15000,
                FailedLogFile = @"D:\crawler_fail_log.log",
            };
        }

        private static void Start()
        {
            Crawler c = new Crawler(
                new WhoIsDataParser(), 
                new WhoIsClient(), 
                new FileLogger(Configuration.Current.FailedLogFile), 
                new ConsoleLogger());
            c.Run();
        }
    }
}
