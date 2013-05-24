using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS.UI.Features.Commons.Biometrics;

namespace CIS.UI.Features.Polices.Maintenances
{
    public class SettingFingerViewModel : ViewModelBase
    {
        public virtual bool Include { get; set; }

        public virtual FingerViewModel Finger { get; set; }

        #region Constructors

        public SettingFingerViewModel() { }

        public SettingFingerViewModel(FingerViewModel finger) : this(false, finger) { }

        public SettingFingerViewModel(bool include, FingerViewModel finger)
        {
            this.Include = include;
            this.Finger = finger;
        }

        #endregion
    }
}
