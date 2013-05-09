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
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Controls;

namespace CIS.UI.Features.Polices.Warrants
{
    /// <summary>
    /// Interaction logic for WarratnView.xaml
    /// </summary>
    public partial class WarratnView : DialogBase
    {
        public WarratnView() : base()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = new WarrantViewModel();
            }
        }
    }
}
