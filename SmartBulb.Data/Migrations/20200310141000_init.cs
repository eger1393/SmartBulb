using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBulb.Data.Migrations
{
    public partial class init : Migration
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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
                    StartMinute = table.Column<int>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Scripts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "Token" },
                values: new object[] { new Guid("650d4def-b04c-434d-866d-dd21a446e776"), "andronov.dmitry@gmail.com", "K@$@P@$$w0rd", null });

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_EndStateId",
                table: "Scripts",
                column: "EndStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_StartStateId",
                table: "Scripts",
                column: "StartStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Scripts_UserId",
                table: "Scripts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SetStateTask_ScriptId",
                table: "SetStateTask",
                column: "ScriptId");

            migrationBuilder.CreateIndex(
                name: "IX_SetStateTask_StateId",
                table: "SetStateTask",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

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

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
