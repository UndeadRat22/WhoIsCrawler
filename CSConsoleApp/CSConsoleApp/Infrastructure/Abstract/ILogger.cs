using System.Collections.Generic;

namespace WhoIsCrawler.Infrastructure.Abstract
{
    public interface ILogger
    {
        void Log(object o);
        void Log<T>(IEnumerable<T> enumerable, char separator);
    }
}
