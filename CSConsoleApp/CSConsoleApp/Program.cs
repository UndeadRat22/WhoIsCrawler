using WhoIsCrawler.Infrastructure;
using WhoIsCrawler.Services;
using Newtonsoft.Json;
using System.IO;
using WhoIsCrawler.Models;

namespace WhoIsCrawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Config.Current = GetConfig<Config>("config.json");
            FilterConfig.Current = GetConfig<FilterConfig>("filters.json");
            Start();
        }

        private static T GetConfig<T>(string file)
        {
            var json = "";
            try
            {
                json = File.ReadAllText(file);
            }
            catch
            {
                System.Console.WriteLine($"Failed to read the configuration file: {file}!");
                System.Environment.Exit(-1);
            }
            return JsonConvert.DeserializeObject<T>(json);
        }

        private static void Start()
        {
            Crawler c = new Crawler(
                new WhoIsDataParser(), 
                new WhoIsClient(), 
                new FileLogger(Config.Current.FailedLogFile), 
                new FileLogger(Config.Current.OutputFileName),
                new ConsoleLogger());
            c.Run();
        }
    }
}
