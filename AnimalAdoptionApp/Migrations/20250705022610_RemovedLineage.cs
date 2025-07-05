using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimalAdoptionApp.Migrations
{
    /// <inheritdoc />
    public partial class RemovedLineage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_Lineages_LineageId",
                table: "Adverts");

            migrationBuilder.DropTable(
                name: "Lineages");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_LineageId",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "LineageId",
                table: "Adverts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LineageId",
                table: "Adverts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Lineages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lineages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_LineageId",
                table: "Adverts",
                column: "LineageId");

            migrationBuilder.CreateIndex(
                name: "IX_Lineages_Name",
                table: "Lineages",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_Lineages_LineageId",
                table: "Adverts",
                column: "LineageId",
                principalTable: "Lineages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
