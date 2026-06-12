namespace MicrowaveApp.Domain.Exceptions;

public class BusinessException : ApplicationException
{
    public string ErrorCode { get;}

    public BusinessException(string message, string errorCode = "BUSINESS_ERROR")
        : base(message)
    {
        ErrorCode = errorCode;
    }
    
}