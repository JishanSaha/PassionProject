using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class payments1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Bookings_BookingBid",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Bid",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "BookingBid",
                table: "Payments",
                newName: "BookingId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_BookingBid",
                table: "Payments",
                newName: "IX_Payments_BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Bookings_BookingId",
                table: "Payments",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Bid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Bookings_BookingId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "Payments",
                newName: "BookingBid");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                newName: "IX_Payments_BookingBid");

            migrationBuilder.AddColumn<int>(
                name: "Bid",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Bookings_BookingBid",
                table: "Payments",
                column: "BookingBid",
                principalTable: "Bookings",
                principalColumn: "Bid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
