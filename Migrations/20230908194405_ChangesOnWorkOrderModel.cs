using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkOrderApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangesOnWorkOrderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetDate",
                table: "WorkOrder",
                newName: "Target");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "WorkOrder",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WorkOrder",
                type: "varchar(200)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "WorkOrder");

            migrationBuilder.RenameColumn(
                name: "Target",
                table: "WorkOrder",
                newName: "TargetDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "WorkOrder",
                newName: "CreatedDate");
        }
    }
}
