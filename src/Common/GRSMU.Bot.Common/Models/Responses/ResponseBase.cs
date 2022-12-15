namespace GRSMU.Bot.Common.Models.Responses
{
    public abstract class ResponseBase
    {
        protected ResponseBase()
        {
            Errors = new List<string>();
        }

        public TelegramResponseStatus Status { get; set; } = TelegramResponseStatus.Successful;

        public List<string> Errors { get; set; }

        public bool HasErrors => Errors?.Any() ?? false;
    }
}
