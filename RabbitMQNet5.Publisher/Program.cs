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

                string topic = "logs-topic";

                channel.ExchangeDeclare(topic, type: ExchangeType.Topic, durable: true);

                Random random = new Random();

                Enumerable.Range(10, 90).ToList().ForEach(x =>
                {
                    LogNames log1 = (LogNames)random.Next(1, 5);
                    LogNames log2 = (LogNames)random.Next(1, 5);
                    LogNames log3 = (LogNames)random.Next(1, 5);

                    var routeKey = $"{log1}.{log2}.{log3}";

                    string message = $"{topic} : {routeKey}  #{x}   >   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    var messageBody = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(topic, routeKey, null, messageBody);

                    Console.WriteLine($"Send {topic}:{routeKey} #{x} :   {message}");
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
