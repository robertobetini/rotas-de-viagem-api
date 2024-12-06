namespace Application.Exceptions;

[Serializable]
public class SameOriginAndDestinationException : Exception
{
    public SameOriginAndDestinationException() { }

    public SameOriginAndDestinationException(string? message) : base(message) { }

    public SameOriginAndDestinationException(string? message, Exception? innerException) : base(message, innerException) { }
}