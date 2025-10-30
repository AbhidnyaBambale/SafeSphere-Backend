using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SafeSphere.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMilestones4And5Features : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisasterAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DisasterType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AffectedArea = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    AffectedRadiusKm = table.Column<double>(type: "double precision", nullable: true),
                    Severity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Moderate"),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    IssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExternalAlertId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Source = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ConfirmationCount = table.Column<int>(type: "integer", nullable: false),
                    SafetyInstructions = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    EmergencyContactInfo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisasterAlerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SafeRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    OriginLat = table.Column<double>(type: "double precision", nullable: false),
                    OriginLng = table.Column<double>(type: "double precision", nullable: false),
                    DestinationLat = table.Column<double>(type: "double precision", nullable: false),
                    DestinationLng = table.Column<double>(type: "double precision", nullable: false),
                    RouteCoordinates = table.Column<string>(type: "text", nullable: false),
                    DistanceMeters = table.Column<double>(type: "double precision", nullable: false),
                    DurationSeconds = table.Column<int>(type: "integer", nullable: false),
                    SafetyScore = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    UnsafeZonesAvoided = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafeRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafeRoutes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnsafeZones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CenterLat = table.Column<double>(type: "double precision", nullable: false),
                    CenterLng = table.Column<double>(type: "double precision", nullable: false),
                    RadiusMeters = table.Column<double>(type: "double precision", nullable: false, defaultValue: 500.0),
                    Severity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Medium"),
                    ThreatType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Other"),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReportedByUserId = table.Column<int>(type: "integer", nullable: true),
                    ConfirmationCount = table.Column<int>(type: "integer", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnsafeZones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherAlerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    WeatherCondition = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Temperature = table.Column<double>(type: "double precision", nullable: true),
                    Severity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Info"),
                    ExternalAlertId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    AdditionalInfo = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DataSource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherAlerts", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "PanicAlerts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2025, 10, 29, 19, 26, 51, 836, DateTimeKind.Utc).AddTicks(1003));

            migrationBuilder.UpdateData(
                table: "PanicAlerts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2025, 10, 29, 20, 56, 51, 836, DateTimeKind.Utc).AddTicks(1025));

            migrationBuilder.UpdateData(
                table: "SOSAlerts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AcknowledgedAt", "Timestamp" },
                values: new object[] { new DateTime(2025, 10, 29, 20, 41, 51, 836, DateTimeKind.Utc).AddTicks(1165), new DateTime(2025, 10, 29, 20, 26, 51, 836, DateTimeKind.Utc).AddTicks(1136) });

            migrationBuilder.UpdateData(
                table: "SOSAlerts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2025, 10, 29, 21, 11, 51, 836, DateTimeKind.Utc).AddTicks(1178));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 29, 21, 26, 51, 408, DateTimeKind.Utc).AddTicks(8958), "$2a$11$SqLdekSJsTJjB91nH4Vd0eHROJlnBAT.TsAxupiuW6hSJpSgSVa02" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 29, 21, 26, 51, 835, DateTimeKind.Utc).AddTicks(9703), "$2a$11$TwNSvE6cB2i3.UVuGMnArOQMR4yFrmaNsQ0hLa4xSAUdRVcWhyA9y" });

            migrationBuilder.CreateIndex(
                name: "IX_DisasterAlerts_ExternalAlertId",
                table: "DisasterAlerts",
                column: "ExternalAlertId");

            migrationBuilder.CreateIndex(
                name: "IX_SafeRoutes_UserId",
                table: "SafeRoutes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherAlerts_ExternalAlertId",
                table: "WeatherAlerts",
                column: "ExternalAlertId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisasterAlerts");

            migrationBuilder.DropTable(
                name: "SafeRoutes");

            migrationBuilder.DropTable(
                name: "UnsafeZones");

            migrationBuilder.DropTable(
                name: "WeatherAlerts");

            migrationBuilder.UpdateData(
                table: "PanicAlerts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Timestamp",
                value: new DateTime(2025, 10, 27, 0, 2, 40, 338, DateTimeKind.Utc).AddTicks(4252));

            migrationBuilder.UpdateData(
                table: "PanicAlerts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2025, 10, 27, 1, 32, 40, 338, DateTimeKind.Utc).AddTicks(4266));

            migrationBuilder.UpdateData(
                table: "SOSAlerts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AcknowledgedAt", "Timestamp" },
                values: new object[] { new DateTime(2025, 10, 27, 1, 17, 40, 338, DateTimeKind.Utc).AddTicks(4338), new DateTime(2025, 10, 27, 1, 2, 40, 338, DateTimeKind.Utc).AddTicks(4336) });

            migrationBuilder.UpdateData(
                table: "SOSAlerts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2025, 10, 27, 1, 47, 40, 338, DateTimeKind.Utc).AddTicks(4346));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 27, 2, 2, 40, 197, DateTimeKind.Utc).AddTicks(5560), "$2a$11$l14925j.HCIw03SM1wwveu5xN4c7WAxEO1cV/rxjEQIFM0v/aHGnS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 27, 2, 2, 40, 338, DateTimeKind.Utc).AddTicks(3283), "$2a$11$pe1sIZnu.vBBFo.YKBQOl.nyUseos.FK3e8V6e9on1Dib/PHr3Yu." });
        }
    }
}
