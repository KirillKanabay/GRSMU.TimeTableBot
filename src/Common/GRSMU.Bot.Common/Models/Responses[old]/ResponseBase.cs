using GRSMU.Bot.Common.Enums;

namespace GRSMU.Bot.Common.Models.Responses
{
    public abstract class ResponseBase
    {
        protected ResponseBase()
        {
            Errors = new List<string>();
        }

        public ResponseStatus Status { get; set; } = ResponseStatus.Successful;

        public List<string> Errors { get; set; }

        public bool HasErrors => Errors?.Any() ?? false;
    }
}
