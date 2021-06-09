// <copyright file="UsersController.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Exchange.Domain.Dtos;
    using Exchange.Domain.Repositories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Users endpoint.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUsersRepository usersRepository;
        private readonly ICurrencyConversionRepository currencyConversionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="usersRepository">The <see cref="usersRepository"/>.</param>
        /// <param name="currencyConversionRepository">The <see cref="ICurrencyConversionRepository"/>.</param>
        /// <exception cref="ArgumentNullException">Throws if mediator is null.</exception>
        public UsersController(IUsersRepository usersRepository, ICurrencyConversionRepository currencyConversionRepository)
        {
            this.usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            this.currencyConversionRepository = currencyConversionRepository ??
                                                throw new ArgumentNullException(nameof(currencyConversionRepository));
        }

        /// <summary>
        /// List of users.
        /// </summary>
        /// <returns>The <see cref="UserDto"/> list.</returns>
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            return await this.usersRepository.ListAsync();
        }

        /// <summary>
        /// Get a user by Id.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns>The <see cref="UserDto"/> or 404 if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(string id)
        {
            var user = await this.usersRepository.FindByIdAsync(id);

            if (user == null)
            {
                return this.NotFound();
            }

            return user;
        }

        /// <summary>
        /// Get a user by Id.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns>The <see cref="GetUserConversions"/> or 404 if not found.</returns>
        [HttpGet("{id}/conversions")]
        public async Task<ActionResult<List<CurrencyConversionDto>>> GetUserConversions(string id)
        {
            var history = await this.currencyConversionRepository.GetUserConversionHistory(id);

            if (history == null)
            {
                return this.NotFound();
            }

            return history;
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="user">The user data to create a new user.</param>
        /// <returns>The created user.</returns>
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(UserDto user)
        {
            var newUser = await this.usersRepository.CreateUserAsync(user);
            return this.CreatedAtAction("Get", new { id = newUser.Id }, newUser);
        }

        /// <summary>
        /// Update the user.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <param name="user">The <see cref="UserDto"/>.</param>
        /// <returns>Not content if user updated, Not found if user not exists.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UserDto user)
        {
            if (id != user.Id)
            {
                return this.BadRequest();
            }

            try
            {
                this.usersRepository.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await this.usersRepository.FindByIdAsync(id) != null)
                {
                    return this.NotFound();
                }

                throw;
            }

            return this.NoContent();
        }

        /// <summary>
        /// Delete a User.
        /// </summary>
        /// <param name="id">User's id to be deleted.</param>
        /// <returns>No content on delete, can returns not found.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await this.usersRepository.FindByIdAsync(id);
            if (user == null)
            {
                return this.NotFound();
            }

            await this.usersRepository.DeleteAsync(id);

            return this.NoContent();
        }
    }
}