using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class bookingpackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Bookings_BookingBid",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_BookingBid",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "BookingBid",
                table: "Packages");

            migrationBuilder.CreateTable(
                name: "BookingPackage",
                columns: table => new
                {
                    BookingsBid = table.Column<int>(type: "int", nullable: false),
                    PackagesPid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPackage", x => new { x.BookingsBid, x.PackagesPid });
                    table.ForeignKey(
                        name: "FK_BookingPackage_Bookings_BookingsBid",
                        column: x => x.BookingsBid,
                        principalTable: "Bookings",
                        principalColumn: "Bid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingPackage_Packages_PackagesPid",
                        column: x => x.PackagesPid,
                        principalTable: "Packages",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingPackage_PackagesPid",
                table: "BookingPackage",
                column: "PackagesPid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingPackage");

            migrationBuilder.AddColumn<int>(
                name: "BookingBid",
                table: "Packages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_BookingBid",
                table: "Packages",
                column: "BookingBid");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Bookings_BookingBid",
                table: "Packages",
                column: "BookingBid",
                principalTable: "Bookings",
                principalColumn: "Bid");
        }
    }
}
