using System;
using System.Collections.Generic;

namespace Elearning.Models
{
    public class ApiResultModel<T>
    {
        public T Data { get; set; }
        public List<string> Message { get; set; }
        public int StatusCode { get; set; }
        public ApiResultModel()
        {
            Message = new List<string>();
        }
    }

    public class ApiResultModel
    {
        public object Data { get; set; }
        public List<string> Message { get; set; }
        public int StatusCode { get; set; }

        public ApiResultModel()
        {
            Message = new List<string>();
        }
    }

    public class ApistringResultModel
    {
        public string Data { get; set; }
        public List<string> Message { get; set; }
        public int StatusCode { get; set; }

        public ApistringResultModel()
        {
            Message = new List<string>();
        }
    }

    public class ApiResultConstants
    {
        public const int StatusCodeSuccess = 1;
        public const int StatusCodeError = 2;
        public const int StatusCodeValidateError = 3;
    }

    public class ErrorValidateModel
    {
        public string Message { get; set; }
    }
}