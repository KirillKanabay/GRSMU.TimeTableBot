using AutoMapper;
using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Domain.Users.Dtos;

namespace GRSMU.Bot.Logic.Features.Users.Queries.GetById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ExecutionResult<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var document = await _userRepository.GetByIdAsync(request.Id);

        if (document is null)
        {
            return ExecutionResult.Failure<UserDto>(Error.NotFound("User.NotFound", "User not found"));
        }

        var dto = _mapper.Map<UserDto>(document);

        return ExecutionResult.Success(dto);
    }
}