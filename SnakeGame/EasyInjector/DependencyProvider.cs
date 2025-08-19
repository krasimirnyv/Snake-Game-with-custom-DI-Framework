using System.Reflection;
using EasyInjector.Utility;

namespace EasyInjector;

public class DependencyProvider
{
    private readonly IDictionary<Type, Dependency> _mappedDependencies;

    internal DependencyProvider()
    {
        _mappedDependencies = new Dictionary<Type, Dependency>();
    }

    public DependencyProvider Register<TAbstractType, TImplementationType>()
        where TImplementationType : class, TAbstractType
    {
        Type abstractType = typeof(TAbstractType);
        Type implementationType = typeof(TImplementationType);

        ValidateNotNullParams(abstractType, implementationType);
        ValidateTypeDoesNotHaveMapping(abstractType);

        _mappedDependencies[abstractType] = new Dependency(implementationType);
        
        return this;
    }

    public DependencyProvider Register<TAbstractType>(object implementationInstance)
        where TAbstractType : class
    {
        Type abstractType = typeof(TAbstractType);
        
        ValidateNotNullParams(abstractType, implementationInstance);
        ValidateTypeDoesNotHaveMapping(abstractType);

        _mappedDependencies[abstractType] = new Dependency(implementationInstance);

        return this;
    }

    public DependencyProvider Register<TAbstractType, TImplementationType>(Func<DependencyProvider, TImplementationType> factoryFunc) 
        where TImplementationType : class, TAbstractType
    {
        Type abstractType = typeof(TAbstractType);
        Type implementationType = typeof(TImplementationType);
        
        ValidateNotNullParams(abstractType, implementationType, factoryFunc);
        ValidateTypeDoesNotHaveMapping(abstractType);
        
        _mappedDependencies[abstractType] = new Dependency(factoryFunc);

        return this;
    }
    
    public DependencyProvider Register(Type abstractType, Type implementationType)
    {
        ValidateNotNullParams(abstractType, implementationType);
        ValidateTypeDoesNotHaveMapping(abstractType);

        _mappedDependencies[abstractType] = new Dependency(implementationType);
        return this;
    }

    public DependencyProvider Register(Type abstractType, object implementationInstance)
    {
        ValidateNotNullParams(abstractType, implementationInstance);
        ValidateTypeDoesNotHaveMapping(abstractType);

        _mappedDependencies[abstractType] = new Dependency(implementationInstance);
        return this;
    }

    public DependencyProvider Register(Type abstractType, Func<DependencyProvider, object> factory)
    {
        ValidateNotNullParams(abstractType, factory);
        ValidateTypeDoesNotHaveMapping(abstractType);

        _mappedDependencies[abstractType] = new Dependency(factory);
        return this;
    }

    public TType Create<TType>()
        where TType : class
    {
        Type type = typeof(TType);
        return (TType)Create(type);
    }

    public object Create(Type type)
    {
        if (type is null)
            throw new ArgumentNullException(ExceptionMessage.TypeCannotBeNull);
        
        if (type.IsInterface)
        {
            ValidateTypeAlreadyExist(type);
            return ResolveDependency(type);
        }
        
        ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

        if (constructors.Length == 0)
            throw new InvalidOperationException(string.Format(ExceptionMessage.TypeDoesNotHaveConstructor, type.FullName));

        if (constructors.Length > 1)
            throw new InvalidOperationException(string.Format(ExceptionMessage.TypeHasMoreThanOneConstructor, type.FullName));
        
        ConstructorInfo constructor = constructors.First();
        ParameterInfo[] parameters = constructor.GetParameters();

        List<object> parameterInstances = new List<object>();
        foreach (ParameterInfo parameter in parameters)
        {
            Type parameterType = parameter.ParameterType;
            
            ValidateTypeAlreadyExist(type, parameterType);

            object parameterInstance = ResolveDependency(parameterType);
            parameterInstances.Add(parameterInstance);
        }
        
        object result = constructor.Invoke(parameterInstances.ToArray());
        
        PopulateInjectableFields(type, result);
        
        return result;
    }

    private object ResolveDependency(Type type)
    {
        Dependency dependency = _mappedDependencies[type];

        if (dependency.Instance is not null)
        {
            return dependency.Instance;
        }
        else if (dependency.Type is not null)
        {
            object parameterInstance = Create(dependency.Type);
            return parameterInstance;
        }
        else if (dependency.FactoryFunc is not null)
        {
            object parameterInstance = dependency.FactoryFunc(this);
            return parameterInstance;
        }
        else
            throw new InvalidOperationException(ExceptionMessage.DependencyNotInValidState);
    }

    private void PopulateInjectableFields(Type type, object instance)
    {
        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        
        if(fields.Length == 0)
            return;

        List<FieldInfo> injectableFields = fields
            .Where(f => f.GetCustomAttribute<InjectAttribute>() != null)
            .ToList();

        foreach (FieldInfo field in injectableFields)
        {
            Type fieldType = field.FieldType;
            object fieldValue = Create(fieldType);
            
            field.SetValue(instance, fieldValue);
        }
    }
    
    private void ValidateNotNullParams(Type abstractType, Type implementationType)
    {
        if (abstractType is null) throw new ArgumentNullException(ExceptionMessage.TypeCannotBeNull);
        if (implementationType is null) throw new ArgumentNullException(ExceptionMessage.InstanceCannotBeNull);
    }
    
    private void ValidateNotNullParams(Type abstractType, Type implementationType, Func<DependencyProvider, object> factory)
    {
        if (abstractType is null) throw new ArgumentNullException(ExceptionMessage.TypeCannotBeNull);
        if (implementationType is null) throw new ArgumentNullException(ExceptionMessage.InstanceCannotBeNull);
        if (factory is null) throw new ArgumentNullException(ExceptionMessage.FuncCannotBeNull);
    }
    
    private void ValidateNotNullParams(Type abstractType, object implementationInstance)
    {
        if (abstractType is null) throw new ArgumentNullException(ExceptionMessage.TypeCannotBeNull);
        if (implementationInstance is null) throw new ArgumentNullException(ExceptionMessage.InstanceCannotBeNull);
    }
    
    private void ValidateNotNullParams(Type abstractType, Func<DependencyProvider, object> factory)
    {
        if (abstractType is null) throw new ArgumentNullException(ExceptionMessage.TypeCannotBeNull);
        if (factory is null) throw new ArgumentNullException(ExceptionMessage.FuncCannotBeNull);
    }
    
    private void ValidateTypeDoesNotHaveMapping(Type abstractType)
    {
        if (_mappedDependencies.ContainsKey(abstractType))
            throw new InvalidOperationException(string.Format(ExceptionMessage.TypeAlreadyRegisteredMessage, abstractType.FullName));
    }
    
    private void ValidateTypeAlreadyExist(Type type)
    {
        if(!_mappedDependencies.ContainsKey(type))
            throw new InvalidOperationException(string.Format(ExceptionMessage.TypeNotRegistered, type.FullName));
    }
    
    private void ValidateTypeAlreadyExist(Type type, Type parameterType)
    {
        if(!_mappedDependencies.ContainsKey(parameterType))
            throw new InvalidOperationException(string.Format(ExceptionMessage.TypeDependsOnNonRegisteredType, type.FullName, parameterType.FullName));
    }
}