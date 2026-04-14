namespace ChangeMind.Application.UseCases.Exercises.Queries;

using ChangeMind.Application.DTOs;
using MediatR;

public class GetExerciseByIdQuery : IRequest<ExerciseDto>
{
    public Guid ExerciseId { get; set; }
}
