

using Microsoft.EntityFrameworkCore;
using Shopniu_identity.Aplication.Authentication.Auth.Ports;
using Shopniu_identity.Infrastructure.Persistance;

namespace Shopniu_identity.Infrastructure.Services.Authentication;

public class PermissionService : IPermissionService
{
    private readonly AppDbContext _context;

    public PermissionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<string>> GetPermissionsByUserIdAsync(int userId)
    {
        // role - user permissions coming
        var rolePermissions = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .SelectMany(ur => _context.RolePermissions
                .Where(rp => rp.RoleId == ur.RoleId)
                .Select(rp => rp.Permission.Code))
            .ToListAsync();

        // 
        var UserPermissions = await _context.UserPermissions
            .Where(up => up.UserId == userId)
            .Select(up => new { up.Permission.Code, up.Allow })
            .ToListAsync();

        var permissionSet = new HashSet<string>(rolePermissions);

        foreach (var permission in UserPermissions)
        {
            if (permission.Allow)
            {
                permissionSet.Add(permission.Code);
            }
            else
            {
                permissionSet.Remove(permission.Code);
            }
        }

        return permissionSet.ToList();
    }
}