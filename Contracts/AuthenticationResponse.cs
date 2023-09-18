namespace WorkOrderApi.Contracts;

public class AuthenticationResponse {
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}