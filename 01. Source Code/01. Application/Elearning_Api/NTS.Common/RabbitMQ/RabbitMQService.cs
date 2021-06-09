using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.Common.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly DefaultObjectPool<IModel> _objectPool;

        public RabbitMQService(IPooledObjectPolicy<IModel> objectPolicy)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
        }

        public void Publish<T>(T message, string exchangeName, string exchangeType, string queueName) where T : class
        {
            if (message == null)
                return;

            var channel = _objectPool.Get();

            try
            {
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;

                channel.BasicPublish(exchangeName, queueName, properties, sendBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }

        public void PublishATGT<T>(T message, string type, string exchangeName, string exchangeType, string queueName) where T : class
        {
            if (message == null)
                return;

            var channel = _objectPool.Get();

            try
            {
                channel.ExchangeDeclare(exchange: queueName, type: ExchangeType.Fanout, durable: false, autoDelete: false, arguments: null);

                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;
                channel.BasicPublish(queueName, queueName, properties, sendBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }

        public void PublishToQueue<T>(PushQueueModel<T> pushQueueModel) where T : class
        {
            if (pushQueueModel == null || pushQueueModel.Message == null)
                return;

            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = pushQueueModel.HostName,
                    UserName = pushQueueModel.UserName,
                    Password = pushQueueModel.Password,
                    Port = pushQueueModel.Port,
                };

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        //channel.ExchangeDeclare(exchange: pushQueueModel.QueueName, type: ExchangeType.Fanout, durable: false, autoDelete: false, arguments: null);
                        channel.QueueDeclare(queue: pushQueueModel.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                        var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(pushQueueModel.Message));
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        properties.DeliveryMode = 2;
                        channel.BasicPublish(pushQueueModel.ExchangeName, pushQueueModel.QueueName, properties, sendBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
