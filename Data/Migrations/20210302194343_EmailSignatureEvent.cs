using Microsoft.EntityFrameworkCore.Migrations;

namespace EventPlanner.Data.Migrations
{
    public partial class EmailSignatureEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailSignature",
                table: "Events",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSignature",
                table: "Events");
        }
    }
}
