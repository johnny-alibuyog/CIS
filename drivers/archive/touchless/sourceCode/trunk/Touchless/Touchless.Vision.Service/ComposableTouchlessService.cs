using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using Touchless.Shared.Extensions;
using Touchless.Vision.Contracts;

namespace Touchless.Vision.Service
{
    [Export]
    public sealed class ComposableTouchlessService : TouchlessService, INotifyImportSatisfaction
    {
        public ComposableTouchlessService()
        {
            AvailableFrameSources = new List<IFrameSource>();
            AvailableObjectDetectors = new List<IObjectDetector>();
        }

        [Import]
        //[ImportMany(typeof(IFrameSource))]
            public IList<IFrameSource> AvailableFrameSources { get; private set; }

        [Import]
        //[ImportMany(typeof (IObjectDetector))]
        public IList<IObjectDetector> AvailableObjectDetectors { get; private set; }

        #region INotifyImportSatisfaction Members

        public void ImportCompleted()
        {
            AvailableObjectDetectors.ForEach(Register);
            AvailableFrameSources.ForEach(Register);
        }

        #endregion

        public static ComposableTouchlessService GetComposedInstance()
        {
            var directoryCatalog = new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var compositionContainer = new CompositionContainer(directoryCatalog);
            var instance = new ComposableTouchlessService();    
            compositionContainer.SatisfyImports(instance);

            return instance;
        }
    }
}