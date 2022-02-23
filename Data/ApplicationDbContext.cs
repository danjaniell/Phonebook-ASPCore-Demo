using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phonebook.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;

namespace Phonebook.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException($"Missing {httpContextAccessor.GetType()}");
        }

        public virtual DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                                       .Where(e => e.Entity is BaseTransactionModel &&
                                                  (e.State == EntityState.Added ||
                                                   e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseTransactionModel)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                    ((BaseTransactionModel)entityEntry.Entity).CreatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
                }
                else
                {
                    Entry((BaseTransactionModel)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    Entry((BaseTransactionModel)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }

                ((BaseTransactionModel)entityEntry.Entity).ModifiedAt = DateTime.UtcNow;
                ((BaseTransactionModel)entityEntry.Entity).ModifiedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
