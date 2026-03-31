namespace ChangeMind.Application.UseCases.Users.Commands;

using MediatR;

public class DeleteUserCommand : IRequest
{
    public Guid UserId { get; set; }
}
