using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakerWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToGallery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Galleries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Galleries_CategoryId",
                table: "Galleries",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Categories_CategoryId",
                table: "Galleries",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Categories_CategoryId",
                table: "Galleries");

            migrationBuilder.DropIndex(
                name: "IX_Galleries_CategoryId",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Galleries");
        }
    }
}
