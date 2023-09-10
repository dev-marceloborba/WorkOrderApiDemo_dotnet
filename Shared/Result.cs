namespace WorkOrderApi.Shared;

public class Result
{
    public bool IsSuccess { get; }
    public Error Error { get; }
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool isFailure => !IsSuccess;
}