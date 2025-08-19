namespace EasyInjector;

public static class Injector
{
    public static DependencyProvider Register<TAbstractType, TImplementationType>()
        where TImplementationType : class, TAbstractType
    {
        DependencyProvider dependencyProvider = new DependencyProvider();
        
        dependencyProvider.Register<TAbstractType, TImplementationType>();
        
        return dependencyProvider;
    }

    public static DependencyProvider Register<TAbstractType>(object implementationInstance)
        where TAbstractType : class
    {
        DependencyProvider dependencyProvider = new DependencyProvider();

        dependencyProvider.Register<TAbstractType>(implementationInstance);

        return dependencyProvider;
    }
    
    public static DependencyProvider Register<TAbstractType, TImplementationType>(Func<DependencyProvider, TImplementationType> factoryFunc) 
        where TImplementationType : class, TAbstractType
    {
        DependencyProvider dependencyProvider = new DependencyProvider();

        dependencyProvider.Register<TAbstractType, TImplementationType>(factoryFunc);

        return dependencyProvider;
    }
    
    public static DependencyProvider Register(Type abstractType, Type implementationType)
    {
        DependencyProvider dependencyProvider = new DependencyProvider();
        
        dependencyProvider.Register(abstractType, implementationType);
        
        return dependencyProvider;
    }

    public static DependencyProvider Register(Type abstractType, object implementationInstance)
    {
        DependencyProvider dependencyProvider = new DependencyProvider();

        dependencyProvider.Register(abstractType, implementationInstance);

        return dependencyProvider;
    }

    public static DependencyProvider Register(Type abstractType, Func<DependencyProvider, object> factory)
    {
        DependencyProvider dependencyProvider = new DependencyProvider();

        dependencyProvider.Register(abstractType, factory);

        return dependencyProvider;
    }
}