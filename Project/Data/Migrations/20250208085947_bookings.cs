using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class bookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookingBid",
                table: "Packages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Bid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCid = table.Column<int>(type: "int", nullable: false),
                    Cid = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Bid);
                    table.ForeignKey(
                        name: "FK_Bookings_Customers_CustomerCid",
                        column: x => x.CustomerCid,
                        principalTable: "Customers",
                        principalColumn: "Cid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Packages_BookingBid",
                table: "Packages",
                column: "BookingBid");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerCid",
                table: "Bookings",
                column: "CustomerCid");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Bookings_BookingBid",
                table: "Packages",
                column: "BookingBid",
                principalTable: "Bookings",
                principalColumn: "Bid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Bookings_BookingBid",
                table: "Packages");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Packages_BookingBid",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "BookingBid",
                table: "Packages");
        }
    }
}
