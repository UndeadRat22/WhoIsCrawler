using System.IO;
using WhoIsCrawler.Infrastructure.Abstract;

namespace WhoIsCrawler.Infrastructure
{
    public class FileLogger : ILogger
    {
        private string _file;

        public FileLogger(string file)
        {
            _file = file;
        }

        public void Log(object o)
        {
            using (StreamWriter writer = File.AppendText(_file))
            {
                writer.WriteLine(o.ToString());
            }
        }
    }
}
