using System;
namespace NTS.Common.Attributes
{
    public class NtsApiResultModel<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }       
    }

    public class NtsApiResultModel
    {
        public object Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }

    public class NtsApiResultConstants
    {
        public const int StatusCodeSuccess = 1;
        public const int StatusCodeError = 2;
        public const int StatusCodeValidateError = 3;
    }

    public class NtsErrorValidateModel
    {
        public string Message { get; set; }
    }
}