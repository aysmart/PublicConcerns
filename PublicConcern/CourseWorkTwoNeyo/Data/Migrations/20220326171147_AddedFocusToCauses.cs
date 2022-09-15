using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWorkTwoNeyo.Data.Migrations
{
    public partial class AddedFocusToCauses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Focus",
                table: "Causes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Focus",
                table: "Causes");
        }
    }
}
