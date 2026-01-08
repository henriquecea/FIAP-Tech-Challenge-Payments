using Newtonsoft.Json;

namespace FCG_Payments.Domain.Model.Payment;

public record PaymentDto(
    [property: JsonProperty("status")]
    string Status,

    [property: JsonProperty("value")]
    decimal Value,

    [property: JsonProperty("userId")]
    Guid UserId,

    [property: JsonProperty("gameId")]
    Guid GameId
);