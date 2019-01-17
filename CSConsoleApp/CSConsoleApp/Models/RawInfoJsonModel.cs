using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WhoIsCrawler.Models
{
    public class RawInfoJsonModel
    {
        public string Domain { get; set; }
        private Dictionary<string, List<string>> matches = new Dictionary<string, List<string>>();

        public void AddText(Match match)
        {
            if (!match.Success)
                return;
            var key = match.Groups[1].Value;
            var value = match.Groups[2].Value;
            try
            {
                matches.Add(key, new List<string>() { value });
            }
            catch (System.ArgumentException)
            {
                matches[match.Groups[1].Value].Add(value);
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder("{"+$"\"Domain\":\"{Domain}\"," + "\"info\": {");
            foreach (var m in matches)
            {
                builder.Append(PairToString(m));
            }
            return builder.Remove(builder.Length - 1, 1).Append("}}").ToString();
        }

        private string PairToString(KeyValuePair<string, List<string>> pair)
        {
            if (pair.Value.Count == 1)
                return $"\"{pair.Key}\" : \"{pair.Value[0]}\",";
            var builder = new StringBuilder($"\"{pair.Key}\" : [");
            pair.Value
                .ForEach(s => builder.Append($" \"{s}\","));
            builder.Remove(builder.Length - 1, 1);
            return builder.Append("],").ToString();
        }
    }
}
