namespace FCG_Payments.Domain.Model.Response;

public class GameResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public decimal Value { get; set; }
}
