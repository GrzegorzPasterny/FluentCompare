
public class ComparisonMismatch
{
    public string Code { get; }
    public string Message { get; }

    internal ComparisonMismatch(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public override string ToString() => $"{Code}: {Message}";
}
