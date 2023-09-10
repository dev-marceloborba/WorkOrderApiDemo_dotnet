namespace WorkOrderApi.Shared;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "O valor especificado é nulo");
    public static readonly Error ConditionNotMet = new("Error.ConditionNotMet", "A condição especificada não foi aceita");
}