namespace JwtCustom.Application.Commands.Authenticate
{
    public class AuthenticateResponse
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string[] Roles { get; set; } = new string[0];

    }
}
