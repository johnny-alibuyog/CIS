using System;
using System.ComponentModel;
using Touchless.Shared.Extensions;
using Touchless.Vision.Contracts;

namespace Touchless.Multitouch.Configuration
{
    public class AddInModel : INotifyPropertyChanged
    {
        private bool _isRegistered;

        public AddInModel(ITouchlessAddIn addIn)
        {
            AddIn = addIn;
        }

        public ITouchlessAddIn AddIn { get; private set; }

        public bool IsRegistered
        {
            get { return _isRegistered; }
            set
            {
                if (_isRegistered != value)
                {
                    if (value)
                    {
                        RegistrationRequested.IfNotNull(i => i(this, AddIn));
                    }
                    else
                    {
                        UnregistrationRequested.IfNotNull(i => i(this, AddIn));
                    }

                    _isRegistered = value;
                    NotifyPropertyChanged("IsRegistered");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public event Action<AddInModel, ITouchlessAddIn> RegistrationRequested , UnregistrationRequested;

        private void NotifyPropertyChanged(string propertyName)
        {
            //TODO: Put this on a Dispatcher
            PropertyChanged.IfNotNull(i => i(this, new PropertyChangedEventArgs(propertyName)));
        }
    }
}