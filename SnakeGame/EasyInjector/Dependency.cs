using EasyInjector.Utility;

namespace EasyInjector;

internal class Dependency
{
    public Dependency(Type type)
    {
        Type = type ?? throw new ArgumentNullException(ExceptionMessage.TypeCannotBeNull);
    }

    public Dependency(object instance)
    {
        Instance = instance ?? throw new ArgumentNullException(ExceptionMessage.InstanceCannotBeNull);
    }
    
    public Dependency(Func<DependencyProvider, object> factoryFunc)
    {
        FactoryFunc = factoryFunc ?? throw new ArgumentNullException(ExceptionMessage.FuncCannotBeNull);
    }
    
    public Type Type { get; private set; }
    public object Instance { get; private set; }
    public Func<DependencyProvider, object> FactoryFunc { get; private set; }
}