using NHibernate.Validator.Engine;

namespace CIS.Data.Common.Exception
{
    public interface IInvalidValueMessageBuilder
    {
        string Build(params InvalidValue[] invalidValues);
        string Build(string key, params InvalidValue[] invalidValues);
    }
}
