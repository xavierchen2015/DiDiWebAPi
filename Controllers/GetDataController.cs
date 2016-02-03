using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DidiWebApi.Models;
using Newtonsoft.Json;
using DidiWebApi.Helper;
using NLog;


namespace DidiWebApi.Controllers
{
    public class GetDataController : ApiController
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            DidiDatas didi = new DidiDatas();
            string json = JsonConvert.SerializeObject(didi);
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent(json);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return result;
        }

        // POST api/<controller>
        public HttpResponseMessage Post(DidiDbData didi)
        {
            log.Info("get post request :{0}", DateTime.Now);
            //取算法參數
            Func func = new Func();
            GetDataArgs gatArgs = new GetDataArgs();
            LinearCoef LinearCoefData = gatArgs.GetDataArgsByFile();
            LogitCoef LogitCoefData = gatArgs.GetLogitCoefArgsByFile();

            log.Info("get file data :{0}", DateTime.Now);
            try
            {
                var didiData = didi;
                string sqlStr = "";
                string didiToJson = JsonConvert.SerializeObject(didi);
                //func.writeFile(didiToJson);
                DidiPerData fixData = new DidiPerData();

                List<DidiDBDetail> listDidiData = new List<DidiDBDetail>();
                foreach (var di in didiData.dididbdetail)
                {
                    if (di.promo_id == "203200334" && di.keyword == "电炉丝免邮资")
                    {
                        string asdadasdas = "asdasdas";
                    }
                    //click : 点击量
                    float click = di.click;
                    fixData.click = click;

                    //roi : 投入产出比
                    float roi = di.roi > 10000 ? 10000 : di.roi;
                    fixData.roi = roi;

                    //imp : 展现量
                    float imp = di.imp;
                    fixData.imp = imp;

                    //imp_rank1 : 调整后的平均展现排名
                    float imp_rank = float.IsNaN(di.imp_rank) ? 0 : di.imp_rank;
                    float imp_rank1 = 1 / imp_rank;
                    imp_rank1 = float.IsInfinity(imp_rank1) ? 0 : imp_rank1;
                    fixData.imp_rank1 = imp_rank1;

                    //pc_soire : 计算机质量分数
                    float pc_soire = di.pc_score;
                    fixData.pc_score = pc_soire;

                    //conv : 转化率
                    float conv = di.conv;
                    fixData.conv = conv;

                    //ctr : 点击率
                    float ctr = di.ctr;
                    fixData.ctr = ctr;

                    //ctr_ind : 点击率Index
                    float ctr_ind = 0;
                    float all_ctr = 0;
                    all_ctr = di.all_ctr == -1 ? float.NaN : di.all_ctr;
                    if (float.IsNaN(all_ctr))
                    {
                        ctr_ind = 1;
                    }
                    else
                    {
                        ctr_ind = di.ctr / all_ctr;
                        ctr_ind = float.IsNaN(ctr_ind) ? 1 : ctr_ind;
                        ctr_ind = float.IsInfinity(ctr_ind) ? 1000 : ctr_ind;
                    }
                    fixData.ctr_ind = ctr_ind;

                    //cpc : 平均点击花费
                    float cpc = di.cpc;
                    fixData.cpc = cpc;

                    //cpc_ind :  平均点击花费index
                    float cpc_ind = 0;
                    float all_price = 0;
                    all_price = di.all_price == -1 ? float.NaN : di.all_price;
                    if (float.IsNaN(all_price))
                    {
                        cpc_ind = 1;
                    }
                    else
                    {
                        cpc_ind = di.cpc / all_price;
                        cpc_ind = float.IsNaN(cpc_ind) ? 1 : cpc_ind;
                        cpc_ind = float.IsInfinity(cpc_ind) ? 1000 : cpc_ind;
                    }
                    fixData.cpc_ind = cpc_ind;

                    //d_limt_ind : 计画日限额index
                    float d_limt_ind = di.daily_limt / di.yes_cpc;
                    d_limt_ind = float.IsInfinity(d_limt_ind) ? 1000 : d_limt_ind;
                    d_limt_ind = di.daily_limt == 20000000 ? 1000 : d_limt_ind;
                    fixData.d_limt_ind = d_limt_ind;

                    //d_spend_ind : 计画花费index
                    float d_spend_ind = di.yes_spend / di.daily_limt;
                    fixData.d_spend_ind = d_spend_ind;

                    // cpc_limit_ind :平均点击花费限价index
                    float cpc_limit_ind = 0;
                    if (di.price_limt == 0)
                    {
                        cpc_limit_ind = 0;
                    }
                    else
                    {
                        if (di.cpc <= di.price_limt)
                        {
                            cpc_limit_ind = di.cpc / di.price_limt;
                        }
                        else
                        {
                            cpc_limit_ind = 1000;
                        }
                    }
                    
                    cpc_limit_ind = float.IsNaN(cpc_limit_ind) ? 0 : cpc_limit_ind;
                    cpc_limit_ind = float.IsInfinity(cpc_limit_ind) ? 1000 : cpc_limit_ind;
                    fixData.cpc_limit_ind = cpc_limit_ind;

                    //price_limit_ind : 調價限價index
                    float price_limit_ind = 0;
                    if (di.price_limt == 0)
                    {
                        price_limit_ind = 0;
                    }
                    else
                    {
                        if (di.price_now <= di.price_limt)
                        {
                            price_limit_ind = di.price_now / di.price_limt;
                        }
                        else
                        {
                            price_limit_ind = 10;
                        }
                    }
                    price_limit_ind = float.IsNaN(price_limit_ind) ? 0 : price_limit_ind;
                    price_limit_ind = float.IsInfinity(price_limit_ind) ? 0 : price_limit_ind;
                    fixData.price_limit_ind = price_limit_ind;
                    
                    //cpc_mm : 平均点击花费平均index
                    float cpc_mm = di.cpc / di.cat_cpc;
                    cpc_mm = float.IsNaN(cpc_mm) ? 0 : cpc_mm;
                    cpc_mm = float.IsInfinity(cpc_mm) ? 0 : cpc_mm;
                    fixData.cpc_mm = cpc_mm;

                    //cpc_p : 平均点击花费综合index
                    float cpc_p = di.cpc >= di.price_now ? 1 : 0;
                    float price_temp = di.price_now > di.price_limt ? 40 : 0;
                    float cpc_mm_temp = cpc_mm >= 0.5 ? 10 : 0;
                    cpc_p = cpc_p + price_temp + cpc_mm_temp;
                    cpc_p = float.IsNaN(cpc_p) ? 0 : cpc_p;
                    fixData.cpc_p = cpc_p;

                    //f_per_cat : 操作类型
                    int f_per_cat = 0;
                    switch (di.output)
                    {
                        case "加价":
                            f_per_cat = 1;
                            break;
                        case "删除":
                            f_per_cat = 2;
                            break;
                        case "减价":
                            f_per_cat = 3;
                            break;
                        case "保留":
                            f_per_cat = 4;
                            break;
                    }
                    fixData.f_per_cat = f_per_cat;


                    //f_per2 : 调价幅度(%)
                    float f_per2 = ((di.price_new - di.price_now) / di.price_now  ) *100;
                    fixData.f_per2 = f_per2;

                    //f_per_cat_p : Migo操作类型预测
                    int f_per_cat_p_int = func.GetLogisticRegression(di, fixData, LogitCoefData);
                    string f_per_cat_p = "";
                    switch (f_per_cat_p_int)
                    {
                        case 1:
                            f_per_cat_p = "加价";
                            break;
                        case 2:
                            f_per_cat_p = "删除";
                            break;
                        case 3:
                            f_per_cat_p = "减价";
                            break;
                        default:
                            f_per_cat_p = "NA";
                            break;
                    }
                    di.f_per_cat_p = f_per_cat_p;

                    //f_per_p : Migo调价幅度预测
                    float f_per_p = func.GetLinearRegression(di, fixData, LinearCoefData, f_per_cat_p_int);
                    f_per_p = float.IsNaN(f_per_p) ? 0 : f_per_p;
                    di.f_per_p = f_per_p;

                    //寫DB
                    //func.InsertDb(di);

                    //寫SQL
                    sqlStr = sqlStr + "insert into sdata  (plan_id, daily_limt, yes_spend, yes_cpc, cat_id, cat_name, cat_cpc, promo_id, promo_strgy, promo_item_cat, promo_imp, promo_click, promo_ctr, promo_tot_spend, promo_cpc, promo_rev, promo_amt, promo_saved, promo_conv, promo_roi, keyword, price_now, pc_score, mb_score, price_limt, keyword_cat, imp, click, ctr, tot_spend, cpc, rev, amt, saved, conv, roi, imp_rank, all_imp, all_click, all_price, all_ctr, all_compe, all_conv, keyword2, price_new, output, adj_p, f_per_cat_p, f_per_p)";
                    sqlStr = sqlStr + "VALUES (";
                    sqlStr = sqlStr + "'" + di.plan_id + "'," + di.daily_limt + "," + di.yes_spend + "," + di.yes_cpc + ",'" + di.cat_id + "','" + di.cat_name + "'," + di.cat_cpc + ",'" + di.promo_id + "','" + di.promo_strgy + "','" + di.promo_item_cat + "'," + di.promo_imp + "," + di.promo_click + "," + di.promo_ctr + "," + di.promo_tot_spend + "," + di.promo_cpc + "," + di.promo_rev + "," + di.promo_amt + "," + di.promo_saved + "," + di.promo_conv + "," + di.promo_roi + ",'" + di.keyword + "'," + di.price_now + "," + di.pc_score + "," + di.mb_score + "," + di.price_limt + ",'" + di.keyword_cat + "'," + di.imp + "," + di.click + "," + di.ctr + "," + di.tot_spend + "," + di.cpc + "," + di.rev + "," + di.amt + "," + di.saved + "," + di.conv + "," + di.roi + "," + di.imp_rank + "," + di.all_imp + "," + di.all_click + "," + di.all_price + "," + di.all_ctr + "," + di.all_compe + "," + di.all_conv + ",'" + di.keyword2 + "'," + di.price_new + ",'" + di.output + "'," + di.adj_p + ",'" + di.f_per_cat_p + "'," + di.f_per_p + ")";
                    sqlStr = sqlStr + "\n";
                    //SQL寫TXT
                    //func.writeFile(@"d:\dididididi.txt", sqlStr);
                    //string fileName = @"d:\"  + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".txt";
                    //string fileName = @"d:\dididididi4.txt";
                    //func.WriteLog(@fileName, sqlStr);

                    
                    
                    //func.InsertZMQ(sqlStr);

                    //一個寶貝的資料先存起來
                    listDidiData.Add(di);
                }
                //一個寶貝的資料先存起來，再一起進DB
                func.InsertDb(listDidiData);

                //SQL寫Queue
                //func.IsertActiveQueue(sqlStr);
                //func.IsertMSQueue(sqlStr);
                //func.InsertNetMQ(sqlStr);

                //SQL寫TXT
                //func.WriteLog(@"d:\dididididiWriteLog.txt", sqlStr);

                log.Info("response data :{0}", DateTime.Now);
                string json = JsonConvert.SerializeObject(didiData);
                //資料存下來
                //func.writeFile(json);
                
                var result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent(json);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return result;
            }
            catch (Exception ex)
            {
                log.Error("error :{0}", ex.Message);
                return null;
            }
            
        }
    }
}