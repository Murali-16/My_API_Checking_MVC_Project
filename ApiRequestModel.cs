using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostmanReplicaMVC.Models
{
    public class ApiRequestModel
    {
        public string Url { get; set; }
       

        public string Body { get; set; }

        public string BodyType { get; set; }

        public List<HeaderModel> Headers { get; set; }
    }

    public class HeaderModel
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}