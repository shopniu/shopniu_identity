using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Shopniu_identity.Domain.Exceptions.Common;
using Shopniu_identity.Domain.Entities.UserPermissionEntity;

namespace Shopniu_identity.Domain.Entities.UserEntity
{
    public class User : IdentityUser<int>
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;

        public List<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

        private User() { }

        public User(string firstName, string lastName, string email, string userName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ValidationsException("First name cannot be empty.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ValidationsException("Last name cannot be empty.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationsException("Email cannot be empty.");
            if (string.IsNullOrWhiteSpace(userName))
                throw new ValidationsException("Username cannot be empty.");

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = userName;
        }


    }
}