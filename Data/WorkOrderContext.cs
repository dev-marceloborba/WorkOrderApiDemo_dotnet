using Microsoft.EntityFrameworkCore;
using WorkOrderApi.Data.Configuration;
using WorkOrderApi.Models;

namespace WorkOrderApi.Data;

public class WorkOrderContext : DbContext
{
    public DbSet<WorkOrderModel> WorkOrders { get; set; }

    public string DbPath { get; }

    public WorkOrderContext()
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
    }
}