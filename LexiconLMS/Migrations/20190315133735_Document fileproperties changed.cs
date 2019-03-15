using Microsoft.EntityFrameworkCore.Migrations;

namespace LexiconLMS.Migrations
{
    public partial class Documentfilepropertieschanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "Document",
                newName: "DueDate");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Document",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Length",
                table: "Document",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "StoredFilePath",
                table: "Document",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "StoredFilePath",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Document",
                newName: "MyProperty");
        }
    }
}
