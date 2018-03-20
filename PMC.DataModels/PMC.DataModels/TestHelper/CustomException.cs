using System;

namespace PMC.DataModels.TestHelper
{
    /// <summary>
    /// Custom exception which is usual exception with CustomErrorCode
    /// </summary>
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
