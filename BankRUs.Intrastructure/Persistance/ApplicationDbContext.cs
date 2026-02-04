using BankRUs.Domain.Entities;
using BankRUs.Intrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Intrastructure.Persistance;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<BankAccount>(builder =>
        {
            builder.Property(x => x.Balance)
              .HasPrecision(18, 2);

            builder
                .HasIndex(b => b.AccountNumber)
                .IsUnique();
        });

        builder.Entity<BankAccount>().
            HasOne<ApplicationUser>().
            WithMany().
            HasForeignKey(b => b.UserId);
    }

    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

}

