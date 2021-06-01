using System;

namespace MicroService.Extensions.Extensions
{
    public class EnumException : Exception
    {
        public EnumException()
        {

        }
        public EnumException(string message) : base(message)
        {

        }
    }
}
