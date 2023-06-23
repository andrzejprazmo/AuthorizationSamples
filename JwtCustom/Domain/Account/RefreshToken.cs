namespace JwtCustom.Domain.Account
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Expires { get; set; }
    }
}
