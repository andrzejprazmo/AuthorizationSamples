namespace JwtDefault.Domain.Account
{
    public class RefreshTokenEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Expires { get; set; }
    }
}
