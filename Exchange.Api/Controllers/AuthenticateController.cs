// <copyright file="AuthenticateController.cs" company="github.com/edu_costa">
// Copyright (c) github.com/edu_costa. All rights reserved.
// </copyright>

namespace Exchange.Api.Controllers
{
    using System.Threading.Tasks;
    using Exchange.Domain.Dtos;
    using Exchange.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents the authentication controller.
    /// </summary>
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class AuthenticateController : Controller
    {
        private readonly AuthenticationService authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateController"/> class.
        /// </summary>
        /// <param name="authenticationService">The <see cref="AuthenticationService"/>.</param>
        public AuthenticateController(AuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// Authenticate in service.
        /// </summary>
        /// <param name="loginDto">The <see cref="LoginDto" />.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await this.authenticationService.Authenticate(loginDto);

            if (token == null)
            {
                return this.Unauthorized("Username or password invalid");
            }

            return this.Ok(token);
        }
    }
}