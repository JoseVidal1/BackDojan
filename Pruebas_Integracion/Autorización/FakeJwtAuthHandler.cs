using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

public class FakeJwtAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public FakeJwtAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, loggerFactory, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, "Administrador")
        };

        var identity = new ClaimsIdentity(claims, "FakeJwt");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "FakeJwt");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
