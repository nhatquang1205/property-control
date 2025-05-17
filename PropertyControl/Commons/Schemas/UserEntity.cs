namespace PropertyControl.Commons.Schemas
{
    public class UserEntity
    {
        public int UserId { get; set; }
        public string Username { get; set; } = default!;
        public int RoleId { get; set; }
    }
}