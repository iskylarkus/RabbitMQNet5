using RabbitMQ.Client;
using RabbitMQNet5.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace RabbitMQNet5.Publisher
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

                channel.ExchangeDeclare(header, type: ExchangeType.Headers, durable: true);


                Dictionary<string, object> headers = new Dictionary<string, object>();
                headers.Add("format", "pdf");
                headers.Add("shape", "a4");


                var properties = channel.CreateBasicProperties();
                properties.Headers = headers;
                properties.Persistent = true;


                var product = new Product { Id = 1, Name = "Pen", Price = 99, Stock = 66 };
                var productJson = JsonSerializer.Serialize(product);


                var messageBody = Encoding.UTF8.GetBytes(productJson);

                channel.BasicPublish(header, string.Empty, properties, messageBody);


                Console.WriteLine($"Send {header} :   {product.Id}-{product.Name}-{product.Price}-{product.Stock}");
            }

            Console.ReadLine();
        }
    }
}
