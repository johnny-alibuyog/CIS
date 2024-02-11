using NHibernate.Validator.Engine;
using System.Text;

namespace CIS.Data.Common.Exception;

public class InvalidaValueMessageBuilder : IInvalidValueMessageBuilder
{
    public string Build(params InvalidValue[] invalidValues)
    {
        var messageBuilder = new StringBuilder();
        const string MESSAGE_FORMAT = "{0}.{1} has an invalid value of {2}.";

        foreach (var invalidValue in invalidValues)
        {
            var message = string.IsNullOrWhiteSpace(invalidValue.Message)
                ? string.Format(MESSAGE_FORMAT,
                    invalidValue.EntityType.Name,
                    invalidValue.PropertyName,
                    invalidValue.Value)
                : invalidValue.Message;

            messageBuilder.AppendLine(message);
        }
        return messageBuilder.ToString();
    }

    public string Build(string key, params InvalidValue[] invalidValues)
    {
        var messageBuilder = new StringBuilder();
        return messageBuilder
            .AppendFormat("Invalid values found for '{0}':", key ?? string.Empty)
            .AppendLine(this.Build(invalidValues))
            .ToString();
    }
}
