namespace Crawler
{
    public class Program
    {
        public static Config Settings { get; private set; }

        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                System.Console.WriteLine("Please enter the files to read and write from.");
                return;
            }
            Settings = new Config
            {
                InputFileName = args[0],
                OutputFileName = args[1],
                ProxyAddress = args[2],
                ProxyUsername = args.Length > 3 ? args[3] : "",
                ProxyPassword = args.Length > 4 ? args[4] : ""
            };
            Crawler c = new Crawler(new WhoIsDataParser(), new WhoIsClient());
            c.Run();
        }

        public class Config
        {
            public string InputFileName { get; set; }
            public string OutputFileName { get; set; }
            public string ProxyAddress { get; set; }
            public string ProxyUsername { get; set; }
            public string ProxyPassword { get; set; }
        }
    }
}
