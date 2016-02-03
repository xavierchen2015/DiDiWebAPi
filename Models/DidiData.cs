namespace DidiWebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class DidiDatas
    {
        public string promo_id { get; set; }
        public List<DidiDataDetail> dididatadetail { get; set; }
    }

    public class DidiDataDetail
    {
        public string plan_id { get; set; }
        public string daily_limt { get; set; }
        public string yes_spend { get; set; }
        public string yes_cpc { get; set; }
        public string cat_id { get; set; }
        public string cat_name { get; set; }
        public string cat_cpc { get; set; }
        public string promo_id { get; set; }
        public string promo_strgy { get; set; }
        public string promo_item_cat { get; set; }
        public string promo_imp { get; set; }
        public string promo_click { get; set; }
        public string promo_ctr { get; set; }
        public string promo_tot_spend { get; set; }
        public string promo_cpc { get; set; }
        public string promo_rev { get; set; }
        public string promo_amt { get; set; }
        public string promo_saved { get; set; }
        public string promo_conv { get; set; }
        public string promo_roi { get; set; }
        public string keyword { get; set; }
        public string price_now { get; set; }
        public string pc_score { get; set; }
        public string mb_score { get; set; }
        public string price_limt { get; set; }
        public string keyword_cat { get; set; }
        public string imp { get; set; }
        public string click { get; set; }
        public string ctr { get; set; }
        public string tot_spend { get; set; }
        public string cpc { get; set; }
        public string rev { get; set; }
        public string amt { get; set; }
        public string saved { get; set; }
        public string conv { get; set; }
        public string roi { get; set; }
        public string imp_rank { get; set; }
        public string all_imp { get; set; }
        public string all_click { get; set; }
        public string all_price { get; set; }
        public string all_ctr { get; set; }
        public string all_compe { get; set; }
        public string all_conv { get; set; }
        public string keyword2 { get; set; }
        public string price_new { get; set; }
        public string output { get; set; }
        public string adj_p { get; set; }
    }
}