using System.Security.Claims;
using WebFood.Models.Entities;

namespace WebFood.Utility
{
    public static class AcessChecker
    {
        public static bool IsAdminOrManager(Restaurant restaurant, ClaimsPrincipal User)
        {
            return (User.FindFirstValue(ClaimTypes.NameIdentifier) == restaurant.ManagerId.ToString()
                        || User.IsInRole($"{Roles.Administator}"));

        }
    }
}
