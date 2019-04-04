using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LexiconLMS.Migrations
{
    public partial class Lmstaskadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LmsTaskId",
                table: "Document",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReadyState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadyState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LmsTask",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TeacherFeedback = table.Column<string>(nullable: true),
                    ReadyDate = table.Column<DateTime>(nullable: false),
                    StudentAnswer = table.Column<string>(nullable: true),
                    StudentComment = table.Column<string>(nullable: true),
                    ReadyStateId = table.Column<string>(nullable: true),
                    ReadyStateId1 = table.Column<int>(nullable: true),
                    LmsActivityId = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LmsTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LmsTask_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LmsTask_LmsActivity_LmsActivityId",
                        column: x => x.LmsActivityId,
                        principalTable: "LmsActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LmsTask_ReadyState_ReadyStateId1",
                        column: x => x.ReadyStateId1,
                        principalTable: "ReadyState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Document_LmsTaskId",
                table: "Document",
                column: "LmsTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_LmsTask_ApplicationUserId",
                table: "LmsTask",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LmsTask_LmsActivityId",
                table: "LmsTask",
                column: "LmsActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_LmsTask_ReadyStateId1",
                table: "LmsTask",
                column: "ReadyStateId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_LmsTask_LmsTaskId",
                table: "Document",
                column: "LmsTaskId",
                principalTable: "LmsTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_LmsTask_LmsTaskId",
                table: "Document");

            migrationBuilder.DropTable(
                name: "LmsTask");

            migrationBuilder.DropTable(
                name: "ReadyState");

            migrationBuilder.DropIndex(
                name: "IX_Document_LmsTaskId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "LmsTaskId",
                table: "Document");
        }
    }
}
