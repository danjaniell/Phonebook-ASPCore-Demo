using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Phonebook.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using Bogus;

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
            var ids = 1;
            var contacts = new Faker<Contact>()
                .RuleFor(m => m.Id, f => ids++)
                .RuleFor(m => m.FirstName, f => f.Person.FirstName)
                .RuleFor(m => m.LastName, f => f.Person.LastName)
                .RuleFor(m => m.Email, f => f.Person.Email)
                .RuleFor(m => m.Mobile, f => f.Random.ReplaceNumbers("+639#########"))
                .RuleFor(m => m.CreatedBy, f => f.Hacker.Noun())
                .RuleFor(m => m.ModifiedBy, f => f.Hacker.Noun())
                .RuleFor(m => m.CreatedAt, f => f.Date.Past())
                .RuleFor(m => m.ModifiedAt, f => f.Date.Recent(days: 60));
            modelBuilder
                .Entity<Contact>()
                .HasData(contacts.GenerateBetween(2000, 2500));

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var entries = ChangeTracker.Entries()
                                       .Where(e => e.Entity is BaseTransactionModel &&
                                                  (e.State == EntityState.Added ||
                                                   e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var utcNow = DateTime.UtcNow;
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseTransactionModel)entityEntry.Entity).CreatedAt = utcNow;
                    ((BaseTransactionModel)entityEntry.Entity).CreatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
                }
                else
                {
                    Entry((BaseTransactionModel)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    Entry((BaseTransactionModel)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }

                ((BaseTransactionModel)entityEntry.Entity).ModifiedAt = utcNow;
                ((BaseTransactionModel)entityEntry.Entity).ModifiedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                                       .Where(e => e.Entity is BaseTransactionModel &&
                                                  (e.State == EntityState.Added ||
                                                   e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var utcNow = DateTime.UtcNow;
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseTransactionModel)entityEntry.Entity).CreatedAt = utcNow;
                    ((BaseTransactionModel)entityEntry.Entity).CreatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
                }
                else
                {
                    Entry((BaseTransactionModel)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    Entry((BaseTransactionModel)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }

                ((BaseTransactionModel)entityEntry.Entity).ModifiedAt = utcNow;
                ((BaseTransactionModel)entityEntry.Entity).ModifiedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "MyApp";
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
