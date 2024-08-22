using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MossadAgentsRest.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location_X = table.Column<int>(type: "int", nullable: false),
                    Location_Y = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TargetModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location_X = table.Column<int>(type: "int", nullable: false),
                    Location_Y = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MissionModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    LeftTime = table.Column<double>(type: "float", nullable: false),
                    KillTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MissionModel_AgentModel_AgentId",
                        column: x => x.AgentId,
                        principalTable: "AgentModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MissionModel_TargetModel_TargetId",
                        column: x => x.TargetId,
                        principalTable: "TargetModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissionModel_AgentId",
                table: "MissionModel",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_MissionModel_TargetId",
                table: "MissionModel",
                column: "TargetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MissionModel");

            migrationBuilder.DropTable(
                name: "AgentModel");

            migrationBuilder.DropTable(
                name: "TargetModel");
        }
    }
}
