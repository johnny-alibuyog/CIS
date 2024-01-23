using System;
using System.Runtime.Serialization;

namespace CIS.Data.Commons.Exceptions;

public class BusinessException : Exception
{
    public BusinessException() : base() { }
    public BusinessException(string message) : base(message) { }
    public BusinessException(string message, Exception innerException) : base(message, innerException) { }
    public BusinessException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

public class AuthenticationException : BusinessException
{
    public AuthenticationException() : base() { }
    public AuthenticationException(string message) : base(message) { }
}

public class AuthorizationException : BusinessException
{
    public AuthorizationException() : base() { }
    public AuthorizationException(string message) : base(message) { }
}
