using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Migrations
{
    public partial class UpdateProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Name",
                keyValue: "Product 1",
                column: "ImageUrl",
                value: "https://cdn-icons-png.freepik.com/256/9517/9517978.png?semt=ais_hybrid"
            );

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Name",
                keyValue: "Product 2",
                column: "ImageUrl",
                value: "https://cdn-icons-png.freepik.com/256/9517/9517978.png?semt=ais_hybrid"
            );

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Name",
                keyValue: "Product 3",
                column: "ImageUrl",
                value: "https://cdn-icons-png.freepik.com/256/9517/9517978.png?semt=ais_hybrid"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Name",
                keyValue: "Product 1",
                column: "ImageUrl",
                value: "https://via.placeholder.com/150"
            );

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Name",
                keyValue: "Product 2",
                column: "ImageUrl",
                value: "https://via.placeholder.com/150"
            );

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Name",
                keyValue: "Product 3",
                column: "ImageUrl",
                value: "https://via.placeholder.com/150"
            );
        }
    }
}
