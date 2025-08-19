namespace EasyInjector;

/// <summary>
/// Injects dependencies directly in private instance fields.
/// Static fields are not supported.
/// </summary>

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class InjectAttribute : Attribute
{
    
}