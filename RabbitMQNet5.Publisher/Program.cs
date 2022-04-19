using RabbitMQ.Client;
using System;
using System.Linq;
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

                string direct = "logs-direct";

                channel.ExchangeDeclare(direct, type: ExchangeType.Direct, durable: true);

                Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
                {
                    var rootKey = $"route-{x}";

                    var queueName = $"queue-{x}";

                    channel.QueueDeclare(queueName, true, false, false);

                    channel.QueueBind(queueName, direct, rootKey, null);
                });

                Enumerable.Range(10, 90).ToList().ForEach(x =>
                {
                    LogNames log = (LogNames)new Random().Next(1, 5);

                    string message = $"log type:{log} #{x}   >   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    var routeKey = $"route-{log}";

                    var messageBody = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(direct, routeKey, null, messageBody);

                    Console.WriteLine($"Send the log type:{log} #{x} :   {message}");
                });
            }

            Console.ReadLine();
        }
    }

    public enum LogNames
    {
        critical = 1,
        error = 2,
        warning = 3,
        information = 4,
    }
}
