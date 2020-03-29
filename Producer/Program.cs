using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Producer
{
    class Program
    {
        const string queue = "task_queue";
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare
                (
                    queue,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchange: "",
                                     routingKey: queue,
                                    //  basicProperties: null,
                                     properties,
                                     body);
                Console.WriteLine(" [x] Sent {0}", message);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

        }
        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}
