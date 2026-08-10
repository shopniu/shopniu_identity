using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Shopniu_identity.Domain.Exceptions.Common;
using Shopniu_identity.Domain.Entities.UserRoleEntity;
using Shopniu_identity.Domain.Entities.RolePermissionEntity;

namespace Shopniu_identity.Domain.Entities.RoleEntity
{
    public class Role : IdentityRole<int>
    {
        public string? Description { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

        private Role() { }

        public Role(string name, string? description = null) : base(name)
        {
            Description = description;
        }
    }
}