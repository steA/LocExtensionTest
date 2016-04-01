using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using LocExtensionTest.ViewModels;

namespace LocExtensionTest
{
  public class Bootstrapper : BootstrapperBase
  {
    private CompositionContainer _container;

    public Bootstrapper()
    {
      Initialize();
    }

    protected override void OnStartup(object sender, StartupEventArgs e)
    {
      DisplayRootViewFor<LocExtensionViewModel>();
    }

    protected override void Configure()
    {
      var catalog = new AggregateCatalog();

      var composablePartCatalog = AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>();

      foreach (var part in composablePartCatalog)
      {
        catalog.Catalogs.Add(part);
      }

      _container = new CompositionContainer(catalog);

      var batch = new CompositionBatch();

      batch.AddExportedValue<IWindowManager>(new WindowManager());
      batch.AddExportedValue<IEventAggregator>(new EventAggregator());
      batch.AddExportedValue(_container);
      _container.Compose(batch);
    }
  }
}
