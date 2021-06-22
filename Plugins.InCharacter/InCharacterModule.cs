using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Elvet.Core.Commands;
using Elvet.InCharacter.Data;

namespace Elvet.InCharacter
{
    [Name("InCharacter")]
    [Alias("ic")]
    internal class InCharacterModule : ElvetModuleBase
    {
        private readonly InCharacterService _service;

        protected override IEmote SuccessReactionEmote { get; } = new Emoji("🥸");

        public InCharacterModule(InCharacterService service)
        {
            _service = service;
        }

        [Command("Configure")]
        [Alias("config")]
        [RequireContext(ContextType.Guild)]
        public async Task ConfigureCharacter(string profilePictureUrl, [Remainder] string name)
        {
            var isValidProfilePictureUrl = Uri.TryCreate(profilePictureUrl, UriKind.Absolute, out Uri? profilePictureParsed);
            if (!isValidProfilePictureUrl)
                // The url wasn't a url, but the starting part of the name
                name = profilePictureUrl + " " + name;

            var newCharacter = new PlayerCharacter
            {
                UserId = Context.User.Id,
                ChannelId = Context.Channel.Id,
                Name = name,
                ProfilePictureUrl = profilePictureParsed?.AbsoluteUri
            };
            await _service.SetCharacterAsync(newCharacter);
            await MarkSuccessful();
        }

        [Command("Disable")]
        [Alias("delete", "del")]
        [RequireContext(ContextType.Guild)]
        public async Task DeleteCharacter()
        {
            await _service.DeleteCharacterAsync(Context.User.Id, Context.Channel.Id);
            await MarkSuccessful();
        }
    }
}
