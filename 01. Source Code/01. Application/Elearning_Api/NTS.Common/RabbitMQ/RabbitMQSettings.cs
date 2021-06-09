using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.Common.RabbitMQ
{
    public class RabbitMQSettings
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; } = 5672;
        public string QueueSendEmailName { get; set; }
    }
}
