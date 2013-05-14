using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIS.Core.Entities.Commons;
using NHibernate.Validator.Cfg.Loquacious;

namespace CIS.Data.EntityDefinition.Commons
{
    public class PictureValidation : ValidationDef<Picture>
    {
        public PictureValidation()
        {
            Define(x => x.Id);

            Define(x => x.Image);
        }
    }
}
