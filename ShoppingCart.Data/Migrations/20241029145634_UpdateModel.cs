using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCart.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportsBulkPricing",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SupportsBulkPricing",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
