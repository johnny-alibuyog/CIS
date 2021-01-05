using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Touchless.Multitouch;
using Touchless.Vision.Service;

namespace Touchless.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TouchlessInputProvider _touchlessInputProvider;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

            ComposableTouchlessService touchlessService = ComposableTouchlessService.GetComposedInstance();
            _touchlessInputProvider = new TouchlessInputProvider(touchlessService);
            configurationDisplay.Content = _touchlessInputProvider.GetConfiguration();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _touchlessInputProvider.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _touchlessInputProvider.Stop();
            base.OnClosing(e);
        }
    }
}