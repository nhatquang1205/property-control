using Microsoft.EntityFrameworkCore;
using PropertyControl.Databases.Entities;

namespace PropertyControl.Databases
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ModelCreate.OnModelCreating(modelBuilder);
            QueryFilter.HasQueryFilter(modelBuilder);
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                if (entry.Entity is IAuditableEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = DateTime.UtcNow;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedAt = DateTime.UtcNow;
                    } else if (entry.State == EntityState.Deleted)
                    {
                        entry.State = EntityState.Modified;
                        entity.DeletedAt = DateTime.UtcNow;
                        entity.IsDeleted = true;
                    }
                }

                // Chuyển đổi các property kiểu DateTimeOffset sang UTC
                var properties = entry.Entity.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?));

                foreach (var prop in properties)
                {
                    var value = (DateTimeOffset?)prop.GetValue(entry.Entity);
                    if (value.HasValue)
                    {
                        prop.SetValue(entry.Entity, value.Value.ToUniversalTime());
                    }
                }
            }
        }
    }
}