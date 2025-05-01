using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ab4f2da-c0e5-4f57-b8f2-dbf0653e1db0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56d859b7-61fa-460f-b678-91bd8a1f2128");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ea9d6b0-14d7-4f55-9a57-39bb9adc584c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5ab7cfa6-b650-41d6-88a7-cb942d3ca891", null, "Instructor", "INSTRUCTOR" },
                    { "bd5036af-a56a-4e9a-be6a-0ea78331f982", null, "Admin", "ADMIN" },
                    { "c594a963-2aab-4130-907a-4e7fcbb045d7", null, "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0c53d99c-fe24-4746-a1d6-fb45a8b7d46f", "AQAAAAIAAYagAAAAECWdv93ycRYkVLgt3YXrPziZjNrIOjPnhj0r1MmfnlqVkOnG+IH6RLR+D7s2NyaN6w==", "ff7751b2-88b5-42b8-becb-f0e9a05432bd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ab7cfa6-b650-41d6-88a7-cb942d3ca891");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd5036af-a56a-4e9a-be6a-0ea78331f982");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c594a963-2aab-4130-907a-4e7fcbb045d7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1ab4f2da-c0e5-4f57-b8f2-dbf0653e1db0", null, "Admin", "ADMIN" },
                    { "56d859b7-61fa-460f-b678-91bd8a1f2128", null, "User", "USER" },
                    { "7ea9d6b0-14d7-4f55-9a57-39bb9adc584c", null, "Instructor", "INSTRUCTOR" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e389545a-53ed-4c29-8b12-df9087b8cb20", "AQAAAAIAAYagAAAAEGr0+1+VXjUrMzJ8zodCm8KYOred74XAonzLQoD/VgtTlAVlpePHdxvrciNA2XekXA==", "57206db3-2ec5-460b-824a-e5b139067046" });
        }
    }
}
