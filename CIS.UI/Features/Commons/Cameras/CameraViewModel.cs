using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using NHibernate.Validator.Constraints;
using ReactiveUI;
using ReactiveUI.Xaml;
using Touchless.Vision.Camera;

namespace CIS.UI.Features.Commons.Cameras
{
    public class CameraViewModel : ViewModelBase
    {
        private readonly CameraController _cameraController;

        [NotNull(Message="Picture is mandataroy")]
        public virtual BitmapSource Picture { get; set; }

        public virtual BitmapSource ImagePreview { get; set; }

        public virtual Camera SelectedCamera { get; set; }

        public virtual IReactiveList<Camera> Cameras { get; set; }

        public virtual string SelectedDevice { get; set; }

        public virtual IReactiveCommand Start { get; set; }

        public virtual IReactiveCommand Stop { get; set; }

        public virtual IReactiveCommand Continue { get; set; }

        public virtual IReactiveCommand Capture { get; set; }

        public CameraViewModel()
        {
            _cameraController = new CameraController(this);
        }
    }
}
