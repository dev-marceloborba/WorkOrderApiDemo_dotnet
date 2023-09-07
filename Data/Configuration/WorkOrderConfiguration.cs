using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkOrderApi.Models;

namespace WorkOrderApi.Data.Configuration;

public class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrderModel>
{
    public void Configure(EntityTypeBuilder<WorkOrderModel> builder)
    {
        builder.ToTable("WorkOrder");
        builder.Property(x => x.EquipmentName).HasColumnType("varchar(50)");
    }
}