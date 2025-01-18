using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace draft_data.Migrations
{
    /// <inheritdoc />
    public partial class AddedHasData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DataSets",
                columns: new[] { "DataSetId", "CreatedOn" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DataSets",
                keyColumn: "DataSetId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DataSets",
                keyColumn: "DataSetId",
                keyValue: 2);
        }
    }
}
