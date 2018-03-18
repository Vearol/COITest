using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMC.DataModels.TestHelper;

namespace PMC.DataModels.Tests
{
    public static class CustomAsserts
    {
        public static void AssertCustomException(Action func, CustomErrorCode code)
        {
            try
            {
                func.Invoke();
            }
            catch (CustomException e)
            {
                var exceptionThrown = e.ErrorCode != null && (CustomErrorCode)e.ErrorCode == code;

                if (!exceptionThrown)
                {
                    throw new AssertFailedException(
                        $"An exception of type {typeof(CustomException)} was expected, but not thrown, when {code}");
                }
            }
        }
    }
}
