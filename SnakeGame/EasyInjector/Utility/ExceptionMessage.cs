namespace EasyInjector.Utility;

public static class ExceptionMessage
{
    // class Dependency & ValidateNotNullParams methods in DependencyProvider class
    
    internal const string TypeCannotBeNull = "Type cannot be null.";
    internal const string InstanceCannotBeNull = "Instance cannot be null.";
    internal const string FuncCannotBeNull = "Factory Function cannot be null.";
    
    // In DependencyProvider class
    // ValidateAbstractTypeIsInstanceOfImplementation methods
    
    internal const string AbstractTypeIsNotTypeOfInstance = "Instance of type \"{0}\" is not \"{1}\" type.";

    // ValidateTypeDoesNotHaveMapping method
    
    internal const string TypeAlreadyRegisteredMessage = "The type \"{0}\" is already registered.";
    
    // Create method
    
    internal const string TypeDoesNotHaveConstructor = "The type \"{0}\" does not have a public instance constructor.";
    internal const string TypeHasMoreThanOneConstructor = "The type \"{0}\" has more than 1 public instance constructor.";
    
    internal const string TypeNotRegistered = "The type \"{0}\" is not registered.";
    internal const string TypeDependsOnNonRegisteredType = "The type \"{0}\" depends on \"{1}\" type, which is not registered.";
    internal const string DependencyNotInValidState = "Dependency is not in a valid state.";
}