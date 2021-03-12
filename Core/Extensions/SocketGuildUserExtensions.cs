using System.Linq;
using Discord.WebSocket;

namespace Elvet.Core.Extensions
{
    public static class SocketGuildUserExtensions
    {
        public static bool CanTarget(this SocketGuildUser user, SocketGuildUser other)
        {
            // Roles aren't sorted by position when fetched, so need to iterate all to find the max.
            var highestUserRolePosition = user.Roles.Max(role => role.Position);
            var highestOtherRolePosition = other.Roles.Max(role => role.Position);
            return highestUserRolePosition > highestOtherRolePosition;
        }
    }
}
