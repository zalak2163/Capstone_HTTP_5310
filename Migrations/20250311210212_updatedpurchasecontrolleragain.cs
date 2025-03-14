using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventPlanningCapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class updatedpurchasecontrolleragain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Purchases");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
