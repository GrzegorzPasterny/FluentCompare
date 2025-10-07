public class ComparisonError
{
    public string Code { get; }
    public string Message { get; }
    public Exception? Exception { get; }

    internal ComparisonError(string code, string message, Exception? exception = null)
    {
        Code = code;
        Message = message;
        Exception = exception;
    }

    public override string ToString()
    {
        return Exception == null
            ? $"{Code}: {Message}"
            : $"{Code}: {Message}. Exception: {Exception.Message}";
    }
}
