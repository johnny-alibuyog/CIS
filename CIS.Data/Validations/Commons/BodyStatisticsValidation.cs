using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.Validations.Commons
{
    public class BodyStatisticsValidation : ValidationDef<BodyStatistics>
    {
        public BodyStatisticsValidation()
        {
            Define(x => x.Height)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);

            Define(x => x.Weight)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);
        }
    }
}
