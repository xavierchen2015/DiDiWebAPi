namespace DidiWebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class LinearCoef
    {
        public IList<ListCoef> ListCoef { get; set; }
    }
    public class ListCoef
    {
        public float coef { get; set; }

        public string coef_name { get; set; }

        public int coef_type { get; set; }

        public string promo_strgy { get; set; }

        public string keyword_cat { get; set; }

        public string direction { get; set; }
    }
}