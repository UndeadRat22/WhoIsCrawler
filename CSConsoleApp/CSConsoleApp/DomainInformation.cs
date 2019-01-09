using System;
using System.Collections.Generic;
using System.Text;

namespace Crawler
{
    public class DomainInformation
    {
        public string Domain { get; set; }
        public string Registrar { get; set; }
        public string Registered { get; set; }
        public string Expires { get; set; }
        public string Updated { get; set; }
        public string[] Status { get; set; }
        public string[] Servers { get; set; }

        public override string ToString()
        {
            return "hello world.exe";
        }
    }
}
