using AutoMapper;
using GRSMU.Bot.Common.Broker.Handlers;
using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Core.DataLoaders;
using GRSMU.Bot.Data.Users.Contracts;
using GRSMU.Bot.Data.Users.Contracts.Filters;
using GRSMU.Bot.Data.Users.Documents;
using GRSMU.Bot.Domain.Users.Dtos;
using GRSMU.Bot.Domain.Users.Requests;

namespace GRSMU.Bot.Application.Users.Handlers
{
    public class GetUsersRequestHandler : RequestHandlerBase<GetUsersRequestMessage, ItemPagedResponse<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly FormDataLoader _formDataLoader;

        public GetUsersRequestHandler(IMapper mapper, IUserRepository userRepository, FormDataLoader formDataLoader)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _formDataLoader = formDataLoader ?? throw new ArgumentNullException(nameof(formDataLoader));
        }

        protected override async Task<ItemPagedResponse<UserDto>> ExecuteAsync(GetUsersRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new ItemPagedResponse<UserDto>
            {
                PagingModel = request.Paging
            };

            var filter = _mapper.Map<UserFilter>(request.Filter);

            var documents = await _userRepository.GetUserListAsync(filter, request.Paging);

            response.Items = await CreateDtos(documents);

            return response;
        }

        private async Task<List<UserDto>> CreateDtos(List<UserDocument> documents)
        {
            var dtos = _mapper.Map<List<UserDto>>(documents);

            var courses = await _formDataLoader.GetCoursesAsync();
            var faculties = await _formDataLoader.GetFacultiesAsync();

            foreach (var userDto in dtos)
            {
                var groups = await _formDataLoader.GetGroupsAsync(userDto.FacultyId, userDto.CourseId);

                userDto.CourseName = GetFormattedFormValue(courses, userDto.CourseId);
                userDto.FacultyName = GetFormattedFormValue(faculties, userDto.FacultyId);
                userDto.GroupName = GetFormattedFormValue(groups, userDto.GroupId);
            }

            return dtos;
        }

        private string GetFormattedFormValue(IReadOnlyDictionary<string, string> dictionary, string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (dictionary.Values.Contains(value, StringComparer.InvariantCultureIgnoreCase))
            {
                return dictionary.First(x => x.Value.Equals(value)).Key;
            }

            return string.Empty;
        }
    }
}
