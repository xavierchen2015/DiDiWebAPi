using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Messaging;
using System.Threading;
using System.Web;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using DidiWebApi.Models;
using Dapper;
using NetMQ;
using NetMQ.Sockets;

namespace DidiWebApi.Helper
{
    public class Func : BaseController
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        static object lockMe = new object();

        public void WriteLog(string filePath, string str)
        {
            lock (lockMe)
            {
                using (StreamWriter sw =
                new StreamWriter(@filePath, true))
                {
                    sw.WriteLine(str);
                    sw.Close();
                }
            }
        }
        public void writeFile(string filePath, string str)
        {
            System.IO.File.AppendAllText(@filePath, str + "\n");
            //System.IO.File.AppendAllText(@"d:\didiRequestData.json", str + "\n");
        }
        public void InsertNetMQ(string didiStr)
        {
                using (var pubSocket = new PublisherSocket())
            {
                pubSocket.Options.SendHighWatermark = 10000;
                pubSocket.Bind("tcp://localhost:12345");
                Thread.Sleep(500);
                pubSocket.SendMoreFrame("").SendFrame(didiStr);
            }
        }
        public void InsertZMQ(string didiStr)
        {
            using (NetMQContext ctx = NetMQContext.Create())
            {
                using (var client = ctx.CreateRequestSocket())
                {
                    client.Connect("tcp://127.0.0.1:5556");
                    client.Send(didiStr);
                    string fromServerMessage = client.ReceiveString();
                }
            }
        }
        public void IsertMSQueue(string didiStr)
        {
            MessageQueue myQueue = new MessageQueue(".\\myQueue");
            myQueue.Send(didiStr);
        }
        public void IsertActiveQueue(string  didiStr)
        {
            try
            {
                //Create the Connection Factory
                IConnectionFactory factory = new ConnectionFactory("tcp://localhost:61616/");
                string msg = "";
                using (IConnection connection = factory.CreateConnection())
                {
                    //Create the Session
                    using (ISession session = connection.CreateSession())
                    {
                        //Create the Producer for the topic/queue
                        IMessageProducer prod = session.CreateProducer(
                            //new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("DidiMQ"));
                            new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue("DidiMQ"));

                        msg = didiStr;

                        //Send Messages
                        prod.Send(msg, Apache.NMS.MsgDeliveryMode.NonPersistent, Apache.NMS.MsgPriority.Normal, TimeSpan.MinValue);
                    }
                }
                log.Info("Send MQ msg :{0}", msg);
            }
            catch (System.Exception e)
            {
                log.Info("Send MQ error :{0}", e.Message);
            }
        }
        public void InsertDb(List<DidiDBDetail> didiData)
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["didi_test"].ToString()))
            {
                cn.Open();
                string sqlcommand = "insert into sdata7  (plan_id, daily_limt, yes_spend, yes_cpc, cat_id, cat_name, cat_cpc, promo_id, promo_strgy, promo_item_cat, promo_imp, promo_click, promo_ctr, promo_tot_spend, promo_cpc, promo_rev, promo_amt, promo_saved, promo_conv, promo_roi, keyword, price_now, pc_score, mb_score, price_limt, keyword_cat, imp, click, ctr, tot_spend, cpc, rev, amt, saved, conv, roi, imp_rank, all_imp, all_click, all_price, all_ctr, all_compe, all_conv, keyword2, price_new, output, adj_p, f_per_cat_p, f_per_p)";
                sqlcommand = sqlcommand + "VALUES (@plan_id, @daily_limt, @yes_spend, @yes_cpc, @cat_id, @cat_name, @cat_cpc, @promo_id, @promo_strgy, @promo_item_cat, @promo_imp, @promo_click, @promo_ctr, @promo_tot_spend, @promo_cpc, @promo_rev, @promo_amt, @promo_saved, @promo_conv, @promo_roi, @keyword, @price_now, @pc_score, @mb_score, @price_limt, @keyword_cat, @imp, @click, @ctr, @tot_spend, @cpc, @rev, @amt, @saved, @conv, @roi, @imp_rank, @all_imp, @all_click, @all_price, @all_ctr, @all_compe, @all_conv, @keyword2, @price_new, @output, @adj_p, @f_per_cat_p, @f_per_p)";
                cn.Execute(sqlcommand, didiData);
            }
        }
        public int GetLogisticRegression(DidiDBDetail detailData, DidiPerData fixData, LogitCoef LogitCoefData)
        {
            log.Info("GetLogisticRegression data :{0}", DateTime.Now);
            float t2 = LogitCoefData.listLogitCoef.Where(w => w.coef_name == "(Intercept):2").Select(s => s.coef).FirstOrDefault();
            t2 = t2 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "click:2").Select(s => s.coef).FirstOrDefault() * fixData.click;
            t2 = t2 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "roi:2").Select(s => s.coef).FirstOrDefault() * fixData.roi;
            t2 = t2 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "imp_rank1:2").Select(s => s.coef).FirstOrDefault() * fixData.imp_rank1;
            t2 = t2 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "pc_score:2").Select(s => s.coef).FirstOrDefault() * fixData.pc_score;
            t2 = t2 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "ctr_ind:2").Select(s => s.coef).FirstOrDefault() * fixData.ctr_ind;
            t2 = t2 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "cpc_ind:2").Select(s => s.coef).FirstOrDefault() * fixData.cpc_ind;
            t2 = t2 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "d_limt_ind:2").Select(s => s.coef).FirstOrDefault() * fixData.d_limt_ind;
            t2 = t2 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "d_spend_ind:2").Select(s => s.coef).FirstOrDefault() * fixData.d_spend_ind;
            t2 = t2 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "price_limt_ind:2").Select(s => s.coef).FirstOrDefault() * fixData.price_limit_ind;
            t2 = t2 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "cpc_mm:2").Select(s => s.coef).FirstOrDefault() * fixData.cpc_mm;
            t2 = t2 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "cpc_p:2").Select(s => s.coef).FirstOrDefault() * fixData.cpc_p;

            float t3 = LogitCoefData.listLogitCoef.Where(w => w.coef_name == "(Intercept):3").Select(s => s.coef).FirstOrDefault();
            t3 = t3 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "click:3").Select(s => s.coef).FirstOrDefault() * fixData.click;
            t3 = t3 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "roi:3").Select(s => s.coef).FirstOrDefault() * fixData.roi;
            t3 = t3 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "imp_rank1:3").Select(s => s.coef).FirstOrDefault() * fixData.imp_rank1;
            t3 = t3 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "pc_score:3").Select(s => s.coef).FirstOrDefault() * fixData.pc_score;
            t3 = t3 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "ctr_ind:3").Select(s => s.coef).FirstOrDefault() * fixData.ctr_ind;
            t3 = t3 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "cpc_ind:3").Select(s => s.coef).FirstOrDefault() * fixData.cpc_ind;
            t3 = t3 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "d_limt_ind:3").Select(s => s.coef).FirstOrDefault() * fixData.d_limt_ind;
            t3 = t3 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "d_spend_ind:3").Select(s => s.coef).FirstOrDefault() * fixData.d_spend_ind;
            t3 = t3 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "price_limt_ind:3").Select(s => s.coef).FirstOrDefault() * fixData.price_limit_ind;
            t3 = t3 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "cpc_mm:3").Select(s => s.coef).FirstOrDefault() * fixData.cpc_mm;
            t3 = t3 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "cpc_p:3").Select(s => s.coef).FirstOrDefault() * fixData.cpc_p;

            float t4 = LogitCoefData.listLogitCoef.Where(w => w.coef_name == "(Intercept):4").Select(s => s.coef).FirstOrDefault();
            t4 = t4 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "click:4").Select(s => s.coef).FirstOrDefault() * fixData.click;
            t4 = t4 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "roi:4").Select(s => s.coef).FirstOrDefault() * fixData.roi;
            t4 = t4 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "imp_rank1:4").Select(s => s.coef).FirstOrDefault() * fixData.imp_rank1;
            t4 = t4 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "pc_score:4").Select(s => s.coef).FirstOrDefault() * fixData.pc_score;
            t4 = t4 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "ctr_ind:4").Select(s => s.coef).FirstOrDefault() * fixData.ctr_ind;
            t4 = t4 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "cpc_ind:4").Select(s => s.coef).FirstOrDefault() * fixData.cpc_ind;
            t4 = t4 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "d_limt_ind:4").Select(s => s.coef).FirstOrDefault() * fixData.d_limt_ind;
            t4 = t4 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "d_spend_ind:4").Select(s => s.coef).FirstOrDefault() * fixData.d_spend_ind;
            t4 = t4 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "price_limt_ind:4").Select(s => s.coef).FirstOrDefault() * fixData.price_limit_ind;
            t4 = t4 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "cpc_mm:4").Select(s => s.coef).FirstOrDefault() * fixData.cpc_mm;
            t4 = t4 + LogitCoefData.listLogitCoef.Where(w => w.coef_name == "cpc_p:4").Select(s => s.coef).FirstOrDefault() * fixData.cpc_p;

            double prob2 = Math.Exp(t2) / (1 + Math.Exp(t2) + Math.Exp(t3) + Math.Exp(t4));
            double prob3 = Math.Exp(t3) / (1 + Math.Exp(t2) + Math.Exp(t3) + Math.Exp(t4));
            double prob4 = Math.Exp(t4) / (1 + Math.Exp(t2) + Math.Exp(t3) + Math.Exp(t4));
            double prob1 = 1 - prob2 - prob3 - prob4;
            double[] maxProb = { prob1, prob2, prob3, prob4 };
            var max = maxProb.Max();
            var result = Array.IndexOf(maxProb, max);
            log.Info("GetLogisticRegression data end :{0}", DateTime.Now);
            return result + 1;
        }

        public float GetLinearRegression(DidiDBDetail detailData, DidiPerData fixData, LinearCoef LinearCoefData, int logisticRegression)
        {
            log.Info("GetLinearRegression data :{0}", DateTime.Now);
            if (logisticRegression == 1 || logisticRegression == 3)
            {
                Dictionary<string, float > dicLinear = new Dictionary<string, float>();
                string direction = logisticRegression == 1 ? "Increase" : "Decrease";
                dicLinear = LinearCoefData.ListCoef.Where(w => w.promo_strgy == detailData.promo_strgy && w.keyword_cat == detailData.keyword_cat && w.direction == direction).Select(s => new { s.coef_name, s.coef }).ToDictionary(x => x.coef_name, x=>x.coef);
                var ddd = LinearCoefData.ListCoef.Where(w => w.coef_type == 1).Select(s => s.coef).FirstOrDefault();
                float f_per_p = dicLinear["click"] * fixData.click
                    + dicLinear["imp"] * fixData.imp
                    + dicLinear["conv"] * fixData.conv
                    + dicLinear["roi"] * fixData.roi
                    + dicLinear["imp_rank1"] * fixData.imp_rank1
                    + dicLinear["pc_score"] * fixData.pc_score
                    + dicLinear["ctr"] * fixData.ctr
                    + dicLinear["ctr_ind"] * fixData.ctr_ind
                    + dicLinear["cpc"] * fixData.cpc
                    + dicLinear["cpc_ind"] * fixData.cpc_ind
                    + dicLinear["d_spend_ind"] * fixData.d_spend_ind
                    + dicLinear["cpc_limt_ind"] * fixData.cpc_limit_ind;

                //float f_per_p = dicLinear["click"] * fixData.click;
                //f_per_p = f_per_p + dicLinear["imp"] * fixData.imp;
                //f_per_p = f_per_p + dicLinear["conv"] * fixData.conv;
                //f_per_p = f_per_p + dicLinear["roi"] * fixData.roi;
                //f_per_p = f_per_p + dicLinear["imp_rank1"] * fixData.imp_rank1;
                // f_per_p = f_per_p + dicLinear["pc_score"] * fixData.pc_score;
                //f_per_p = f_per_p + dicLinear["ctr"] * fixData.ctr;
                //f_per_p = f_per_p + dicLinear["ctr_ind"] * fixData.ctr_ind;
                //f_per_p = f_per_p + dicLinear["cpc"] * fixData.cpc;
                //f_per_p = f_per_p + dicLinear["cpc_ind"] * fixData.cpc_ind;
                //f_per_p = f_per_p + dicLinear["d_spend_ind"] * fixData.d_spend_ind;
                //f_per_p = f_per_p + dicLinear["cpc_limt_ind"] * fixData.cpc_limit_ind;
                log.Info("GetLinearRegression data end :{0}", DateTime.Now);
                return f_per_p;
            }
            else
            {
                log.Info("GetLinearRegression data end nan :{0}", DateTime.Now);
                return float.NaN;
            }
        }
    }
}