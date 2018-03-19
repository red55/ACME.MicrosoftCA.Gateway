using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ACME.MicrosoftCA.Gateway.Migrations
{
    public partial class _000_CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IssuedNonces",
                columns: table => new
                {
                    Nonce = table.Column<string>(nullable: false),
                    IssedAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuedNonces", x => x.Nonce);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssuedNonces");
        }
    }
}
