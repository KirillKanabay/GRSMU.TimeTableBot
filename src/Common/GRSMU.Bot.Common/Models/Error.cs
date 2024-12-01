using GRSMU.Bot.Common.Enums;

namespace GRSMU.Bot.Common.Models
{
    public record Error
    {
        public Error(string code, string? description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public Error(string code, string? description, ErrorType type, string field) : this(code, description, type)
        {
            Field = field;
        }

        public string? Field { get; }

        public string Code { get; }

        public string? Description { get; set; }

        public ErrorType Type { get; }

        public static Error ValidationError(string code, string? description = null, string? field = null)
            => new Error(code, description, ErrorType.Validation);

        public static Error NotFound(string code, string? description = null)
            => new (code, description, ErrorType.NotFound);

        public static Error Problem(string code, string? description = null) =>
            new(code, description, ErrorType.Problem);

        public static Error Conflict(string code, string? description = null) =>
            new(code, description, ErrorType.Conflict);
    }
}
