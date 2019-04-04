using Microsoft.EntityFrameworkCore.Migrations;

namespace LexiconLMS.Migrations
{
    public partial class readystatechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LmsTask_ReadyState_ReadyStateId1",
                table: "LmsTask");

            migrationBuilder.DropIndex(
                name: "IX_LmsTask_ReadyStateId1",
                table: "LmsTask");

            migrationBuilder.DropColumn(
                name: "ReadyStateId1",
                table: "LmsTask");

            migrationBuilder.AlterColumn<int>(
                name: "ReadyStateId",
                table: "LmsTask",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LmsTask_ReadyStateId",
                table: "LmsTask",
                column: "ReadyStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_LmsTask_ReadyState_ReadyStateId",
                table: "LmsTask",
                column: "ReadyStateId",
                principalTable: "ReadyState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LmsTask_ReadyState_ReadyStateId",
                table: "LmsTask");

            migrationBuilder.DropIndex(
                name: "IX_LmsTask_ReadyStateId",
                table: "LmsTask");

            migrationBuilder.AlterColumn<string>(
                name: "ReadyStateId",
                table: "LmsTask",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ReadyStateId1",
                table: "LmsTask",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LmsTask_ReadyStateId1",
                table: "LmsTask",
                column: "ReadyStateId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LmsTask_ReadyState_ReadyStateId1",
                table: "LmsTask",
                column: "ReadyStateId1",
                principalTable: "ReadyState",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
