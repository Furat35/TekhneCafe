using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekhneCafe.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class notificationRefactored : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Notification");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Notification",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 9, 6, 11, 17, 28, 141, DateTimeKind.Local).AddTicks(2302),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 31, 16, 9, 24, 461, DateTimeKind.Local).AddTicks(4735));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "AppUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 9, 6, 11, 17, 28, 140, DateTimeKind.Local).AddTicks(8970),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 31, 16, 9, 24, 461, DateTimeKind.Local).AddTicks(1421));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notification");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Notification",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 31, 16, 9, 24, 461, DateTimeKind.Local).AddTicks(4735),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 9, 6, 11, 17, 28, 141, DateTimeKind.Local).AddTicks(2302));

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Notification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "AppUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 31, 16, 9, 24, 461, DateTimeKind.Local).AddTicks(1421),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 9, 6, 11, 17, 28, 140, DateTimeKind.Local).AddTicks(8970));
        }
    }
}
