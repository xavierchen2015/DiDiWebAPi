using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DidiWebApi.Models
{
    public class LogitCoef
    {
        public IList<ListLogitCoef> listLogitCoef { get; set; }
    }

    public class ListLogitCoef
    {
        public float coef { get; set; }

        public string coef_name { get; set; }
    }
}