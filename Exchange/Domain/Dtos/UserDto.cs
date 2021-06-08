namespace Exchange.Domain.Dtos
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// DTO to render user.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or Sets user id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or Sets user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets not encrypted password.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Password { get; set; }
    }
}