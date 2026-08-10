

using Shopniu_identity.Domain.Entities.common;
using Shopniu_identity.Domain.Entities.RolePermissionEntity;
using Shopniu_identity.Domain.Exceptions.Common;

namespace Shopniu_identity.Domain.Entities.PermissionEntity;

public class Permission : BaseEntity
{
    public string Code { get; set; } = default!;
    public string? Description { get; set; }

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();


    private Permission() { }
    public Permission(string code, string? description)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ValidationsException("Permission code cannot be empty.");

        Code = code;
        Description = description;
    }
}