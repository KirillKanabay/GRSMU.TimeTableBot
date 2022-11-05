using System.Globalization;
using AutoMapper;
using GRSMU.TimeTableBot.Common.Broker.Handlers;
using GRSMU.TimeTableBot.Common.Extensions;
using GRSMU.TimeTableBot.Common.Models;
using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Core.DataLoaders;
using GRSMU.TimeTableBot.Data.TimeTable.Contracts;
using GRSMU.TimeTableBot.Data.TimeTable.Documents;
using GRSMU.TimeTableBot.Domain.Timetables.Requests;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace GRSMU.TimeTableBot.Application.Timetables.Handlers
{
    public class GrabTimeTablesRequestHandler : TelegramRequestHandlerBase<GrabTimeTablesRequestMessage>
    {
        private readonly ITimeTableLoader _timeTableLoader;
        private readonly FormDataLoader _formDataLoader;
        private readonly IMapper _mapper;
        private readonly ITimeTableRepository _timeTableRepository;
        private readonly ILogger<GrabTimeTablesRequestHandler> _logger;

        public GrabTimeTablesRequestHandler(ITelegramBotClient client, ITimeTableLoader timeTableLoader, FormDataLoader formDataLoader, IMapper mapper, ITimeTableRepository timeTableRepository, ILogger<GrabTimeTablesRequestHandler> logger) : base(client)
        {
            _timeTableLoader = timeTableLoader ?? throw new ArgumentNullException(nameof(timeTableLoader));
            _formDataLoader = formDataLoader ?? throw new ArgumentNullException(nameof(formDataLoader));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _timeTableRepository = timeTableRepository ?? throw new ArgumentNullException(nameof(timeTableRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<EmptyResponse> ExecuteAsync(GrabTimeTablesRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new EmptyResponse(request.UserContext, ResponseStatus.Finished);

            var weeks = await _formDataLoader.GetWeeksAsync();

            var weeksForGrab = GetWeeksForGrab(weeks);
            var coursesForGrab = await _formDataLoader.GetCoursesAsync();
            var facultiesForGrab = await _formDataLoader.GetFacultiesAsync();

            _logger.LogInformation("Starting clear TimeTable collection");
            
            await _timeTableRepository.ClearCollection();
            
            _logger.LogInformation("TimeTable collection cleared successfully");

            foreach (var (_, courseId) in coursesForGrab)
            {
                foreach (var (_, facultyId) in facultiesForGrab)
                {
                    var groupsForGrab = await _formDataLoader.GetGroupsAsync(facultyId, courseId);

                    foreach (var (_, groupId) in groupsForGrab)
                    {
                        foreach (var week in weeksForGrab)
                        {
                            var query = new TimetableQuery
                            {
                                CourseId = courseId,
                                FacultyId = facultyId,
                                GroupId = groupId,
                                Week = week
                            };

                            try
                            {
                                var timeTables = await _timeTableLoader.GrabTimeTableModels(query);
                                await ProcessTimeTables(timeTables, query);
                            }
                            catch (Exception e)
                            {
                                _logger.LogError($"Error while grabbing timetable for GroupId: {query.GroupId}, FacultyId: {query.FacultyId}, CourseId: {query.CourseId}, Week: {query.Week}", e);
                                return response;
                            }
                        }
                    }
                }
            }

            _logger.LogInformation("Successfully grabbed!!!");

            return response;
        }

        private async Task ProcessTimeTables(List<TimeTableParsedModel> timeTableModels, TimetableQuery query)
        {
            timeTableModels = timeTableModels.Where(x => !x.Date.Equals(DateTime.MinValue)).ToList();

            var week = DateTime.Parse(query.Week, styles: DateTimeStyles.AssumeUniversal);

            var documents = timeTableModels.Select(x =>
            {
                var doc = _mapper.Map<TimeTableDocument>(x);
                doc.GroupId = query.GroupId;
                doc.Week = week;

                return doc;
            }).ToList();

            if (!documents.Any())
            {
                _logger.LogWarning($"Can't store timetables for GroupId: {query.GroupId}, FacultyId: {query.FacultyId}, CourseId: {query.CourseId}, Week: {query.Week}");

                return;
            }

            await _timeTableRepository.InsertManyAsync(documents);

            _logger.LogInformation($"Successfully grabbed GroupId: {query.GroupId}, FacultyId: {query.FacultyId}, CourseId: {query.CourseId}, Week: {query.Week}");
        }

        private List<string> GetWeeksForGrab(IReadOnlyDictionary<DateTime, string> availableWeeks)
        {
            var today = DateTime.Today;

            if (today.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                today = today.AddDays(3);
            }

            return new List<string>
            {
                availableWeeks[today.StartOfWeek()],
                availableWeeks[today.AddDays(7).StartOfWeek()]
            };
        }
    }
}
