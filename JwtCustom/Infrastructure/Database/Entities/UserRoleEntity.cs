namespace JwtCustom.Infrastructure.Database.Entities
{
    public record UserRoleEntity(Guid user_id
        , string user_name
        , string user_first_name
        , string user_last_name
        , string user_email_address
        , Guid role_id
        , string role_name
        , string role_desc);
}
