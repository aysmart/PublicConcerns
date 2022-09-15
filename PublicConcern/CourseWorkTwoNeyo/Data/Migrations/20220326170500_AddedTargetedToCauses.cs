using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWorkTwoNeyo.Data.Migrations
{
    public partial class AddedTargetedToCauses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Targeted",
                table: "Causes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Targeted",
                table: "Causes");
        }
    }
}
