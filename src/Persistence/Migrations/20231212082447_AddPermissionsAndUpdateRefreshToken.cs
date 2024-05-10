using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoursesWebAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionsAndUpdateRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityContract_Activities_ClassesId",
                table: "ActivityContract");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "ClassesId",
                table: "ActivityContract",
                newName: "ActivitiesId");

            migrationBuilder.AddColumn<string>(
                name: "Fingerprint",
                table: "RefreshTokens",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    PermissionsId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_PermissionRole_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "ViewRoles" },
                    { 3, "ManageRoles" },
                    { 4, "ViewActivities" },
                    { 5, "ManageActivities" },
                    { 6, "ViewActivityTypes" },
                    { 7, "ManageActivityTypes" },
                    { 8, "ViewUsers" },
                    { 9, "ManageUsers" },
                    { 10, "ViewStudents" },
                    { 11, "ManageStudents" },
                    { 12, "ViewEmployees" },
                    { 13, "ManageEmployees" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RoleId",
                table: "PermissionRole",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityContract_Activities_ActivitiesId",
                table: "ActivityContract",
                column: "ActivitiesId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityContract_Activities_ActivitiesId",
                table: "ActivityContract");

            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropColumn(
                name: "Fingerprint",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "ActivitiesId",
                table: "ActivityContract",
                newName: "ClassesId");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "RefreshTokens",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityContract_Activities_ClassesId",
                table: "ActivityContract",
                column: "ClassesId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
