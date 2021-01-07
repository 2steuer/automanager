using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace automanager.Middleware
{
    public class AccessCodeAuthenticationHandlerOptions
        : AuthenticationSchemeOptions
    {
        public string AccessCode { get; set; }
    }

    public class AccessCodeAuthenticationHandler
        : AuthenticationHandler<AccessCodeAuthenticationHandlerOptions>
    {
        private IConfiguration _cfg;
        public AccessCodeAuthenticationHandler(IConfiguration configuration, IOptionsMonitor<AccessCodeAuthenticationHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _cfg = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {   
            bool isAuthenticated = Context.Session.GetInt32("_access") == 1;
                        
            if (isAuthenticated 
                || (!isAuthenticated && Request.Query.ContainsKey("accessCode")
                    && Request.Query["accessCode"] == Options.AccessCode))
            {
                if (!isAuthenticated)
                {
                    Context.Session.SetInt32("_access", 1);
                    await Context.Session.CommitAsync();
                }

                var claims = new[] {
                    new Claim(ClaimTypes.Name, "Anon"),
                    new Claim(ClaimTypes.Role, "All") };

                // generate claimsIdentity on the name of the class
                var claimsIdentity = new ClaimsIdentity(claims,
                            nameof(AccessCodeAuthenticationHandler));


                return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name));
            }
            
            return AuthenticateResult.NoResult();
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect($"{_cfg["PathBase"]}/Home/Login");
            return Task.CompletedTask;
        }
    }
}