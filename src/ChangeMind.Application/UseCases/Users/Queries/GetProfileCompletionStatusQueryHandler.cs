namespace ChangeMind.Application.UseCases.Users.Queries;

using MediatR;
using ChangeMind.Application.Repositories;
using ChangeMind.Domain.Exceptions;

public class GetProfileCompletionStatusQueryHandler(
    IUserRepository userRepository) : IRequestHandler<GetProfileCompletionStatusQuery, bool>
{
    public async Task<bool> Handle(GetProfileCompletionStatusQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User with ID '{request.UserId}' not found.");

        return user.IsCompletedProfile;
    }
}
