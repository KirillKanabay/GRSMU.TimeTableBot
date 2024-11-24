﻿using GRSMU.Bot.Common.Enums;

namespace GRSMU.Bot.Common.Models
{
    public record Error
    {
        public Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        public Error(string code, string description, ErrorType type, string field) : this(code, description, type)
        {
            Field = field;
        }

        public string? Field { get; }

        public string Code { get; }

        public string Description { get; }

        public ErrorType Type { get; }

        public static Error NotFound(string code, string description)
            => new Error(code, description, ErrorType.NotFound);

        public static Error Problem(string code, string description) =>
            new(code, description, ErrorType.Problem);

        public static Error Conflict(string code, string description) =>
            new(code, description, ErrorType.Conflict);
    }
}