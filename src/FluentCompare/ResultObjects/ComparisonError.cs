public class ComparisonError
{
    public string Message { get; }
    public Exception? Exception { get; }

    public ComparisonError(string message, Exception? exception = null)
    {
        Message = message;
        Exception = exception;
    }

    public override string ToString()
    {
        return Exception == null
            ? $"Comparison error: {Message}"
            : $"Comparison error: {Message}. Exception: {Exception.Message}";
    }
}
