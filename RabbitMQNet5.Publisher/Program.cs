using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQNet5.Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://efijhuug:yCOtUP8zIYnjfJlj4jTyLk9t6vSF8uTF@toad.rmq.cloudamqp.com/efijhuug");

            using (var connection = factory.CreateConnection())
            {
                var channel = connection.CreateModel();

                string queue = "hello-queue";

                channel.QueueDeclare(queue, true, false, false);

                string message = "Hello World > " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(string.Empty, queue, null, messageBody);
            }

            Console.WriteLine("Send the message...");

            Console.ReadLine();
        }
    }
}
