using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoursesWebAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName", "UserId" },
                values: new object[,]
                {
                    { new Guid("02e9db0c-9e8b-4aa1-a0a4-db85b8a55dc4"), null, "HR", "HR", null },
                    { new Guid("43d804cc-c721-4bd9-bb27-f9903b83d0bb"), null, "Student", "STUDENT", null },
                    { new Guid("84a2e635-0b80-4e30-af39-7a57df06ba3d"), null, "Teacher", "TEACHER", null },
                    { new Guid("cf3c9527-51dd-4b9e-8a34-e00d7c5b341e"), null, "Employee", "EMPLOYEE", null },
                    { new Guid("dc390911-2b01-42c5-a396-604289a8c08b"), null, "HolyFather", "HOLYFATHER", null },
                    { new Guid("f4b26c13-a1b7-43db-831d-277ca58d3ff6"), null, "Administrator", "ADMINISTRATOR", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PersonId", "PhoneNumber", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("f31733be-7d8c-422d-81c5-9feb708226e2"), 0, "f63ac60d-2fcf-4722-a308-730a1cbc7c18", "hammer000destroyer@gmail.com", true, false, null, "HAMMER000DESTROYER@GMAIL.COM", "IVAN", null, null, "", null, false, "Ivan" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("dc390911-2b01-42c5-a396-604289a8c08b"), new Guid("f31733be-7d8c-422d-81c5-9feb708226e2") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("02e9db0c-9e8b-4aa1-a0a4-db85b8a55dc4"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("43d804cc-c721-4bd9-bb27-f9903b83d0bb"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("84a2e635-0b80-4e30-af39-7a57df06ba3d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cf3c9527-51dd-4b9e-8a34-e00d7c5b341e"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f4b26c13-a1b7-43db-831d-277ca58d3ff6"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("dc390911-2b01-42c5-a396-604289a8c08b"), new Guid("f31733be-7d8c-422d-81c5-9feb708226e2") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("dc390911-2b01-42c5-a396-604289a8c08b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f31733be-7d8c-422d-81c5-9feb708226e2"));
        }
    }
}
