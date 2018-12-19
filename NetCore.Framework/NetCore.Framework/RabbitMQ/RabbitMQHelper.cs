using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.Framework.RabbitMQ
{
    public class RabbitMQHelper
    {
        private IConnectionFactory _connFactory;
        public RabbitMQHelper(RabbitMQSetting _rabbitMQSetting)
        {
            IConnectionFactory conFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = _rabbitMQSetting.HostName,//IP地址
                Port = _rabbitMQSetting.Port,//端口号
                UserName = _rabbitMQSetting.UserName,//用户账号
                Password = _rabbitMQSetting.Password//用户密码
            };
            _connFactory = conFactory;
        }
        public void Send(string message, string queueName= "queueDefault")
        {

            using (IConnection con = _connFactory.CreateConnection())//创建连接对象
            {
                using (IModel channel = con.CreateModel())//创建连接会话对象
                {
                    if (string.IsNullOrWhiteSpace(queueName))
                    {
                        queueName = "queueDefault";
                    }
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                    //var isok= channel.WaitForConfirms();
                    channel.Close();
                }
            }
        }
        public string Receive(string queueName = "queueDefault")
        {
            using (IConnection conn = _connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    string result = string.Empty;
                    if (string.IsNullOrWhiteSpace(queueName))
                    {
                        queueName = "queueDefault";
                    }
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    //告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
                    channel.BasicQos(0, 1, false);

                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        byte[] message = ea.Body;//接收到的消息
                        Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));
                        result = Encoding.UTF8.GetString(message);
                        //返回消息确认
                        //channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //消费者开启监听
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                    return result;
                }
            }
        }
    }
}
