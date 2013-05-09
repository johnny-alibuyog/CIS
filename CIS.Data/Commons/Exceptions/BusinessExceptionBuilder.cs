using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Engine;

namespace CIS.Data.Commons.Exceptions
{
    public class BusinessExceptionBuilder : IExceptionBuilder<BusinessException>
    {
        public BusinessException Build(params InvalidValue[] invalidValues)
        {
            var messageBuilder = new StringBuilder();
            const string MESSAGE_FORMAT = "{0}.{1} has an invalid value of {2}.";

            foreach (var invalidValue in invalidValues)
            {
                var message = string.IsNullOrWhiteSpace(invalidValue.Message)
                    ? string.Format(MESSAGE_FORMAT, invalidValue.EntityType.Name, invalidValue.PropertyName, invalidValue.Value)
                    : invalidValue.Message;

                messageBuilder.AppendLine(message);
            }

            return new BusinessException(messageBuilder.ToString());
        }
    }
}
