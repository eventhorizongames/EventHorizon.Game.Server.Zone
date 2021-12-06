namespace EventHorizon.Zone.Core.Model.Command;

public static class CommandResultExtensions
{
    public static CommandResult<TResult> ToCommandResult<TResult>(
        this TResult obj
    ) => new CommandResult<TResult>(
        obj
    );
}

public class CommandResult<TResult>
{
    public bool Success { get; }
    public TResult Result { get; }
    public string ErrorCode { get; }

    public CommandResult(
        TResult result
    )
    {
        Success = true;
        Result = result;
        ErrorCode = string.Empty;
    }

    public CommandResult(
        bool success,
        TResult result
    )
    {
        Success = success;
        Result = result;
        ErrorCode = string.Empty;
    }

    public CommandResult(
        string errorCode
    )
    {
        Success = false;
        Result = default!;
        ErrorCode = errorCode;
    }

    public static implicit operator CommandResult<TResult>(
        TResult result
    ) => new(
        result
    );

    public static implicit operator CommandResult<TResult>(
        string errorCode
    ) => new(
        errorCode
    );

    public static implicit operator bool(
        CommandResult<TResult> result
    ) => result?.Success ?? false;
}
