namespace Exchange.Infrastructure.Data
{
    using Exchange.Domain.Dtos;
    using Exchange.Domain.Repositories;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Database Identity user initializer.
    /// </summary>
    public class ExchangeDbInitializer : IDbInitializer
    {
        private readonly IUsersRepository usersRepository;
        private readonly ILogger<ExchangeDbInitializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeDbInitializer"/> class.
        /// </summary>
        /// <param name="usersRepository">The <see cref="UsersRepository"/>.</param>
        /// <param name="logger">The <see cref="ILogger{ExchangeDbInitializer}"/>.</param>
        public ExchangeDbInitializer(IUsersRepository usersRepository, ILogger<ExchangeDbInitializer> logger)
        {
            this.usersRepository = usersRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Seed admin user if not exists.
        /// </summary>
        public async void Initialize()
        {
            const string username = "admin";
            var admin = await this.usersRepository.FindByUserNameAsync(username);

            if (admin == null)
            {
                this.logger.LogInformation("No admin user found in database. Creating admin user");

                admin = new UserDto()
                {
                    UserName = username,
                    Email = "admin@exchange.com",
                    Password = "Pw1@exchange",
                };

                admin = await this.usersRepository.CreateUserAsync(admin);

                if (admin != null)
                {
                    this.logger.LogInformation("Admin user created");
                }
                else
                {
                    this.logger.LogError("Failed to create admin user");
                }
            }
        }
    }
}