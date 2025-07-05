using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnimalAdoptionApp.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteAdvertComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_Adverts_AdvertId",
                table: "comments");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_Adverts_AdvertId",
                table: "comments",
                column: "AdvertId",
                principalTable: "Adverts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_Adverts_AdvertId",
                table: "comments");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_Adverts_AdvertId",
                table: "comments",
                column: "AdvertId",
                principalTable: "Adverts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
