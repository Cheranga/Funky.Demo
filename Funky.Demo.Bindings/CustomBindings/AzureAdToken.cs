using System.Security.Claims;

namespace Funky.Demo.CustomBindings
{
    public class AzureAdToken
    {
        public ClaimsPrincipal User { get; set; }
    }
}