using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueriesAPI.Migrations
{
    /// <inheritdoc />
    public partial class Intial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QueriesApi",
                columns: table => new
                {
                    QueryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueriesApi", x => x.QueryId);
                });

            migrationBuilder.InsertData(
                table: "QueriesApi",
                columns: new[] { "QueryId", "Email", "Message", "UserId" },
                values: new object[] { new Guid("86fe8732-2559-4282-9b0b-f62e9c30fdc1"), "Admin@gmail.com", "This is a test Query", "ADM001" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueriesApi");
        }
    }
}
