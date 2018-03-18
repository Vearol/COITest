using System;

namespace PMC.DataModels.TestHelper
{
    public class CustomException : Exception
    {
        public byte? ErrorCode { get; private set; }
        
        public CustomException(string message, byte? errorCode = null)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }
    }
}
