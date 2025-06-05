namespace Domain.Common;

public class ValueResult<T>
{
    public bool IsFailed { get; set; }
    public T Value { get; set; }
    public string ErrorMessage { get; set; }

    public static ValueResult<T> Success(T value) => new() { IsFailed = false, Value = value };
    public static ValueResult<T> Failure(string error) => new() { IsFailed = true, ErrorMessage = error };
}
