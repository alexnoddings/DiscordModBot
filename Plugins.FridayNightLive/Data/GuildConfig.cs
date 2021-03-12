namespace Elvet.FridayNightLive.Data
{
    internal class GuildConfig
    {
        public ulong GuildId { get; set; }

        public ulong WinnerRoleId { get; set; }
        public ulong HostRoleId { get; set; }

        public string? ThumbnailUrl { get; set; }

        public ulong LeaderboardMessageId { get; set; }
    }
}
