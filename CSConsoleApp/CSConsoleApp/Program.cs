using System.Diagnostics;
using WhoIsCrawler.Infrastructure;
using WhoIsCrawler.Services;
using Newtonsoft.Json;
using System.IO;

namespace WhoIsCrawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitConfig(args);
            Start();
        }

        private static void InitConfig(string[] args)
        {
            var json = "";
            try
            {
                json = File.ReadAllText("config.json");
            }
            catch
            {
                System.Console.WriteLine("Failed to read the configuration file!");
                System.Environment.Exit(-1);
            }
            Configuration.Current = JsonConvert.DeserializeObject<Configuration>(json);
        }

        private static void Start()
        {
            Crawler c = new Crawler(
                new WhoIsDataParser(), 
                new WhoIsClient(), 
                new FileLogger(Configuration.Current.FailedLogFile), 
                new FileLogger(Configuration.Current.OutputFileName),
                new ConsoleLogger());
            c.Run();
        }
    }
}
