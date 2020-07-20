using System;
using Discord.Commands;

namespace Elvet.Plugins.Dice
{
    public partial class DiceModule : ModuleBase<SocketCommandContext>
    {
        private static readonly Random InsecureRandom = new Random();
    }
}
