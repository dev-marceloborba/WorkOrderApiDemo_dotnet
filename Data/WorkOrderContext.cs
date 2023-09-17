using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkOrderApi.Data.Configuration;
using WorkOrderApi.Models;

namespace WorkOrderApi.Data;

public class WorkOrderContext : IdentityUserContext<IdentityUser>
{
    public DbSet<WorkOrder> WorkOrders { get; set; }

    public string DbPath { get; }

    public WorkOrderContext(DbContextOptions<WorkOrderContext> options)
        : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "work-order.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WorkOrderConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityUserLoginConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityUserTokenConfiguraton());
    }
}