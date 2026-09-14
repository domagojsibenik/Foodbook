using Foodbook.Models;

namespace Foodbook.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
