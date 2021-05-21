using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BatchFileWebAPI.Migrations
{
    public partial class AddModelsToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BatchPublishedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    BusinessUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.BatchId);
                });

            migrationBuilder.CreateTable(
                name: "AccessControls",
                columns: table => new
                {
                    AclPKID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadUsers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadGroups = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessControls", x => x.AclPKID);
                    table.ForeignKey(
                        name: "FK_AccessControls_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "BatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    AttributePkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.AttributePkID);
                    table.ForeignKey(
                        name: "FK_Attributes_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "BatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessControls_BatchId",
                table: "AccessControls",
                column: "BatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_BatchId",
                table: "Attributes",
                column: "BatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessControls");

            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "Batches");
        }
    }
}
