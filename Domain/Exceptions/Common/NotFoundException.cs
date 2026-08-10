namespace Shopniu_identity.Domain.Exceptions.Common;

public class NotFoundException : DomainException
{
    public NotFoundException(string entityName, object entityId)
        : base($"The {entityName} with ID {entityId} was not found.")
    {

    }

    // public NotFoundExeption(string message) : base(message)
    // {

    // }
}