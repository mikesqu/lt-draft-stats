using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace draft_data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataSets",
                columns: table => new
                {
                    DataSetId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSets", x => x.DataSetId);
                });

            migrationBuilder.CreateTable(
                name: "Draftee",
                columns: table => new
                {
                    DrafteeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    BirthDateYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Department = table.Column<int>(type: "INTEGER", nullable: false),
                    Info = table.Column<string>(type: "TEXT", nullable: false),
                    DataSetId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Draftee", x => x.DrafteeId);
                    table.ForeignKey(
                        name: "FK_Draftee_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "DataSetId");
                });

            migrationBuilder.CreateTable(
                name: "KeyValueCombo",
                columns: table => new
                {
                    KeyValueComboId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    DataSetId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyValueCombo", x => x.KeyValueComboId);
                    table.ForeignKey(
                        name: "FK_KeyValueCombo_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "DataSetId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Draftee_DataSetId",
                table: "Draftee",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyValueCombo_DataSetId",
                table: "KeyValueCombo",
                column: "DataSetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Draftee");

            migrationBuilder.DropTable(
                name: "KeyValueCombo");

            migrationBuilder.DropTable(
                name: "DataSets");
        }
    }
}
