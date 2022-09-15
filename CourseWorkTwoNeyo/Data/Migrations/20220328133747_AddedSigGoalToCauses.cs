using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWorkTwoNeyo.Data.Migrations
{
    public partial class AddedSigGoalToCauses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SigGoal",
                table: "Causes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SigGoal",
                table: "Causes");
        }
    }
}
