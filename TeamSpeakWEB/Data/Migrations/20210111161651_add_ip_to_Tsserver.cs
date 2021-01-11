using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamSpeakWEB.Migrations
{
    public partial class add_ip_to_Tsserver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "Tsservers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ip",
                table: "Tsservers");
        }
    }
}
