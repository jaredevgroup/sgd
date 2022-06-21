using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGD.Admin.WebApi.Models
{
    public class ResponseMessageCustom<T>
    {
        public string status { get; set; }
        public string message { get; set; }
        public T result { get; set; }
    }
}