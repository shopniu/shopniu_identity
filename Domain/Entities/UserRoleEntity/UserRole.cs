using Shopniu_identity.Domain.Entities.RoleEntity;
using Shopniu_identity.Domain.Entities.UserEntity;

namespace Shopniu_identity.Domain.Entities.UserRoleEntity;

public class UserRole
{
    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;
}