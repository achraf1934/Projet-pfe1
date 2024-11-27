using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backen_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class contractDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractDuration",
                table: "Offres",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractDuration",
                table: "Offres");
        }
    }
}
