using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesWomanBE.Migrations
{
    /// <inheritdoc />
    public partial class AddDatesAndContactMethodToMeetingNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactMethod",
                table: "MeetingNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dates",
                table: "MeetingNotes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactMethod",
                table: "MeetingNotes");

            migrationBuilder.DropColumn(
                name: "Dates",
                table: "MeetingNotes");
        }
    }
}
