namespace Exchange.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Exchange.Domain.Dtos;
    using Exchange.Domain.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The User Repository.
    /// </summary>
    public class UsersesRepository : IUsersRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersesRepository"/> class.
        /// </summary>
        /// <param name="userManager">The <see cref="UserManager{ApplicationUser}"/>.</param>
        public UsersesRepository(UserManager<ApplicationUser> userManager)
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
            var appUser = new ApplicationUser()
            {
                UserName = user.UserName,
                Email = user.Email,
            };

            var result = await this.userManager.CreateAsync(appUser, user.Password);
            return result.Succeeded ? ConvertToDto(appUser) : null;
        }

        /// <summary>
        /// Update a user.
        /// </summary>
        /// <param name="user">Data to update the user.</param>
        public async void UpdateAsync(UserDto user)
        {
            var appUser = await this.userManager.FindByIdAsync(user.Id);
            if (appUser.UserName != user.UserName || appUser.Email != user.Email)
            {
                appUser.UserName = user.UserName;
                appUser.Email = user.Email;
                await this.userManager.UpdateAsync(appUser);
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
        private static UserDto ConvertToDto(ApplicationUser user)
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