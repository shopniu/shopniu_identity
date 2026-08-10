

using Shopniu_identity.Domain.Entities.common;
using Shopniu_identity.Domain.Entities.PermissionEntity;
using Shopniu_identity.Domain.Entities.RoleEntity;

namespace Shopniu_identity.Domain.Entities.RolePermissionEntity;

public class RolePermission : BaseEntity
{
    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;

    public int PermissionId { get; set; }
    public Permission Permission { get; set; } = default!;

    private RolePermission() { }

    public RolePermission(Role role, Permission permission)
    {
        Role = role;
        RoleId = role.Id;
        Permission = permission;
        PermissionId = permission.Id;
    }
}