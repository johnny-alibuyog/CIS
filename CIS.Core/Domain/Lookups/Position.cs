using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIS.Core.Entities.Lookups
{
    public class Position
    {
        private string _id;
        private string _name;

        public virtual string Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        #region Constructors

        public Position() { }

        public Position(string id, string name)
        {
            _id = id;
            _name = name;
        }

        #endregion
    }
}
