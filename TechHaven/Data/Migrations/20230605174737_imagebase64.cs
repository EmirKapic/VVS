using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechHaven.Data.Migrations
{
    public partial class imagebase64 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Image");

            migrationBuilder.AddColumn<string>(
                name: "base64Content",
                table: "Image",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "base64Content",
                table: "Image");

            migrationBuilder.AddColumn<byte[]>(
                name: "Content",
                table: "Image",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
