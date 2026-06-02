using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileAccessSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddAIReasonToRiskLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AIReason",
                table: "RiskLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AIReason",
                table: "RiskLogs");
        }
    }
}
