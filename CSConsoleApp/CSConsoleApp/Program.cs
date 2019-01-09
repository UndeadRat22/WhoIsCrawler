namespace Crawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Crawler c = new Crawler(new WhoIsDataParser(), new WhoIsClient());
            c.Run();
        }
    }
}
