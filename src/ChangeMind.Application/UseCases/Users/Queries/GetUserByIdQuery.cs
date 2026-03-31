namespace ChangeMind.Application.UseCases.Users.Queries;

using MediatR;
using ChangeMind.Application.DTOs;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public Guid UserId { get; set; }
}
