using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventPlanner.Migrations
{
    public partial class Configurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Packages_PackageId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "AppConfigurations");

            migrationBuilder.AlterColumn<Guid>(
                name: "PackageId",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmailSignature = table.Column<string>(type: "TEXT", nullable: true),
                    EventProgramTemplate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Packages_PackageId",
                table: "Events",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Packages_PackageId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.AlterColumn<Guid>(
                name: "PackageId",
                table: "Events",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "AppConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EmailSignature = table.Column<string>(type: "TEXT", nullable: true),
                    EventProgramTemplate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConfigurations", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Packages_PackageId",
                table: "Events",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id");
        }
    }
}
