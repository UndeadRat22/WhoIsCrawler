using System.Collections.Generic;
using System.IO;
using System.Text;
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

        public void Log<T>(IEnumerable<T> enumerable, char separator)
        {
            using (StreamWriter writer = File.AppendText(_file))
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendJoin(separator, enumerable);
                writer.WriteLine(builder.ToString());
            }
        }
    }
}
