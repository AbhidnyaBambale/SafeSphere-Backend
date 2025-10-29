using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SafeSphere.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    EmergencyContacts = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PanicAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LocationLat = table.Column<double>(type: "double precision", nullable: false),
                    LocationLng = table.Column<double>(type: "double precision", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    AdditionalInfo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PanicAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PanicAlerts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SOSAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LocationLat = table.Column<double>(type: "double precision", nullable: true),
                    LocationLng = table.Column<double>(type: "double precision", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Acknowledged = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AcknowledgedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOSAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SOSAlerts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "EmergencyContacts", "Name", "PasswordHash", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 27, 2, 2, 40, 197, DateTimeKind.Utc).AddTicks(5560), "john.doe@example.com", "Jane Doe:+1234567891,Police:911", "John Doe", "$2a$11$l14925j.HCIw03SM1wwveu5xN4c7WAxEO1cV/rxjEQIFM0v/aHGnS", "+1234567890", null },
                    { 2, new DateTime(2025, 10, 27, 2, 2, 40, 338, DateTimeKind.Utc).AddTicks(3283), "jane.smith@example.com", "John Smith:+1234567893,Emergency:911", "Jane Smith", "$2a$11$pe1sIZnu.vBBFo.YKBQOl.nyUseos.FK3e8V6e9on1Dib/PHr3Yu.", "+1234567892", null }
                });

            migrationBuilder.InsertData(
                table: "PanicAlerts",
                columns: new[] { "Id", "AdditionalInfo", "LocationLat", "LocationLng", "Status", "Timestamp", "UserId" },
                values: new object[,]
                {
                    { 1, "False alarm", 40.712800000000001, -74.006, "Resolved", new DateTime(2025, 10, 27, 0, 2, 40, 338, DateTimeKind.Utc).AddTicks(4252), 1 },
                    { 2, "Need help", 34.052199999999999, -118.2437, "Active", new DateTime(2025, 10, 27, 1, 32, 40, 338, DateTimeKind.Utc).AddTicks(4266), 2 }
                });

            migrationBuilder.InsertData(
                table: "SOSAlerts",
                columns: new[] { "Id", "Acknowledged", "AcknowledgedAt", "Location", "LocationLat", "LocationLng", "Message", "Timestamp", "UserId" },
                values: new object[] { 1, true, new DateTime(2025, 10, 27, 1, 17, 40, 338, DateTimeKind.Utc).AddTicks(4338), "Highway 101, San Francisco", 37.774900000000002, -122.4194, "Car accident on Highway 101", new DateTime(2025, 10, 27, 1, 2, 40, 338, DateTimeKind.Utc).AddTicks(4336), 1 });

            migrationBuilder.InsertData(
                table: "SOSAlerts",
                columns: new[] { "Id", "AcknowledgedAt", "Location", "LocationLat", "LocationLng", "Message", "Timestamp", "UserId" },
                values: new object[] { 2, null, "Downtown, Los Angeles", 34.052199999999999, -118.2437, "Feeling unsafe, please check on me", new DateTime(2025, 10, 27, 1, 47, 40, 338, DateTimeKind.Utc).AddTicks(4346), 2 });

            migrationBuilder.CreateIndex(
                name: "IX_PanicAlerts_UserId",
                table: "PanicAlerts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SOSAlerts_UserId",
                table: "SOSAlerts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PanicAlerts");

            migrationBuilder.DropTable(
                name: "SOSAlerts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
