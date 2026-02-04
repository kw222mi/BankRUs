namespace BankRUs.Infrastructure.Configuration;

/*
  "Jwt": {
    "Issuer": "BankRUs.Api",
    "Audience": "BankRUs.Client",
    "Secret": "THIS_IS_A_LONG_RANDOM_SECRET_KEY_32+_CHARS",
    "ExpiresMinutes": 60
  }
 * */

public sealed record JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = "";
    public string Audience { get; init; } = "";
    public string Secret { get; init; } = "";
    public int ExpiresMinutes { get; init; } = 60;
}
