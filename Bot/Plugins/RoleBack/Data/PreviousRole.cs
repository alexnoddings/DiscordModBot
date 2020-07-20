using System;

namespace Elvet.Plugins.RoleBack.Data
{
    public class PreviousRole
    {
        public Guid Id { get; set; }
        public Guid DetailsId { get; set; }
        public ulong RoleId { get; set; }
    }
}
