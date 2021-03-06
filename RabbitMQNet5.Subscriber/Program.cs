using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQNet5.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
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

                string header = "header-exchange";

                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);

                var queueName = channel.QueueDeclare().QueueName;


                Dictionary<string, object> headers = new Dictionary<string, object>();
                headers.Add("format", "pdf");
                headers.Add("shape", "a4");
                headers.Add("x-match", "all");


                channel.QueueBind(queueName, header, string.Empty, headers);

                channel.BasicConsume(queueName, false, consumer);

                Console.WriteLine("Listening the messages...");

                consumer.Received += (object sender, BasicDeliverEventArgs e) =>
                {
                    var message = Encoding.UTF8.GetString(e.Body.ToArray());

                    Product product = JsonSerializer.Deserialize<Product>(message);

                    Console.WriteLine($"Received Message:   {product.Id}-{product.Name}-{product.Price}-{product.Stock}");

                    channel.BasicAck(e.DeliveryTag, false);

                    Thread.Sleep(1000);
                };

                Console.ReadLine();
            }
        }
    }
}
