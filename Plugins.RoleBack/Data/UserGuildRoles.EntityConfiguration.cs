using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Elvet.RoleBack.Data
{
    internal class UserGuildRolesEntityConfiguration : IEntityTypeConfiguration<UserGuildRoles>
    {
        private static readonly ValueConverter<List<ulong>, string> RolesConverter = new(
            ulongList => string.Join(";", ulongList),
            str => str.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToList()
        );

        private static readonly ValueComparer<List<ulong>> RolesComparer = new(
            (a, b) => a.SequenceEqual(b),
            instance => instance.GetHashCode(),
            instance => instance.ToList()
        );

        public void Configure(EntityTypeBuilder<UserGuildRoles> builder)
        {
            builder
                .HasKey(nameof(UserGuildRoles.User), nameof(UserGuildRoles.Guild))
                .IsClustered();

            builder
                .Property(userGuildRoles => userGuildRoles.Roles)
                .HasConversion(RolesConverter, RolesComparer);
        }
    }
}
