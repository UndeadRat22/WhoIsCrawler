using WhoIsCrawler.Infrastructure;
using WhoIsCrawler.Services;

namespace WhoIsCrawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
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
                ProxyAddress = args.Length > 2 ? args[2] : "",
                ProxyUsername = args.Length > 3 ? args[3] : "",
                ProxyPassword = args.Length > 4 ? args[4] : "",
                FailedLogFile = args.Length > 5 ? args[5] : @"C:\Users\Public\failed_domains.log",
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
