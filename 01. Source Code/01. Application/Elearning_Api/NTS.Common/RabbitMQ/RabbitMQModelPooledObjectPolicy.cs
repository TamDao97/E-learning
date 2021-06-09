using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;

namespace NTS.Common.RabbitMQ
{
  public  class RabbitMQModelPooledObjectPolicy: IPooledObjectPolicy<IModel>
    {
        private readonly RabbitMQSettings _rabbitMQSettings;

        private readonly IConnection _connection;

        public RabbitMQModelPooledObjectPolicy(IOptions<RabbitMQSettings> optionsAccs)
        {
            _rabbitMQSettings = optionsAccs.Value;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQSettings.HostName,
                UserName = _rabbitMQSettings.UserName,
                Password = _rabbitMQSettings.Password,
                Port = _rabbitMQSettings.Port,
            };

            return factory.CreateConnection();
        }

        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
