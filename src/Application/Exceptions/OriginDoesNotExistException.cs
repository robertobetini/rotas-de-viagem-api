namespace Application.Exceptions;

[Serializable]
public class OriginDoesNotExistException : Exception
{
    public OriginDoesNotExistException() { }

    public OriginDoesNotExistException(string? cityName) : base($"No connection starting at city {cityName} exist") { }

    public OriginDoesNotExistException(string? message, Exception? innerException) : base(message, innerException) { }
}