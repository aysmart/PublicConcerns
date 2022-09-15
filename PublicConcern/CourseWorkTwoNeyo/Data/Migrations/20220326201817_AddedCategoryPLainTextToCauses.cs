using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWorkTwoNeyo.Data.Migrations
{
    public partial class AddedCategoryPLainTextToCauses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Causes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlainContent",
                table: "Causes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Causes");

            migrationBuilder.DropColumn(
                name: "PlainContent",
                table: "Causes");
        }
    }
}
