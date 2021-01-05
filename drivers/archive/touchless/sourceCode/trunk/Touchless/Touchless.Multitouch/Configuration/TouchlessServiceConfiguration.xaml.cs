using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Touchless.Multitouch.Configuration;
using Touchless.Shared.Extensions;
using Touchless.Vision.Contracts;
using Touchless.Vision.Service;

namespace Touchless.Multitouch.Configuration
{
    /// <summary>
    /// Interaction logic for TouchlessServiceConfiguration.xaml
    /// </summary>
    public partial class TouchlessServiceConfiguration : Window
    {
        private readonly ComposableTouchlessService _touchlessService;

        public TouchlessServiceConfiguration()
            : this(null)
        {
        }

        public TouchlessServiceConfiguration(ComposableTouchlessService touchlessService)
        {
            _touchlessService = touchlessService;
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                PopulateDisplay();
            }
        }

        private void PopulateDisplay()
        {
            var registeredFrameSources = _touchlessService.RegisteredFrameSources;
            var registeredObjectDetectors = _touchlessService.RegisteredObjectDetectors;

            this.lstFrameSources.ItemsSource = BuildAddInModels(_touchlessService.AvailableFrameSources.Select(i => i as ITouchlessAddIn),
                                                                   registeredFrameSources.Select(i => i as ITouchlessAddIn).ToList());

            this.lstObjectDetectors.ItemsSource = BuildAddInModels(_touchlessService.AvailableObjectDetectors.Select(i => i as ITouchlessAddIn),
                                                                   registeredObjectDetectors.Select(i => i as ITouchlessAddIn).ToList());

            if (this.lstFrameSources.Items.Count > 0)
            {
                this.lstFrameSources.SelectedIndex = 0;
            }

            if (this.lstObjectDetectors.Items.Count > 0)
            {
                this.lstObjectDetectors.SelectedIndex = 0;
            }
        }

        private List<AddInModel> BuildAddInModels(IEnumerable<ITouchlessAddIn> addIns, ICollection<ITouchlessAddIn> registeredAddIns)
        {
            var addInModels = new List<AddInModel>();

            foreach (var addIn in addIns)
            {
                var addInModel = new AddInModel(addIn)
                {
                    IsRegistered = registeredAddIns.Contains(addIn)
                };
                addInModel.RegistrationRequested += AddInRegistrationRequested;
                addInModel.UnregistrationRequested += AddInUnregistrationRequested;
                addInModels.Add(addInModel);
            }

            return addInModels;
        }

        void AddInUnregistrationRequested(AddInModel arg1, ITouchlessAddIn addIn)
        {
            if (addIn is IFrameSource)
            {
                var frameSource = addIn as IFrameSource;
                _touchlessService.Unregister(frameSource);
                frameSource.StopFrameCapture();
            }
            else if (addIn is IObjectDetector)
            {
                _touchlessService.Unregister(addIn as IObjectDetector);
            }
        }

        void AddInRegistrationRequested(AddInModel arg1, ITouchlessAddIn addIn)
        {
            if (addIn is IFrameSource)
            {
                var frameSource = addIn as IFrameSource;
                _touchlessService.Register(frameSource);

                if (_touchlessService.IsFrameProcessing)
                {
                    frameSource.StartFrameCapture();
                }
            }
            else if (addIn is IObjectDetector)
            {
                _touchlessService.Register(addIn as IObjectDetector);
            }
        }

        private void DisplayAddInConfiguration(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListBox;

            if (list != null && list.SelectedItem != null && list.SelectedItem is AddInModel)
            {
                var addin = (list.SelectedItem as AddInModel).AddIn;
                if (addin != null)
                {
                    if (addin is IFrameSource)
                    {
                        this.frameSourceConfiguration.Content = addin.HasConfiguration
                                                                    ? addin.ConfigurationElement
                                                                    : null;
                    }
                    else if (addin is IObjectDetector)
                    {
                        this.objectDetectorConfiguration.Content = addin.HasConfiguration
                                                                       ? addin.ConfigurationElement
                                                                       : null;
                    }
                }
            }
        }
    }
}
