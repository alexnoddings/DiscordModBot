using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Elvet.Core.Config;
using Elvet.InCharacter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Elvet.InCharacter
{
    internal class InCharacterService
    {
        private readonly IBotConfig _botConfig;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<(ulong, ulong), PlayerCharacter?> _characterCache = new();

        public InCharacterService(IBotConfig botConfig, IServiceProvider serviceProvider)
        {
            _botConfig = botConfig;
            _serviceProvider = serviceProvider;
        }

        public async Task OnMessageReceivedAsync(SocketMessage message)
        {
            if (message.Author.IsBot || message.Author.IsWebhook)
                return;

            if (message.Content.StartsWith(_botConfig.CommandPrefix, true, CultureInfo.InvariantCulture))
                return;

            var character = await GetCharacterAsync(message.Author.Id, message.Channel.Id);
            if (character is null) return;

            var embed =
                new EmbedBuilder()
                    .WithTitle(character.Name)
                    .WithColor(196, 21, 139)
                    .WithDescription(message.Content);

            if (character.ProfilePictureUrl is not null)
                embed.WithThumbnailUrl(character.ProfilePictureUrl);
            
            await message.Channel.SendMessageAsync(embed: embed.Build(), allowedMentions: AllowedMentions.None);
            await message.DeleteAsync();
        }

        public async Task<PlayerCharacter?> GetCharacterAsync(ulong userId, ulong channelId)
        {
            var key = (userId, channelId);
            var isCharacterCached = _characterCache.TryGetValue(key, out var character);
            if (isCharacterCached)
                return character;

            using var serviceScope = _serviceProvider.CreateScope();
            await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<InCharacterDbContext>();

            character = await dbContext.Characters.FindAsync(key.userId, key.channelId);
            _characterCache[key] = character;
            return character;
        }

        public async Task SetCharacterAsync(PlayerCharacter character)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<InCharacterDbContext>();

            var existing = await dbContext.Characters.FindAsync(character.UserId, character.ChannelId);
            if (existing is not null)
            {
                existing.Name = character.Name;
                existing.ProfilePictureUrl = character.ProfilePictureUrl;
            }
            else
            {
                dbContext.Characters.Add(character);
            }

            await dbContext.SaveChangesAsync();
            _characterCache[(character.UserId, character.ChannelId)] = character;
        }

        public async Task DeleteCharacterAsync(ulong userId, ulong channelId)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            await using var dbContext = serviceScope.ServiceProvider.GetRequiredService<InCharacterDbContext>();

            var character = new PlayerCharacter {UserId = userId, ChannelId = channelId};
            dbContext.Entry(character).State = EntityState.Deleted;

            await dbContext.SaveChangesAsync();

            _characterCache[(userId, channelId)] = null;
        }
    }
}
