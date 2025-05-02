using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddThumbnailUrlColumn : Migration
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
                    { "89f7157e-275e-4de5-84dd-413c115e9082", null, "User", "USER" },
                    { "d0044b86-bb50-46d6-9b5b-16e19b88c2ca", null, "Admin", "ADMIN" },
                    { "fc8a7c3c-1736-44ef-b5db-142b6f1e02d7", null, "Instructor", "INSTRUCTOR" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "99a6682b-1aac-4284-aca2-17b65b99d59e", "AQAAAAIAAYagAAAAEJ24JcvFVmWzT/XVmhN93/cn1GrP10HD0esANCNV2bHY3EFJmh7B1wO78tUEMQFxmQ==", "27830798-b709-42db-8126-e79a3c315f1c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89f7157e-275e-4de5-84dd-413c115e9082");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0044b86-bb50-46d6-9b5b-16e19b88c2ca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc8a7c3c-1736-44ef-b5db-142b6f1e02d7");

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
