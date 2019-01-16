﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsCrawler.Models
{
    public class DomainInformation
    {
        public string LastUpdate { get; set; }
        public string Domain { get; set; }
        public string Registrar { get; set; }
        public string Registered { get; set; }
        public string Expires { get; set; }
        public string Updated { get; set; }
        public string[] Status { get; set; }
        public string[] Servers { get; set; }
    }
}
