using System;
using System.Collections.Generic;

namespace Elvet.FridayNightLive.Data
{
    internal class Session
    {
        public ulong GuildId { get; set; }
        public int Number { get; set; }
        public DateTime? Date { get; set; }
        public List<SessionWinner> Winners { get; set; } = new();
        public List<SessionHost> Hosts { get; set; } = new();
        public string Activity { get; set; } = string.Empty;
    }
}
