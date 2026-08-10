

using Shopniu_identity.Domain.Entities.common;
using Shopniu_identity.Domain.Entities.PermissionEntity;
using Shopniu_identity.Domain.Entities.UserEntity;

namespace Shopniu_identity.Domain.Entities.UserPermissionEntity;

public class UserPermission : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public int PermissionId { get; set; }
    public Permission Permission { get; set; } = default!;

    public bool Allow { get; set; } = true;
}