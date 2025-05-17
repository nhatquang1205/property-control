namespace PropertyControl.Databases.Entities
{
    public class Role : BaseEntity<int>
    {
        public required string Name { get; set; }
        public ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}