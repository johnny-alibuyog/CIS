using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Engine;

namespace CIS.Data.Commons.Exceptions
{
    public interface IExceptionBuilder<TException> where TException : Exception
    {
        TException Build(params InvalidValue[] invalidValues);
    }
}
