using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CIS.UI.Utilities.Converters;

namespace CIS.UI.Features.Commons.Addresses
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class AddresssView : UserControl
    {
        public AddresssView()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                if (AppDomain.CurrentDomain.BaseDirectory.Contains("Blend 4"))
                {
                    //// load styles resources
                    //var resourceDictionary  = new ResourceDictionary();
                    //resourceDictionary.Source = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "/Assets/Styles/FormInputMatrix.xaml"), UriKind.Absolute);
                    //this.Resources.MergedDictionaries.Add(resourceDictionary);

                    //// load any other resources this control needs such as Converters
                    //this.Resources.Add("EnumToBooleanConverter", new EnumToBooleanConverter());
                }
            }

            InitializeComponent();
        }
    }
}
