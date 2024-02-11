using NHibernate.Validator.Engine;
using System;

namespace CIS.Data.Common.Exception;

public interface IExceptionBuilder<TException> where TException : System.Exception
{
    TException Build(params InvalidValue[] invalidValues);
}
