namespace Exchange.Domain.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Exchange.Domain.Dtos;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The User Repository
    /// </summary>
    public class UsersRepository
    {
        private readonly UserManager<IdentityUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRepository"/> class.
        /// </summary>
        /// <param name="userManager">The <see cref="UserManager{IdentityUser}"/>.</param>
        public UsersRepository(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// List users.
        /// </summary>
        /// <returns>A user list <see cref="List{UserDto}"/>.</returns>
        public async Task<List<UserDto>> ListAsync()
        {
            return await this.userManager.Users
                .Select(user => ConvertToDto(user))
                .ToListAsync();
        }

        /// <summary>
        /// Find user by id.
        /// </summary>
        /// <param name="id">Id to find user.</param>
        /// <returns>The found user or null if not found.</returns>
        public async Task<UserDto> FindByIdAsync(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            return user != null ? ConvertToDto(user) : null;
        }

        /// <summary>
        /// Find user by username.
        /// </summary>
        /// <param name="username">Username to find user.</param>
        /// <returns>The found user or null if not found.</returns>
        public async Task<UserDto> FindByUserNameAsync(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);
            return user != null ? ConvertToDto(user) : null;
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="user">The user data to create the new user.</param>
        /// <returns>The new user created or null if failed.</returns>
        public async Task<UserDto> CreateUserAsync(UserDto user)
        {
            var identityUser = new IdentityUser()
            {
                UserName = user.UserName,
                Email = user.Email,
            };

            var result = await this.userManager.CreateAsync(identityUser, user.Password);
            return result.Succeeded ? ConvertToDto(identityUser) : null;
        }

        /// <summary>
        /// Update a user.
        /// </summary>
        /// <param name="user">Data to update the user.</param>
        public async Task UpdateAsync(UserDto user)
        {
            var identityUser = await this.userManager.FindByIdAsync(user.Id);
            if (identityUser.UserName != user.UserName || identityUser.Email != user.Email)
            {
                identityUser.UserName = user.UserName;
                identityUser.Email = user.Email;
                await this.userManager.UpdateAsync(identityUser);
            }
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="id">User id to be deleted.</param>
        /// <returns>True if deleted, false if not deleted or not found.</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            var result = await this.userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        /// <summary>
        /// Convert a User to DTO.
        /// </summary>
        /// <param name="user">User to be converted.</param>
        /// <returns>The <see cref="UserDto"/>.</returns>
        private static UserDto ConvertToDto(IdentityUser user)
        {
            return new ()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            };
        }
    }
}