using AutoMapper;
using GRSMU.Bot.Application.Features.Gradebooks.Models;
using GRSMU.Bot.Common.Extensions;
using GRSMU.Bot.Common.Models.Options;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Models;
using GRSMU.Bot.Common.Telegram.Services;
using GRSMU.Bot.Data.Gradebooks.Contracts;
using GRSMU.Bot.Data.Gradebooks.Documents;
using GRSMU.Bot.Domain.Gradebooks.Dtos;

namespace GRSMU.Bot.Application.Features.Gradebooks.Helpers
{
    public class GradebookProcessor
    {
        private readonly SourceOptions _options;
        private readonly GradebookParser _parser;
        private readonly IGradebookRepository _gradebookRepository;
        private readonly ITelegramUserService _userService;
        private readonly IMapper _mapper;
        private readonly GradebookIdGenerator _gradebookIdGenerator;

        public GradebookProcessor(SourceOptions options, GradebookParser parser, IGradebookRepository gradebookRepository, ITelegramUserService userService, IMapper mapper, GradebookIdGenerator gradebookIdGenerator)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _gradebookRepository = gradebookRepository ?? throw new ArgumentNullException(nameof(gradebookRepository));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _gradebookIdGenerator = gradebookIdGenerator ?? throw new ArgumentNullException(nameof(gradebookIdGenerator));
        }

        public async Task<bool> TrySignInAsync(TelegramUser user)
        {
            var login = user.Login;
            var password = user.StudentCardId;

            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var rawPage = await GetRawPageAsync(login, password);

            if (rawPage == null)
            {
                return false;
            }

            var result = await _parser.ParseAsync(rawPage);

            if (!result.SignInSuccessful)
            {
                return false;
            }

            await UpdateGradebook(user, result);

            return true;
        }
        
        private async Task UpdateGradebook(TelegramUser user, GradebookParserResult result)
        {
            if (string.IsNullOrWhiteSpace(user.StudentFullName))
            {
                user.StudentFullName = result.StudentFullName;
                await _userService.UpdateUserAsync(user);
            }

            var dto = result.Result;
            dto.UserId = user.MongoId;

            foreach (var discipline in dto.Disciplines)
            {
                discipline.Id = await _gradebookIdGenerator.GenerateIdAsync(discipline.Name);
            }

            var document = _mapper.Map<GradebookDocument>(dto);

            await _gradebookRepository.DeleteGradebookByUserAsync(dto.UserId);
            await _gradebookRepository.InsertAsync(document);
        }

        public async Task<GradebookDto> GetGradebookDto(TelegramUser user)
        {
            var document = await _gradebookRepository.GetByUserAsync(user.MongoId);

            if (document == null || (document.CreatedDate.HasValue && (document.CreatedDate.Value - DateTime.UtcNow).TotalHours > 1))
            {
                if (await TrySignInAsync(user))
                {
                    document = await _gradebookRepository.GetByUserAsync(user.MongoId);
                }
                else
                {
                    return null;
                }
            }
            
            return _mapper.Map<GradebookDto>(document);
        }

        private async Task<string> GetRawPageAsync(string login, string password)
        {
            var formParams = new Dictionary<string, string>();

            formParams.Upsert(RequestKeys.Login, login);
            formParams.Upsert(RequestKeys.Password, password);

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(formParams);
                var result = await client.PostAsync(_options.GradebookUrl, content);

                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
