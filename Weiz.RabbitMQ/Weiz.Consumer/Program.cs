using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weiz.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            //OrderProcessMessage order = new OrderProcessMessage();
            //MQ.MyMessage msg = new MQ.MyMessage();
            //msg.MessageID = "1";
            //msg.MessageRouter = "order.notice.lisi";

            //MQ.MQHelper.Subscribe(msg, order);

            //Console.WriteLine("Listening for messages.");


            var factory = new ConnectionFactory() { HostName = "127.0.01", UserName = "admin", Password = "123",Port= 5672,VirtualHost="rb" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "exchange-direct", type: "direct");
                    string name = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: name, exchange: "exchange-direct", routingKey: "routing-delay");

                    //回调，当consumer收到消息后会执行该函数
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(ea.RoutingKey);
                        Console.WriteLine(" [x] Received {0}", message);
                    };

                    //Console.WriteLine("name:" + name);
                    //消费队列"hello"中的消息
                    channel.BasicConsume(queue: name,
                                         noAck: true,
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }

        }
    }
}
