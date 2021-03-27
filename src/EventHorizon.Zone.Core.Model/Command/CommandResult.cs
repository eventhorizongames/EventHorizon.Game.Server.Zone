namespace EventHorizon.Zone.Core.Model.Command
{
    using System;


    public class CommandResult<T>
    {
        public bool Success { get; }
        public T Result { get; }
        public string ErrorCode { get; }

        public CommandResult(
            T result
        )
        {
            Success = true;
            Result = result;
            ErrorCode = null;
        }

        public CommandResult(
            bool success,
            T result
        )
        {
            Success = success;
            Result = result;
            ErrorCode = null;
        }

        public CommandResult(
            string errorCode
        )
        {
            Success = false;
            Result = default;
            ErrorCode = errorCode;
        }

        public static implicit operator CommandResult<T>(
            T result
        ) => new(
            result
        );

        public static implicit operator CommandResult<T>(
            string errorCode
        ) => new(
            errorCode
        );

        public static implicit operator bool(
            CommandResult<T> result
        ) => result?.Success ?? false;
    }
}
