namespace DidiWebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class DidiDbData
    {
        public string promo_id { get; set; }
        public List<DidiDBDetail> dididbdetail { get; set; }
        
    }
    public class DidiDBDetail
    {
        public string plan_id { get; set; }
        public float daily_limt { get; set; }
        public float yes_spend { get; set; }
        public float yes_cpc { get; set; }
        public string cat_id { get; set; }
        public string cat_name { get; set; }
        public float cat_cpc { get; set; }
        public string promo_id { get; set; }
        public string promo_strgy { get; set; }
        public string promo_item_cat { get; set; }
        public float promo_imp { get; set; }
        public float promo_click { get; set; }
        public float promo_ctr { get; set; }
        public float promo_tot_spend { get; set; }
        public float promo_cpc { get; set; }
        public float promo_rev { get; set; }
        public float promo_amt { get; set; }
        public float promo_saved { get; set; }
        public float promo_conv { get; set; }
        public float promo_roi { get; set; }
        public string keyword { get; set; }
        public float price_now { get; set; }
        public float pc_score { get; set; }
        public float mb_score { get; set; }
        public float price_limt { get; set; }
        public string keyword_cat { get; set; }
        public float imp { get; set; }
        public float click { get; set; }
        public float ctr { get; set; }
        public float tot_spend { get; set; }
        public float cpc { get; set; }
        public float rev { get; set; }
        public float amt { get; set; }
        public float saved { get; set; }
        public float conv { get; set; }
        public float roi { get; set; }
        public float imp_rank { get; set; }
        public float all_imp { get; set; }
        public float all_click { get; set; }
        public float all_price { get; set; }
        public float all_ctr { get; set; }
        public float all_compe { get; set; }
        public float all_conv { get; set; }
        public string keyword2 { get; set; }
        public float price_new { get; set; }
        public string output { get; set; }
        public float adj_p { get; set; }
        public string f_per_cat_p { get; set; }
        public float f_per_p { get; set; }
    }
}