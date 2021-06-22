using System;

namespace Elvet.InCharacter.Data
{
    public class PlayerCharacter
    {
        public ulong UserId { get; set; }
        public ulong ChannelId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
    }
}
