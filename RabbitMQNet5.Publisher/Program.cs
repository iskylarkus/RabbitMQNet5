using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQNet5.Publisher
{
    internal class Program
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

                string fanout = "logs-fanout";

                channel.ExchangeDeclare(fanout, type: ExchangeType.Fanout, durable: true);

                Enumerable.Range(10, 90).ToList().ForEach(x =>
                {
                    string message = $"log #{x}   >   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    var messageBody = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(fanout, "", null, messageBody);

                    Console.WriteLine($"Send the log #{x} :   {message}");
                });
            }

            Console.ReadLine();
        }
    }
}
