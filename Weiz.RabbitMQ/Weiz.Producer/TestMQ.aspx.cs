using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Weiz.MQ;

namespace Weiz.Producer
{
    public partial class TestMQ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            MyMessage msg = new MyMessage();
            msg.MessageID = "1";
            msg.MessageBody = "Msg " + DateTime.Now.ToString();
            msg.MessageTitle = "1";
            msg.MessageRouter = "order.notice.lisi";





            MQHelper.Publish(msg);

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var factory = new ConnectionFactory() { HostName = "127.0.01", UserName = "admin", Password = "123", Port = 5672, VirtualHost = "rb" };
            using (var connection = factory.CreateConnection())
            {

                using (var channel = connection.CreateModel())
                {

                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("x-expires", 30000);
                    dic.Add("x-message-ttl", 0);//队列上消息过期时间，应小于队列过期时间  
                    dic.Add("x-dead-letter-exchange", "exchange-direct");//过期消息转向路由  
                    dic.Add("x-dead-letter-routing-key", "routing-delay");//过期消息转向路由相匹配routingkey  
                                                                          //创建一个名叫"zzhello"的消息队列
                    channel.QueueDeclare(queue: "zzhello",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: dic);

                    var message = DateTime.Now;
                    var body = Encoding.UTF8.GetBytes(message.ToString());

                    //向该消息队列发送消息message
                    channel.BasicPublish(exchange: "",
                        routingKey: "zzhello",
                        basicProperties: null,
                        body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }


        }
    }
}