using Microsoft.EntityFrameworkCore.Migrations;

namespace EventPlanner.Data.Migrations
{
    public partial class EventModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "BaseEvents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Celebrant",
                table: "BaseEvents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "BaseEvents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "BaseEvents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Social",
                table: "BaseEvents",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "BaseEvents");

            migrationBuilder.DropColumn(
                name: "Celebrant",
                table: "BaseEvents");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "BaseEvents");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "BaseEvents");

            migrationBuilder.DropColumn(
                name: "Social",
                table: "BaseEvents");
        }
    }
}
