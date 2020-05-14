using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weiz.Consumer
{
    public class OrderProcessMessage:MQ.IProcessMessage
    {
        public void ProcessMsg(MQ.MyMessage msg)
        {
            Console.WriteLine(msg.MessageBody);
        }
    }
}