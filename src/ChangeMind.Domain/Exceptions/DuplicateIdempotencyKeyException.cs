namespace ChangeMind.Domain.Exceptions;

public sealed class DuplicateIdempotencyKeyException : ConflictException
{
    public DuplicateIdempotencyKeyException(Guid idempotencyKey)
        : base($"A payment with idempotency key '{idempotencyKey}' has already been processed.") { }
}
