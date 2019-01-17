namespace WhoIsCrawler
{
    public class Configuration
    {
        public static Configuration Current { get; set; }

        public string InputFileName { get; set; }
        public string OutputFileName { get; set; }
        public string ProxyAddress { get; set; }
        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }
        public string WhoIsDomain { get; set; }
        public int Timeout { get; set; }
        public string FailedLogFile { get; set; }
    }
}
