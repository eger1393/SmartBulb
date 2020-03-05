using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBulb.Data.Migrations
{
    public partial class firstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BulbState",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Power = table.Column<int>(nullable: true),
                    Brightness = table.Column<int>(nullable: true),
                    Hue = table.Column<int>(nullable: true),
                    Saturation = table.Column<int>(nullable: true),
                    ColorTemp = table.Column<int>(nullable: true),
                    TransitionTime = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulbState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SetStateTask",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DeviceId = table.Column<string>(nullable: true),
                    StateId = table.Column<Guid>(nullable: true),
                    WaitTime = table.Column<int>(nullable: true),
                    ScriptId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetStateTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetStateTask_BulbState_StateId",
                        column: x => x.StateId,
                        principalTable: "BulbState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scripts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    StartStateId = table.Column<Guid>(nullable: true),
                    EndStateId = table.Column<Guid>(nullable: true),
                    RepeatCount = table.Column<int>(nullable: false),
                    StartHour = table.Column<int>(nullable: true),
                    StartMinute = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scripts_SetStateTask_EndStateId",
                        column: x => x.EndStateId,
                        principalTable: "SetStateTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scripts_SetStateTask_StartStateId",
                        column: x => x.StartStateId,
                        principalTable: "SetStateTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_EndStateId",
                table: "Scripts",
                column: "EndStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_StartStateId",
                table: "Scripts",
                column: "StartStateId");

            migrationBuilder.CreateIndex(
                name: "IX_SetStateTask_ScriptId",
                table: "SetStateTask",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_SetStateTask_StateId",
                table: "SetStateTask",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_SetStateTask_Scripts_ScriptId",
                table: "SetStateTask",
                column: "ScriptId",
                principalTable: "Scripts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scripts_SetStateTask_EndStateId",
                table: "Scripts");

            migrationBuilder.DropForeignKey(
                name: "FK_Scripts_SetStateTask_StartStateId",
                table: "Scripts");

            migrationBuilder.DropTable(
                name: "SetStateTask");

            migrationBuilder.DropTable(
                name: "Scripts");

            migrationBuilder.DropTable(
                name: "BulbState");
        }
    }
}
