namespace DidiWebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    public class DidiPerData
    {
        public float click { get; set; }
        public float roi { get; set; }
        public float imp { get; set; }
        public float imp_rank1 { get; set; }
        public float pc_score { get; set; }
        public float conv { get; set; }
        public float ctr { get; set; }
        public float ctr_ind { get; set; }
        public float cpc { get; set; }
        public float cpc_ind { get; set; }
        public float d_limt_ind { get; set; }
        public float d_spend_ind { get; set; }
        public float cpc_limit_ind { get; set; }
        public float price_limit_ind { get; set; }
        public float cpc_mm { get; set; }
        public float cpc_p { get; set; }
        public float f_per_cat { get; set; }
        public float f_per_cat_p { get; set; }
        public float f_per2 { get; set; }
        public float f_per_p { get; set; }
    }
}