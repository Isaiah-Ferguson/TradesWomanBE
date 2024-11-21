using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradesWomanBE.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    FirstTimeLogin = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    RecruiterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumOfContacts = table.Column<int>(type: "int", nullable: false),
                    LastDateContacted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastContactMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreferredContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfSupportRequested = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    ProgramEnrolled = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnrollDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgramEndDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecruiterInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleInitial = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstTimeLogin = table.Column<bool>(type: "bit", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HireDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruiterInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stipends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TypeOfStipend = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreApprenticeshipProgram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StipendDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StipendAmountRequested = table.Column<int>(type: "int", nullable: true),
                    StipendPaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedDate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stipends", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeetingNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecruiterInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeetingId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeetingsModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeetingNotes_Meetings_MeetingsModelId",
                        column: x => x.MeetingsModelId,
                        principalTable: "Meetings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleInitial = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildrenUnderSix = table.Column<int>(type: "int", nullable: true),
                    ChildrenOverSix = table.Column<int>(type: "int", nullable: true),
                    TotalHouseholdFamily = table.Column<int>(type: "int", nullable: true),
                    SSNLastFour = table.Column<int>(type: "int", nullable: true),
                    ValidSSNAuthToWrk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CriminalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disabled = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FoundUsOn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateJoinedEAW = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Employed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecruiterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateRegistered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActiveOrFormerMilitary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalMonthlyIncome = table.Column<int>(type: "int", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgramInfoId = table.Column<int>(type: "int", nullable: true),
                    HighestEducation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidCALicense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    County = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ethnicity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeetingsId = table.Column<int>(type: "int", nullable: true),
                    StipendsId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientInfo_Meetings_MeetingsId",
                        column: x => x.MeetingsId,
                        principalTable: "Meetings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClientInfo_Programs_ProgramInfoId",
                        column: x => x.ProgramInfoId,
                        principalTable: "Programs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClientInfo_Stipends_StipendsId",
                        column: x => x.StipendsId,
                        principalTable: "Stipends",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientInfo_MeetingsId",
                table: "ClientInfo",
                column: "MeetingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInfo_ProgramInfoId",
                table: "ClientInfo",
                column: "ProgramInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInfo_StipendsId",
                table: "ClientInfo",
                column: "StipendsId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingNotes_MeetingsModelId",
                table: "MeetingNotes",
                column: "MeetingsModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "ClientInfo");

            migrationBuilder.DropTable(
                name: "MeetingNotes");

            migrationBuilder.DropTable(
                name: "RecruiterInfo");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "Stipends");

            migrationBuilder.DropTable(
                name: "Meetings");
        }
    }
}
