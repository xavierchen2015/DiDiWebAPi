using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using DidiWebApi.Helper;
using DidiWebApi.Models;

using NetMQ.Sockets;
using Autofac;
using Autofac.Integration.WebApi;

namespace DidiWebApi
{
    public class Startup
    {
        //public static LinearCoef LinearCoefData = new LinearCoef();
        public void Configuration(IAppBuilder app)
        {
            //LinearCoefData = GetDataArgs.GetDataArgsByFile();
            // 如需如何設定應用程式的詳細資訊，請參閱  http://go.microsoft.com/fwlink/?LinkID=316888
            //PublisherSocket pubSocket = new PublisherSocket();
            //pubSocket.Options.SendHighWatermark = 10000;
            //pubSocket.Bind("tcp://localhost:12345");
            //BaseController baseController = new BaseController();
            
        }
    }
}
