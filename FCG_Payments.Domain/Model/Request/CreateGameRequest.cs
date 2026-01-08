namespace FCG_Payments.Domain.Model.Request;

public class CreateGameRequest
{
    public string Name { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public decimal Value { get; set; }
}
