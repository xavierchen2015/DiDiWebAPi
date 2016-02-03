using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web;
using System.Configuration;
using DidiWebApi.Models;
using Dapper;
using Newtonsoft.Json;

namespace DidiWebApi.Helper
{
    public class GetDataArgs
    {
        public List<LinearCoef> GetLinearCoefData()
        {
            List<LinearCoef> LinearCoefData = new List<LinearCoef>();
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["didi_test"].ToString()))
            {
                cn.Open();
                string sqlcommand = "select * from linear_coef";
                var datas = cn.Query<LinearCoef>(sqlcommand);
                foreach (var coef in datas)
                {
                    LinearCoefData.Add(coef);
                }
            }
            return LinearCoefData;
        }

        public LinearCoef GetDataArgsByFile()
        {
            string lines = File.ReadAllText(@"d:\linear_coef2.json");
            LinearCoef linearCoef = new LinearCoef();
            var dd = JsonConvert.DeserializeObject<LinearCoef>(lines);
            linearCoef.ListCoef = dd.ListCoef.ToList();
            //linearCoef.ListCoef = dd.ListCoef.Where(w => w.coef_type == 1).ToList();
            return linearCoef;
        }

        public LogitCoef GetLogitCoefArgsByFile()
        {
            string lines = File.ReadAllText(@"d:\logit_coef.json");
            LogitCoef logitCoef = new LogitCoef();
            var dd = JsonConvert.DeserializeObject<LogitCoef>(lines);
            logitCoef.listLogitCoef = dd.listLogitCoef.ToList();
            //linearCoef.ListCoef = dd.ListCoef.Where(w => w.coef_type == 1).ToList();
            return logitCoef;
        }
    }
}