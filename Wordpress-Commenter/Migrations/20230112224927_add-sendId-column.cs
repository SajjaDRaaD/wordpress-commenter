using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientApp.Migrations
{
    public partial class addsendIdcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SendId",
                table: "SendConfigs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendId",
                table: "SendConfigs");
        }
    }
}
