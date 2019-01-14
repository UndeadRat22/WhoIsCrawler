using System.Diagnostics;
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
                DomainOutputFileName = args[1],
                RegistrantsOutputFileName = args[2],
                FailedLogFile = args.Length > 3 ? args[3] : @"D:\failed_domains.log",
                ProxyAddress = args.Length > 4 ? args[4] : "",
                ProxyUsername = args.Length > 5 ? args[5] : "",
                ProxyPassword = args.Length > 6 ? args[6] : "",
                WhoIsDomain = "https://www.whois.com/",
                Timeout = 15000,
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
