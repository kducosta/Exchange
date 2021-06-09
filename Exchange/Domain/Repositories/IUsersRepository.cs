// <copyright file="IUserRepository.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Exchange.Domain.Dtos;

    /// <summary>
    /// The User Repository interface.
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// List users.
        /// </summary>
        /// <returns>A user list <see cref="List{T}"/>.</returns>
        Task<List<UserDto>> ListAsync();

        /// <summary>
        /// Find user by id.
        /// </summary>
        /// <param name="id">Id to find user.</param>
        /// <returns>The found user or null if not found.</returns>
        Task<UserDto> FindByIdAsync(string id);

        /// <summary>
        /// Find user by username.
        /// </summary>
        /// <param name="username">Username to find user.</param>
        /// <returns>The found user or null if not found.</returns>
        Task<UserDto> FindByUserNameAsync(string username);

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="user">The user data to create the new user.</param>
        /// <returns>The new user created or null if failed.</returns>
        Task<UserDto> CreateUserAsync(UserDto user);

        /// <summary>
        /// Update a user.
        /// </summary>
        /// <param name="user">Data to update the user.</param>
        void UpdateAsync(UserDto user);

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="id">User id to be deleted.</param>
        /// <returns>True if deleted, false if not deleted or not found.</returns>
        Task<bool> DeleteAsync(string id);
    }
}