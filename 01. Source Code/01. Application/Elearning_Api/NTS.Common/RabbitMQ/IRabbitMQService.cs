using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.Common.RabbitMQ
{
    public interface IRabbitMQService
    {
        void Publish<T>(T message, string exchangeName, string exchangeType, string queueName)
           where T : class;

        void PublishATGT<T>(T message,string type, string exchangeName, string exchangeType, string queueName)
           where T : class;
        void PublishToQueue<T>(PushQueueModel<T> pushQueueModel) where T : class;
    }
}
