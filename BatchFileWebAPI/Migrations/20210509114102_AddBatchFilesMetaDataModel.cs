using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BatchFileWebAPI.Migrations
{
    public partial class AddBatchFilesMetaDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BatchFilesMetaData",
                columns: table => new
                {
                    BatchFilesMetaDataPkID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<int>(type: "int", nullable: false),
                    MIMEType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchFilesMetaData", x => x.BatchFilesMetaDataPkID);
                    table.ForeignKey(
                        name: "FK_BatchFilesMetaData_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "BatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchFilesMetaData_BatchId",
                table: "BatchFilesMetaData",
                column: "BatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchFilesMetaData");
        }
    }
}
