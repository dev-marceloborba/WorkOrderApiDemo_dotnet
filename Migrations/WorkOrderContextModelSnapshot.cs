﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkOrderApi.Data;

#nullable disable

namespace WorkOrderApi.Migrations
{
    [DbContext(typeof(WorkOrderContext))]
    partial class WorkOrderContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("WorkOrderApi.Models.WorkOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("EquipmentName")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("TargetDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkOrderStatus")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("WorkOrder", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
