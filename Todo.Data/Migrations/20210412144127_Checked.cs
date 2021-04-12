using Microsoft.EntityFrameworkCore.Migrations;

namespace Todo.Data.Migrations
{
    public partial class Checked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e7dde73-aa1e-46db-b65c-ddb1a5538832");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87593355-ae42-4326-97e0-a4aaa2b8a57b");

            migrationBuilder.AddColumn<bool>(
                name: "Checked",
                table: "TodoLines",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f6113c51-6df2-440f-b0e7-09a10cdee3c8", "50f8be05-1985-40d6-abbc-b351a35a6a52", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5dc6cdac-5c89-4e7c-8bc3-cf3f91cf8673", "540f7c93-1d89-4841-aae3-c558670506c4", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5dc6cdac-5c89-4e7c-8bc3-cf3f91cf8673");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6113c51-6df2-440f-b0e7-09a10cdee3c8");

            migrationBuilder.DropColumn(
                name: "Checked",
                table: "TodoLines");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7e7dde73-aa1e-46db-b65c-ddb1a5538832", "0ed960f0-4bbf-4862-a0a5-eca7d9564501", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "87593355-ae42-4326-97e0-a4aaa2b8a57b", "33bd004f-6469-4eec-9adf-cd1faa69a79c", "User", "USER" });
        }
    }
}
