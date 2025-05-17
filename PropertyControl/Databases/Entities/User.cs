namespace PropertyControl.Databases.Entities
{
    public class User : BaseEntity<int>
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public int Age { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = default!;
    }
}