using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQNet5.Subscriber
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please press enter key to start...");
            Console.ReadLine();

            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://efijhuug:yCOtUP8zIYnjfJlj4jTyLk9t6vSF8uTF@toad.rmq.cloudamqp.com/efijhuug");

            using (var connection = factory.CreateConnection())
            {
                var channel = connection.CreateModel();

                string topic = "logs-topic";

                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);

                var queueName = channel.QueueDeclare().QueueName;

                var routeKey = "*.error.*"; //"info.#" //"*.*.warning" //"#.critical"

                channel.QueueBind(queueName, topic, routeKey, null);

                channel.BasicConsume(queueName, false, consumer);

                Console.WriteLine("Listening the logs...");

                consumer.Received += (object sender, BasicDeliverEventArgs e) =>
                {
                    var message = Encoding.UTF8.GetString(e.Body.ToArray());

                    Console.WriteLine("Received Message: " + message);

                    channel.BasicAck(e.DeliveryTag, false);

                    Thread.Sleep(1000);
                };

                Console.ReadLine();
            }
        }
    }
}
