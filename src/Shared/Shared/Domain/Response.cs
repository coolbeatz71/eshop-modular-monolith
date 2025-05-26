namespace EShop.Shared.Domain;

public class Response<TResult>
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public TResult? Value { get; }

    private Response(bool isSuccess, TResult? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Response<TResult> Success(TResult value) => new(true, value, null);
    public static Response<TResult> Failure(string error) => new(false, default, error);
}