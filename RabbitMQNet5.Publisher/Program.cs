using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

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


                string message = $"{header}   my-header-message   >   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(header, string.Empty, properties, messageBody);


                Console.WriteLine($"Send {header} :   {message}");
            }

            Console.ReadLine();
        }
    }
}
