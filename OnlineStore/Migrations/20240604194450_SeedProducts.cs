using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Migrations
{
    public partial class SeedProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Name", "Description", "Price", "ImageUrl" },
                values: new object[,]
                {
                { "Product 1", "Description for Product 1", 10.99m, "https://via.placeholder.com/150" },
                { "Product 2", "Description for Product 2", 20.99m, "https://via.placeholder.com/150" },
                { "Product 3", "Description for Product 3", 30.99m, "https://via.placeholder.com/150" }
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Name",
                keyValues: new object[] { "Product 1", "Product 2", "Product 3" }
            );
        }
    }
}
