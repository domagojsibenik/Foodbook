using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Foodbook.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a815cc3c-b1b3-488d-871e-e2d679df7336");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ede6da8b-09e0-4c75-a4db-46a1bcc8d848");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "389b6ee3-43e1-4918-84b8-a2e10065f0ec", null, "User", "USER" },
                    { "9922bc5f-2ed2-4b1d-b67a-ea004d2dc1d6", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "389b6ee3-43e1-4918-84b8-a2e10065f0ec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9922bc5f-2ed2-4b1d-b67a-ea004d2dc1d6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a815cc3c-b1b3-488d-871e-e2d679df7336", null, "User", "USER" },
                    { "ede6da8b-09e0-4c75-a4db-46a1bcc8d848", null, "Admin", "ADMIN" }
                });
        }
    }
}
