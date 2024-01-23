namespace CIS.UI.Bootstraps.InversionOfControl;

public static class IoC
{
    public static IDependencyResolver Container => Ninject.DependencyResolver.Instance; // Castle.DependencyResolver.Instance;
}
