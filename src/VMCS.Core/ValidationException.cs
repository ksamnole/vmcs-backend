namespace VMCS.Core;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, object value) : base(message)
    {
        Value = value;
    }

    public object Value { get; }
}