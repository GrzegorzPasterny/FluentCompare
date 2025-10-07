
public class ComparisonMismatch
{
    public string Code { get; }
    public string Message { get; }
    public string VerboseMessage { get; }

    internal ComparisonMismatch(string code, string message, string verboseMessage = "")
    {
        Code = code;
        Message = message;

        if (string.IsNullOrWhiteSpace(verboseMessage))
            VerboseMessage = string.Empty;
        else
            VerboseMessage = verboseMessage;
    }

    public override string ToString() => $"{Code}: {Message}";
}
