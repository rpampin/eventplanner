using Microsoft.EntityFrameworkCore.Migrations;

namespace EventPlanner.Data.Migrations
{
    public partial class PlanString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventProgramTemplate",
                table: "Configurations",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventProgramTemplate",
                table: "Configurations");
        }
    }
}
