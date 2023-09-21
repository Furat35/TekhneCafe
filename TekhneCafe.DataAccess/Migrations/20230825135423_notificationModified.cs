using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekhneCafe.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class notificationModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "From",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "To",
                table: "Notification");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Notification",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 25, 16, 54, 23, 895, DateTimeKind.Local).AddTicks(5778),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 24, 17, 51, 15, 784, DateTimeKind.Local).AddTicks(1325));

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Notification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "AppUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 25, 16, 54, 23, 895, DateTimeKind.Local).AddTicks(3106),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 24, 17, 51, 15, 783, DateTimeKind.Local).AddTicks(8278));

            migrationBuilder.CreateIndex(
                name: "IX_Notification_AppUserId",
                table: "Notification",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AppUser_AppUserId",
                table: "Notification",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AppUser_AppUserId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_AppUserId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Notification");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Notification",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 24, 17, 51, 15, 784, DateTimeKind.Local).AddTicks(1325),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 25, 16, 54, 23, 895, DateTimeKind.Local).AddTicks(5778));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Notification",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "Notification",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "Notification",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "AppUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 24, 17, 51, 15, 783, DateTimeKind.Local).AddTicks(8278),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 8, 25, 16, 54, 23, 895, DateTimeKind.Local).AddTicks(3106));
        }
    }
}
