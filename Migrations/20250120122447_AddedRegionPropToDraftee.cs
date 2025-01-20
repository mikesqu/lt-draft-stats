using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace draft_data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRegionPropToDraftee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Draftee",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Draftee");
        }
    }
}
