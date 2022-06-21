using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGD.Admin.WebApi.Models
{
    public class DataTableCustom<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }
    }
}