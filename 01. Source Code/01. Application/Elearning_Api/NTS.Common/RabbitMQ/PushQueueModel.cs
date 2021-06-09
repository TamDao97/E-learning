using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.Common.RabbitMQ
{
    public class PushQueueModel<T>
    {
        /// <summary>
        /// Nội dung 
        /// </summary>
        public object Message { get; set; }
        public string Type { get; set; }
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }

        /// <summary>
        /// Tên Queue
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// IP/tên server
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Tài khoản đăng nhạp Queue
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///  Mật khẩu đăng nhạp Queue
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; set; }
    }
}
