using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Principal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Currency
{
    public class Bank
    {
        public string title { get; set; }
        public Dictionary<string, Currency> currencies { get; set; }

        public static List<Bank> GetBanks(string fileObj)
        {
            JObject obj = JObject.Parse(fileObj);
            List<Bank> organizations = obj["organizations"].ToObject<List<Bank>>();
            return organizations;
        }

        public class Currency
        {
            public string ask { get; set; }
            public string bid { get; set; }
        }
    }
}