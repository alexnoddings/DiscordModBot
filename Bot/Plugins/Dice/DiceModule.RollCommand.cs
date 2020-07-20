using System.Threading.Tasks;
using Discord.Commands;

namespace Elvet.Plugins.Dice
{
    public partial class DiceModule
    {
        private static int DefaultMinValue = 1;
        private static int DefaultMaxValue = 1;

        [Command("rtd")]
        public Task RollTheDiceAsync()
        {
            // Next(a, b)'s range is a <= x < b, which is counter intuitive for a dice roll, as such 1 is added
            int num = InsecureRandom.Next(DefaultMinValue, DefaultMaxValue);
            return ReplyAsync($"🎲 {num}");
        }

        [Command("rtd")]
        public Task RollTheDiceAsync(int max)
        {
            if (DefaultMinValue > max)
            {
                return ReplyAsync($"The min value {DefaultMinValue} cannot be greater than the max value {max}");
            }

            // Next(a, b)'s range is a <= x < b, which is counter intuitive for a dice roll, as such 1 is added
            int num = InsecureRandom.Next(DefaultMinValue, max == int.MaxValue ? max : max + 1);
            return ReplyAsync($"🎲 {num}");
        }

        [Command("rtd")]
        public Task RollTheDiceAsync(int min, int max)
        {
            if (min > max)
            {
                return ReplyAsync($"The min value {min} cannot be greater than the max value {max}");
            }

            // Next(a, b)'s range is a <= x < b, which is counter intuitive for a dice roll, as such 1 is added
            int num = InsecureRandom.Next(min, max == int.MaxValue ? max : max + 1);
            return ReplyAsync($"🎲 {num}");
        }
    }
}
