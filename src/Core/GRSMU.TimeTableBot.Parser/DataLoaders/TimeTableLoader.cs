using GRSMU.TimeTableBot.Common.Models;
using GRSMU.TimeTableBot.Common.Options;
using GRSMU.TimeTableBot.Core.DataLoaders.Common;
using GRSMU.TimeTableBot.Core.DataLoaders.Handlers;

namespace GRSMU.TimeTableBot.Core.DataLoaders
{
    public class TimeTableLoader : ITimeTableLoader
    {
        private readonly SourceOptions _sourceOptions;

        public TimeTableLoader(SourceOptions sourceOptions)
        {
            _sourceOptions = sourceOptions ?? throw new ArgumentNullException(nameof(sourceOptions));
        }

        public async Task<string> LoadTimeTable(TimetableQuery query)
        {
            var root = new TimeTableLoaderHandlerRoot();

            var context = new ParserTimeTableContext
            {
                Url = _sourceOptions.Url,
                Query = query
            };

            root.Add(new TimeTableLoaderPrepareFormParamsHandler());
            root.Add(new TimeTableLoaderGrabFormHandler());
            root.Add(new TimeTableLoaderParseTableHandler());
            root.Add(new TimeTableLoaderModelPresenterHandler());

            await root.Handle(context);

            return context.Response;
        }

        public async Task<List<TimeTableParsedModel>> GrabTimeTableModels(TimetableQuery query)
        {
            var root = new TimeTableLoaderHandlerRoot();

            var context = new ParserTimeTableContext
            {
                Url = _sourceOptions.Url,
                Query = query
            };

            root.Add(new TimeTableLoaderPrepareFormParamsHandler());
            root.Add(new TimeTableLoaderGrabFormHandler());
            root.Add(new TimeTableLoaderParseTableHandler());

            await root.Handle(context);

            return context.TimeTableModels;
        }
    }
}
