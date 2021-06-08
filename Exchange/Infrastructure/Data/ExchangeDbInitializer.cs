namespace Exchange.Infrastructure.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Database Identity user initializer.
    /// </summary>
    public class ExchangeDbInitializer : IDbInitializer
    {
        private readonly ExchangeDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILogger<ExchangeDbInitializer> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeDbInitializer"/> class.
        /// </summary>
        /// <param name="context">The database context <see cref="ExchangeDbContext"/>.</param>
        /// <param name="userManager">The <see cref="UserManager{IdentityUser}"/>.</param>
        /// <param name="logger">The <see cref="ILogger{ExchangeDbInitializer}"/>.</param>
        public ExchangeDbInitializer(
            ExchangeDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<ExchangeDbInitializer> logger)
        {
            this.context = context;
            this.userManager = userManager;
            this.logger = logger;
        }

        /// <summary>
        /// Seed admin user if not exists.
        /// </summary>
        public async void Initialize()
        {
            var username = "admin";
            await this.context.Database.EnsureCreatedAsync();

            var admin = await this.userManager.FindByNameAsync(username);

            if (admin == null)
            {
                this.logger.LogInformation("No admin user found in database. Creating admin user");

                admin = new IdentityUser()
                {
                    UserName = username,
                    Email = "admin@exchange.com",
                    EmailConfirmed = true,
                };

                var result = await this.userManager.CreateAsync(admin, "Pw1@exchange");
                if (result.Succeeded)
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