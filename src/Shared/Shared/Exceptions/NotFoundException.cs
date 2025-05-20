namespace Eshop.Shared.Exceptions;

public class NotFoundException: Exception
{
    public NotFoundException(string message): base(message) { }
    public NotFoundException(string entityName, object key): base($"Could not find {entityName} with id: {key}") { }
}