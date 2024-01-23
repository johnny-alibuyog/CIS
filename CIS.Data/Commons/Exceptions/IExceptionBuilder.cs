using NHibernate.Validator.Engine;
using System;

namespace CIS.Data.Commons.Exceptions;

public interface IExceptionBuilder<TException> where TException : Exception
{
    TException Build(params InvalidValue[] invalidValues);
}
