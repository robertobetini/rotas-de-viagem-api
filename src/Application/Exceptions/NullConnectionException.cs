namespace Application.Exceptions;

[Serializable]
public class NullConnectionException : Exception
{
    public NullConnectionException() { }

    public NullConnectionException(string? message) : base(message) { }

    public NullConnectionException(string origin, string destination) 
        : base($"Connection between {origin} and {destination} does not exist") { }

    public NullConnectionException(string? message, Exception? innerException) : base(message, innerException) { }
}