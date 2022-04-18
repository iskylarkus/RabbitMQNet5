using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQNet5.Subscriber
{
    public class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://efijhuug:yCOtUP8zIYnjfJlj4jTyLk9t6vSF8uTF@toad.rmq.cloudamqp.com/efijhuug");

            using (var connection = factory.CreateConnection())
            {
                var channel = connection.CreateModel();

                string queue = "hello-queue";

                var consumer = new EventingBasicConsumer(channel);

                channel.BasicConsume(queue, true, consumer);

                consumer.Received += (object sender, BasicDeliverEventArgs e) =>
                {
                    var message = Encoding.UTF8.GetString(e.Body.ToArray());

                    Console.WriteLine("Received Message: " + message);
                };

                Console.ReadLine();
            }
        }
    }
}
