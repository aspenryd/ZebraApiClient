using System;

namespace ZebraApiClient.Exceptions
{
    public class ZebraApiException : Exception
    {
        public ZebraApiException(string message)
            :base(message)
        {            
        }
    }
}